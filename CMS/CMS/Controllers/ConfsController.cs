using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS.Models;
using Microsoft.AspNet.Identity;

namespace CMS.Controllers
{
    //[Authorize(Roles = "Chair,Admin")]
    public class ConfsController : Controller
    {
        private Entities db = new Entities();

        // GET: Confs
        public ActionResult Index()
        {

            var conference = db.Conf.Include(c => c.AspNetUsers);
            return View(conference.ToList());
        }

        // GET: Confs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conf conference = db.Conf.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        [Authorize(Roles ="Admin,Chair")]
        // GET: Confs/Create
        public ActionResult Create()
        {
            Conf conference = new Conf();
            string currentUser = User.Identity.GetUserId();
            conference.chair_id = currentUser;
            return View(conference);
        }

        // POST: Confs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Chair")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,topic,chair_id,sub_deadline,rev_deadline")] Conf conf)
        {
            string currentUser = User.Identity.GetUserId();
            conf.chair_id = currentUser;
            if (ModelState.IsValid)
            {
                db.Conf.Add(conf);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.chair_id = new SelectList(db.AspNetUsers, "Id", "Email", conf.chair_id);
            return View(conf);
        }

        // GET: Confs/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conf conf = db.Conf.Find(id);
            if (conf == null)
            {
                return HttpNotFound();
            }
            ViewBag.chair_id = new SelectList(db.AspNetUsers, "Id", "Email", conf.chair_id);
            return View(conf);
        }

        // POST: Confs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,topic,chair_id,sub_deadline,rev_deadline")] Conf conf)
        {
            string currentUser = User.Identity.GetUserId();
            conf.chair_id = currentUser;
            if (ModelState.IsValid)
            {
                db.Entry(conf).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.chair_id = new SelectList(db.AspNetUsers, "Id", "Email", conf.chair_id);
            return View(conf);
        }

        // GET: Confs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conf conf = db.Conf.Find(id);
            if (conf == null)
            {
                return HttpNotFound();
            }
            return View(conf);
        }

        // POST: Confs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conf conf = db.Conf.Find(id);
            db.Conf.Remove(conf);
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
