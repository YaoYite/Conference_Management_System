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
    public class papersController : Controller
    {
        private Entities db = new Entities();
        private static int theId = 0;
        private static int reserved = 0;

        // Assign paper to reviewers
        [Authorize(Roles = "Admin,Chair")]
        [HttpGet]
        public ActionResult Assign(int id)
        {
            theId = id;
            assign assign = new assign();
            ViewBag.reviewer_id = new SelectList(db.AspNetUsers.Where(x => x.AspNetUserRoles.Select(y => y.RoleId).Contains("3")), "Id", "UserName");
            return View(assign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign([Bind(Include = "paper_id, reviewer_id, aaa")] assign assign)
        {
            assign.paper_id = theId;
            ViewBag.reviewer_id = new SelectList(db.AspNetUsers, "Id", "UserName", assign.reviewer_id);
            if (ModelState.IsValid)
            {
                db.assign.Add(assign);
                db.SaveChanges();
                if (User.IsInRole("Chair"))
                {
                    return RedirectToAction("IndexChair");
                }
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("IndexAdmin");
                }
                return RedirectToAction("Index");
            }

            return View(assign);
        }

        // Review a paper
        [Authorize(Roles = "Admin,Reviewer")]
        [HttpGet]
        public ActionResult Review(int id)
        {
            reserved = id;
            review review = new review();
            string current = User.Identity.GetUserId();
            // Make sure a paper can't be reviewed twice by the same person
            // Check if <= deadline
            var deadpaper = db.paper.FirstOrDefault(p => p.Id == id);
            var deadconf = db.Conf.FirstOrDefault(c => c.Id == deadpaper.conf_id);
            DateTime? deadline = deadconf.rev_deadline;

            if (db.review.Any(r => r.paper_id == reserved && r.user_id == current) || deadline <= DateTime.Now)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("IndexAdmin");
                return RedirectToAction("IndexRev");
            }
            return View(review);
        }

        [Authorize(Roles = "Admin,Reviewer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Review([Bind(Include = "Id,rating,comment,user_id,paper_id")] review review)
        {
            string current = User.Identity.GetUserId();
            review.user_id = current;
            review.paper_id = reserved;
            if (ModelState.IsValid)
            {
                db.review.Add(review);
                db.SaveChanges();
                if (User.IsInRole("Admin"))
                    return RedirectToAction("IndexAdmin");
                return RedirectToAction("IndexRev");
            }
            return View(review);
        }

        // See review results
        public ActionResult SeeResult(int id)
        {
            double average = 0;
            var res = db.review.Where(r => r.paper_id == id);
            // Count the average rating
            if (!res.Count().Equals(0))
            {
                average = res.Sum(r => r.rating) / res.Count();
                ViewBag.Avg = average;
            }
            if (res == null)
            {
                return HttpNotFound();
            }
            return View(res.ToList());
        }

        // Display all assign results of a paper
        public ActionResult AllAssign(int id)
        {
            return View(db.assign.Where(a => a.paper_id == id).ToList());
        }

        // GET: papers
        // This is Index of Author, author, author
        public ActionResult Index()
        {

            string current = User.Identity.GetUserId();
            return View(db.paper.Where(m => m.user_id == current).ToList());

        }

        public ActionResult IndexAdmin()
        {
            var paper = db.paper.Include(p => p.AspNetUsers).Include(p => p.Conf);
            return View(paper.ToList());
        }

        public ActionResult IndexChair()
        {
            string current = User.Identity.GetUserId();
            var chairPaper = db.paper.Where(m => m.Conf.chair_id == current).Include(p => p.AspNetUsers).Include(p => p.Conf);
            return View(chairPaper.ToList());
        }

        public ActionResult IndexRev()
        {
            string current = User.Identity.GetUserId();
            var reviewerPaper = from u in db.AspNetUsers
                                join a in db.assign.Where(f => f.reviewer_id == current) on u.Id equals a.reviewer_id
                                join p in db.paper on a.paper_id equals p.Id
                                select p;
            return View(reviewerPaper.ToList());
        }

        // GET: papers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paper paper = db.paper.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }

            return View(paper);
        }

        [Authorize(Roles = "Admin,Author")]
        // GET: papers/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.conf_id = new SelectList(db.Conf, "Id", "name");
            //return View();
            paper peiper = new paper();
            string current = User.Identity.GetUserId();
            peiper.user_id = current;
            return View(peiper);
        }
        public ActionResult deadline()
        {
            return View();
        }
        // POST: papers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Author")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,title,body,assigned,accepted,user_id,conf_id")] paper peiper)
        {
            string current = User.Identity.GetUserId();
            peiper.user_id = current;
            peiper.assigned = false;
            peiper.accepted = false;
            if (ModelState.IsValid)
            {
                // Check if <= deadline
                var deadconf = db.Conf.FirstOrDefault(c => c.Id == peiper.conf_id);
                DateTime? deadline = deadconf.sub_deadline;
                if (deadline <= DateTime.Now)
                {
                    if (User.IsInRole("Admin"))
                            return RedirectToAction("deadline");
                        return RedirectToAction("deadline");
                }
                db.paper.Add(peiper);
                db.SaveChanges();
                if (User.IsInRole("Admin"))
                    return RedirectToAction("deadline");
                    return RedirectToAction("deadline");
            }

            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", peiper.user_id);
            ViewBag.conf_id = new SelectList(db.Conf, "Id", "name", peiper.conf_id);
            return View(peiper);
        }

        // GET: papers/Edit/5
        [Authorize(Roles = "Chair,Admin")]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paper paper = db.paper.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", paper.user_id);
            ViewBag.conf_id = new SelectList(db.Conf, "Id", "name", paper.conf_id);
            return View(paper);
        }

        // POST: papers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,body,assigned,accepted,user_id,conf_id")] paper paper)
        {
            //string current = User.Identity.GetUserId();
            //paper.user_id = current;
            if (ModelState.IsValid)
            {
                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                if (User.IsInRole("Admin"))
                    return RedirectToAction("IndexAdmin");
                return RedirectToAction("IndexChair");
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", paper.user_id);
            ViewBag.conf_id = new SelectList(db.Conf, "Id", "name", paper.conf_id);
            return View(paper);
        }

        // GET: papers/Delete/5
        [Authorize(Roles = "Author,Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paper paper = db.paper.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // POST: papers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            paper paper = db.paper.Find(id);
            db.paper.Remove(paper);
            db.SaveChanges();
            if (User.IsInRole("Admin"))
                return RedirectToAction("IndexAdmin");
            return RedirectToAction("Index");
        }

        public ActionResult AssignError()
        {
            return View();
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
