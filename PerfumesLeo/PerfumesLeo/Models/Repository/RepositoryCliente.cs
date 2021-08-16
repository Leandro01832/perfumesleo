using business;
using DataContextPerfume;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PerfumesLeo.Models.Repository
{
    public interface IClienteRepository
    {
        IEnumerable<Cliente> GetAll();
        Task<Cliente> Get(string email);
        void Add(Cliente item);
        void Update(Cliente item);
        bool Delete(int id);
    }

    public class RepositoryCliente : BaseRepository<Cliente>, IClienteRepository
    {

        public RepositoryCliente(BD contexto) : base(contexto)
        {
            Contexto = contexto;
        }

        public BD Contexto { get; }

        public void Add(Cliente item)
        {
            dbSet.Add(item);
            Contexto.SaveChanges();
        }

        public bool Delete(int id)
        {
            var prod = dbSet.FirstOrDefault(p => p.IdCliente == id);

            if (prod != null)
            {
                dbSet.Remove(prod);
                Contexto.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Cliente> Get(string email)
        {
            return await dbSet.FirstAsync(p => p.UserName == email);
        }

        public IEnumerable<Cliente> GetAll()
        {
            return dbSet.ToList();
        }

        public void Update(Cliente item)
        {
            Contexto.Entry(item).State = EntityState.Modified;
            Contexto.SaveChanges();
        }
    }
}