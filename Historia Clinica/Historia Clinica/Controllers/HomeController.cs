using Historia_Clinica.Data;
using Historia_Clinica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace Historia_Clinica.Controllers
{
    
    [AllowAnonymous]
    public class HomeController : Controller
    {
        #region Properties & Constructor
        private readonly ILogger<HomeController> _logger;
        private readonly HistoriaClinicaContext _context;

        public HomeController(ILogger<HomeController> logger, HistoriaClinicaContext context)
        {
            _logger = logger;
            this._context = context;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            TestCollections();//AL llamar al Index invoco al metodo TestCollection.
            return View();
        }
        #endregion

        #region Methods
        [HttpPost]
        private void TestCollections()
        {
            try
            {
                Console.Clear();
            }
            catch
            {

            }
            _logger.Log(LogLevel.Information, $"Declaracion y asignacion Generco");
            var personasGen = _context.Personas;

            _logger.Log(LogLevel.Information, $"Declaracion y asignacion IEnu");
            // Basico, enumera a traves de una lista de objetos o intems. Ejecucion diferida, no va a buscar a BD, salvo iteracion forzada. 
            IEnumerable<Persona> personasIE = _context.Personas;

            _logger.Log(LogLevel.Information, $"Declaracion y asignacion Icol");
            //Implementa IEnumerable y agrega caracteristicas para agregar/remover items de la lista de objetos o items.
            ICollection<Persona> personasIC = _context.Personas.ToList();

            _logger.Log(LogLevel.Information, $"Declaracion y asignacion Icol");
            //Implementa ICollection y adicionalmente posee caracteristicas para manejo de indices para agregar, remover, etc. 
            IList<Persona> personasIL = _context.Personas.ToList();

            foreach(Persona persona in personasIC)
            {
                _logger.Log(LogLevel.Information, $"Icollection - {persona.Fullname}");
            }
            var algoIE = personasIE.Where(p => p.Nombre.Contains("E")).OrderBy(p => p.Apellido);
            var algoIC = personasIC.OrderBy(p => p.Nombre);
            var algoIL = personasIL.OrderBy(p => p.Apellido).ToList();
        }
        #endregion

        #region Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}