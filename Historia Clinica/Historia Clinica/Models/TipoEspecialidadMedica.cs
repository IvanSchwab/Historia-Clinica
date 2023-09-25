using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Historia_Clinica.Models
{
    public enum TipoEspecialidadMedica //74 especialidades medicas. 70 segun Reso. 1814/2015 Min. Salud y 4 segun Reso. 1337/2011 Min. Salud, (CO.FE.SA, Republica Argentina).
    {
        [Display(Name = "Anatomia Patologica")]
        ANATOMIA_PATOLOGICA,
        [Display(Name = "Alergia e Inmunologia Pediatrica")]
        ALERGIA_E_INMUNOLOGIA_PEDIATRICA,
        [Display(Name = "Clinica Medica")]
        CLINICA_MEDICA,
        [Display(Name = "Cardiologia")]
        CARDIOLOGIA,
        [Display(Name = "Dermatologia")]
        DERMATOLOGIA,
        [Display(Name = "Endocrinilogia")]
        ENDOCRINOLOGIA,
        [Display(Name = "Hetatologia")]
        HEPATOLOGIA,
        [Display(Name = "Infectologia")]
        INFECTOLOGIA,
        [Display(Name = "Medicina del Deporte")]
        MEDICINA_DEL_DEPORTE,
        [Display(Name = "Nutricion")]
        NUTRICION,
        [Display(Name = "Cirugua General")]
        CIRUGIA_GENERAL,
        [Display(Name = "Ginecologia")]
        GINECOLOGIA,
        [Display(Name = "Oftalmologia")]
        OFTALMOLOGIA,
        [Display(Name = "Ortopedia y Traumatologia")]
        ORTOPEDIA_Y_TRAUMATOLOGIA,
        
       
       
    }
}
