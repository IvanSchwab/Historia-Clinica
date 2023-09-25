namespace Historia_Clinica.Models
{
    public class ErrorMsg
    {
        public const string MsgReq = "El campo {0} es requerido";
        public const string MsgReqAlfa = "El {0} solo admite caracteres alfabeticos";
        public const string MsgRangoString = "El campo {0} debe tener entre {2} y {1} caracteres";
        public const string MsgErrorDNI = "El {0} debe tener un formato 00000000";
        public const string MsgErrorTelefono = "El numero ingresado no tiene un formato valido";
        public const string MsgErrorMatricula = "La {0} debe tener un formato NN000000";
        public const string MsgReqNum = "El {0} solo admite caracteres numericos positivos";
        public const string MsgErrorRangoNumerico = "El valor ingresado excede el rango permitido";
        public const string MsgReqAlfaNum = "El {0} solo admite caracteres alfanumericos";
        public const string MsgErrorLegajo = "El {0} debe tener un formato 00000000";
        public const string MsgErrorMissmatch = "El campo {0} no coincide";
        public const string MsgContraseñaMin = "La contraseña debe tener como minimo 8 caracteres";
        public const string HistoriaClinicaIsNull = "La entidad, Historia Clinica es nula";
        public const string ErrorAlCargarRol = "Error al cargar Rol de ";

    }
}
