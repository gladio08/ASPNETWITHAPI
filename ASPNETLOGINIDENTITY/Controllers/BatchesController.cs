using API1.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace ASPNETLOGINIDENTITY.Controllers
{
    public class BatchesController : Controller
    {
        // GET: Batches
        //public ActionResult Index()
        //{
        //    return View();
        //}
        readonly HttpClient client = new HttpClient();
        public BatchesController()
        {
            client.BaseAddress = new Uri("https://brmapi.azurewebsites.net/API/"); //Link API to Client
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public ActionResult Index()
        {
            return View(List());
        }
        public JsonResult List()
        {
            IEnumerable<BatchesVM> batches = null;
            var responseTask = client.GetAsync("Batches");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<BatchesVM>>();
                readTask.Wait();
                batches = readTask.Result;
            }
            else
            {
                batches = Enumerable.Empty<BatchesVM>();
                ModelState.AddModelError(String.Empty, "404 Not Found");
            }
            return Json(new { data = batches }, JsonRequestBehavior.AllowGet);

        }
    }
}