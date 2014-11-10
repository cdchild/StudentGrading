using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentGrading.Models;
using StudentGrading.ViewModels;
using Microsoft.AspNet.Identity;

namespace StudentGrading.Controllers
{
    [Authorize(Roles = "Professor")]
    public class RubricController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Rubric/
        public async Task<ActionResult> Index()
        {
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);
            var rubrics = db.Rubrics.Where(r => r.owningUserId == currUser.Id || r.global);
            return View(await rubrics.ToListAsync());
        }

        // GET: /Rubric/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rubric rubric = await db.Rubrics.FindAsync(id);
            if (rubric == null)
            {
                return HttpNotFound();
            }
            return View(rubric);
        }

        // GET: /Rubric/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Rubric/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,name,global,aspect")] AddRubricViewModel addRubric)
        {
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                DateTime now = DateTime.Now;
                Rubric rubric = new Rubric
                {
                    name = addRubric.name,
                    global = addRubric.global,
                    owningUserId = currUser.Id,
                    created = DateTime.Now
                };
                db.Rubrics.Add(rubric);
                await db.SaveChangesAsync();
                //rubric = db.Rubrics.First(r => r.created == now);
                RubricAspect aspect = new RubricAspect
                {
                    aspect = addRubric.aspect,
                    order = 1,
                    rubricId = rubric.id
                };
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = aspect.rubricId });
            }

            return View(addRubric);
        }

        // GET: /Rubric/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rubric rubric = await db.Rubrics.Include(r => r.aspects).FirstAsync(r => r.id == id);
            if (rubric == null)
            {
                return HttpNotFound();
            }
            return View(rubric);
        }

        // POST: /Rubric/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id,name,global")] Rubric rubric)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rubric).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rubric);
        }

        // GET: /Rubric/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rubric rubric = await db.Rubrics.FindAsync(id);
            if (rubric == null)
            {
                return HttpNotFound();
            }
            return View(rubric);
        }

        // POST: /Rubric/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Rubric rubric = await db.Rubrics.FindAsync(id);
            db.Rubrics.Remove(rubric);
            await db.SaveChangesAsync();
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
