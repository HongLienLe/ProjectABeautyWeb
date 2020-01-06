using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectABeautyWeb.Data;
using ProjectABeautyWeb.Models;

namespace ProjectABeautyWeb.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationContext _context;

        public ContactController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Contact
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enquiries.ToListAsync());
        }

        // GET: Contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _context.Enquiries
                .FirstOrDefaultAsync(m => m.EnquiryId == id);
            if (enquiry == null)
            {
                return NotFound();
            }

            return View(enquiry);
        }

        // GET: Contact/Create
        public IActionResult Create()
        {
            Enquiry emptyEnquiry = new Enquiry();
            return View(emptyEnquiry);
        }

        // POST: Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnquiryId,Name,Email,ContactNumber,Note")] Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                _context.Enquiries.Add(enquiry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enquiry);
        }

        // GET: Contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _context.Enquiries.FindAsync(id);
            if (enquiry == null)
            {
                return NotFound();
            }
            return View(enquiry);
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnquiryId,Name,Email,ContactNumber,Note")] Enquiry enquiry)
        {
            if (id != enquiry.EnquiryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enquiry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnquiryExists(enquiry.EnquiryId))
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
            return View(enquiry);
        }

        // GET: Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _context.Enquiries
                .FirstOrDefaultAsync(m => m.EnquiryId == id);
            if (enquiry == null)
            {
                return NotFound();
            }

            return View(enquiry);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enquiry = await _context.Enquiries.FindAsync(id);
            _context.Enquiries.Remove(enquiry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnquiryExists(int id)
        {
            return _context.Enquiries.Any(e => e.EnquiryId == id);
        }
    }
}
