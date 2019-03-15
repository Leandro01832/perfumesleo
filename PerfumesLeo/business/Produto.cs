using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace business
{
   public class Produto
    {
        [Key]
        public int IdProduto { get; set; }
        public double Preco { get; set; }
        [MaxLength(250, ErrorMessage = "Não é possivel adicionar mais de 250 caracteres")]        
        [Required(ErrorMessage = "O campo Nome é requirido!!!")]
        [Index("Produto_Marca_Index", IsUnique = true)]
        public string Marca { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Imagem { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImagemFile { get; set; }
        public virtual Fragrancia Fragrancia { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
