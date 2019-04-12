using Assessment.Data;
using Assessment.ViewModels;
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

namespace AssessmentFriendWebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FriendsController : ApiController
    {
        private AssessmentDbContext db = new AssessmentDbContext();

        // GET: api/Friends
        public async Task<IHttpActionResult> GetFriends()
        {
            var friends = await db.Friends.Include(i => i.Friends).Include(i => i.State.Country).ToListAsync();

            var model = friends.Select(friend => new FriendModel
            {
                Id = friend.Id,
                Name = $"{friend.Name} {friend.LastName}",
                BirthDate = friend.BirthDate,
                PhotoUrl = friend.PhotoUrl,
                Email = friend.Email,
                PhoneNumber = friend.PhoneNumber,
                StateId = friend.State.Id,
                StateName = friend.State.Name,
                StateFlagPhotoUrl = friend.State.FlagPhotoUrl,
                CountryId = friend.State.Country.Id,
                CountryName = friend.State.Country.Name,
                CountryFlagPhotoUrl = friend.State.Country.PhotoUrl,
                TotalFriends = db.Database.SqlQuery<int>($"select dbo.TotalOfFriendsOf('{friend.Id}')").FirstOrDefault()

            });
            return Ok(model);
        }

        // GET: api/Friends/5
        [ResponseType(typeof(FriendModel))]
        public async Task<IHttpActionResult> GetFriend(Guid id)
        {
            var friend = await db.Friends.Include(f => f.State.Country).Include(f => f.Friends).FirstOrDefaultAsync(f => f.Id.Equals(id));

            if (friend == null)
            {
                return NotFound();
            }

            var model = new FriendModel
            {
                Id = friend.Id,
                Name = friend.Name,
                LastName = friend.LastName,
                PhoneNumber = friend.PhoneNumber,
                Email = friend.Email,
                PhotoUrl = friend.PhotoUrl,
                BirthDate = friend.BirthDate,
                StateName = friend.State.Name,
                StateId = friend.State.Id,
                CountryName = friend.State.Country.Name,
                CountryId = friend.State.Country.Id,
                Friends = friend.Friends.Select(f => new FriendModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    LastName = f.LastName,
                    PhoneNumber = f.PhoneNumber,
                    Email = f.Email,
                    PhotoUrl = f.PhotoUrl,
                    BirthDate = f.BirthDate,
                    StateName = f.State.Name,
                    StateId = f.State.Id,
                    CountryName = f.State.Country.Name,
                    CountryId = f.State.Country.Id,

                }).ToList()

            };

            return Ok(model);
        }

        // PUT: api/Friends/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFriend(Guid id, FriendCreateEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            var state = await db.States.FirstOrDefaultAsync(s => s.Id == model.StateId);

            if (state == null) return BadRequest("state invalid");

            var friend = new Friend
            {
                Id = model.Id,
                Name = model.Name,
                LastName = model.LastName,
                PhotoUrl = model.PhotoUrl,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                State = state,
                BirthDate = model.BirthDate
            };




            try
            {
                db.Entry(friend).State = EntityState.Modified;

                await db.SaveChangesAsync();

                var query = $"select ChildFriendId from [Friendship] where MainFriendId = '{model.Id}'";
                var dbFriendships = db.Database.SqlQuery<Guid>(query).ToList();

                foreach (var item in model.FriendsGuidList)
                {
                    if (!dbFriendships.Any(f => f.Equals(item)))
                        await db.Database.ExecuteSqlCommandAsync($"insert into [Friendship] values('{model.Id}','{item}')");

                }

                foreach (var item in dbFriendships)
                {
                    if (!model.FriendsGuidList.Any(f => f.Equals(item)))
                        await db.Database.ExecuteSqlCommandAsync($"DELETE from [Friendship] where (MainFriendId = '{model.Id}' AND ChildFriendId = '{item}')");

                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(id))
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

        // POST: api/Friends
        [ResponseType(typeof(Friend))]
        public async Task<IHttpActionResult> PostFriend(FriendCreateEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var state = await db.States.FirstOrDefaultAsync(s => s.Id == model.StateId);
            if (state == null) return BadRequest("state invalid");
            var friend = new Friend
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                LastName = model.LastName,
                PhotoUrl = model.PhotoUrl,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                State = state,
                BirthDate = model.BirthDate,
            };

            try
            {
                db.Friends.Add(friend);

                await db.SaveChangesAsync();

                foreach (var friendId in model.FriendsGuidList)
                {
                    var query = $"insert into [Friendship] values('{friend.Id}','{friendId}')";

                    await db.Database.ExecuteSqlCommandAsync(query);
                }
            }

            catch (Exception)
            {
                if (FriendExists(friend.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = friend.Id }, friend);
        }

        // DELETE: api/Friends/5
        [ResponseType(typeof(Friend))]
        public async Task<IHttpActionResult> DeleteFriend(Guid id)
        {
            Friend friend = await db.Friends.FindAsync(id);


            if (friend == null)
            {
                return NotFound();
            }

            if (HasFriends(id))
            {
                var query = $@"DELETE FROM [FriendShip] WHERE 
                mainFriendId = '{id:N}' or childfriendId = '{id:N}'";

                await db.Database.ExecuteSqlCommandAsync(query);
            }

            db.Friends.Remove(friend);
            await db.SaveChangesAsync();

            return Ok(friend);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FriendExists(Guid id)
        {
            return db.Friends.Count(e => e.Id == id) > 0;
        }

        private bool HasFriends(Guid id)
        {
            var countOfFriends = db.Database.SqlQuery<int>($"select dbo.TotalOfFriendsOf('{id}')").FirstOrDefault();
            return countOfFriends > 0;
        }
    }
}