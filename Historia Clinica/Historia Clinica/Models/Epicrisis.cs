using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Historia_Clinica.Models
{
    public class Epicrisis
    {
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public int Id { get; set; }

        public Episodio Episodio { get; set; }

        public int EpisodioId { get; set; }

        public Medico Medico { get; set; }

        public int MedicoId { get; set; }

        [Display(Name = "Fecha y Hora")]
        [DataType(DataType.DateTime)]
        public DateTime fechaYHora { get; set; } = DateTime.Now;

    }
}
