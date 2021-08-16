using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace business
{
    public class Endereco
    {
        [Key, ForeignKey("Cliente")]
        public int IdEndereco { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public long Numero { get; set; }
        public long Cep { get; set; }
        public string Complemento { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}
