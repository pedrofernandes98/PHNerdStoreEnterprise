namespace PHNSE.Autenticacao.API.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public double ExpiracaoEmHoras { get; set; }

        public string Emissor { get; set; }

        public string ValidoEm { get; set; }
    }
}
