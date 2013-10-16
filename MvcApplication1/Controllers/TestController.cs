using MvcApplication1.Models;
using Oak.Controllers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /Test/

        public ActionResult ShowTasks()
        {
            string tokenJson = (string)Application["TokenJson"];

            Auth auth = null;
            var jsonSerializer = new DataContractJsonSerializer(typeof(Auth));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(tokenJson)))
            {

                auth = (Auth)jsonSerializer.ReadObject(ms); ;
            }
            string token = auth.access_token;
            var client = new RestClient("https://www.producteev.com/api/tasks/search?sort=updated_at&order=desc");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("Authorization", string.Format( "Bearer {0}", token ), ParameterType.HttpHeader );
            request.AddParameter("Content-Type", "application/json");
            request.AddBody("{}");
            IRestResponse response = client.Execute(request);
            ViewBag.ClientResponse = response;

            return View();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Authenticate(string clientId, string clientSecret)
        {
            Application["ClientId"] = clientId;
            Application["ClientSecret"] = clientSecret;
            return new RedirectResult(
                string.Format("https://www.producteev.com/oauth/v2/auth?client_id={0}&response_type=code&redirect_uri={1}", clientId, Url.Encode( "http://localhost:3032/Test/Token" ) )
            );
        }
        public ActionResult Token(string code)
        {
            string clientId = (string)Application["ClientId"];
            string clientSecret = (string)Application["ClientSecret"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                string.Format("https://www.producteev.com/oauth/v2/token?client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri={2}&code={3}",
                clientId, clientSecret, Url.Encode("http://localhost:3032/Test/Token"), code
                )
            );
            using (WebResponse response = request.GetResponse())
            {
                // Do something with response
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    string content = sr.ReadToEnd();
                    Application["TokenJson"] = content;
                }
            }

            return View("Index");
        }

        //
        // GET: /Test/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Test/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Test/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Test/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Test/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Test/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Test/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
