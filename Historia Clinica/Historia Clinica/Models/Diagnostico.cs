using System.ComponentModel.DataAnnotations;

namespace Historia_Clinica.Models
{
    public class Diagnostico
    {
      
        public int Id { get; set; }

        public Epicrisis Epicrisis { get; set; }

        public int EpicrisisId { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = ErrorMsg.MsgRangoString)]
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public string Descripcion { get; set; }

        [StringLength(500, ErrorMessage = ErrorMsg.MsgRangoString)]
        public string Recomendacion { get; set; }


    }
}
