using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XmlPdfMapper.Models;

namespace XmlPdfMapper.Controllers
{
    public class MappersController : Controller
    {
        private MapperContext db = new MapperContext();

        // GET: Mappers
        public ActionResult Index()
        {
            return View(db.Mappers.ToList());
        }

        // GET: Mappers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapper mapper = db.Mappers.Find(id);
            if (mapper == null)
            {
                return HttpNotFound();
            }
            return View(mapper);
        }

        // GET: Mappers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mappers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MapperId,FileName,PdfName,XmlXpath")] Mapper mapper)
        {
            if (ModelState.IsValid)
            {
                db.Mappers.Add(mapper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mapper);
        }

        // GET: Mappers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapper mapper = db.Mappers.Find(id);
            if (mapper == null)
            {
                return HttpNotFound();
            }
            return View(mapper);
        }

        // POST: Mappers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MapperId,FileName,PdfName,XmlXpath")] Mapper mapper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mapper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mapper);
        }

        // GET: Mappers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapper mapper = db.Mappers.Find(id);
            if (mapper == null)
            {
                return HttpNotFound();
            }
            return View(mapper);
        }

        // POST: Mappers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mapper mapper = db.Mappers.Find(id);
            db.Mappers.Remove(mapper);
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
