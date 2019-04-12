using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TP3WebApi.Models;

namespace TP3WebApi.Controllers
{
    public class FriendsController : ApiController
    {
        private Tp3DbContext db = new Tp3DbContext();

        // GET: api/Friends
        public IEnumerable<Friend> GetFriends()
        {
            return db.Database.SqlQuery<Friend>("GetAllFriends").ToList();
        }

        // GET: api/Friends/5
        [ResponseType(typeof(Friend))]
        public async Task<IHttpActionResult> GetFriend(Guid id)
        {
            Friend friend = await db.Friends.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }

            return Ok(friend);
        }

        // PUT: api/Friends/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFriend(Guid id, Friend friend)
        {

            // por eu ter configurado o dbcontext para user sp, todas as operacoes de cud serão feitas automaticamente com as sp
            //configuracao: modelBuilder.Entity<Friend>().MapToStoredProcedures();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != friend.Id)
            {
                return BadRequest();
            }

            db.Entry(friend).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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
        public async Task<IHttpActionResult> PostFriend(Friend friend)
        {
            // por eu ter configurado o dbcontext para user sp, todas as operacoes de cud serão feitas automaticamente com as sp
            //configuracao: modelBuilder.Entity<Friend>().MapToStoredProcedures();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            friend.Id = Guid.NewGuid();

            db.Friends.Add(friend);


            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
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
            // por eu ter configurado o dbcontext para user sp, todas as operacoes de cud serão feitas automaticamente com as sp
            //configuracao: modelBuilder.Entity<Friend>().MapToStoredProcedures();

            Friend friend = await db.Friends.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
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
    }
}