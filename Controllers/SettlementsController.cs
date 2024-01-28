using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Settlements.Data;
using Settlements.Models;
using X.PagedList;

namespace Settlements.Controllers
{
    public static class Extensions
    {
        public static int findIndex<T>(this T[] array, T item)
        {
            return Array.IndexOf(array, item);
        }
    }

    public class SettlementsController : Controller
    {
        private static char[] hebrewsSymbols = { '\'', 'ק', 'ר', 'א', 'ט', 'ו', 'ן', 'ם', 'פ', 'ש', 'ד', 'ג', 'כ', 'ע', 'י', 'ח', 'ל', 'ך', 'ף', 'ז', 'ס', 'ב', 'ה', 'נ', 'מ', 'צ', 'ת', 'ץ' };
        private static char[] englishSymbols = { 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';', 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.' };
        private const int PAGE_SIZE = 5;
        private readonly SettlementsContext _context;

        public SettlementsController(SettlementsContext context)
        {
            _context = context;
        }

        // GET: Settlements
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (_context.SettlementModel == null)
            {
                return Problem("Entity set 'SettlementsContext.SettlementModel'  is null.");
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var settlements = from m in _context.SettlementModel select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                char[] phraseAsChars = searchString.ToCharArray();
                char[] searchChars = searchString.ToCharArray();
                for (int i = 0; i < phraseAsChars.Length; i++)
                {
                    int symbolIndex = englishSymbols.findIndex(phraseAsChars[i]);
                    if (symbolIndex != -1)
                    {
                        searchChars[i] = hebrewsSymbols[symbolIndex];
                    }
                }
                string updatedSearchString = new string(searchChars);
                settlements = settlements.Where(s => s.Name!.Contains(updatedSearchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    settlements = settlements.OrderByDescending(s => s.Name);
                    break;
                default:
                    settlements = settlements.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = PAGE_SIZE;
            int pageNumber = (page ?? 1);
            var onePageOfSettlements = settlements.ToPagedList(pageNumber, pageSize);
            ViewBag.onePageOfSettlements = onePageOfSettlements;

            return View(onePageOfSettlements);
        }

        // GET: Settlements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settlementModel = await _context.SettlementModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settlementModel == null)
            {
                return NotFound();
            }

            return View(settlementModel);
        }

        // GET: Settlements/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SettlementModel settlementModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(settlementModel);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    ViewData["errorMsg"] = e.InnerException.Message;
                    return View();
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(settlementModel);
        }

        // GET: Settlements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settlementModel = await _context.SettlementModel.FindAsync(id);
            if (settlementModel == null)
            {
                return NotFound();
            }
            return View(settlementModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SettlementModel settlementModel)
        {
            if (id != settlementModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(settlementModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettlementModelExists(settlementModel.Id))
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
            return View(settlementModel);
        }

        // GET: Settlements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settlementModel = await _context.SettlementModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settlementModel == null)
            {
                return NotFound();
            }

            return View(settlementModel);
        }

        // POST: Settlements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var settlementModel = await _context.SettlementModel.FindAsync(id);
            if (settlementModel != null)
            {
                _context.SettlementModel.Remove(settlementModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SettlementModelExists(int id)
        {
            return _context.SettlementModel.Any(e => e.Id == id);
        }
    }
}
