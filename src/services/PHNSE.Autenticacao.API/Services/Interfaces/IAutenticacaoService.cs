using PHNSE.Autenticacao.API.Models;

namespace PHNSE.Autenticacao.API.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<UsuarioRespostaErrosViewModel> RegistrarNovoUsuarioAsync(UsuarioRegistroViewModel usuario);

        Task<UsuarioRespostaErrosViewModel> EfetuarLoginUsuarioAsync(UsuarioLoginViewModel usuario);

        Task<UsuarioRespostaLoginViewModel> GerarTokenJwt(string email);
    }
}
