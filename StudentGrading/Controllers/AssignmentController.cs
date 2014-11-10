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
using Microsoft.AspNet.Identity;
using StudentGrading.ViewModels;

namespace StudentGrading.Controllers
{
    //only professors can use these methods
    [Authorize(Roles = "Professor")]
    public class AssignmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //set a temp role var from the roles enum
        private string professorRole = Role.Professor.ToString();

        // GET: /Assignment/
        public async Task<ActionResult> Index()
        {
            //get the current user object
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);
            //only select assignments from current user include the user object and rubrics
            var assignments = db.Assignments.Where(a => a.owningUser.Id == currUser.Id).Include(a => a.course).Include(a => a.owningUser).Include(a => a.rubric);
            return View(await assignments.ToListAsync());
        }

        // GET: /Assignment/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: /Assignment/Create
        public ActionResult Create()
        {
            //get current user object so we can use in building selectlists for this user
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);
            //selectlist for sections showing only those that the current user is professor
            ViewBag.sectionId = new SelectList(
                db.Sections.Where(s => s.Registrations.Any(r => r.user.Id == currUser.Id && r.role.Name == professorRole)),
                "id", "dispShort");
            //selectlist that only shows global rubrics or those owned by current user
            ViewBag.rubricId = new SelectList(db.Rubrics.Where(r => r.global || r.owningUser.Id == currUser.Id), "id", "name");
            return View();
        }

        // POST: /Assignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "id,sectionId,rubricId,name,description,dueDate,dueTime,gradingStart,gradingStartTime,gradingEnd,gradingEndTime,ptsPossible,studentGraderVisible,allowGraderOverride,allowComments")]
            AddAssignmentViewModel addAssignment)
        {
            //get current user object so we can use in building selectlists for this user
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                //from the viewmodel create the assignment object
                Assignment asn = new Assignment()
                {
                    studentGraderVisible = addAssignment.studentGraderVisible,
                    courseId = db.Sections.First(s => s.id == addAssignment.sectionId).courseId,
                    rubricId = addAssignment.rubricId,
                    ptsPossible = addAssignment.ptsPossible,
                    owningUserId = currUser.Id,
                    name = addAssignment.name,
                    description = addAssignment.description,
                    allowGraderOverride = addAssignment.allowGraderOverride,
                    allowComments = addAssignment.allowComments
                };
                //add and save the new assignment to the db
                db.Assignments.Add(asn);

                await db.SaveChangesAsync();
                //create grading start and end date objects using helper to combine fields
                DateTime addGradingStartDate = Helpers.CombineDate1Time2(addAssignment.gradingStart, addAssignment.gradingStartTime);
                DateTime addGradingEndDate = Helpers.CombineDate1Time2(addAssignment.gradingEnd, addAssignment.gradingEndTime);
                //create the assignment object to add
                SectionAssignment secAsn = new SectionAssignment()
                {
                    sectionId = addAssignment.sectionId,
                    gradingStart = addGradingStartDate,
                    gradingEnd = addGradingEndDate,
                    assignmentId = db.Assignments.First(a => a.name == asn.name && a.ptsPossible == asn.ptsPossible && a.owningUserId == currUser.Id && a.rubricId == asn.rubricId && a.courseId == asn.courseId).id
                };
                //since due date is optional apply the value of the date and time if both objects have values
                if (addAssignment.dueDate.HasValue && addAssignment.dueTime.HasValue)
                {
                    secAsn.dueDate = Helpers.CombineDate1Time2(addAssignment.dueDate.Value, addAssignment.dueTime);
                }//otherwise if only the date object has a value add just the date
                else if (addAssignment.dueDate.HasValue)
                {
                    secAsn.dueDate = addAssignment.dueDate.Value;
                }
                //add the section assignment to the db
                db.SectionAssignments.Add(secAsn);
                await db.SaveChangesAsync();
                //go to the index
                return RedirectToAction("Index");
            }
            //set a temp role var from the roles enum
            string professorRole = Role.Professor.ToString();
            //Selectlist for courses that this user is registered as professor
            ViewBag.courseId = new SelectList(
                db.Courses.Where(c => c.Sections.Any(s => s.Registrations.Any(r => r.userId == currUser.Id && r.role.Name == professorRole))),
                "id", "dispShort", addAssignment.section.courseId);
            //selectlist that only shows global rubrics or those owned by current user
            ViewBag.rubricId = new SelectList(db.Rubrics.Where(r => r.global || r.owningUserId == currUser.Id), "id", "name", addAssignment.rubricId);
            return View(addAssignment);
        }

        // GET: /Assignment/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            //get current user
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            //Selectlist for courses that this user is registered as professor
            ViewBag.courseId = new SelectList(
                db.Courses.Where(c => c.Sections.Any(s => s.Registrations.Any(r => r.userId == currUser.Id && r.role.Name == professorRole))),
                "id", "dispShort", assignment.courseId);
            //selectlist that only shows global rubrics or those owned by current user
            ViewBag.rubricId = new SelectList(db.Rubrics.Where(r => r.global || r.owningUserId == currUser.Id), "id", "name", assignment.rubricId);
            return View(assignment.ToAssignmentViewModel());
        }

        // POST: /Assignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id,courseId,rubricId,name,description,ptsPossible,studentGraderVisible,allowGraderOverride,allowComments")] AssignmentViewModel assignment)
        {
            //get current user
            ApplicationUser currUser = db.Users.First(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //Selectlist for courses that this user is registered as professor
            ViewBag.courseId = new SelectList(
                db.Courses.Where(c => c.Sections.Any(s => s.Registrations.Any(r => r.userId == currUser.Id && r.role.Name == professorRole))),
                "id", "dispShort", assignment.courseId);
            //selectlist that only shows global rubrics or those owned by current user
            ViewBag.rubricId = new SelectList(db.Rubrics.Where(r => r.global || r.owningUserId == currUser.Id), "id", "name", assignment.rubricId);
            return View(assignment);
        }

        // GET: /Assignment/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: /Assignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Assignment assignment = await db.Assignments.FindAsync(id);
            db.Assignments.Remove(assignment);
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
