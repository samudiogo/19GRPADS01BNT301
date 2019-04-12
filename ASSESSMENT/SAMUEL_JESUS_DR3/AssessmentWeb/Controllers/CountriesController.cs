using AssessmentDomain.Common;
using AssessmentDomain.DomainService;
using AssessmentDomain.Entities;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssessmentWeb.Controllers
{
    public class CountriesController : ControllerBase
    {
        private const string UrlBase = "api/countries";

        public CountriesController(IPhotoService iPhotoService) : base(iPhotoService)
        {
            ApiBasePath = "https://assessmentcountrflagwebapi20190325105438.azurewebsites.net/";
        }

        // GET: Countries
        public async Task<ActionResult> Index()
        {
            var model = await ListBy<Country>(UrlBase);
            if (!Request.IsAjaxRequest())
                ViewBag.ShowCreateButton = true;
            return View(model);
        }

        public PartialViewResult ListOfCountries(int max = 5)
        {
            var model = Task.Run(async () => await ListBy<Country>(UrlBase)).Result;
            return PartialView("Index", model);
        }

        // GET: Countries/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null || id.Value.Equals(Guid.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var country = JsonConvert.DeserializeObject<Country>(modelResult);

            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // GET: States/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,PhotoUrl")] Country country, HttpPostedFileBase binaryFile)
        {
            if (!ModelState.IsValid) return View(country);

            country.Id = Guid.NewGuid();

            country.PhotoUrl = await UploadPhotoAsync(binaryFile, ContainerName.CountryFlag);

            await Post(UrlBase, JsonConvert.SerializeObject(country));

            return RedirectToAction("Index");

        }

        // GET: Countries/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null || id.Value.Equals(Guid.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var country = JsonConvert.DeserializeObject<Country>(modelResult);

            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,PhotoUrl")] Country country, HttpPostedFileBase binaryFile)
        {
            if (!ModelState.IsValid) return View(country);

            if (binaryFile != null)
                country.PhotoUrl = await UploadPhotoAsync(binaryFile, ContainerName.CountryFlag);

            await Put(UrlBase, JsonConvert.SerializeObject(country));

            return RedirectToAction("Index");
        }

        // GET: Countries/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var modelResult = await Get($"{UrlBase}/{id}");

            if (string.IsNullOrEmpty(modelResult))
                return HttpNotFound();

            var country = JsonConvert.DeserializeObject<Country>(modelResult);

            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Countries/Delete/5
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
