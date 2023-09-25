using Historia_Clinica.Helpers;
using Historia_Clinica.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Historia_Clinica.ViewModels
{
    public class RegistroMedico
    {
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1]{2}[0-9]{6}", ErrorMessage = ErrorMsg.MsgErrorMatricula)]
        public string Matricula { get; set; }

        public int DireccionId { get; set; }

        [Display(Name = "Especialidad")]
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public TipoEspecialidadMedica Tipo { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        public string Apellido { get; set; }


        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range(1, 99999999, ErrorMessage = ErrorMsg.MsgErrorDNI)]
        public string DNI { get; set; }


        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [RegularExpression(@"^([\+]?[1-9][0-9]{1,3}[-]?|[0])?[1-9][0-9]{1,2}[1-9][0-9]{7}$", ErrorMessage = ErrorMsg.MsgErrorTelefono)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = Alias.CorreoElectronico)]
        public String Email { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        public string Calle { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range(1, 999999, ErrorMessage = ErrorMsg.MsgErrorRangoNumerico)]
        [RegularExpression(@"[0-9]*", ErrorMessage = ErrorMsg.MsgReqNum)]
        public int Altura { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú 0-9]*", ErrorMessage = ErrorMsg.MsgReqAlfaNum)]
        public string Localidad { get; set; }

    }
}

