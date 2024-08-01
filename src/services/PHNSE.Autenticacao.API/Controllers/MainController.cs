using Microsoft.AspNetCore.Mvc;

namespace PHNSE.Autenticacao.API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ICollection<string> Erros = new List<string>();

        protected IActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                {"Mensagens", Erros.ToArray() }
            }));
        }

        protected bool OperacaoValida() => !Erros.Any();

        protected void AdicionarErroProcessamento(string erro) => Erros.Add(erro);

        protected void LimparErrosProcessamento() => Erros.Clear();

    }
}
