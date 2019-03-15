using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataContextPerfume;
using business;
using Microsoft.AspNet.Identity;
using PagSeguro;
using System.Diagnostics;

namespace PerfumesLeo.Controllers
{
    public class PedidoController : Controller
    {
        private BD db = new BD();

        // GET: Pedido
        public ActionResult Index()
        {
            var pedido = db.Pedido.Include(p => p.Endereco);
            return View(pedido.ToList());
        }

        // GET: Pedido/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedido/Create
        public ActionResult Create()
        {
            ViewBag.IdPedido = new SelectList(db.Endereco, "IdEndereco", "Estado");
            return View();
        }

        // POST: Pedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPedido,ValorPedido,Datapedido,Status")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedido.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPedido = new SelectList(db.Endereco, "IdEndereco", "Estado", pedido.IdPedido);
            return View(pedido);
        }

        // GET: Pedido/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPedido = new SelectList(db.Endereco, "IdEndereco", "Estado", pedido.IdPedido);
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPedido,ValorPedido,Datapedido,Status,Endereco")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                pedido.Status = "Finalizado";
                pedido.ValorPedido = Request["precototal"].Replace(".", ",");
                pedido.Endereco.Estado = Request["Estado"];
                pedido.Endereco.Bairro = Request["Bairro"];
                pedido.Endereco.Cep = Request["Cep"];
                pedido.Endereco.Cidade = Request["Cidade"];
                pedido.Endereco.Rua = Request["Rua"];
                try
                {
                    pedido.Endereco.Numero = long.Parse(Request["Numero"]);
                }
                catch (Exception)
                {
                    return RedirectToAction("Edit", "Pedido", new { pedido });
                }       

                db.Entry(pedido).State = EntityState.Modified;
                db.Entry(pedido.Endereco).State = EntityState.Modified;
                

                Dados dados = new Dados();
                var email = User.Identity.GetUserName();
                dados.Email = email;
                var cli = db.Cliente.FirstOrDefault(c => c.UserName == email);
                dados.Nome = cli.FirstName + " " + cli.LastName;
                dados.DDD = cli.Telefone.DDD_Celular;
                dados.NumeroTelefone = cli.Telefone.Celular;
                dados.NumeroPedido = pedido.IdPedido;

                dados.Valor = pedido.ValorPedido;                

                dados.MeuEmail = "leandro91luis@gmail.com";
                dados.MeuToken = "36366C5419B84EAD8AE2AAF14E7A1E36";
                dados.TituloPagamento = "Pagamento";
                dados.Referencia = "001";

                dados = sPagSeguro.GerarPagamento(dados);                

                db.Dados.Add(dados);
                db.SaveChanges();

                return RedirectToAction("IndexCliente", "Cliente");

            }
            ViewBag.IdPedido = new SelectList(db.Endereco, "IdEndereco", "Estado", pedido.IdPedido);
            return View(pedido);
        }

        // GET: Pedido/Edit/5
        [Authorize]
        public ActionResult AdicionarCarrinho(int? id)
        {
            var email = User.Identity.GetUserName();
            Cliente cliente = null;
            
            try
            {
                 cliente = db.Cliente.First(e => e.UserName == email);
            }
            catch (Exception)
            {
                return RedirectToAction("Create", "Cliente");
            }
            
            if (cliente.Pedidos.Count == 0 || cliente.Pedidos.Last().Status == "Finalizado")
            {
                cliente.Pedidos.Add(new Pedido { Datapedido = DateTime.Now, Endereco = new Endereco { }, Status = "Nao finalizado" });
                cliente.Pedidos.Last().Produtos = new List<Produto>();
            }

            
            db.SaveChanges();

            ViewBag.produto = db.Produto.Find(id).IdProduto;

            return View(cliente.Pedidos.Last()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AdicionarCarrinho([Bind(Include = "IdPedido,ValorPedido,Datapedido,Status")] Pedido ped)
        {
            if (ModelState.IsValid)
            {
                var produtos = db.Pedido.First(m => m.IdPedido == ped.IdPedido).Produtos;

                int id = int.Parse(Request["perfume"]);
                Produto prod = db.Produto.First(p => p.IdProduto == id);
                if(ped.Produtos == null)
                {
                    ped.Produtos = new List<Produto>();
                }
                produtos.Add(prod);
                ped.Produtos.AddRange(produtos);

                db.SaveChanges();
                return RedirectToAction("IndexCliente", "Cliente");
            }
            ViewBag.IdPedido = new SelectList(db.Endereco, "IdEndereco", "Estado", ped.IdPedido);
            return View(ped);
        }

        // GET: Pedido/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            db.Pedido.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
