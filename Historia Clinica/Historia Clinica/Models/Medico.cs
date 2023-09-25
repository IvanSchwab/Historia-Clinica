using System.ComponentModel.DataAnnotations;

namespace Historia_Clinica.Models
{
    public class Medico : Empleado
    {
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1]{2}[0-9]{6}", ErrorMessage = ErrorMsg.MsgErrorMatricula)]
        public string Matricula { get; set; }


        [Display(Name ="Especialidad")]
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public TipoEspecialidadMedica Tipo { get; set; }


    }
}
