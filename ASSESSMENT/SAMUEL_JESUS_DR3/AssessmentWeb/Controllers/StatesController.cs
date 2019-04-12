using AssessmentDomain.Common;
using AssessmentDomain.DomainService;
using AssessmentDomain.Entities;
using AssessmentWeb.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AssessmentWeb.Controllers
{
    public class StatesController : ControllerBase
    {
        private const string UrlBase = "api/States";

        public StatesController(IPhotoService iPhotoService) : base(iPhotoService)
        {
            ApiBasePath = "https://assessmentcountrflagwebapi20190325105438.azurewebsites.net/"; //"http://localhost:50402/";
        }

        // GET: States
        public async Task<ActionResult> Index()
        {
            var model = await ListBy<State>(UrlBase);

            ViewBag.ShowCreateButton = true;

            return View(model);
        }
        public PartialViewResult ListOfStates(int max = 5)
        {
            var model = Task.Run(async () => await ListBy<State>(UrlBase)).Result;
            return PartialView("Index", model);
        }
        // GET: States/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null || id.Value.Equals(Guid.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var state = JsonConvert.DeserializeObject<StateEditViewModel>(modelResult);

            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        // GET: Countries/Create
        public async Task<ActionResult> Create()
        {
            var countries = await ListBy<Country>("api/countries");

            //ViewBag.CountryId = countries.Select(country => new SelectListItem
            //{
            //    Text = country.Name,
            //    Value = country.Id.ToString()
            //});
            ViewBag.CountryId = countries;
            return View();
        }



        // POST: States/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name, CountryId, FlagPhotoUrl")] StateCreateViewModel state, HttpPostedFileBase binaryFile)
        {
            if (!ModelState.IsValid || binaryFile == null)
            {
                ViewBag.CountryId = await ListBy<Country>("api/countries");
                return View(state);
            }


            state.Id = Guid.NewGuid();

            state.FlagPhotoUrl = await UploadPhotoAsync(binaryFile, ContainerName.StateFlag);

            await Post(UrlBase, JsonConvert.SerializeObject(state));

            return RedirectToAction("Index");

        }

        // GET: States/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null || id.Value.Equals(Guid.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var state = JsonConvert.DeserializeObject<StateEditViewModel>(modelResult);

            if (state == null)
            {
                return HttpNotFound();
            }



            return View(state);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,FlagPhotoUrl, CountryId")] StateEditViewModel state, HttpPostedFileBase binaryFile)
        {
            if (!ModelState.IsValid) return View(state);

            if (binaryFile != null)
                state.FlagPhotoUrl = await UploadPhotoAsync(binaryFile, ContainerName.StateFlag);

            await Put($"{UrlBase}/{state.Id}", JsonConvert.SerializeObject(state));
            return RedirectToAction("Index");
        }

        // GET: States/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var state = JsonConvert.DeserializeObject<StateEditViewModel>(modelResult);

            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {

            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var state = JsonConvert.DeserializeObject<State>(modelResult);

            if (state == null)
            {
                return HttpNotFound();
            }
            await DeleteApi($"{UrlBase}/{state.Id}");
            return RedirectToAction("Index");
        }

    }
}
