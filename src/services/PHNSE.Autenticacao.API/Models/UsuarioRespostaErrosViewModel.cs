namespace PHNSE.Autenticacao.API.Models
{
    public class UsuarioRespostaErrosViewModel
    {
        public UsuarioRespostaErrosViewModel()
        {
            
        }

        public UsuarioRespostaErrosViewModel(IList<string> erros)
        {
            AdicionaErros(erros);
        }

        private List<string> _erros = new List<string>();

        public bool Sucesso => !Erros.Any();

        public IReadOnlyCollection<string> Erros => _erros;

        public void AdicionaErro(string erro) => _erros.Add(erro);

        public void AdicionaErros(IList<string> erros) => _erros.AddRange(erros);
    }
}
