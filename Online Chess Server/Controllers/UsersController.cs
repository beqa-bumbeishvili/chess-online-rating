﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Chess_Server.Models;

namespace Online_Chess_Server.Controllers
{
    public class UsersController : Controller
    {
        private ChessModel db = new ChessModel();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult UserProfile( int userId)
        {

            var model = new UserProfileModel();
            model.Progress = db.Ratings.Where(r => r.UserID == userId)
                                       .OrderBy(r => r.RatingChangeDate)
                                       .Select(e=>new ChartViewModel() { Time=e.RatingChangeDate.ToString(),Value=e.RatingValue})
                                       .ToList();


            model.FullName = db.Users.Where(e => e.ID == userId).Select(x => x.FullName).FirstOrDefault().ToString();
            model.UserName = db.Users.Where(e => e.ID == userId).FirstOrDefault().UserName;
            model.LichessName = db.Users.Where(e => e.ID == userId).Select(x => x.LichessName).FirstOrDefault().ToString();
            model.GamesPlayedLink = db.Games.Where(e => e.FirstPlayerID == userId).Count() + db.Games.Where(e => e.SecondPlayerID == userId).Count();
            model.CurrentRating = db.Ratings.Where(e => e.UserID == userId).OrderByDescending(x => x.RatingChangeDate).Select(z => z.RatingValue).FirstOrDefault();
            model.ImageName = "Pawn";
            if (model.CurrentRating >= 1525)
                model.ImageName = "Bishop";
            if (model.CurrentRating >= 1560) 
                model.ImageName = "Knight";

            if (model.CurrentRating >= 1646)
                model.ImageName = "Rock";

            if (model.CurrentRating >= 1750)
                model.ImageName = "Queen";

            if (model.CurrentRating >= 1800)
                model.ImageName = "King";

            if (model.CurrentRating >= 1900)
                model.ImageName = "Candidate";
            if (model.CurrentRating >= 2000)
                model.ImageName = "Expert";

            if (model.CurrentRating >= 2200)
                model.ImageName = "Master";

            if (model.CurrentRating >= 2400)
                model.ImageName = "Senior Master";

            if (model.CurrentRating >= 2600)
                model.ImageName = "Grandmaster";



            return View(model);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FullName,UserName,LichessName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FullName,UserName,LichessName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
