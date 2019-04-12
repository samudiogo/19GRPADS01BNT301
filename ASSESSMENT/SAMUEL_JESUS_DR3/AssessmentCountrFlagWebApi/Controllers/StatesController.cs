using Assessment.Data;
using AssessmentCountrFlagWebApi.Models;
using AssessmentDomain.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace AssessmentCountrFlagWebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public class StatesController : ApiController
    {
        private readonly AssessmentDbContext _db = new AssessmentDbContext();

        // GET: api/States
        public IHttpActionResult GetStates()
        {
            var model = _db.States.Select(state => new StateCreateModel
            {
                Id = state.Id,
                Name = state.Name,
                FlagPhotoUrl = state.FlagPhotoUrl,
                CountryId = state.Country.Id,
                CountryName = state.Country.Name
            }).ToList();

            return Ok(model);
        }
        
        // GET: api/States/5
        [ResponseType(typeof(State))]
        public async Task<IHttpActionResult> GetState(Guid id)
        {
            var state = await _db.States.Include(s => s.Country).FirstOrDefaultAsync(s => s.Id.Equals(id));

            if (state?.Country == null)
            {
                return NotFound();
            }

            var model = new StateCreateModel
            {
                Id = state.Id,
                Name = state.Name,
                FlagPhotoUrl = state.FlagPhotoUrl,
                CountryId = state.Country.Id,
                CountryName = state.Country.Name
            };

            return Ok(model);
        }

        // PUT: api/States/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutState(Guid id, StateEditModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            var state = new State {Id = model.Id }; // stub model, only has Id

            _db.States.Attach(state); // track your stub model

            _db.Entry(state).CurrentValues.SetValues(model);  // reflection

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/States
        [ResponseType(typeof(StateCreateModel))]
        public async Task<IHttpActionResult> PostState(StateCreateModel model)
        {
            if (model.CountryId.Equals(Guid.Empty))
                ModelState.AddModelError("CountryId", "Country identification is obrigatory");
            var country = await _db.Countries.FindAsync(model.CountryId);

            if (country == null)
                ModelState.AddModelError("CountryId", "Country not found");


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.CountryName = country?.Name;

            var state = new State
            {
                Id = model.Id,
                Name = model.Name,
                FlagPhotoUrl = model.FlagPhotoUrl,
                Country = country
            };

            _db.States.Add(state);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StateExists(state.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = state.Id }, model);
        }

        // DELETE: api/States/5
        [ResponseType(typeof(State))]
        public async Task<IHttpActionResult> DeleteState(Guid id)
        {
            State state = await _db.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _db.States.Remove(state);
            await _db.SaveChangesAsync();

            return Ok(state);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StateExists(Guid id)
        {
            return _db.States.Count(e => e.Id == id) > 0;
        }
    }
}