namespace Historia_Clinica.Helpers
{
    public static class Varios
    {
        public static string GetUrlPath(string root, string nombre, string nombreDefecto)
        {
            string resu = string.Concat(root, string.IsNullOrEmpty(nombre) ? nombreDefecto : nombre);
            return resu;
        }
    }
}
