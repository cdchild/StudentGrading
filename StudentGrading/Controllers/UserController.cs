using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentGrading.ViewModels;
using StudentGrading.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StudentGrading.Controllers
{
    [Authorize(Roles = "Registrar")]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //create a rolemanager identity object for the roles in the db
        private RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

        //create a usermanager identity object/context with the users in the database
        private UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        // GET: /User0/
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        // GET: /User0/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user.ToUserViewModel());
            }
            catch
            {
                return HttpNotFound();
            }
        }

        // GET: /User0/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: /User0/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserName,Password,ConfirmPassword,firstName,lastName,email,IsRegistrar")] AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {   
                //create a new applicationUser from the given model
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    email = model.email
                };
                //send the model and password to the usermanager to create
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.IsRegistrar)
                    {
                        //Add user to role if the IsRegistrar checkbox is checked
                        var a = UserManager.AddToRole(UserManager.FindByName(user.UserName).Id, Role.Registrar.ToString());
                    }
                    //go to the index
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            return View(model);
        }

        // GET: /User0/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user.ToUserViewModel());
            }
            catch
            {
                return HttpNotFound();
            }
        }

        // POST: /User0/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,UserName,Password,ConfirmPassword,firstName,lastName,email,IsRegistrar")] UserViewModel userviewmodel)
        {
            if (ModelState.IsValid)
            {
                //Update the password if the const masked password as defined in view model wasn't sent in this request
                if (!userviewmodel.Password.Equals(ViewModels.UserViewModel.maskedPassword))
                {
                    try
                    {
                        //Since the identity model doesn't include option to change password without the old password, remove the old password
                        var result0 = await UserManager.RemovePasswordAsync(userviewmodel.id);
                        if (result0 == null)
                        {
                            ModelState.AddModelError("", "Unspecified error (0) updating password for user " + userviewmodel.UserName + ".");
                            return View(userviewmodel);
                        }
                        //Now add the new password
                        var result1 = await UserManager.AddPasswordAsync(userviewmodel.id, userviewmodel.Password);
                        if (result1 == null)
                        {
                            ModelState.AddModelError("", "Unspecified error (1) updating password for user " + userviewmodel.UserName + ".");
                            return View(userviewmodel);
                        }
                        if (!result1.Succeeded)
                        {
                            ModelState.AddModelError("", "Error (1) updating password for user " + userviewmodel.UserName + ".  " + result1.Errors.FirstOrDefault());
                            return View(userviewmodel);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Unspecified error updating password for user" + userviewmodel.UserName + ".");
                        return View(userviewmodel);
                    }
                }
                //try updating user properties from the view model that was passed in
                try
                {
                    //find the ApplicationUser object in the database
                    var result2 = await db.Users.FirstAsync(u => u.Id == userviewmodel.id);

                    if (result2 == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    //change properties
                    result2.Id = userviewmodel.id;
                    result2.UserName = userviewmodel.UserName;
                    result2.firstName = userviewmodel.firstName;
                    result2.lastName = userviewmodel.lastName;
                    result2.email = userviewmodel.email;

                    //save
                    if (userviewmodel.IsRegistrar && !UserManager.IsInRole(userviewmodel.id, Role.Registrar.ToString()))
                    {
                        var result4 = await UserManager.AddToRoleAsync(result2.Id, Role.Registrar.ToString());
                        if (result4 == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                    }
                    if (UserManager.IsInRole(userviewmodel.id, Role.Registrar.ToString()) && !userviewmodel.IsRegistrar)
                    {
                        var result4 = await UserManager.RemoveFromRoleAsync(result2.Id, Role.Registrar.ToString());
                        if (result4 == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                    }

                    int result3 = await db.SaveChangesAsync();

                    if (result3 != 1 && result3 != 0)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    

                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Unspecified error (2) updating this user.");
                    return View(userviewmodel);
                }

            }
            return View(userviewmodel);
        }

        // GET: /User0/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            //check if null
            if (id == null)
            {
                return HttpNotFound();
            }
            //find user entity
            var user = await UserManager.FindByIdAsync(id);
            //if no matches return an HTTP 404
            if (user == null)
            {
                return HttpNotFound();
            }
            //send the user entity converted to a viewmodel to the view
            return View(user.ToUserViewModel());
        }

        // POST: /User0/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {   
            //check if null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {   //find user entity
                ApplicationUser user = await db.Users.FirstAsync(u => u.Id == id);
                //if no matches return an HTTP 404
                if (user == null)
                {
                    return HttpNotFound();
                }
                //remove from db
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                //go to index
                return RedirectToAction("Index");
            }
            catch
            {//if there's a problem return an HTTP 404
                return HttpNotFound();
            }
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
