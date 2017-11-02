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
    [Authorize]
    public class userkeysController : Controller
    {
        private Entities db = new Entities();
        //private static int currentUserId = 0;
        [Authorize(Roles = "Chair,Admin")]
        public ActionResult AllInfo()
        {
            var userkey = db.userkey.Include(u => u.AspNetUsers).Include(u => u.keyword);
            return View(userkey.ToList());
        }
        // GET: userkeys
        public ActionResult Index()
        {
            string currentId = User.Identity.GetUserId();
            var userkey = db.userkey.Include(u => u.AspNetUsers).Include(u => u.keyword);
            return View(db.userkey.Where(u => u.AspNetUsers.Id == currentId).Include(u => u.keyword));
        }

        // GET: userkeys/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userkey userkey = db.userkey.Find(id);
            if (userkey == null)
            {
                return HttpNotFound();
            }
            return View(userkey);
        }

        // GET: userkeys/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.key_id = new SelectList(db.keyword, "Id", "key_name");
            userkey userKeyword = new userkey();
            string currentId = User.Identity.GetUserId();
            userKeyword.user_id = currentId;
            return View(userKeyword);
        }

        // POST: userkeys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,key_id,aaa")] userkey userKeyword)
        {
            string currentId = User.Identity.GetUserId();
            userKeyword.user_id = currentId;
            if (ModelState.IsValid)
            {
                db.userkey.Add(userKeyword);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName", userKeyword.user_id);
            ViewBag.key_id = new SelectList(db.keyword, "Id", "key_name", userKeyword.key_id);
            return View(userKeyword);
        }

        // GET: userkeys/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userkey userkey = db.userkey.Find(id);
            if (userkey == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName", userkey.user_id);
            ViewBag.key_id = new SelectList(db.keyword, "Id", "key_name", userkey.key_id);
            return View(userkey);
        }

        // POST: userkeys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,key_id,aaa")] userkey userkey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userkey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName", userkey.user_id);
            ViewBag.key_id = new SelectList(db.keyword, "Id", "key_name", userkey.key_id);
            return View(userkey);
        }

        // GET: userkeys/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userkey userkey = db.userkey.Find(id);
            if (userkey == null)
            {
                return HttpNotFound();
            }
            return View(userkey);
        }

        // POST: userkeys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            userkey userkey = db.userkey.Find(id);
            db.userkey.Remove(userkey);
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
