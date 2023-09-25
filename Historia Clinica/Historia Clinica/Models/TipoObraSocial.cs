using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Historia_Clinica.Models
{
    public enum TipoObraSocial
    {
        [Display(Name = "SIN OBRA SOCIAL")]
        SIN_OBRA_SOCIAL,
        [Display(Name = "ANDAR")]
        OBRA_SOCIAL_DE_VIAJANTES_VENDEDORES_DE_LA_REPUBLICA_ARGENTINA,
        [Display(Name = "JERARQUICOS")]
        OBRA_SOCIAL_DE_TRABAJADORES_SOCIOS_DE_LA_ASOCIACION_MUTUAL_DEL_PERSONAL_JERARQUICO_DE_BANCOS_OFICIALES_NACIONALES_JERARQUICOS_SALUD,
        [Display(Name = "OSALARA")]
        OBRA_SOCIAL_DE_AGENTES_DE_LOTERIAS_Y_AFINES_DE_LA_REPUBLICA_ARGENTINA,
        [Display(Name = "OSDE")]
        OBRA_SOCIAL_DE_LOS_LEGISLADORES_DE_LA_REPUBLICA_ARGENTINA,
        [Display(Name = "OSDEPYM")]
        OBRA_SOCIAL_DE_EMPRESARIOS_PROFESIONALES_Y_MONOTRIBUTISTAS,
        [Display(Name = "OSFE")]
        OBRA_SOCIAL_FERROVIARIA,
        [Display(Name = "OSPACA")]
        OBRA_SOCIAL_DEL_PERSONAL_DEL_AUTOMOVIL_CLUB_ARGENTINO,
        [Display(Name = "OSPE")]
        OBRA_SOCIAL_DE_PETROLEROS,
        [Display(Name = "OSSACRA")]
        OBRA_SOCIAL_DE_LA_ASOCIACION_CIVIL_PROSINDICATO_DE_AMAS_DE_CASA_DE_LA_REPUBLICA_ARGENTINA,
        [Display(Name = "OSSEG")]
        OBRA_SOCIAL_DEL_SINDICATO_DE_TRABAJADORES_DE_SEGUROS,
        [Display(Name = "UNION PERSONAL")]
        OBRA_SOCIAL_UNION_PERSONAL_DE_LA_UNION_DEL_PERSONAL_CIVIL_DE_LA_NACION,
    }
}
