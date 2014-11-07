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
using Microsoft.AspNet.Identity.EntityFramework;

namespace StudentGrading.Controllers
{
    //only registrars can change class & registration info
    [Authorize(Roles = "Registrar")]
    public class ClassController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Class/
        public async Task<ActionResult> Index()
        {
            // include the course for the section
            var sections = db.Sections.Include(s => s.course);
            return View(await sections.ToListAsync());
        }

        // GET: /Class/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = await db.Sections.FindAsync(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: /Class/Create
        public ActionResult Create()
        {
            ViewBag.courseId = new SelectList(db.Courses, "id", "dispShortWithTitle");
            ViewBag.UserId = new SelectList(db.Users, "id", "dispFull");
            return View();
        }

        // POST: /Class/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,courseId,code,begin,end,UserId")] AddSectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                //from the view model create a section object to add later
                Section section = new Section
                {
                    id = model.id,
                    courseId = model.courseId,
                    code = model.code,
                    begin = model.begin,
                    end = model.end
                };
                //save section to db
                db.Sections.Add(section);
                await db.SaveChangesAsync();
                // create a string for the professor-like role to be used in the linq query
                string addRole = Role.Professor.ToString();
                //load the user profile identified by the id selected
                var professorUser = db.Users.First(u => u.Id == model.UserId);
                try
                {   
                    //create the registration object for the professor, every class needs at least one professor
                    Registration professor = new Registration
                    {
                        //set sectionId by finding the one in the db that matches what was in the view model
                        sectionId = db.Sections
                            .Single(
                                s => s.courseId == model.courseId &&
                                s.code == model.code && s.begin == model.begin &&
                                s.end == model.end &&
                                !s.Registrations.Any(
                                    r => r.userId == model.UserId && r.role.Name == addRole
                                )
                            ).id,
                        userId = model.UserId,
                        roleId = db.Roles.Single(r => r.Name == addRole).Id,
                        begin = model.begin,
                        end = model.end
                    };
                    //save registration of professor in registration table
                    db.Registrations.Add(professor);
                    //save user role if not already considered a professor
                    if (professorUser.UserRoles.Count(x => x.RoleId == professor.roleId) == 0)
                        professorUser.UserRoles.Add(new IdentityUserRole { UserId = model.UserId, RoleId = professor.roleId });
                    await db.SaveChangesAsync();
                }
                catch { }

                return RedirectToAction("Edit", new { id = section.id });
            }

            ViewBag.courseId = new SelectList(db.Courses, "id", "dispShortWithTitle", model.courseId);
            ViewBag.UserId = new SelectList(db.Users, "id", "dispFull");
            return View("Create", model);
        }

        //
        // POST: /Class/AddExisting

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExisting(
            [Bind(Include = "id")] Section section,
            [Bind(Include = "roleId, userId")] Registration registrtn)
        {
            int i = section.id;
            //continue binding the details of the registration that are implied
            registrtn.begin = DateTime.Now.Date;
            registrtn.end = db.Sections.Find(i).end;
            registrtn.sectionId = i;
            // load the user object
            var user = db.Users.First(u => u.Id == registrtn.userId);
            try
            {
                //add registration object
                db.Registrations.Add(registrtn);
                //save user in role if user not already considered in it
                if (user.UserRoles.Count(x => x.RoleId == registrtn.roleId) == 0)
                    user.UserRoles.Add(new IdentityUserRole { UserId = registrtn.userId, RoleId = registrtn.roleId });
                //commit to db
                db.SaveChanges();
            }
            catch { }
            //go back to the edit page/form sending to the same section as identified by id
            return RedirectToAction("Edit", new { id = i });
        }





        // GET: /Class/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //load the section including current registrations
            Section section = await db.Sections
                .Include(s => s.Registrations)
                .Where(s => s.Registrations.Any(r => r.begin >= s.begin && r.end <= s.end))
                .FirstAsync(s => s.id == id);
            if (section == null)
            {
                return HttpNotFound();
            }
            // create string for the student role to be used in linq query
            string roleStudent = Role.Student.ToString();
            // create list of courses
            ViewBag.courseId = new SelectList(db.Courses, "id", "dispShortWithTitle", section.courseId);
            // create list of users not currently registered
            ViewBag.UserId = new SelectList(
                db.Users.Where(
                    u => !db.Registrations
                    .Where(r => r.sectionId == id)
                    .Select(s => s.userId)
                    .Contains(u.Id)),
                "id", "dispFull");
            // list for roles, defaulting to the student role so that students aren't as likely to be accidentally added as professor, etc.
            ViewBag.RoleId = new SelectList(db.Roles, "id", "Name", db.Roles.Single(r => r.Name == roleStudent).Id);
            return View(section);
        }

        // POST: /Class/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id,courseId,code,begin,end")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // create string for student role to be used in linq query
            string roleStudent = Role.Student.ToString();
            // list of courses
            ViewBag.courseId = new SelectList(db.Courses, "id", "dispShortWithTitle", section.courseId);
            // list of users not currently registered
            ViewBag.UserId = new SelectList(
                db.Users.Where(
                    u => !db.Registrations
                    .Where(r => r.sectionId == section.id)
                    .Select(s => s.userId)
                    .Contains(u.Id)),
                "id", "dispFull");
            // list for roles, defaulting to the student role so that students aren't as likely to be accidentally added as professor, etc.
            ViewBag.RoleId = new SelectList(db.Roles, "id", "Name", db.Roles.Single(r => r.Name == roleStudent).Id);
            return View(section);
        }

        //
        // POST: /Class/Remove

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(int id, int i)
        {
            //find/load the registration object in the db
            Registration regstrn = db.Registrations.Find(i);
            if (regstrn != null)
            {
                //remove user in role if user is not registered in any current or future classes in the same role
                if (
                    db.Registrations
                        .Where(
                            r => r.userId == regstrn.userId &&
                            r.roleId == regstrn.roleId)
                        .All(x => DateTime.Today >= x.end)
                    )
                    db.Users.Find(regstrn.userId).UserRoles.Remove(new IdentityUserRole { UserId = regstrn.userId, RoleId = regstrn.roleId });
                //if it's currently before the end of the registration date change the registration date to lock them out of this section
                // although if this is run a second time for the same user, and otherwise remove the registration (since it'd now be the same day as the ending date)
                if (DateTime.Today < regstrn.end)
                    regstrn.end = DateTime.Today;
                else
                    db.Registrations.Remove(regstrn);
                db.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = id });
        }

        // GET: /Class/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = await db.Sections.FindAsync(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: /Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Section section = await db.Sections.FindAsync(id);
            db.Sections.Remove(section);
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
