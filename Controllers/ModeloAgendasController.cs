using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agenda.Context;
using Agenda.Models;

namespace Agenda.Controllers
{
    public class ModeloAgendasController : Controller
    {
        private readonly AppDbContext _context;

        public ModeloAgendasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ModeloAgendas
        public async Task<IActionResult> Index(string data)
        {
            // Define a data padrão (hoje) se não for informada
            var dataFiltro = DateTime.Today;

            if (!string.IsNullOrEmpty(data) && DateTime.TryParse(data, out var dataParse))
            {
                dataFiltro = dataParse.Date;
            }

            var registros = await _context.agenda
                .Where(a => a.Data.Date == dataFiltro)
                .OrderBy(a => a.Data)
                .AsNoTracking()
                .ToListAsync();

            // Detecção de dispositivo móvel
            var userAgent = Request.Headers["User-Agent"].ToString();
            bool isMobile = userAgent.Contains("Mobi") ||
                           userAgent.Contains("Android") ||
                           userAgent.Contains("iPhone") ||
                           userAgent.Contains("Windows Phone");

            return isMobile ? View("IndexCel", registros) : View(registros);
        }

        // GET: ModeloAgendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeloAgenda = await _context.agenda
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modeloAgenda == null)
            {
                return NotFound();
            }

            return View(modeloAgenda);
        }


    }
}
