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
using PerfumesLeo.classes;
using PagedList;

namespace PerfumesLeo.Controllers
{
    public class ProdutoController : Controller
    {
        private BD db = new BD();

        // GET: Produto
        public ActionResult Index(int? pagina)
        {
            int TamanhoPagina = 3;
            int NumeroPagina = pagina ?? 1;
            var produto = db.Produto.Include(p => p.Fragrancia);
            return View(produto.ToList().ToPagedList(NumeroPagina, TamanhoPagina));
        }

        // GET: Produto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produto/Create
        public ActionResult Create()
        {
            ViewBag.IdProduto = new SelectList(db.Fragrancias, "IdFragrancia", "DescricaoFragrancia");
            return View();
        }

        // POST: Produto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProduto,Preco,Marca,Imagem,Fragrancia,ImagemFile")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Produto.Add(produto);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                ex.InnerException.InnerException != null &&
                ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "Não é possível ADICIONAR duas marcas com o mesmo nome ou dois numeros de fragrancias iguais !!!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    return View(produto);
                }

                if (produto.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagem";
                    var file = string.Format("{0}.jpg", produto.IdProduto);

                    var response = FileHelpers.UploadPhoto(produto.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        produto.Imagem = pic;
                    }
                }

                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.IdProduto = new SelectList(db.Fragrancias, "IdFragrancia", "DescricaoFragrancia", produto.IdProduto);
            return View(produto);
        }

        // GET: Produto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProduto = new SelectList(db.Fragrancias, "IdFragrancia", "DescricaoFragrancia", produto.IdProduto);
            return View(produto);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProduto,Preco,Marca,Imagem,Fragrancia,ImagemFile")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                if (produto.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagem";
                    var file = string.Format("{0}.jpg", produto.IdProduto);

                    var response = FileHelpers.UploadPhoto(produto.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        produto.Imagem = pic;
                    }
                }

                db.Entry(produto.Fragrancia).State = EntityState.Modified;
                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProduto = new SelectList(db.Fragrancias, "IdFragrancia", "DescricaoFragrancia", produto.IdProduto);
            return View(produto);
        }

        // GET: Produto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produto produto = db.Produto.Find(id);
            db.Produto.Remove(produto);
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
