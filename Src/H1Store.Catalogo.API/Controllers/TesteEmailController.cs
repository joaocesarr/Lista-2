using H1Store.Catalogo.Infra.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace H1Store.Catalogo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesteEmailController : ControllerBase
    {
        private readonly EmailConfig _emailConfig;

        public TesteEmailController(IOptions<EmailConfig> options)
        {
            _emailConfig = options.Value;
        }

        [HttpPost]
        [Route("EnviarEmail")]
        public async Task<IActionResult> Post()
        {
            string assunto = "Estoque Esgotado";
            string corpoEmailModelo = "Olá Comprador(a) , o produto {descricaoProduto} está abaixo do estoque mínimo{estoqueMinimo} definido no sistema, logo, você precisa avaliar se é necessário realizar um novo pedido de compra.";
            string nome = "Eric";
            string emailDestino = "joaocdcj@gmail.com";

            string corpoEmail = corpoEmailModelo.Replace("{nomeCliente}", nome);

            if (!string.IsNullOrEmpty(emailDestino))
                Email.Enviar(assunto, corpoEmail, emailDestino, _emailConfig);

            return Ok("Emails Enviado Com Sucesso!");

        }
    }
}
