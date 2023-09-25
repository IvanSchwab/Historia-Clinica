using Historia_Clinica.Helpers;
using Historia_Clinica.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Historia_Clinica.ViewModels
{
    public class Representacion
    {
        public IFormFile Imagen { get; set; } //Permite representar el envio de archivo en un request, desde el cliente al servidor. 
    }
}
    