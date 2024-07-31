using Microsoft.AspNetCore.Mvc;
using PHNSE.Autenticacao.API.Models;
using PHNSE.Autenticacao.API.Services.Interfaces;

namespace PHNSE.Autenticacao.API.Controllers
{
    [ApiController]
    [Route("api/autenticacao")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AutenticacaoController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("novo-usuario")]
        public async Task<IActionResult> RegistrarNovoUsuario([FromBody] UsuarioRegistroViewModel usuario)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (await _autenticacaoService.RegistrarNovoUsuarioAsync(usuario))
            {
                return Ok(await _autenticacaoService.GerarTokenJwt(usuario.Email));
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> EfetuarLoginUsuario([FromBody] UsuarioLoginViewModel usuario)
        {
            if (!ModelState.IsValid) return BadRequest();

            if(await _autenticacaoService.EfetuarLoginUsuarioAsync(usuario))
            {
                return Ok(await _autenticacaoService.GerarTokenJwt(usuario.Email));
            }

            return BadRequest();
        }
    }
}
