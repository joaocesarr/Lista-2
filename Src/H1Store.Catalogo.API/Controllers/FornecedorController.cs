using H1Store.Catalogo.Application.Interfaces;
using H1Store.Catalogo.Application.Services;
using H1Store.Catalogo.Application.ViewModels;
using H1Store.Catalogo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace H1Store.Catalogo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FornecedorController : ControllerBase
    {
        private readonly IFornecedorService _fornecedorService;

        public FornecedorController(IFornecedorService fornecedorService)
        {
            _fornecedorService = fornecedorService;
        }

        [HttpGet(Name = "ObterTodosFornecedores")]
        public IActionResult Get()
        {
            return Ok(_fornecedorService.ObterTodos());
        }

        [HttpGet("Fornecedor/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var fornecedor = await _fornecedorService.ObterPorId(id);
            return Ok(fornecedor);
        }

        [HttpGet("Fornecedor/BuscarPorNome/{nome}")]
        public async Task<IActionResult> ObterPorNome(string nome)
        {
            var fornecedor = await _fornecedorService.ObterPorNome(nome);

            if (fornecedor.Any())
            {
                return Ok(fornecedor);
            }
            else
            {
                return NotFound("Nenhum fornecedor encontrado com o valor digitado.");
            }
        }

        [HttpPost]
        public IActionResult Post(NovoFornecedorViewModel novoFornecedorViewModel)
        {
            _fornecedorService.Adicionar(novoFornecedorViewModel);

            return Ok("Registro adicionado");
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, NovoFornecedorViewModel novoFornecedorViewModel)
        {
            novoFornecedorViewModel.CodigoID = id;
            _fornecedorService.Atualizar(novoFornecedorViewModel);

            return Ok("Registro atualizado");
        }


        [HttpPut]
        [Route("Reativar/{id}")]
        public async Task<IActionResult> Reativar(Guid id)
        {
            await _fornecedorService.Reativar(id);
            return Ok("Fornecedor ativado");
        }

        [HttpPut]
        [Route("Fornecedor/Desativar/{id}")]
        public async Task<IActionResult> Desativa(Guid id)
        {
            await _fornecedorService.Desativar(id);
            return Ok("Fornecedor desativado");
        }

    }
}
