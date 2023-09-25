using System.ComponentModel.DataAnnotations;

namespace Historia_Clinica.Models
{
    public class Direccion

    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1\u00f1\u00d1 áéíóúu]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        public string Calle { get; set; }

        [Range(1,999999,ErrorMessage = ErrorMsg.MsgErrorRangoNumerico)]
        [RegularExpression(@"[0-9]*", ErrorMessage = ErrorMsg.MsgReqNum)]
        public int Altura { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú 0-9]*", ErrorMessage = ErrorMsg.MsgReqAlfaNum)]
        public string Localidad {get;set;}

        public string DireccionCompleta
        {
            get { return string.Format("{0} - {1} // {2}", Calle, Altura, Localidad); }
        }

    }
}