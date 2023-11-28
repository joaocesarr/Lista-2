using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Application.ViewModels
{
    public class FornecedorViewModel
    {
        #region Propriedades

        public Guid CodigoID { get; private set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public DateTime DataCadastro { get; private set; } = DateTime.Now;
        public bool Ativo { get; set; }
        #endregion
    }
}
