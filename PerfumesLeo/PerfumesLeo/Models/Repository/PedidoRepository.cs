using business;
using DataContextPerfume;
using Loja_Aline.Models.ViewModels;
using PerfumesLeo.Models.Repository;
using PerfumesLeo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PerfumesLeo.Models.Repository
{
    public interface IPedidoRepository
    {
        IEnumerable<Pedido> GetAll();
        Task<Pedido> Get(int id);
        Pedido Add(Cliente cli);
        void Update(Pedido item);
        bool Delete(int id);
        Task<Pedido> UpdateCadastroAsync(Pedido pedido);
        Task AddItemAsync(int codigo, int IdPedido);
        Task<UpdateQuantidadeResponse> UpdateQuantidadeAsync(ItemPedido itemPedido, int IdPedido);
    }

    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(BD contexto, IClienteRepository repositoryCliente) : base(contexto)
        {
            Contexto = contexto;
            RepositoryCliente = repositoryCliente;
        }

        public BD Contexto { get; }
        public IClienteRepository RepositoryCliente { get; }

        public Pedido Add(Cliente cli)
        {
            Pedido ped = new Pedido(cli.IdCliente);
            ped.ClienteId = cli.IdCliente;
            dbSet.Add(ped);
            Contexto.SaveChanges();
            return ped;
        }

        public IEnumerable<Pedido> GetAll()
        {
            return dbSet.ToList();
        }

        public bool Delete(int id)
        {
            var prod = dbSet.FirstOrDefault(p => p.IdPedido == id);

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

        public async Task<Pedido> Get(int id)
        {
            return await dbSet.Include(p => p.Itens).FirstAsync(p => p.IdPedido == id);
        }

        public async Task AddItemAsync(int codigo, int IdPedido)
        {
            var produto = await
                            contexto.Set<Produto>()
                            .Where(p => p.IdProduto == codigo)
                            .SingleOrDefaultAsync();

            if (produto == null)
            {
                throw new ArgumentException("Produto não encontrado");
            }

            var pedido = await Get(IdPedido);

            var itemPedido = await
                                contexto.Set<ItemPedido>()
                                .Where(i => i.produto.IdProduto == codigo
                                        && i.pedido.IdPedido == pedido.IdPedido)
                                .SingleOrDefaultAsync();

            if (itemPedido == null)
            {
                itemPedido = new ItemPedido(pedido, produto, 1, produto.Preco);
                contexto.Set<ItemPedido>().Add(itemPedido);

                await contexto.SaveChangesAsync();
            }
        }

        

        public void Update(Pedido item)
        {
            Contexto.Entry(item).State = EntityState.Modified;
            Contexto.SaveChanges();
        }

        public async Task<Pedido> UpdateCadastroAsync(Pedido pedido)
        {
            Contexto.Entry(pedido.Cliente).State = EntityState.Modified;
            Contexto.Entry(pedido.Cliente.Endereco).State = EntityState.Modified;
            Contexto.Entry(pedido.Cliente.Telefone).State = EntityState.Modified;
            await Contexto.SaveChangesAsync();
            return pedido;
        }

        public async Task<UpdateQuantidadeResponse> UpdateQuantidadeAsync(ItemPedido itemPedido, int IdPedido)
        {
            var itemPedidoDB = await GetItemPedidoAsync(itemPedido.IdItem);

            if (itemPedidoDB != null)
            {
                itemPedidoDB.AtualizaQuantidade(itemPedido.Quantidade);

                if (itemPedido.Quantidade == 0)
                {
                    await RemoveItemPedidoAsync(itemPedido.IdItem);
                }

                await contexto.SaveChangesAsync();

                var pedido = await Get(IdPedido);
                var carrinhoViewModel = new CarrinhoViewModel(pedido.Itens);

                return new UpdateQuantidadeResponse(itemPedidoDB, carrinhoViewModel);
            }

            throw new ArgumentException("ItemPedido não encontrado");
        }

        private async Task<ItemPedido> GetItemPedidoAsync(int itemPedidoId)
        {
            return
            await contexto.Set<ItemPedido>()
                .Where(ip => ip.IdItem == itemPedidoId)
                .SingleOrDefaultAsync();
        }

        private async Task RemoveItemPedidoAsync(int itemPedidoId)
        {
            contexto.Set<ItemPedido>()
                .Remove(await GetItemPedidoAsync(itemPedidoId));
        }
    }
}