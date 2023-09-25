using System.ComponentModel.DataAnnotations;

namespace Historia_Clinica.Models
{
    public class Evolucion
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("Nota")]
        public int NotaId { get; set; }
        public List <Nota> Notas { get; set; }

        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        [StringLength(200, MinimumLength = 3, ErrorMessage = ErrorMsg.MsgRangoString)]
        [Display(Name = "Descripcion")]
        public String DescripcionAtencion { get; set; }

        [Display(Name = "Fecha y Hora de Inicio")]
        [DataType(DataType.DateTime)]
        public DateTime FechaYHoraInicio { get; set; } = DateTime.Now;

        [Display(Name = "Fecha y Hora de Alta")]
        [DataType(DataType.DateTime)]
        public DateTime? FechayHoraAlta { get; set; }

        [Display(Name = "Fecha y Hora de Cierre")]
        [DataType(DataType.DateTime)]
        public DateTime? FechayHoraCierre { get; set; }

        [Display(Name = "Abierto")]
        public Boolean EstadoAbierto { get; set; } = true;

        [Display(Name = "Creado Por")]
        [Required]
        public int MedicoId { get; set; }

        public Medico Medico { get; set; }

        [Required]
        public int  EpisodioId { get; set; }

        public Episodio Episodio{ get; set; }

    }
}
