using DataContextPerfume;
using System.Data.Entity;

namespace PerfumesLeo.Models.Repository
{
    public class BaseRepository<T> where T : class
    {
        protected readonly BD contexto;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(BD contexto)
        {
            this.contexto = contexto;
            dbSet = contexto.Set<T>();
        }
    }
}