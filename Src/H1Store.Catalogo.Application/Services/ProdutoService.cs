using AutoMapper;
using H1Store.Catalogo.Application.Interfaces;
using H1Store.Catalogo.Application.ViewModels;
using H1Store.Catalogo.Domain.Entities;
using H1Store.Catalogo.Domain.Interfaces;
using H1Store.Catalogo.Infra.EmailService;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Application.Services
{
	public class ProdutoService : IProdutoService
	{
		#region - Construtores
		private readonly IProdutoRepository _produtoRepository;
		private IMapper _mapper;
        private readonly EmailConfig _emailConfig;

        public ProdutoService(IProdutoRepository produtoRepository, IMapper mapper, IOptions<EmailConfig> options)
		{
			_produtoRepository = produtoRepository;
			_mapper = mapper;
            _emailConfig = options.Value;
        }
        #endregion

        #region - Funções
        public void Adicionar(NovoProdutoViewModel novoProdutoViewModel)
        {
            var novoProduto = _mapper.Map<Produto>(novoProdutoViewModel);
            _produtoRepository.Adicionar(novoProduto);

        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterPorNome(string nomeProduto)
        {
            if (string.IsNullOrWhiteSpace(nomeProduto))
            {
                return Enumerable.Empty<ProdutoViewModel>();
            }

            var produtos = await _produtoRepository.ObterPorNome(nomeProduto);

            var produtosViewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);

            return produtosViewModel;
        }

        public async Task Atualizar(NovoProdutoViewModel novoProdutoViewModel)
        {
            var produto = _mapper.Map<Produto>(novoProdutoViewModel);
            await _produtoRepository.Atualizar(produto);
        }

        public IEnumerable<ProdutoViewModel> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(_produtoRepository.ObterTodos());
        }

        public async Task<ProdutoViewModel> ObterProdutoCodigo(Guid id)
        {
            var produto = await _produtoRepository.ObterProdutoCodigo(id);
            return _mapper.Map<ProdutoViewModel>(produto);
        }





        public async Task AlterarPreco(Guid id, decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentException("O valor negativo ou igual a zero.");
            }

            var EncontrarProduto = await _produtoRepository.ObterProdutoCodigo(id);

            if (EncontrarProduto == null)
            {
                throw new ApplicationException("Este produto não existe.");
            }

            EncontrarProduto.AlterarPreco(valor);

            await _produtoRepository.AlterarPreco(EncontrarProduto, valor);
        }

        public async Task AtualizarEstoque(Guid id, int quantidade)
        {
            var encontradoProduto = await _produtoRepository.ObterProdutoCodigo(id);

            if (encontradoProduto == null)
            {
                throw new ApplicationException("Não é possível alterar o estoque de um produto que não existe!");
            }

            if (quantidade < 0)
            {
                if (!encontradoProduto.PossuiEstoque(-quantidade))
                {
                    throw new ArgumentException("Estoque insuficiente para debitar!");
                }

                encontradoProduto.DebitarEstoque(-quantidade);
            }
            else if (quantidade > 0)
            {
                encontradoProduto.ReporEstoque(quantidade);
            }
            else
            {
                throw new ArgumentException("A quantidade não pode ser zero.");
            }

            int estoqueMinimo = encontradoProduto.EstoqueMinimo;

            if (encontradoProduto.QuantidadeEstoque <= -1)
            {
                throw new InvalidOperationException("O estoque não pode menor que zero.");
            }
            else if (encontradoProduto.QuantidadeEstoque < estoqueMinimo)
            {
                string assunto = "Estoque Baixo";
                string corpoEmailModelo = $"Olá Comprador(a), o produto {encontradoProduto.Descricao} está abaixo do estoque mínimo {encontradoProduto.EstoqueMinimo} definido no sistema. Por favor, verifique o estoque.";
                string emailDestino = "ericsantos497@gmail.com";

                if (!string.IsNullOrEmpty(emailDestino))
                {
                    Email.Enviar(assunto, corpoEmailModelo, emailDestino, _emailConfig);
                }
            }
            await _produtoRepository.AtualizarEstoque(encontradoProduto, quantidade);
        }



        public async Task Ativar(Guid id)
        {
            var EncontrarProduto = await _produtoRepository.ObterProdutoCodigo(id);

            if (EncontrarProduto == null) throw new ApplicationException("Não é possível desativar um produto que não existe.");

            EncontrarProduto.Ativar();

            await _produtoRepository.Ativar(EncontrarProduto);
        }
        public async Task Desativar(Guid id)
        {
            var EncontrarProduto = await _produtoRepository.ObterProdutoCodigo(id);

            if (EncontrarProduto == null) throw new ApplicationException("Não é possível desativar um produto que não existe.");

            EncontrarProduto.Desativar();

            await _produtoRepository.Desativar(EncontrarProduto);

        }

        public async Task Remover(Guid id)
        {
            await _produtoRepository.Remover(id);
        }

        #endregion
    }
}
