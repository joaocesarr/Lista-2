using AutoMapper;
using H1Store.Catalogo.Application.Interfaces;
using H1Store.Catalogo.Application.ViewModels;
using H1Store.Catalogo.Domain.Entities;
using H1Store.Catalogo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Application.Services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _Mapper;

        public FornecedorService(IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _Mapper = mapper;
        }

        public async Task Adicionar(NovoFornecedorViewModel novoFornecedorViewModel)
        {
            var novoFornecedor = _Mapper.Map<Fornecedor>(novoFornecedorViewModel);
            await _fornecedorRepository.Adicionar(novoFornecedor);
        }

        public async Task Atualizar(NovoFornecedorViewModel novoFornecedorViewModel)
        {
            var fornecedor = _Mapper.Map<Fornecedor>(novoFornecedorViewModel);
            await _fornecedorRepository.Atualizar(fornecedor);
        }

        public async Task Desativar(Guid id)
        {
            var fornecedorBusca = await _fornecedorRepository.ObterPorId(id);

            if (fornecedorBusca == null) throw new ApplicationException("Não é possível desativar um produto que não existe.");

            fornecedorBusca.Desativar();

            await _fornecedorRepository.Desativar(fornecedorBusca);
        }

        public async Task<FornecedorViewModel> ObterPorId(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterPorId(id);
            return _Mapper.Map<FornecedorViewModel>(fornecedor);
        }

        public async Task<IEnumerable<FornecedorViewModel>> ObterPorNome(string nomeFornecedor)
        {
            if (string.IsNullOrWhiteSpace(nomeFornecedor))
            {
                return Enumerable.Empty<FornecedorViewModel>();
            }

            var fornecedor = await _fornecedorRepository.ObterPorNome(nomeFornecedor);

            var fornecedorViewModel = _Mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedor);

            return fornecedorViewModel;
        }

        public IEnumerable<FornecedorViewModel> ObterTodos()
        {
            return _Mapper.Map<IEnumerable<FornecedorViewModel>>(_fornecedorRepository.ObterTodos());
        }

        public async Task Reativar(Guid id)
        {
            var fornecedorBusca = await _fornecedorRepository.ObterPorId(id);

            if (fornecedorBusca == null) throw new ApplicationException("Não é possível reativar um fornecedor que não existe.");

            fornecedorBusca.Ativar();

            await _fornecedorRepository.Reativar(fornecedorBusca);
        }

    }
}

