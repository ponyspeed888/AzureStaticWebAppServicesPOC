using AzureStaticWebAppServices.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureStaticWebAppServices.Controllers
{
    public class SimpleFormPostController : Controller
    {

        private readonly AzureStaticWebAppServicesContext _context;

        public SimpleFormPostController(AzureStaticWebAppServicesContext context)
        {
            _context = context;
        }



        [HttpGet]
        [HttpPost]

        public ActionResult GenerateHTML(string FormFields = null, string ClientID = null  )
        {

            //var clients = _context.Clients.ToList();
            //var clientslist = "";
            //foreach ( var c in clients)
            //{
            //    clientslist += c.ClientUrl + "\r\n"  ;
            //    //clientslist += HttpUtility.HtmlEncode (c.ClientUrl + "<br/>")  ;

            //}

            //ViewBag.clientslist = clientslist;


      


            var result = "";
            if (this.HttpContext.Request.Method.ToLower () == "post")
            {
                GenerateHTML gen = new GenerateHTML();
                var fullUrl = this.Url.Action("ReceivePost", nameof (SimpleFormPostController), null, HttpContext.Request.Scheme);

                if (ClientID == null)
                {
                    return Content($"ClientID not specified");
                }



                Client client = _context.Clients.Where(x => x.ClientId == int.Parse ( ClientID) ).FirstOrDefault();
                if (client == null)
                    return Content($"ClientID {ClientID} is not registered");



                gen.ClientID = client.ClientId.ToString () ;
                gen.url = fullUrl.Replace("Controller", "");
                gen.FormFields = FormFields.Split(";");

                result = gen.TransformText();
            }

            ViewBag.result = result;
            return View();
        }


      


        [HttpPost]
        //[Route("ReceivePost")]
        [EnableCors(Startup.MYPOLICY)]

        public IActionResult ReceivePost(IFormCollection formFields)
        {

            var result = "";

            try
            {

                var href = formFields["window.location.href"];
                if ( href.Count == 0  )
                {
                    return Content( $"href not specified" );
                }



                var cid = formFields["ClientID"];
                if (cid.Count == 0)
                {
                    return Content($"ClientID not specified");
                }

                Uri myUri = new Uri( href.ToString ()  );
                string host = myUri.Host;  // host is "www.contoso.com"


                //string host = HttpContext.Request.Host.Value;

                string json = JsonConvert.SerializeObject(formFields);

                FormPosted f = new FormPosted();
                Client client = _context.Clients.Where(x => x.ClientUrl.Contains(host) && x.ClientId == int.Parse (cid) ).FirstOrDefault();
                if (client == null)
                    return Content($"Host {host} with ClientID {cid} is not registered");

                f.FormData = json;

                f.ClientId = client.ClientId;
                _context.FormPosteds.Add(f);
                _context.SaveChanges();

                //foreach (var f in formFields)
                //{

                //    Microsoft.Extensions.Primitives.StringValues y = f.Value;

                //    var str = $"From {host} : {f.Key} : {f.Value.ToString() }";
                //    Debug.WriteLine(str);
                //    result += str + "\r\n";



                //}

                result = $"From {host} : {json}";

            }
            catch (Exception exp )
            {

                return Content(exp.Message);

            }

            return Content(result);

    
        }




        // GET: SimpleFormPostController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SimpleFormPostController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SimpleFormPostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SimpleFormPostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SimpleFormPostController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SimpleFormPostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SimpleFormPostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SimpleFormPostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
