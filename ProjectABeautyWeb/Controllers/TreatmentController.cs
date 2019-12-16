using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectABeautyWeb.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectABeautyWeb.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly ApplicationContext _context;

        public TreatmentController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            return View(await _context.Treatments.ToListAsync());
        }
    }
}
