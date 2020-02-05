using ASPNETLOGINIDENTITY.viewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using API1.Models;
using Newtonsoft.Json;

namespace ASPNETLOGINIDENTITY.Controllers
{
    public class RolesController : Controller
    {
        //SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        //// GET: Roles
        //public ActionResult Index()
        //{
        //    return View(GetDataRoles());
        //}

        //public async Task<ActionResult> GetDataRoles() //Mengambil Data dalam Table Roles
        //{
        //    var result = await sqlConnection.QueryAsync<RoleVM>("EXEC SP_Retrieve_Role");
        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<ActionResult> Create(RoleVM roleVM)
        //{
        //    var affectedRows = await sqlConnection.ExecuteAsync("EXEC SP_Insert_Role @name", new { Name = roleVM.Name});
        //    return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<ActionResult> Details(int id)
        //{
        //    var result = await sqlConnection.QueryAsync<RoleVM>("EXEC SP_Retrieve_Role_By_Id @id", new {Id = id });
        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<ActionResult> Edit(RoleVM roleVM)
        //{
        //    var affectedRows = await sqlConnection.ExecuteAsync("EXEC SP_Update_Role @id, @name", new { Name = roleVM.Name, Id = roleVM.Id });
        //    return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<ActionResult> Delete(int id)
        //{
        //    var affectedRows = await sqlConnection.ExecuteAsync("EXEC SP_Delete_Role @id", new { Id = id });
        //    return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        //}
        readonly HttpClient client = new HttpClient();
        public RolesController()
        {
            /*client.BaseAddress = new Uri("https://brmapi.azurewebsites.net/API/Batches");*/ //Link API to Client
            client.BaseAddress = new Uri("http://localhost:53903/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public ActionResult Index()
        {
            return View(List());
        }

        public JsonResult List()
        {
            IEnumerable<Roles> role = null;
            var responseTask = client.GetAsync("Roles");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Roles>>();
                readTask.Wait();
                role = readTask.Result;
            }
            else
            {
                role = Enumerable.Empty<Roles>();
                ModelState.AddModelError(String.Empty, "404 Not Found");
            }
            return Json(new { data = role }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult Create(Roles role)
        //{
        //    var postTask = client.PostAsJsonAsync<Roles>("Roles", role);
        //    postTask.Wait();

        //    var result = postTask.Result;

        //    if (result.IsSuccessStatusCode)
        //    {
        //        return Json("Index");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "404 Not Found");
        //    }
        //    return Json(role);
        //}

        public async Task<JsonResult> Create(Roles role)
        {
            var myContent = JsonConvert.SerializeObject(role);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var affectedRow = client.PostAsync("Roles", byteContent).Result;
            return Json(new { data = affectedRow }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(int id, Roles role)
        {
            var myContent = JsonConvert.SerializeObject(role);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var roles = client.PutAsync("Roles/" + id, byteContent).Result;
            return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Details(int id)
        {
            var cek = client.GetAsync("Roles/" + id.ToString()).Result;
            var read = cek.Content.ReadAsAsync<Roles>().Result;
            //Roles role = null;
            //var responseTask = client.GetAsync("Roles/" + id);
            //responseTask.Wait();

            //var result = responseTask.Result;
            //if (result.IsSuccessStatusCode)
            //{
            //    var readTask = result.Content.ReadAsAsync<Roles>();
            //    readTask.Wait();
            //    role = readTask.Result;
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "404 Not Found");
            //}
            return Json(new { data = read }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(Roles role, int id)
        {
            var affectedRow = client.DeleteAsync("Roles/" + id).ToString();
            return Json(new { data = affectedRow }, JsonRequestBehavior.AllowGet);
        }
    }
}