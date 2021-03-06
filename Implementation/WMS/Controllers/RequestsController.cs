﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WMS.Models;

namespace WMS.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly WMSContext _context;

        public RequestsController(WMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if(!User.IsInRole("Manager") && !User.IsInRole("Employee"))
            {
                var import = await _context.ImportRequests.Where(r => r.Firm.UserName == User.Identity.Name).Include(r => r.StorageSpace).ToListAsync();
                var export = await _context.ExportRequests.Where(r => r.Firm.UserName == User.Identity.Name).Include(r => r.StorageSpace).ToListAsync();
                var requests = new List<Request>();
                requests.AddRange(import);
                requests.AddRange(export);
                return View(requests);
            }
            else
            {
                var import = await _context.ImportRequests.Include(r => r.StorageSpace).ToListAsync();
                var export = await _context.ExportRequests.Include(r => r.StorageSpace).ToListAsync();
                var requests = new List<Request>();
                requests.AddRange(import);
                requests.AddRange(export);
                return View(requests);
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.Include(r => r.StorageSpace).Include(r => r.Items).FirstOrDefaultAsync(m => m.Id == id);
            foreach (ItemCount itemCount in request.Items)
            {
                var something = await _context.ItemCounts.Include(tempItem => tempItem.Item).FirstOrDefaultAsync(m => m.Id == itemCount.Id);
                itemCount.Item = something.Item;
            }

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        public IActionResult CreateImport()
        {
            var storageSpaces = _context.StorageSpaces.Where(sp => sp.Firm.UserName == User.Identity.Name && sp.Available);
            ViewBag.FirmId = _context.Firms.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.StorageSpace = new SelectList(storageSpaces, "Id", "Name");
            return View();
        }

        public IActionResult CreateExport()
        {
            var storageSpaces = _context.StorageSpaces.Where(sp => sp.Firm.UserName == User.Identity.Name && sp.Available);
            ViewBag.FirmId = _context.Firms.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.StorageSpace = new SelectList(storageSpaces, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateImport(ImportRequest importRequest)
        {
            if (ModelState.IsValid)
            {
                var firm = _context.Firms.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                importRequest.Firm = firm;
                importRequest.RequestDate = DateTime.Now;
                _context.Add(importRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(importRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExport(ExportRequest exportRequest)
        {
            if (ModelState.IsValid)
            {
                var firm = _context.Firms.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                exportRequest.Firm = firm;
                exportRequest.RequestDate = DateTime.Now;
                _context.Add(exportRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exportRequest);
        }

        public async Task<IActionResult> Approve(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ImportRequest import = await _context.ImportRequests.Include(r => r.StorageSpace).Include(r => r.Items).FirstOrDefaultAsync(m => m.Id == id);
            ExportRequest export = await _context.ExportRequests.Include(r => r.StorageSpace).Include(r => r.Items).FirstOrDefaultAsync(m => m.Id == id);

            if(import != null)
            {
                return View(import);
            }
            else if(export != null)
            {
                return View(export);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveImport(string id, ImportRequest request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            try
            {
                request = _context.ImportRequests.Include(r => r.StorageSpace).Include(r => r.Items).Single(r => r.Id == id);
                request.StorageSpace = _context.StorageSpaces.Single(sp => sp.Id == request.StorageSpaceId);
                var itemCounts = new List<ItemCount>();

                var capacity = 0.0;
                for (int i = 0; i < request.Items.Count; i++)
                {
                    ItemCount itemCount = _context.ItemCounts.Include(ic => ic.Item).Single(ic => ic.Id == request.Items.ElementAt(i).Id);
                    capacity += itemCount.Item.Volume;
                    itemCounts.Add(itemCount);
                }

                bool requestValid = capacity > request.StorageSpace.Capacity * (1 - request.StorageSpace.UsedUp/100.0);
                if (requestValid)
                {
                    _context.Requests.Remove(request);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                request.Processed = true;
                for(int i = 0; i < itemCounts.Count; i++)
                {
                    ItemCount itemCount = itemCounts.ElementAt(i);
                    for (int j = 0; j < itemCount.Count; j++)
                    {
                        string itemID = itemCount.Item.UPC + "-" + DateTime.Now.Ticks + j;
                        Item item = new Item { Id = itemID, ItemDetails = itemCount.Item, StorageSpace = request.StorageSpace };
                        _context.Items.Add(item);
                    }
                }

                _context.Entry(request).Property("Processed").IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.Id))
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveExport(string id, ExportRequest request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            try
            {
                request = _context.ExportRequests.Include(r => r.StorageSpace).Include(r => r.Items).Single(r => r.Id == id);
                request.StorageSpace = _context.StorageSpaces.Single(sp => sp.Id == request.StorageSpaceId);
                var itemCounts = new List<ItemCount>();

                bool requestValid = true;
                for (int i = 0; i < request.Items.Count; i++)
                {
                    ItemCount itemCount = _context.ItemCounts.Include(ic => ic.Item).Single(ic => ic.Id == request.Items.ElementAt(i).Id);
                    var numOfItemsOfThisType = _context.Items.Where(item => item.ItemDetails.UPC == itemCount.Item.UPC && item.StorageSpace.Id == request.StorageSpaceId).ToList().Count;
                    //if (itemCount.Count > numOfItemsOfThisType)
                    //{
                    //    requestValid = false;
                    //    break;
                    //}
                    itemCounts.Add(itemCount);
                }
                
                //if (requestValid)
                //{
                //    _context.Requests.Remove(request);
                //    await _context.SaveChangesAsync();
                //    return RedirectToAction(nameof(Index));
                //}

                request.Processed = true;
                for (int i = 0; i < itemCounts.Count; i++)
                {
                    ItemCount itemCount = itemCounts.ElementAt(i);
                    var toRemove = _context.Items.Where(item => item.ItemDetails.UPC == itemCount.Item.UPC && item.StorageSpace.Id == request.StorageSpaceId).ToList();
                    for (int j = 0; j < itemCount.Count; j++)
                    {
                        _context.Items.Remove(toRemove.ElementAt(i));
                    }
                }

                _context.Entry(request).Property("Processed").IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.Id))
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

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var request = await _context.Requests.FindAsync(id);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(string id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
