using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Historia_Clinica.Data;
using Historia_Clinica.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Historia_Clinica.Helpers;

namespace Historia_Clinica.Controllers
{
    [Authorize(Roles = $"{Config.PacienteRolname}, {Config.MedicoRolName}, {Config.AdminRolName}")]
    public class PersonasController : Controller
    {
        private readonly HistoriaClinicaContext _context;
        private readonly UserManager<Persona> _userManager;

        public PersonasController(HistoriaClinicaContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var personas = await GetPersonasListAsync();
            return View(personas);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await GetPersonaByIdAsync(id.Value);

            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nombre,Apellido,DNI,Telefono,Email,DireccionId,FechaAlta,Password,UserName")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                AddPersona(persona);
                return RedirectToAction(nameof(Index));
            }

            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", persona.DireccionId);
            return View(persona);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = GetPersonaById(id.Value);
            if (persona == null)
            {
                return NotFound();
            }

            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", persona.DireccionId);
            return View(persona);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nombre,Apellido,DNI,Telefono,Email,DireccionId,FechaAlta,Password,UserName")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var personaEnDB = GetPersonaById(persona.Id);
                    if (personaEnDB != null)
                    {
                        UpdatePersona(personaEnDB, persona);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Id))
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

            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", persona.DireccionId);
            return View(persona);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = GetPersonaWithDireccion(id.Value);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var persona = GetPersonaById(id);
            if (persona != null)
            {
                DeletePersona(persona);
            }

            return RedirectToAction(nameof(Index));
        }

        #region Private Methods

        private async Task<List<Persona>> GetPersonasListAsync()
        {
            return await _context.Personas.Include(p => p.Direccion).ToListAsync();
        }

        private async Task<Persona> GetPersonaByIdAsync(int id)
        {
            return await _context.Personas.Include(p => p.Direccion).FirstOrDefaultAsync(m => m.Id == id);
        }

        private void AddPersona(Persona persona)
        {
            _context.Personas.Add(persona);
            _context.SaveChanges();
        }

        private Persona GetPersonaById(int id)
        {
            return _context.Personas.Find(id);
        }

        private void UpdatePersona(Persona personaEnDB, Persona persona)
        {
            personaEnDB.Nombre = persona.Nombre;
            personaEnDB.Apellido = persona.Apellido;

            _context.Personas.Update(personaEnDB);
            _context.SaveChanges();
        }

        private bool PersonaExists(int id)
        {
            return _context.Personas.Any(e => e.Id == id);
        }

        private Persona GetPersonaWithDireccion(int id)
        {
            return _context.Personas.Include(p => p.Direccion).FirstOrDefault(m => m.Id == id);
        }

        private void DeletePersona(Persona persona)
        {
            _context.Personas.Remove(persona);
            _context.SaveChanges();
        }

        #endregion
    }
}
