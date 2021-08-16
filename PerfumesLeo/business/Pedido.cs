using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace business
{
    public class Pedido
    {
        public Pedido()
        {

        }

        public Pedido(int Id)
        {
            this.ClienteId = Id;
            Datapedido = DateTime.Now;
        }

        [Key]
        public int IdPedido { get; set; }
        public string ValorPedido { get; set; }
        public virtual List<ItemPedido> Itens { get; set; }
        
        public virtual DateTime Datapedido { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public string Status { get; set; }

    }
}
