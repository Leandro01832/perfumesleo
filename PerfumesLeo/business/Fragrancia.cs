using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Fragrancia
    {
        [Key, ForeignKey("Perfume")]
        public int IdFragrancia { get; set; }
        [Index("Fragrancia_NumeroFragrancia_Index", IsUnique = true)]
        public long NumeroFragrancia { get; set; }
        public string DescricaoFragrancia { get; set; }        
        public virtual Perfume Perfume { get; set; }
        
    }
}
