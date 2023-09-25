using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;

using Historia_Clinica.Data;
using Historia_Clinica.Helpers;
using Historia_Clinica.Models;
using Historia_Clinica.ViewModels;

namespace Historia_Clinica.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region Properties
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly HistoriaClinicaContext _context;
        private readonly RoleManager<Rol> _roleManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        #endregion

        #region Constructor
        public AccountController(
            UserManager<Persona> usermanager,
            SignInManager<Persona> signInManager,
            HistoriaClinicaContext context,
            RoleManager<Rol> roleManager,
            IWebHostEnvironment hostingEnvironment
            ) 
        {
            this._usermanager = usermanager;
            this._signInManager = signInManager;
            this._context = context;
            this._roleManager = roleManager;
            this._hostingEnvironment = hostingEnvironment;
        }
        #endregion

        #region Registrar


        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([Bind("Email,Password,ConfirmacionPassword,Calle,Altura,Localidad,Nombre,Apellido,DNI,Telefono")]
        RegistroPaciente viewModel)
        {
            if (ModelState.IsValid)
            {
                Direccion direccion = new Direccion() {
                    Calle = viewModel.Calle,
                    Altura = viewModel.Altura,
                    Localidad = viewModel.Localidad
                };

                _context.Direcciones.Add(direccion); 
                _context.SaveChanges();
                //El Method Add/Update siempre debe estar acompañado de un SavaChanges,
                //de lo contrario no tendrá relevancia en nuestra base de datos.
                
                Paciente pacienteACrear = new Paciente() {
                    Nombre = viewModel.Nombre,
                    Apellido = viewModel.Apellido,
                    DNI = viewModel.DNI,
                    Email = viewModel.Email,
                    Telefono = viewModel.Telefono,
                    UserName = viewModel.Email, 
                    DireccionId = direccion.Id
                };

                var resultadoCreate = await _usermanager.CreateAsync(pacienteACrear, viewModel.Password); //Devuelve un IdentityResult. Tmb sobrecargamos con el pass que recibe para que haga el hasheo y lo guarde el mismo create. 

                if (resultadoCreate.Succeeded)
                {

                    var resultadoAddRole = await _usermanager.AddToRoleAsync(pacienteACrear, Config.PacienteRolname);
                    if (resultadoAddRole.Succeeded)
                    {
                        resultadoAddRole = await _usermanager.AddToRoleAsync(pacienteACrear, Config.PacienteRolname);
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, $"Error al cargar Rol de {Config.PacienteRolname}");
                    }

                    await _signInManager.SignInAsync(pacienteACrear, isPersistent: false);
                    //isPersistent = Indica si la cookie debe persistir una vez el navegador se haya cerrado.
                    return RedirectToAction("Details", "Pacientes", new { id = pacienteACrear.Id });
                }
                else { 
                foreach (var error in resultadoCreate.Errors)
                {
                    //Generamos una lista de errores.
                    ModelState.AddModelError(String.Empty, error.Description);
                        //AddModelError de base recibe 2 parametros, string key y string errorMsg
                        //como no queremos relacionarlo con ninguna key dejamos String.Empty.
                    }
                }
            }
            return View(viewModel);
        }
        #endregion

        #region Iniciar Sesion
        [HttpGet]
        public IActionResult IniciarSesion(string returnUrl)
        { 
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login ViewModel)
        {
            // Gracias a los diccionarios de TempData, podemos utilizar su cookie
            // y asi no perder la informacion después del Return.
            string ReturnUrl = TempData["ReturnUrl"] as string;

            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(ViewModel.Email, ViewModel.Password, ViewModel.Recordarme, false);
                // PasswordSignInAsync recibe 4 parametros: username, password, isPersistent y lockoutOnFailure
                // Username pasamos el email, password pasamos la del viewmodel, isPersistent lo delegamos a 
                // check-label del viewmodel y lockoutOnFailure lo dejamos en false.

                if (resultado.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Usuario o contraseña inválidos");
                }
            }
            return View(ViewModel);
        }
        #endregion Cerrar Sesion

        #region Cerrar Sesion
        [Authorize]
        public IActionResult CerrarSesion()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Listados
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult ListarRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult ListarMedicos()
        {
            IEnumerable<Medico> medicos = _context.Medico;
            return View(medicos);
        }
        #endregion

        #region Methods
        public IActionResult AccesoDenegado(string returnUrl) 
        {
            //Sobre opciones.AccessDeniedPath = "/Account/AccesoDenegado"; -Startup-.
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public IActionResult TestCurrentUser()
        {

            if (_signInManager.IsSignedIn(User)) 
            {
                string nombreUsuario = User.Identity.Name;
                Persona persona = _context.Personas.FirstOrDefault(p => p.NormalizedUserName == nombreUsuario.ToUpper());
                //Trae a la variable el objeto persona.
                int personaId = Int32.Parse(_usermanager.GetUserId(User)); 
                //GetUserId espera un ClaimPrincipal. Al ser los claims string necesito convertirlo a int con el parse.  
                int personaId2 = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
                //Evito usar el "Claims.First" por si se modifico el orden de los mismos. Devuelve string. 
                var PersonaId3 = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                //Si me paro en NameIdentifier, veremos que hace lo mismo. 
            }
            return null;
        }
        #endregion

        #region Subir Foto
        [HttpGet]
        [Authorize]
        public IActionResult SubirFoto()
        {
            return View(new Representacion());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubirFoto(Representacion modelo)
        {
            var Persona = await _usermanager.FindByNameAsync(User.Identity.Name);
            string rootPath = _hostingEnvironment.WebRootPath;
            string fotoPath = "img\\fotos";
            string userName = User.Identity.Name;

            if (ModelState.IsValid)
            {
                string nombreArchivoUnico = null;
                if (modelo.Imagen != null && Persona != null)
                {
                    //Si tengo todo lo necesario, avanzo.
                    if (!string.IsNullOrEmpty(rootPath) && !string.IsNullOrEmpty(fotoPath) && modelo.Imagen != null)
                    {
                        try
                        {
                            string carpetaDestino = Path.Combine(rootPath, fotoPath);

                            //Verifico si era para un usuario o por sistema.
                            nombreArchivoUnico = Guid.NewGuid().ToString() + 
                                (!string.IsNullOrEmpty(userName) ? "_" + userName : "_" + "Sistema") + "_" + modelo.Imagen.FileName;

                            string rutaCompletaArchivo = Path.Combine(carpetaDestino, nombreArchivoUnico);
                            modelo.Imagen.CopyTo(new FileStream(rutaCompletaArchivo, FileMode.Create));
                            Persona.Foto = nombreArchivoUnico;
                            
                            if (!string.IsNullOrEmpty(Persona.Foto))
                            {
                                _context.Personas.Update(Persona);
                                _context.SaveChanges();
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Error en el proecso de carga");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error datos insuficientes");
                    }
                }
            }
            return View(modelo);
        }
        #endregion

        #region Eliminar foto
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EliminarFoto(int? id)
        {
            Persona persona;
            if (id == null)
            {
                return NotFound();
            }
            if(id != 0){
             persona = _context.Personas.Find(id.Value);
            }
            else
            {
             persona = await _usermanager.FindByNameAsync(@User.Identity.Name);
            }

            if(persona != null)
            {
                if(persona.Foto != null)
                {
                    string nuevoNombre = Config.FotoDef;
                    persona.Foto = nuevoNombre;
                    _context.Personas.Update(persona);
                    _context.SaveChanges();
                }
            }        
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
