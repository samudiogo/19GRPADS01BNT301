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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CountriesController : ApiController
    {
        private AssessmentDbContext db = new AssessmentDbContext();

        // GET: api/Countries
        public IHttpActionResult GetCountries()
        {
            return Ok(db.Countries.Include(c => c.States).Select(country => new CountryModel
            {
                Id = country.Id,
                Name = country.Name,
                PhotoUrl = country.PhotoUrl,
                States = country.States.Select(state => new StateCreateModel
                {
                    Id = state.Id,
                    Name = state.Name,
                    FlagPhotoUrl = state.FlagPhotoUrl,
                    CountryId = country.Id,
                    CountryName = country.Name
                }).ToList()
            }));
        }

        [HttpGet]
        [Route("api/countries/{id}/states")]
        public async Task<IHttpActionResult> GetStatesByCountry(Guid id)
        {
            var country = await db.Countries.Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (country == null)
                return NotFound();

            return Ok(country.States.Select(state => new StateCreateModel
            {
                Id = state.Id,
                Name = state.Name,
                FlagPhotoUrl = state.FlagPhotoUrl,
                CountryId = country.Id,
                CountryName = country.Name
            }).ToList());

        }

        [HttpGet]
        [Route("api/Countries/Total")]
        public async Task<IHttpActionResult> GetTotalOfCountries()
        {

            var result = await db.Database.SqlQuery<int>(@"SELECT [dbo].[TotalOfContries] () result").FirstAsync();

            return Ok(result);
        }

        // GET: api/Countries/5
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> GetCountry(Guid id)
        {
            var country = await db.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(new CountryModel { Id = country.Id, Name = country.Name, PhotoUrl = country.PhotoUrl });
        }

        // PUT: api/Countries/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCountry(Guid id, CountryCreateEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            var country = new Country { Id = id };

            db.Countries.Attach(country);
            db.Entry(country).CurrentValues.SetValues(model);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        // POST: api/Countries
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> PostCountry(CountryCreateEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.Id = Guid.NewGuid();
            var country = new Country
            {
                Id = model.Id,
                Name = model.Name,
                PhotoUrl = model.PhotoUrl
            };

            db.Countries.Add(country);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CountryExists(model.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }

        // DELETE: api/Countries/5
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> DeleteCountry(Guid id)
        {
            Country country = await db.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            db.Countries.Remove(country);
            await db.SaveChangesAsync();

            return Ok(country);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CountryExists(Guid id)
        {
            return db.Countries.Count(e => e.Id == id) > 0;
        }
    }
}