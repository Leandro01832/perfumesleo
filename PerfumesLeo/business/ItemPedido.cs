using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class ItemPedido
    {
        public ItemPedido()
        {

        }

        public ItemPedido(Pedido Pedido, Produto Produto, int quantidade, decimal precoUnitario)
        {
            pedido = Pedido;
            produto = Produto;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        public void AtualizaQuantidade(int quantidade)
        {
            Quantidade = quantidade;
        }

        [Key]
        public int IdItem { get; set; }
        [Required(ErrorMessage = "Este campo precisa ser preenchido")]
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int pedido_ { get; set; }
        [ForeignKey("pedido_")]
        public virtual Pedido pedido { get; set; }
        public int produto_ { get; set; }
        [ForeignKey("produto_")]
        public virtual Produto produto { get; set; }

        public decimal Subtotal => Quantidade * PrecoUnitario;

    }
}
