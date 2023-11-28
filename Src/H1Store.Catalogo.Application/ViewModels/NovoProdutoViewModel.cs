﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Application.ViewModels
{
	public class NovoProdutoViewModel
	{
        public Guid CodigoId { get; set; }
        public string Nome { get; set; }
		public string Descricao { get; set; }
		public bool Ativo { get; set; }
		public decimal Valor { get; set; }
		public DateTime DataCadastro { get; set; }
		public int QuantidadeEstoque { get; set; }
	}
}
