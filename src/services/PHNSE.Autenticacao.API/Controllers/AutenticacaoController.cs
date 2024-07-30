using Microsoft.AspNetCore.Mvc;
using PHNSE.Autenticacao.API.Models;
using PHNSE.Autenticacao.API.Services.Interfaces;

namespace PHNSE.Autenticacao.API.Controllers
{
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AutenticacaoController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("novo-usuario")]
        public async Task<IActionResult> RegistrarNovoUsuario(UsuarioRegistroViewModel usuario)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (await _autenticacaoService.RegistrarNovoUsuarioAsync(usuario))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> EfetuarLoginUsuario(UsuarioLoginViewModel usuario)
        {
            if (!ModelState.IsValid) return BadRequest();

            if(await _autenticacaoService.EfetuarLoginUsuarioAsync(usuario))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
