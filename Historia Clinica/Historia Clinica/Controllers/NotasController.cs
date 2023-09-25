using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Historia_Clinica.Data;
using Historia_Clinica.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Historia_Clinica.Helpers;

namespace Historia_Clinica.Controllers
{
    [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.MedicoRolName}, {Config.AdminRolName}")]
    public class NotasController : Controller
    {
        #region Properties & Constructor
        private readonly HistoriaClinicaContext _context;

        public NotasController(HistoriaClinicaContext context)
        {
            _context = context;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index(int? evolucionId)
        {
            if(evolucionId != null)
            {
                var historiaClinicaContext = _context.Notas.Include(n => n.Empleado).Where(e => e.EvolucionId.Equals(evolucionId)).ToList();
                return View(historiaClinicaContext.ToList());
            }
            else
            {
                var historiaClinicaContext = _context.Notas.Include(n => n.Empleado);
                return View(historiaClinicaContext.ToList());
            }
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Notas == null)
            {
                return NotFound();
            }
            var nota = _context.Notas
                .Include(n => n.Empleado)
                .FirstOrDefault(m => m.Id == id);
            if (nota == null)
            {
                return NotFound();
            }
            return View(nota);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create(int? evolucionId)
        {
            if (evolucionId == null)
            {
                return NotFound();
            }

            Nota nota = new Nota();
            nota.EvolucionId = evolucionId.Value;
            nota.EmpleadoId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Fullname");
            return View(nota);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Mensaje,EmpleadoId,EvolucionId")] Nota nota)
        {
            if (ModelState.IsValid)
            {
                _context.Notas.Add(nota);
                _context.SaveChanges();
                return RedirectToAction("Index", "Notas", new { EvolucionId = nota.EvolucionId });

            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Fullname", nota.EmpleadoId);
            return View(nota);
        }
        #endregion

        #region Edit
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Notas == null)
            {
                return NotFound();
            }

            var nota = _context.Notas.Find(id);
            if (nota == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Fullname", nota.EmpleadoId);
            return View(nota);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult Edit(int id, [Bind("Id,Mensaje,FechaYHora,EmpleadoId")] Nota nota)
        {
            if (id != nota.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var notaEnDB = _context.Notas.Find(nota.Id);
                    if (notaEnDB != null)
                    {
                        notaEnDB.FechaYHora = nota.FechaYHora;
                        notaEnDB.Mensaje = nota.Mensaje;
                        notaEnDB.EmpleadoId = nota.EmpleadoId;



                        _context.Notas.Update(notaEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaExists(nota.Id))
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
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Fullname", nota.EmpleadoId);
            return View(nota);
        }
        #endregion

        #region Delete
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Notas == null)
            {
                return NotFound();
            }
            var nota = _context.Notas
                .Include(n => n.Empleado)
                .FirstOrDefault(m => m.Id == id);
            if (nota == null)
            {
                return NotFound();
            }
            return View(nota);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Notas == null)
            {
                return Problem(ErrorMsg.HistoriaClinicaIsNull);
            }
            var nota = _context.Notas.Find(id);
            if (nota != null)
            {
                _context.Notas.Remove(nota);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Methods
        private bool NotaExists(int id)
        {
          return _context.Notas.Any(e => e.Id == id);
        }
        #endregion
    }
}
