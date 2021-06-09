using AzureStaticWebAppServices.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ActionResult GenerateHTML(string FormFields = null, string ClientUrl = null  )
        {


            var lst = new List<SelectListItem>();
            var urls = _context.Clients.Select(x => x.ClientUrl).ToList();
            foreach (var itm in urls)
                    lst.Add(new SelectListItem(itm, itm ) );



            ViewBag.clientUrlList = lst;



            var result = "";
            if (this.HttpContext.Request.Method.ToLower () == "post")
            {
                var fullUrl = this.Url.Action("ReceivePost", nameof (SimpleFormPostController), null, HttpContext.Request.Scheme);

                if (ClientUrl == null)
                {
                    return Content($"ClientUrl not specified");
                }



                Client client = _context.Clients.Where(x => x.ClientUrl == ClientUrl).FirstOrDefault();
                if (client == null)
                    return Content($"ClientUrl '{ClientUrl}' is not registered");

                if ( String.IsNullOrEmpty ( FormFields) )
                {

                    SiteAccessed genSiteAccess = new SiteAccessed();

                    genSiteAccess.url = fullUrl.Replace("Controller", "");

                    result = genSiteAccess.TransformText();

                }
                else
                {

                    GenerateHTML gen = new GenerateHTML();

                    gen.url = fullUrl.Replace("Controller", "");
                    gen.FormFields = FormFields.Split(";");
                    result = gen.TransformText();



                }

            }

            ViewBag.result = result;
            return View();
        }


      


        [HttpPost]
        //[Route("ReceivePost")]
        [EnableCors(Startup.MYPOLICY)]

        public IActionResult ReceivePost(IFormCollection formFields)
        {


            foreach (var r in Request.Headers)
            {
                Debug.WriteLine($"{r.Key} {r.Value}");

            }

            string origin = Request.Headers["origin"];
            string referer = Request.Headers["referer"];

  

            var result = "";

            try
            {

  

                FormPosted f = new FormPosted();
                Client client = _context.Clients.Where(x => x.ClientUrl.StartsWith(origin) ).FirstOrDefault();
                if (client == null)
                    return Content($"Host {origin} is not registered");



                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (var ff in formFields)
                    dict.Add(ff.Key, ff.Value.ToString());

                string json = JsonConvert.SerializeObject(dict);


                f.FormData = json;

                f.ClientId = client.ClientId;
                _context.FormPosteds.Add(f);
                _context.SaveChanges();

                 result = $"From {origin} : {json}";

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
