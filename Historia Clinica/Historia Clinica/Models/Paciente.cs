using Historia_Clinica.Helpers;
using Historia_Clinica.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Historia_Clinica.Models
{
    public class Paciente : Persona
    {
 
        [StringLength(20, MinimumLength = 2, ErrorMessage = ErrorMsg.MsgRangoString)]
        [RegularExpression(@"[a-zA-Z\u00f1\u00d1 áéíóú]*", ErrorMessage = ErrorMsg.MsgReqAlfa)]
        
        public List<Episodio> Episodios { get; set; }

        [Display(Name="Obra Social")]
        [Required(ErrorMessage = ErrorMsg.MsgReq)]
        public  TipoObraSocial TipoObraSocial { get; set; }

    }
}
  