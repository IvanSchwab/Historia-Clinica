using Historia_Clinica.Helpers;
using Historia_Clinica.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Historia_Clinica.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = Alias.CorreoElectronico)]
        public String Email { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }
        public bool Recordarme { get; set; }

    }
}
