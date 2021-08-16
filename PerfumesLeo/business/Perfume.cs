using System.ComponentModel.DataAnnotations;

namespace business
{
    public class Perfume : Produto
    {
        public Perfume(){ }
        public Perfume(string nome) : base(nome){ }        
        public virtual Fragrancia Fragrancia { get; set; }
    }
}