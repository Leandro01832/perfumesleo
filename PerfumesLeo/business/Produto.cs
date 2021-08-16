using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace business
{
   public abstract class Produto
    {
        public Produto(){ }

        public Produto(string nome)
        {
            this.Nome = nome;
        }

        [Key]
        public int IdProduto { get; set; }
        public decimal Preco { get; set; }
        public string Nome { get; set; }
        public int Estoque { get; set; }
        [MaxLength(250, ErrorMessage = "Não é possivel adicionar mais de 250 caracteres")]        
        [Required(ErrorMessage = "O campo Nome é requirido!!!")]
        [Index("Produto_Marca_Index", IsUnique = true)]
        public string Marca { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Imagem { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImagemFile { get; set; }
    }
}
