using Microsoft.AspNetCore.Identity;
using PHNSE.Autenticacao.API.Models;
using PHNSE.Autenticacao.API.Services.Interfaces;

namespace PHNSE.Autenticacao.API.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public AutenticacaoService(
            UserManager<IdentityUser> userManager
,           SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> EfetuarLoginUsuarioAsync(UsuarioLoginViewModel usuario)
        {
            var resut = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Senha, false, true);

            return resut.Succeeded;
        }

        public async Task<bool> RegistrarNovoUsuarioAsync(UsuarioRegistroViewModel usuario)
        {
            var identityUser = new IdentityUser
            {
                UserName = usuario.Email,
                Email = usuario.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, usuario.Senha);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(identityUser, false);
            }

            return result.Succeeded;
        }
    }
}
