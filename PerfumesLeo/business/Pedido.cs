using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        public string ValorPedido { get; set; }
        public virtual List<Produto> Produtos { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual DateTime Datapedido { get; set; }
        public virtual Cliente Cliente { get; set; }
        public string Status { get; set; }

    }
}
