using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS.Models;

namespace CMS.Controllers
{
    [Authorize(Roles = "Author")]
    public class paperkeysController : Controller
    {
        private Entities db = new Entities();

        // GET: paperkeys
        public ActionResult Index()
        {
            var paperkey = db.paperkey.Include(p => p.keyword).Include(p => p.paper);
            return View(paperkey.ToList());
        }

        // GET: paperkeys/Details/id
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paperkey paperkey = db.paperkey.Find(id);
            if (paperkey == null)
            {
                return HttpNotFound();
            }
            return View(paperkey);
        }

        // GET: paperkeys/Create
        public ActionResult Create()
        {
            ViewBag.key_id = new SelectList(db.keyword, "Id", "key_name");
            ViewBag.paper_id = new SelectList(db.paper, "Id", "title");
            return View();
        }

        // POST: paperkeys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "paper_id,key_id,aaa")] paperkey paperkey)
        {
            if (ModelState.IsValid)
            {
                db.paperkey.Add(paperkey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.key_id = new SelectList(db.keyword, "Id", "key_name", paperkey.key_id);
            ViewBag.paper_id = new SelectList(db.paper, "Id", "title", paperkey.paper_id);
            return View(paperkey);
        }

        // GET: paperkeys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paperkey paperkey = db.paperkey.Find(id);
            if (paperkey == null)
            {
                return HttpNotFound();
            }
            ViewBag.key_id = new SelectList(db.keyword, "Id", "key_name", paperkey.key_id);
            ViewBag.paper_id = new SelectList(db.paper, "Id", "title", paperkey.paper_id);
            return View(paperkey);
        }

        // POST: paperkeys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "paper_id,key_id,aaa")] paperkey paperkey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paperkey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.key_id = new SelectList(db.keyword, "Id", "key_name", paperkey.key_id);
            ViewBag.paper_id = new SelectList(db.paper, "Id", "title", paperkey.paper_id);
            return View(paperkey);
        }

        // GET: paperkeys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paperkey paperkey = db.paperkey.Find(id);
            if (paperkey == null)
            {
                return HttpNotFound();
            }
            return View(paperkey);
        }

        // POST: paperkeys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            paperkey paperkey = db.paperkey.Find(id);
            db.paperkey.Remove(paperkey);
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
