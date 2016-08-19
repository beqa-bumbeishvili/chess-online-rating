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
    public class LichessController : Controller
    {
        private ChessModel db = new ChessModel();

        // GET: Lichess
        public ActionResult Index()
        {
            var games = db.Games.Include(g => g.User).Include(g => g.User1);
            return View(games.ToList());
        }

        // GET: Lichess/Details/5
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

        // GET: Lichess/Create
        public ActionResult Create()
        {
            ViewBag.FirstPlayerID = new SelectList(db.Users, "ID", "FullName");
            ViewBag.SecondPlayerID = new SelectList(db.Users, "ID", "FullName");
            return View();
        }

        // POST: Lichess/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstPlayerID,SecondPlayerID,Result,GameLink,PlayDate")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FirstPlayerID = new SelectList(db.Users, "ID", "FullName", game.FirstPlayerID);
            ViewBag.SecondPlayerID = new SelectList(db.Users, "ID", "FullName", game.SecondPlayerID);
            return View(game);
        }

        // GET: Lichess/Edit/5
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

        // POST: Lichess/Edit/5
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

        // GET: Lichess/Delete/5
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

        // POST: Lichess/Delete/5
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
