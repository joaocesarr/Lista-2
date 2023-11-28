using AutoMapper;
using H1Store.Catalogo.Data.Providers.MongoDb.Collections;
using H1Store.Catalogo.Data.Providers.MongoDb.Interfaces;
using H1Store.Catalogo.Domain.Entities;
using H1Store.Catalogo.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Data.Repository
{
	public class FornecedorRepository : IFornecedorRepository
	{

		private readonly IMongoRepository<FornecedorCollection> _fornecedorRepository;
		private readonly IMapper _mapper;

		public FornecedorRepository(IMongoRepository<FornecedorCollection> fornecedorRepository,
			IMapper mapper
			)
		{
			_fornecedorRepository = fornecedorRepository;
			_mapper = mapper;
		}

		public async Task Adicionar(Fornecedor fornecedor)
		{
            FornecedorCollection fornecedorCollection = new FornecedorCollection();
            fornecedorCollection.CodigoId = fornecedor.CodigoId;
            fornecedorCollection.Nome = fornecedor.Nome;
            fornecedorCollection.Cnpj = fornecedor.Cnpj;
            fornecedorCollection.RazaoSocial = fornecedor.RazaoSocial;
            fornecedorCollection.DataCadastro = fornecedor.DataCadastro;
            fornecedorCollection.Ativo = fornecedor.Ativo;

            await _fornecedorRepository.InsertOneAsync(fornecedorCollection);
        }

		public async Task Atualizar(Fornecedor fornecedor)
		{
			var buscaFornecedor = _fornecedorRepository.FilterBy(f => f.CodigoId == fornecedor.CodigoID);
			var fornecedorExiste = buscaFornecedor.FirstOrDefault();

			if (fornecedorExiste != null)
			{
				fornecedorExiste.Nome = fornecedor.Nome;
				fornecedorExiste.Cnpj = fornecedor.Cnpj;
				fornecedorExiste.RazaoSocial = fornecedor.RazaoSocial;
				fornecedorExiste.DataCadastro = fornecedor.DataCadastro;


				await _fornecedorRepository.ReplaceOneAsync(_mapper.Map<FornecedorCollection>(fornecedorExiste));
			}

			if (fornecedorExiste == null) throw new ApplicationException("Não é possivel editar um fornecedor que não existe");

		}
		public async Task Reativar(Fornecedor fornecedor)
		{
			var buscaFornecedor = _fornecedorRepository.FilterBy(filter => filter.CodigoId == fornecedor.CodigoID);
			var AtivarFornecedor = buscaFornecedor.FirstOrDefault();
			AtivarFornecedor.Ativo = fornecedor.Ativo;
			await _fornecedorRepository.ReplaceOneAsync(_mapper.Map<FornecedorCollection>(AtivarFornecedor));
		}
		public async Task Desativar(Fornecedor fornecedor)
		{

			var buscaFornecedor = _fornecedorRepository.FilterBy(filter => filter.CodigoId == fornecedor.CodigoID);

			if (buscaFornecedor == null) throw new ApplicationException("Não é possível desativar um fornecedor que não existe");

			var fornecedorCollection = _mapper.Map<FornecedorCollection>(fornecedor);

			fornecedorCollection.Id = buscaFornecedor.FirstOrDefault().Id;

			await _fornecedorRepository.ReplaceOneAsync(fornecedorCollection);
		}

		public async Task<Fornecedor> ObterPorCnpj(string cnpj)
		{
			throw new NotImplementedException();
		}

		public async Task<Fornecedor> ObterPorId(Guid id)
		{
			var buscaFornecedor = _fornecedorRepository.FilterBy(f => f.CodigoId == id);
			var fornecedor = _mapper.Map<Fornecedor>(buscaFornecedor.FirstOrDefault());
			return fornecedor;
		}

		public async Task<IEnumerable<Fornecedor>> ObterPorNome(string nomeFornecedor)
		{
			var buscaFornecedores = _fornecedorRepository.FilterBy(f => f.Nome == nomeFornecedor);
			return _mapper.Map<IEnumerable<Fornecedor>>(buscaFornecedores);
		}

		public IEnumerable<Fornecedor> ObterTodos()
		{
            var fornecedorList = _fornecedorRepository.FilterBy(filter => true);

            List<Fornecedor> lista = new List<Fornecedor>();
            foreach (var item in fornecedorList)
            {
                lista.Add(new Fornecedor(item.CodigoId, item.Nome, item.Cnpj, item.RazaoSocial, item.DataCadastro, item.Ativo));
            }
            return lista;

        }

    }
}
