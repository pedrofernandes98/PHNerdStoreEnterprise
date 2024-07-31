using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PHNSE.Autenticacao.API.Extensions;
using PHNSE.Autenticacao.API.Models;
using PHNSE.Autenticacao.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PHNSE.Autenticacao.API.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;

        public AutenticacaoService(
            UserManager<IdentityUser> userManager
,           SignInManager<IdentityUser> signInManager,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
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

        public async Task<UsuarioRespostaLoginViewModel> GerarTokenJwt(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            var claimsUsuario = await _userManager.GetClaimsAsync(usuario);
            var rolesUsuario = await _userManager.GetRolesAsync(usuario);

            var identityClaims = ObterIdentityClaimsUsuario(claimsUsuario, rolesUsuario, usuario);
            var encodedToken = CriarCodificarTokenJwt(identityClaims);

            return ObterRespostaToken(encodedToken, identityClaims, usuario);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private ClaimsIdentity ObterIdentityClaimsUsuario(IList<Claim>? claimsUsuario, IList<string> rolesUsuario, IdentityUser usuario)
        {
            claimsUsuario.Add(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id));
            claimsUsuario.Add(new Claim(JwtRegisteredClaimNames.Email, usuario.Email));
            claimsUsuario.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            claimsUsuario.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claimsUsuario.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var role in rolesUsuario)
                claimsUsuario.Add(new Claim("role", role));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claimsUsuario);

            return identityClaims;
        }

        private string CriarCodificarTokenJwt(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoEmHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        private UsuarioRespostaLoginViewModel ObterRespostaToken(string encodedToken, ClaimsIdentity identityClaims, IdentityUser usuario)
        {
            return new UsuarioRespostaLoginViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoEmHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Claims = identityClaims.Claims.Select(claim => new UsuarioClaim { Type = claim.Type, Value = claim.Value })
                }
            };
        }

    }
}
