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

namespace StudentGrading.Controllers
{
    [Authorize(Roles = "Professor")]
    public class SectionAssignmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //set a temp role var from the roles enum
        private string professorRole = Role.Professor.ToString();

        // GET: /SectionAssignment/5
        // must have a parameter as id to narrow down the list of section assignments to just one assignment
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.assignmentId = id;
            var sectionassignments = db.SectionAssignments.Include(s => s.assignment).Include(s => s.section).Include(s => s.section.course).Where(sa => sa.assignmentId == id);
            return View(await sectionassignments.ToListAsync());
        }

        // GET: /SectionAssignment/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectionAssignment sectionassignment = await db.SectionAssignments.FindAsync(id);
            if (sectionassignment == null)
            {
                return HttpNotFound();
            }
            return View(sectionassignment);
        }

        // GET: /SectionAssignment/Create
        public ActionResult Create()
        {
            // get current user so we can obtain lists narrowed down for them
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);
            // send in the blind flag to tell view to render select lists for assignment and section ids
            ViewBag.Blind = true;
            //selectlists for this users' created assignments
            ViewBag.assignmentId = new SelectList(db.Assignments.Where(a => a.owningUser.Id == currUser.Id), "id", "name");
            //selectlist for this users' sections
            ViewBag.sectionId = new SelectList(
                db.Sections.Where(
                    s => s.Registrations.Any(r => r.user.Id == currUser.Id && r.role.Name == professorRole)
                ), "id", "dispShort");
            return View();
        }

        // GET: /SectionAssignment/Assign
        // Another create method that accepts input for the assignment as id
        public ActionResult Assign(int id)
        {
            // get current user so we can obtain lists narrowed down for them
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);
            //mark the blind flag as false so view knows to render hidden element, it doesn't have to act blind
            ViewBag.Blind = false;
            // send in the section and assignment ids in the viewbag
            ViewBag.assignmentId = id;
            //create var to be used in the linq query below when narrowing list of sections available to assign
            var assignment = db.Assignments.Find(id);
            //selectlist for this users' sections for this course
            ViewBag.sectionId = new SelectList(
                db.Sections.Where(
                    s => s.Registrations.Any(r => r.user.Id == currUser.Id && r.role.Name == professorRole)
                    && s.courseId == assignment.courseId
                ), "id", "dispShort");
            return View("Create");
        }

        // POST: /SectionAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "assignmentId,sectionId,dueDate,gradingStart,gradingEnd")] SectionAssignmentViewModel secAsnModel)
        {
            if (ModelState.IsValid)
            {
                //create section assignment from the view model
                SectionAssignment sectionassignment = new SectionAssignment
                {
                    assignment = db.SectionAssignments.Find(secAsnModel.id).assignment,
                    assignmentId = db.SectionAssignments.Find(secAsnModel.id).assignmentId,
                    dueDate = Helpers.CombineDate1Time2(secAsnModel.dueDate, secAsnModel.dueTime),
                    gradingEnd = Helpers.CombineDate1Time2(secAsnModel.gradingEnd, secAsnModel.gradingEndTime),
                    gradingStart = Helpers.CombineDate1Time2(secAsnModel.gradingStart, secAsnModel.gradingStartTime),
                    section = db.SectionAssignments.Find(secAsnModel.id).section,
                    sectionId = db.SectionAssignments.Find(secAsnModel.id).sectionId
                };
                //save to db
                db.SectionAssignments.Add(sectionassignment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = sectionassignment.assignmentId });
            }

            // get current user so we can obtain lists narrowed down for them
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);

            //create var to be used in the linq query below when narrowing list of sections available to assign
            var assignment = db.Assignments.Find(secAsnModel.assignmentId);
            //selectlist for this users' sections for this course
            ViewBag.sectionId = new SelectList(
                db.Sections.Where(
                    s => s.Registrations.Any(r => r.user.Id == currUser.Id && r.role.Name == professorRole)
                    && s.courseId == assignment.courseId
                ), "id", "dispShort");
            return View("Assign", secAsnModel.assignmentId);
        }

        // GET: /SectionAssignment/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectionAssignment sectionassignment = await db.SectionAssignments.FindAsync(id);
            if (sectionassignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.assignmentId = new SelectList(db.Assignments, "id", "name", sectionassignment.assignmentId);
            // get current user so we can obtain lists narrowed down for them
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);

            //create var to be used in the linq query below when narrowing list of sections available to assign
            var assignment = db.Assignments.Find(sectionassignment.assignmentId);
            //selectlist for this users' sections for this course
            ViewBag.sectionId = new SelectList(
                db.Sections.Where(
                    s => s.Registrations.Any(r => r.user.Id == currUser.Id && r.role.Name == professorRole)
                    && s.courseId == assignment.courseId
                ), "id", "dispShort", sectionassignment.sectionId);
            return View(sectionassignment.ToSectionAssignmentViewModel());
        }

        // POST: /SectionAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,dueDate,gradingStart,gradingEnd,dueTime,gradingStartTime,gradingEndTime")] SectionAssignmentViewModel secAsnModel)
        {
            if (ModelState.IsValid)
            {
                //create section assignment object from view model
                SectionAssignment sectionassignment = new SectionAssignment
                {
                    id = secAsnModel.id,
                    assignment = db.SectionAssignments.Find(secAsnModel.id).assignment,
                    assignmentId = db.SectionAssignments.Find(secAsnModel.id).assignmentId,
                    dueDate = Helpers.CombineDate1Time2(secAsnModel.dueDate, secAsnModel.dueTime),
                    gradingEnd = Helpers.CombineDate1Time2(secAsnModel.gradingEnd, secAsnModel.gradingEndTime),
                    gradingStart = Helpers.CombineDate1Time2(secAsnModel.gradingStart, secAsnModel.gradingStartTime),
                    section = db.SectionAssignments.Find(secAsnModel.id).section,
                    sectionId = db.SectionAssignments.Find(secAsnModel.id).sectionId
                };
                db.Entry(sectionassignment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = sectionassignment.assignmentId });
            }
            ViewBag.assignmentId = new SelectList(db.Assignments, "id", "name", secAsnModel.assignmentId);
            // get current user so we can obtain lists narrowed down for them
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);

            //create var to be used in the linq query below when narrowing list of sections available to assign
            var assignment = db.Assignments.Find(secAsnModel.assignmentId);
            //selectlist for this users' sections for this course
            ViewBag.sectionId = new SelectList(
                db.Sections.Where(
                    s => s.Registrations.Any(r => r.user.Id == currUser.Id && r.role.Name == professorRole)
                    && s.courseId == assignment.courseId
                ), "id", "dispShort", secAsnModel.sectionId);
            return View(secAsnModel);
        }

        // GET: /SectionAssignment/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectionAssignment sectionassignment = await db.SectionAssignments.FindAsync(id);
            if (sectionassignment == null)
            {
                return HttpNotFound();
            }
            return View(sectionassignment);
        }

        // POST: /SectionAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SectionAssignment sectionassignment = await db.SectionAssignments.FindAsync(id);
            db.SectionAssignments.Remove(sectionassignment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { id = sectionassignment.assignmentId });
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
