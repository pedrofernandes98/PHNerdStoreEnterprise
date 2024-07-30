using PHNSE.Autenticacao.API.Models;

namespace PHNSE.Autenticacao.API.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<bool> RegistrarNovoUsuarioAsync(UsuarioRegistroViewModel usuario);

        Task<bool> EfetuarLoginUsuarioAsync(UsuarioLoginViewModel usuario);
    }
}
