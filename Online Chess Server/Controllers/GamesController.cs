using System;
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
    public class GamesController : Controller
    {
        private ChessModel db = new ChessModel();

        // GET: Games
        public ActionResult Index()
        {
            var games = db.Games.Include(g => g.User).Include(g => g.User1);
            return View(games.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            ViewBag.FirstPlayerID = new SelectList(db.Users, "ID", "FullName");
            ViewBag.SecondPlayerID = new SelectList(db.Users, "ID", "FullName");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,FirstPlayerID,SecondPlayerID,Result,GameLink,PlayDate")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                const int K = 32;
                double r1 =(double)(db.Ratings.Where(a => a.UserID == game.FirstPlayerID).OrderByDescending(c => c.RatingChangeDate).Select(b => b.RatingValue).FirstOrDefault()); // first player latest rating
                double r2 = (double)(db.Ratings.Where(a => a.UserID == game.SecondPlayerID).OrderByDescending(c => c.RatingChangeDate).Select(b => b.RatingValue).FirstOrDefault()); // second player latest rating

                //transformed rating
                double R1 = Math.Pow(10, (r1 / 400));
                double R2 = Math.Pow(10, (r2 / 400));

                // Calculate the expected score for each player
                double E1 = R1 / (R1 + R2);
                double E2 = R2 / (R1 + R2);

                // Check The Winner
                double S1, S2;
                if (game.Result == 1) { S1 = 1; S2 = 0; }
                else if (game.Result == -1) { S1 = 0; S2 = 1; }
                else { S1 = 0.5; S2 = 0.5; }

                // new rating 
                double nr1 = r1 + (K * (S1 - E1)) * 1.4;
                double nr2 = r2 + (K * (S2 - E2)) * 1.4;

                //new ceilled rates and rating changes
                double finalRatingFirstPlayer = Math.Ceiling(nr1 * 10) / 10;
                double ratingChangeForFirst = Math.Ceiling((finalRatingFirstPlayer - r1)*10)/10;
                double finalRatingSecondPlayer = Math.Ceiling(nr2 * 10) / 10;
                double ratingChangeForSecond = Math.Ceiling((finalRatingSecondPlayer - r2) * 10) / 10;

                if (ratingChangeForFirst > 0) ratingChangeForSecond = -ratingChangeForFirst; 
                else if (ratingChangeForFirst < 0)  ratingChangeForFirst = -ratingChangeForSecond; 
                else { ratingChangeForFirst = 0; ratingChangeForSecond = 0; }

                var newestFirstRating = new Rating{ UserID = game.FirstPlayerID, RatingChangeDate = game.PlayDate, RatingValue = (int)finalRatingFirstPlayer };
                var newestSecondRating = new Rating{ UserID = game.SecondPlayerID, RatingChangeDate = game.PlayDate, RatingValue = (int)finalRatingSecondPlayer };

                db.Ratings.Add(newestFirstRating);
                db.Ratings.Add(newestSecondRating);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FirstPlayerID = new SelectList(db.Users, "ID", "FullName", game.FirstPlayerID);
            ViewBag.SecondPlayerID = new SelectList(db.Users, "ID", "FullName", game.SecondPlayerID);
            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.FirstPlayerID = new SelectList(db.Users, "ID", "FullName", game.FirstPlayerID);
            ViewBag.SecondPlayerID = new SelectList(db.Users, "ID", "FullName", game.SecondPlayerID);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstPlayerID,SecondPlayerID,Result,GameLink,PlayDate")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FirstPlayerID = new SelectList(db.Users, "ID", "FullName", game.FirstPlayerID);
            ViewBag.SecondPlayerID = new SelectList(db.Users, "ID", "FullName", game.SecondPlayerID);
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
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
