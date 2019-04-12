using Assessment.ViewModels;
using AssessmentDomain.Common;
using AssessmentDomain.DomainService;
using AssessmentDomain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AssessmentWeb.Controllers
{
    public class FriendsController : ControllerBase
    {

        private const string UrlBase = "api/Friends";

        public FriendsController(IPhotoService iPhotoService) : base(iPhotoService)
        {
            ApiBasePath = "https://assessmentfriendwebapi20190325104818.azurewebsites.net";
        }

        // GET: Friends
        public async Task<ActionResult> Index()
        {
            var model = await ListBy<FriendModel>(UrlBase);
            if (!Request.IsAjaxRequest())
                ViewBag.ShowCreateButton = true;

            return View(model);
        }
        public PartialViewResult ListOfFriends(int max = 5)
        {
            var model = Task.Run(async () => await ListBy<FriendModel>(UrlBase)).Result;
            return PartialView("Index", model);
        }

        // GET: Friends/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null || id.Value.Equals(Guid.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var friend = JsonConvert.DeserializeObject<FriendModel>(modelResult);

            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // GET: Friends/Create
        public async Task<ActionResult> Create()
        {
            var tempApiBasePath = ApiBasePath;

            ApiBasePath = "https://assessmentcountrflagwebapi20190325105438.azurewebsites.net";

            ViewBag.CountryId = await ListBy<Country>("api/countries");
            ApiBasePath = tempApiBasePath;

            var friends = (await ListBy<Friend>("api/friends")).Select(friend => new { MyFriendId = friend.Id, FriendFullName = $"{friend.Name} {friend.LastName}" }).ToList();
            if (friends.Any())
                ViewBag.MyFriends = new MultiSelectList(friends, "MyFriendId", "FriendFullName");



            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,LastName,PhotoUrl,Email,PhoneNumber,BirthDate, StateId")] Friend friend, HttpPostedFileBase binaryFile)
        {
            if (!ModelState.IsValid || binaryFile == null)
            {
                ViewBag.CountryId = await ListBy<Country>("api/countries");
                return View(friend);
            }
            friend.Id = Guid.NewGuid();
            friend.PhotoUrl = await UploadPhotoAsync(binaryFile, ContainerName.Profile);

            await Post(UrlBase, JsonConvert.SerializeObject(friend));

            return RedirectToAction("Index");

        }

        // GET: Friends/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null || id.Value.Equals(Guid.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tempApiBasePath = ApiBasePath;

            ApiBasePath = "https://assessmentcountrflagwebapi20190325105438.azurewebsites.net";

            ViewBag.CountryId = await ListBy<Country>("api/countries");
            ApiBasePath = tempApiBasePath;

            var friends = (await ListBy<Friend>("api/friends")).Where(fs => fs.Id != id.Value).Select(f => new { MyFriendId = f.Id, FriendFullName = $"{f.Name} {f.LastName}" }).ToList();



            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();


            var friend = JsonConvert.DeserializeObject<FriendModel>(modelResult);
            if (friend == null)
            {
                return HttpNotFound();
            }
            if (friends.Any())
            {
                var aleradyFriends = friend.Friends.Select(f => f.Id).ToList();
                Session["friendList"] = aleradyFriends;
                ViewBag.MyFriends = new MultiSelectList(friends, "MyFriendId", "FriendFullName", aleradyFriends);
            }

            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,LastName,PhotoUrl,Email,PhoneNumber,BirthDate,StateId, myfriendId")] FriendCreateEditModel model,
            HttpPostedFileBase binaryFile, IEnumerable<Guid> myfriendId)
        {
            if (!ModelState.IsValid)
            {
                var tempApiBasePath = ApiBasePath;

                ApiBasePath = "https://assessmentcountrflagwebapi20190325105438.azurewebsites.net";

                ViewBag.CountryId = await ListBy<Country>("api/countries");
                ApiBasePath = tempApiBasePath;

                var friends = (await ListBy<Friend>("api/friends")).Select(f => new { MyFriendId = f.Id, FriendFullName = $"{f.Name} {f.LastName}" });

                ViewBag.MyFriends = new MultiSelectList(friends, "MyFriendId", "FriendFullName", (List<Guid>)Session["friendList"]);

                return View(model);
            }

            if (binaryFile != null)
                model.PhotoUrl = await UploadPhotoAsync(binaryFile, ContainerName.Profile);

            model.FriendsGuidList = myfriendId.ToList();

            var friend = JsonConvert.SerializeObject(model);

            await Put($"{UrlBase}/{model.Id}", friend);

            Session["friendList"] = null;

            return RedirectToAction("Index");
        }

        // GET: Friends/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var friend = JsonConvert.DeserializeObject<Friend>(modelResult);

            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var friend = JsonConvert.DeserializeObject<Friend>(modelResult);

            if (friend == null)
            {
                return HttpNotFound();
            }
            await DeleteApi($"{UrlBase}/{friend.Id}");
            return RedirectToAction("Index");
        }


    }
}
