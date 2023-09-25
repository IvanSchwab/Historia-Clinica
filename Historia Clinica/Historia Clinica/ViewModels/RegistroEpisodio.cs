using Historia_Clinica.Helpers;
using Historia_Clinica.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Historia_Clinica.ViewModels


{
    public class RegistroEpisodio
    {
        [StringLength(500, ErrorMessage = ErrorMsg.MsgRangoString)]
        public string Descripcion { get; set; }

        [StringLength(100, ErrorMessage = ErrorMsg.MsgRangoString)]
        public string Motivo { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaYHoraInicio { get; set; } 

        [DataType(DataType.DateTime)]
        public DateTime FechayHoraAlta { get; set; } 
        //Carga manualmente el Medico

        [DataType(DataType.DateTime)]
        public DateTime FechayHoraCierre { get; set; } 
        //Fecha de cierre del episodio (solo cierra el medico)

        public Boolean EstadoAbierto { get; set; }

        public int EmpleadoRegistraId { get; set; } 
        //Corresponde al empleado que registra el Episodio
    }
}
