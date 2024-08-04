using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PHNSE.Autenticacao.API.Models;

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

        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);

            //Variação utilizando linq - > erros.ToList().ForEach(erro => AdicionarErroProcessamento(erro.ErrorMessage));

            foreach(var erro in erros)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected IActionResult CustomResponse(UsuarioRespostaErrosViewModel resposta)
        {
            foreach (var erro in resposta.Erros)
                AdicionarErroProcessamento(erro);

            return CustomResponse();
        }

        protected bool OperacaoValida() => !Erros.Any();

        protected void AdicionarErroProcessamento(string erro) => Erros.Add(erro);

        protected void LimparErrosProcessamento() => Erros.Clear();

    }
}
