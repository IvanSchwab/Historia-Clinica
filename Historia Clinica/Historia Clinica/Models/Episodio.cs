using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Historia_Clinica.Models
{
    public class Episodio
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Paciente")]
        [Required]
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(200, MinimumLength = 3, ErrorMessage = ErrorMsg.MsgRangoString)]
        public string Motivo { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(200, MinimumLength = 3, ErrorMessage = ErrorMsg.MsgRangoString)]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha Inicio")]
        [DataType(DataType.DateTime)]
        public DateTime FechaYHoraInicio { get; set; } = DateTime.Now;

        [Display(Name = "Fecha Alta")]
        [DataType(DataType.DateTime)]
        public DateTime? FechayHoraAlta { get; set; }

        [Display(Name = "Fecha Cierre")]
        [DataType(DataType.DateTime)]
        public DateTime? FechayHoraCierre { get; set; }

        [Display(Name = "Activo")]
        public Boolean EstadoAbierto { get; set; }

        public List<Evolucion> Evoluciones { get; set; }

        public Epicrisis Epicrisis { get; set; }

        [Display(Name = "Creado Por")]
        public int EmpleadoRegistraId { get; set; }

        [Display(Name = "Creado Por")]
        public Empleado EmpleadoRegistra { get; set; }
    }
}
