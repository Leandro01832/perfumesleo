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

namespace PerfumesLeo.Controllers
{
    public class FragranciaController : Controller
    {
        private BD db = new BD();

        // GET: Fragrancia
        public ActionResult Index()
        {
            var fragrancias = db.Fragrancias.Include(f => f.Produto);
            return View(fragrancias.ToList());
        }

        // GET: Fragrancia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fragrancia fragrancia = db.Fragrancias.Find(id);
            if (fragrancia == null)
            {
                return HttpNotFound();
            }
            return View(fragrancia);
        }

        // GET: Fragrancia/Create
        public ActionResult Create()
        {
            ViewBag.IdFragrancia = new SelectList(db.Produto, "IdProduto", "Marca");
            return View();
        }

        // POST: Fragrancia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFragrancia,NumeroFragrancia,DescricaoFragrancia")] Fragrancia fragrancia)
        {
            if (ModelState.IsValid)
            {
                db.Fragrancias.Add(fragrancia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdFragrancia = new SelectList(db.Produto, "IdProduto", "Marca", fragrancia.IdFragrancia);
            return View(fragrancia);
        }

        // GET: Fragrancia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fragrancia fragrancia = db.Fragrancias.Find(id);
            if (fragrancia == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdFragrancia = new SelectList(db.Produto, "IdProduto", "Marca", fragrancia.IdFragrancia);
            return View(fragrancia);
        }

        // POST: Fragrancia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFragrancia,NumeroFragrancia,DescricaoFragrancia")] Fragrancia fragrancia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fragrancia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdFragrancia = new SelectList(db.Produto, "IdProduto", "Marca", fragrancia.IdFragrancia);
            return View(fragrancia);
        }

        // GET: Fragrancia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fragrancia fragrancia = db.Fragrancias.Find(id);
            if (fragrancia == null)
            {
                return HttpNotFound();
            }
            return View(fragrancia);
        }

        // POST: Fragrancia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fragrancia fragrancia = db.Fragrancias.Find(id);
            db.Fragrancias.Remove(fragrancia);
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
