using Microsoft.AspNetCore.Mvc;
using PHNSE.Autenticacao.API.Models;
using PHNSE.Autenticacao.API.Services.Interfaces;

namespace PHNSE.Autenticacao.API.Controllers
{
    [ApiController]
    [Route("api/autenticacao")]
    public class AutenticacaoController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AutenticacaoController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("novo-usuario")]
        public async Task<IActionResult> RegistrarNovoUsuario([FromBody] UsuarioRegistroViewModel usuario)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var resposta = await _autenticacaoService.RegistrarNovoUsuarioAsync(usuario);

            if (resposta.Sucesso)
            {
                return CustomResponse(await _autenticacaoService.GerarTokenJwt(usuario.Email));
            }

            return CustomResponse(resposta);
        }

        [HttpPost("login")]
        public async Task<IActionResult> EfetuarLoginUsuario([FromBody] UsuarioLoginViewModel usuario)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var resposta = await _autenticacaoService.EfetuarLoginUsuarioAsync(usuario);

            if (resposta.Sucesso)
            {
                return CustomResponse(await _autenticacaoService.GerarTokenJwt(usuario.Email));
            }

            return CustomResponse(resposta);
        }
    }
}
