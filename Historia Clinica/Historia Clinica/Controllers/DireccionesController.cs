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

namespace Historia_Clinica.Controllers
{
    [Authorize]
    public class DireccionesController : Controller
    {
        #region Properties & Constructor
        private readonly HistoriaClinicaContext _context;

        public DireccionesController(HistoriaClinicaContext context)
        {
            _context = context;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
              return View(_context.Direcciones.ToList());
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Direcciones == null)
            {
                return NotFound();
            }
            else
            {
            var direccion = _context.Direcciones.FirstOrDefault(m => m.Id == id);
            if (direccion == null)
            {
                return NotFound();
            }
            return View(direccion);
            }
        }
        #endregion

        #region Create
        [HttpGet]        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Calle,Altura,Localidad")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(direccion);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(direccion);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Direcciones == null)
            {
                return NotFound();
            }
            var direccion = _context.Direcciones.Find(id);
            if (direccion == null)
            {
                return NotFound();
            }
            return View(direccion);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Calle,Altura,Localidad")] Direccion direccion)
        {
            if (id != direccion.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                    {
                    var direccionEnDB = _context.Direcciones.Find(id);
                    if (direccionEnDB != null)
                        {
                        direccionEnDB.Altura = direccion.Altura;
                        direccionEnDB.Calle = direccion.Calle;
                        direccionEnDB.Localidad = direccion.Localidad;

                        _context.Update(direccionEnDB);
                        _context.SaveChanges();
                        }
                    else
                        {
                        return NotFound();
                        }       
                    }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DireccionExists(direccion.Id))
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
            return View(direccion);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Direcciones == null)
            {
                return NotFound();
            }
            var direccion = _context.Direcciones
                .FirstOrDefault(m => m.Id == id);
            if (direccion == null)
            {
                return NotFound();
            }
            return View(direccion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Direcciones == null)
            {
                return Problem(ErrorMsg.HistoriaClinicaIsNull);
            }
            var direccion = _context.Direcciones.Find(id);
            if (direccion != null)
            {
                _context.Direcciones.Remove(direccion);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Methods
        private bool DireccionExists(int id)
        {
          return _context.Direcciones.Any(e => e.Id == id);
        }
        #endregion

    }
}
