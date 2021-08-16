using System.ComponentModel.DataAnnotations;

namespace business
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Cpf { get; set; }
        public virtual Telefone Telefone { get; set; }
        public virtual Endereco Endereco { get; set; }
    }
}
