using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.SqlClient;

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
            Status_Pedido = "Esperando pagamento.";
        }

        [Key]
        public int IdPedido { get; set; }
        public string ValorPedido { get; set; }
        public virtual List<ItemPedido> Itens { get; set; }
        
        public DateTime Datapedido { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public static SqlTransaction transacao;

        public  void salvar(SqlTransaction transaction)
        {
            transacao = transaction;
            var sql = $"Insert into Pedido (ClienteId, Datapedido, Status_Pedido, ValorPedido) values ({this.ClienteId}, '{this.Datapedido.ToString("yyyy-MM-dd")}', '{this.Status_Pedido}', '{this.ValorPedido}' )";
            SqlCommand comando = new SqlCommand(sql, transaction.Connection, transaction);

            comando.ExecuteNonQuery();
        }

        public void editar(SqlTransaction transaction)
        {
            transacao = transaction;
            var sql = $"update Pedido set ClienteId={this.ClienteId}, Datapedido={this.Datapedido.ToString("yyyy-MM-dd")}', Status_Pedido='{this.Status_Pedido}', ValorPedido='{this.ValorPedido}' where IdPedido={this.IdPedido}";
            SqlCommand comando = new SqlCommand(sql, transaction.Connection, transaction);

            comando.ExecuteNonQuery();
        }

        [Required]
        public int ClienteId { get; set; }
        public string Status_Pedido { get; set; }

    }
}
