using System.ComponentModel.DataAnnotations;

namespace Historia_Clinica.Models
{
    public class Empleado : Persona
    {
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Range(00000001,99999999, ErrorMessage = ErrorMsg.MsgErrorLegajo)]
        [RegularExpression(@"[0-9]*")]
        public int Legajo { get; set; }
    }
}
