using Historia_Clinica.Helpers;
using Historia_Clinica.Models;
using System.ComponentModel.DataAnnotations;

namespace Historia_Clinica.ViewModels
{
    public class RegistroPaciente 
    {
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = Alias.CorreoElectronico)]
        public String Email{ get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [RegularExpression(@"^([\+]?[1-9][0-9]{1,3}[-]?|[0])?[1-9][0-9]{1,2}[1-9][0-9]{7}$", ErrorMessage = ErrorMsg.MsgErrorTelefono)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8,ErrorMessage = ErrorMsg.MsgContraseñaMin)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.ConfirmPassword)]
        [Compare("Password", ErrorMessage = ErrorMsg.MsgErrorMissmatch)]
        public string ConfirmacionPassword { get; set; } 

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1\u00f1\u00d1 áéíóúu]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        public string Calle { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range(1, 999999, ErrorMessage = ErrorMsg.MsgErrorRangoNumerico)]
        [RegularExpression(@"[0-9]*", ErrorMessage = ErrorMsg.MsgReqNum)]
        public int Altura { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1\u00f1\u00d1 áéíóú]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range(1, 99999999, ErrorMessage = ErrorMsg.MsgErrorDNI)]
        public string DNI { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú 0-9]*", ErrorMessage = ErrorMsg.MsgReqAlfaNum)]
        public string Localidad { get; set; }
    }
}
