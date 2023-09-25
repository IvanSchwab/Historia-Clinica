using Historia_Clinica.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Historia_Clinica.Models
{
    public class Persona : IdentityUser<int>
    {
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range (1000000, 99999999, ErrorMessage = ErrorMsg.MsgErrorDNI)]
        public string DNI { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [RegularExpression(@"^([\+]?[1-9][0-9]{1,3}[-]?|[0])?[1-9][0-9]{1,2}[1-9][0-9]{7}$", ErrorMessage = ErrorMsg.MsgErrorTelefono)]
        public string Telefono { get; set; }
        
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [DataType(DataType.EmailAddress)]
        public override String Email { 
            get { return base.Email; }
            set { base.Email = value; }
        }
        public int  DireccionId { get; set; }
                
        public Direccion Direccion { get; set; }

        [Display(Name="Fecha Alta")]
        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set; } = DateTime.Today;

        [Display(Name = "Nombre de Usuario")]
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(10, MinimumLength = 5, ErrorMessage = ErrorMsg.MsgRangoString)]
        public override string UserName {
            get { return base.UserName; }
            set { base.UserName = value; }
        }
        public string Fullname
        {
            get { return string.Format("{0} {1}", Nombre, Apellido); }
        }
        public string Foto { get; set; } = Config.FotoDef;
    }
}
