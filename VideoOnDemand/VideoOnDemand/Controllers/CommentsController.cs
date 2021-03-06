﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VideoOnDemand.Entity;
using VideoOnDemand.Models;

namespace VideoOnDemand.Controllers
{
    public class CommentsController : Controller
    {
        private VODContext db = new VODContext();

        // GET: Comments
        public ActionResult Index()
        {
            return View(db.Comments.ToList().OrderBy(comment=>comment.Date).Reverse());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if ((String)Session["LoginAdmin"] == "True")
            {
                return View(comment);
            }
            else
            {
                return RedirectToAction("../Films");
            }
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Message,Marks")] Comment comment,int Id_User, int Id_Film)
        {
            if (ModelState.IsValid)
            {
                User author = db.Users.Find(Id_User);
                Film film = db.Films.Find(Id_Film);
                comment.Author = author;
                comment.Film = film;
                comment.Date = DateTime.Now;
                comment.Validated = false;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Films", new { id = Id_Film });
            }

            return View("~/View/Film/Index.cshtml");
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            } if ((String)Session["LoginAdmin"] == "True")
            {
                return View(comment);
            }
            else
            {
                return RedirectToAction("../Films");
            }
        }

        // POST: Comments/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message,Marks,Date,Validated")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if ((String)Session["LoginAdmin"] == "True")
            {
                return View(comment);
            }
            else
            {
                return RedirectToAction("../Films");
            }
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
