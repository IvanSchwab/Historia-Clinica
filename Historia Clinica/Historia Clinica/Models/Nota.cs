using Historia_Clinica.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Historia_Clinica.Models
{
    public class Nota
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(200, MinimumLength = 3, ErrorMessage = ErrorMsg.MsgRangoString)]
        public string Mensaje { get; set; }


        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [Display(Name= "Fecha de Creacion")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaYHora { get; set; } = DateTime.Now;


        public Empleado Empleado { get; set; }

        public int EvolucionId { get; set; }
       
        public Evolucion Evolucion { get; set; }


        [Display(Name = $"{Config.EmpleadoRolName}/{Config.MedicoRolName}")]
        public int EmpleadoId { get; set; } 

    }
}
