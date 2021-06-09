using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureStaticWebAppServices.Models;
using Newtonsoft.Json;

namespace AzureStaticWebAppServices.Controllers
{
    public class FormPostedsController : Controller
    {
        private readonly AzureStaticWebAppServicesContext _context;

        public FormPostedsController(AzureStaticWebAppServicesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Clean()
        {
            foreach ( var f in _context.FormPosteds.ToList ())
            {
                _context.FormPosteds.Remove(f);

            }

            _context.SaveChanges();

            var azureStaticWebAppServicesContext = _context.FormPosteds.Include(f => f.Client);

            return RedirectToAction("index");

        }



        // GET: FormPosteds
        public async Task<IActionResult> Index(string ClientID = null  )
        {
            ViewBag.clients = _context.Clients.Select(x => x.ClientUrl).ToArray();


            var lst = new List<SelectListItem>();
            foreach (var itm in _context.Clients)
                if (itm.ClientUrl == ClientID)
                    lst.Add(new SelectListItem(itm.ClientUrl, itm.ClientUrl, true));
                else
                    lst.Add(new SelectListItem(itm.ClientUrl, itm.ClientUrl));

            ViewBag.clientsList = lst;


            var azureStaticWebAppServicesContext = _context.FormPosteds.Include(f => f.Client);
            if (ClientID != null )
                azureStaticWebAppServicesContext = _context.FormPosteds.Where(x => x.Client.ClientUrl.Contains(ClientID)).Include(f => f.Client); ;

            var xx = await azureStaticWebAppServicesContext.ToListAsync() ;
            return View(await azureStaticWebAppServicesContext.ToListAsync());
        }

        // GET: FormPosteds/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formPosted = await _context.FormPosteds
                .Include(f => f.Client)
                .FirstOrDefaultAsync(m => m.FormPostedId == id);
            if (formPosted == null)
            {
                return NotFound();
            }


            var dict = new Dictionary<string, string>();

            try
            {
                dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(formPosted.FormData);

            }
            catch (Exception)
            {

                //throw;
            }

            ViewBag.dict = dict;
            return View(formPosted);
        }

        // GET: FormPosteds/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientUrl");
            return View();
        }

        // POST: FormPosteds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FormPostedId,ClientId,FormData,PostedTime")] FormPosted formPosted)
        {
            if (ModelState.IsValid)
            {
                _context.Add(formPosted);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientUrl", formPosted.ClientId);
            return View(formPosted);
        }

        // GET: FormPosteds/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formPosted = await _context.FormPosteds.FindAsync(id);
            if (formPosted == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientUrl", formPosted.ClientId);
            return View(formPosted);
        }

        // POST: FormPosteds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FormPostedId,ClientId,FormData,PostedTime")] FormPosted formPosted)
        {
            if (id != formPosted.FormPostedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(formPosted);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormPostedExists(formPosted.FormPostedId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientUrl", formPosted.ClientId);
            return View(formPosted);
        }

        // GET: FormPosteds/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formPosted = await _context.FormPosteds
                .Include(f => f.Client)
                .FirstOrDefaultAsync(m => m.FormPostedId == id);
            if (formPosted == null)
            {
                return NotFound();
            }

            return View(formPosted);
        }

        // POST: FormPosteds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var formPosted = await _context.FormPosteds.FindAsync(id);
            _context.FormPosteds.Remove(formPosted);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormPostedExists(long id)
        {
            return _context.FormPosteds.Any(e => e.FormPostedId == id);
        }
    }
}
