using H1Store.Catalogo.Application.ViewModels;
using H1Store.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Application.Interfaces
{
    public interface IFornecedorService
    {
        public Task Adicionar(NovoFornecedorViewModel novoFornecedorViewModel);
        public Task Atualizar(NovoFornecedorViewModel novoFornecedorViewModel);
        public Task Reativar(Guid id);
        public Task Desativar(Guid id);
        public Task<FornecedorViewModel> ObterPorId(Guid id);
        public Task<IEnumerable<FornecedorViewModel>> ObterPorNome(string nomeFornecedor);
        IEnumerable<FornecedorViewModel> ObterTodos();

    }
}
