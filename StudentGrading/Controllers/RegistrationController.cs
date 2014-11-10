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

namespace StudentGrading.Controllers
{
    [Authorize(Roles = "Registrar")]
    public class RegistrationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Registration/
        public async Task<ActionResult> Index()
        {
            var registrations = db.Registrations.Include(r => r.role).Include(r => r.section).Include(r => r.user);
            return View(await registrations.ToListAsync());
        }

        // GET: /Registration/4
        public async Task<ActionResult> SIndex(long id)
        {
            var registrations = db.Registrations.Include(r => r.role).Include(r => r.section).Include(r => r.user).Where(r => r.sectionId == id);
            ViewBag.Subtitle = db.Sections.Find(id).dispShort;
            return View("Index", await registrations.ToListAsync());
        }

        // GET: /Registration/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = await db.Registrations.FindAsync(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: /Registration/Create
        public ActionResult Create()
        {
            ViewBag.roleId = new SelectList(db.Roles, "Id", "Name");
            ViewBag.sectionId = new SelectList(db.Sections, "id", "dispShort");
            ViewBag.UserId = new SelectList(db.Users, "id", "dispFull");
            return View();
        }

        // POST: /Registration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="id,userId,sectionId,roleId,begin,end")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.roleId = new SelectList(db.Roles, "Id", "Name", registration.roleId);
            ViewBag.sectionId = new SelectList(db.Sections, "id", "dispShort", registration.sectionId);
            ViewBag.userId = new SelectList(db.Users, "id", "dispFull", registration.userId);
            return View(registration);
        }

        // GET: /Registration/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = await db.Registrations.FindAsync(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.roleId = new SelectList(db.Roles, "Id", "Name", registration.roleId);
            ViewBag.sectionId = new SelectList(db.Sections, "id", "dispShort", registration.sectionId);
            ViewBag.userId = new SelectList(db.Users, "id", "dispFull", registration.userId);
            return View(registration);
        }

        // POST: /Registration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id,userId,sectionId,roleId,begin,end")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.roleId = new SelectList(db.Roles, "Id", "Name", registration.roleId);
            ViewBag.sectionId = new SelectList(db.Sections, "id", "dispShort", registration.sectionId);
            ViewBag.userId = new SelectList(db.Users, "id", "dispFull", registration.userId);
            return View(registration);
        }

        // GET: /Registration/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = await db.Registrations.FindAsync(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: /Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Registration registration = await db.Registrations.FindAsync(id);
            db.Registrations.Remove(registration);
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
