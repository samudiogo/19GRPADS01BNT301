using AssessmentDomain.Common;
using AssessmentDomain.DomainService;
using AssessmentDomain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AssessmentWeb.Controllers
{

    public interface IControllerBase
    {
        Task<string> Get(string url);
        Task<string> Post(string url, string dados);
        Task<string> Put(string url, string dados);
        Task<string> DeleteApi(string url);
        Task<IEnumerable<T>> ListBy<T>(string url);
    }

    public abstract class ControllerBase : Controller, IControllerBase
    {
        protected string ApiBasePath { get; set; }
        private readonly IPhotoService _fileService;

        public static readonly JsonSerializerSettings Configuracoes = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = { new IsoDateTimeConverter
                {
                    DateTimeStyles = DateTimeStyles.AssumeUniversal
                }
            }
        };

        protected ControllerBase(IPhotoService fileService)
        {
            _fileService = fileService;
            ViewBag.ShowCreateButton = false;
        }

        protected JsonResult ContentResult<T>(T result = null) where T : class
        {

            if (result == null) return Json(new { StatusCode = HttpStatusCode.NotFound }, JsonRequestBehavior.AllowGet);

            return Json(new { StatusCode = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(result, Configuracoes) }, JsonRequestBehavior.AllowGet);

        }

        public async Task<string> Get(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBasePath);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"{ApiBasePath}/{url}");

                return await response.Content.ReadAsStringAsync();

            }
        }

        public async Task<string> Post(string url, string dados)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBasePath);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpContent = new StringContent(dados, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{ApiBasePath}/{url}", httpContent);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> Put(string url, string dados)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBasePath);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpContent = new StringContent(dados, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{ApiBasePath}/{url}", httpContent);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> DeleteApi(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBasePath);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync($"{ApiBasePath}/{url}");

                return await response.Content.ReadAsStringAsync();

            }
        }

        public async Task<IEnumerable<T>> ListBy<T>(string url)
        {
            var responseResult = string.Empty;
            var modelListResult = new List<T>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBasePath);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"{ApiBasePath}/{url}");

                responseResult = await response.Content.ReadAsStringAsync();

            }

            if (!string.IsNullOrEmpty(responseResult) && responseResult != "[]")
                modelListResult = JsonConvert.DeserializeObject<List<T>>(responseResult);

            return modelListResult;
        }

        public async Task<string> UploadPhotoAsync(HttpPostedFileBase binaryFile, ContainerName containerName = ContainerName.General)
        {
            if (binaryFile == null)
                return string.Empty;

            var photo = new Photo
            {
                ContainerName = containerName.GetDescription(),
                FileName = binaryFile.FileName.GetRandomBlobName(),
                BinaryContent = binaryFile.InputStream,
                ContentType = binaryFile.ContentType
            };

            return await _fileService.CreateAsync(photo);

        }


    }
}