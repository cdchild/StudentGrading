    using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Data.Entity.Validation;

namespace StudentGrading.Models
{
    public class DBInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        //define default admin user and password
        private const short defAdminUserId = 0;
        private const string defAdminUserName = "admin";
        private const string defAdminPassword = "testing123TESTING";

        protected override void Seed(ApplicationDbContext db)
        {
            /*-------------------------------------------*
             *                                           *
             *            Seed User & Roles              *
             *                                           *
             *-------------------------------------------*
             */
            //create a usermanager identity object/context with the users in the database
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //create a rolemanager identity object for the roles in the db
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));


            //create needed roles
            //Enumerate an enum: http://stackoverflow.com/questions/105372/how-do-i-enumerate-an-enum-in-c
            //If casted into an array the compiler apparently doesn't have to convert the entire enum into an array, it just casts each entry into a Role
            foreach (Role role in (Role[])Enum.GetValues(typeof(Role)))
            {
                if (!RoleManager.RoleExists(role.ToString()))
                    RoleManager.Create(new IdentityRole(role.ToString()));
            }

            //Create default user

            var user = new ApplicationUser();
            user.UserName = defAdminUserName;
            //this still isn't being created....
            user.Id = defAdminUserId.ToString();
            user.email = "admin@localhost.local";
            user.firstName = "Sally";
            user.lastName = "Registrarino";
            try
            {
                var addResult = UserManager.Create(user, defAdminPassword);
                //Add default user to admin roles
                if (addResult.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, Role.Registrar.ToString());
                }
            }
            catch (DbEntityValidationException)
            {
                return;
            }

            
            //check if it already exists by seeing if id lookup returns different type of object as the user
            if (UserManager.FindById<ApplicationUser>(defAdminUserId.ToString()).GetType().ToString() != user.GetType().ToString())
            {
                return;
            }


            /*-------------------------------------------*
             *                                           *
             *             Seed TEST USERS               *
             *                                           *
             *-------------------------------------------*
             */
            UserManager.Create(new ApplicationUser() { UserName = "12345655", firstName = "John", lastName = "Student", email = "stuj@test.com" }, "11223344mnop");
            UserManager.Create(new ApplicationUser() { UserName = "12345678", firstName = "Joe", lastName = "Professor", email = "jb@test.com" }, "11223344mnop");

            /*-------------------------------------------*
             *                                           *
             *              Seed Subjects                *
             *                                           *
             *-------------------------------------------*
             */

            db.Subjects.Add(new Subject { abbr = "ACC", name = "Accounting" });
            db.Subjects.Add(new Subject { abbr = "AERO", name = "Aerospace Studies" });
            db.Subjects.Add(new Subject { abbr = "AMST", name = "American Studies" });
            db.Subjects.Add(new Subject { abbr = "ANTH", name = "Anthropology" });
            db.Subjects.Add(new Subject { abbr = "APPR", name = "Apprentice" });
            db.Subjects.Add(new Subject { abbr = "ARCH", name = "Archaeology" });
            db.Subjects.Add(new Subject { abbr = "ART", name = "Art" });
            db.Subjects.Add(new Subject { abbr = "ARTH", name = "Art History" });
            db.Subjects.Add(new Subject { abbr = "ASL", name = "American Sign Language" });
            db.Subjects.Add(new Subject { abbr = "ASTR", name = "Astronomy" });
            db.Subjects.Add(new Subject { abbr = "AUT", name = "Auto Mechanics" });
            db.Subjects.Add(new Subject { abbr = "AVSC", name = "Aviation Science" });
            db.Subjects.Add(new Subject { abbr = "BESC", name = "Behavioral Science" });
            db.Subjects.Add(new Subject { abbr = "BIOL", name = "Biology" });
            db.Subjects.Add(new Subject { abbr = "BIT", name = "Building Inspection Technology" });
            db.Subjects.Add(new Subject { abbr = "BMED", name = "Business/Marketing Education" });
            db.Subjects.Add(new Subject { abbr = "BOT", name = "Botany" });
            db.Subjects.Add(new Subject { abbr = "BTEC", name = "Biotechnology" });
            db.Subjects.Add(new Subject { abbr = "CA", name = "Culinary Arts" });
            db.Subjects.Add(new Subject { abbr = "CAW", name = "Cabinetry and Architectural Woodwork" });
            db.Subjects.Add(new Subject { abbr = "CHEM", name = "Chemistry" });
            db.Subjects.Add(new Subject { abbr = "CHIN", name = "Chinese" });
            db.Subjects.Add(new Subject { abbr = "CHST", name = "Chinese Studies" });
            db.Subjects.Add(new Subject { abbr = "CINE", name = "Cinema Studies" });
            db.Subjects.Add(new Subject { abbr = "CJ", name = "Criminal Justice" });
            db.Subjects.Add(new Subject { abbr = "CLSS", name = "College Success Studies" });
            db.Subjects.Add(new Subject { abbr = "CLST", name = "Classical Studies" });
            db.Subjects.Add(new Subject { abbr = "CMGT", name = "Construction Management" });
            db.Subjects.Add(new Subject { abbr = "CNST", name = "Constitutional Studies" });
            db.Subjects.Add(new Subject { abbr = "COMM", name = "Communication" });
            db.Subjects.Add(new Subject { abbr = "COMP", name = "Computing" });
            db.Subjects.Add(new Subject { abbr = "CRT", name = "Collision Repair Technology" });
            db.Subjects.Add(new Subject { abbr = "CS", name = "Computer Science" });
            db.Subjects.Add(new Subject { abbr = "DANC", name = "Dance" });
            db.Subjects.Add(new Subject { abbr = "DENT", name = "Dental Hygiene" });
            db.Subjects.Add(new Subject { abbr = "DGM", name = "Digital Media" });
            db.Subjects.Add(new Subject { abbr = "DMT", name = "Diesel Mechanics" });
            db.Subjects.Add(new Subject { abbr = "EART", name = "Electrical Automation and Robotics Technology" });
            db.Subjects.Add(new Subject { abbr = "ECFS", name = "Education, Child, and Family Studies" });
            db.Subjects.Add(new Subject { abbr = "ECON", name = "Economics" });
            db.Subjects.Add(new Subject { abbr = "EDEC", name = "Early Childhood Education" });
            db.Subjects.Add(new Subject { abbr = "EDEL", name = "Elementary Education" });
            db.Subjects.Add(new Subject { abbr = "EDSC", name = "Secondary Education" });
            db.Subjects.Add(new Subject { abbr = "EDSP", name = "Special Education" });
            db.Subjects.Add(new Subject { abbr = "EDUC", name = "Education" });
            db.Subjects.Add(new Subject { abbr = "EENG", name = "Electrical Engineering" });
            db.Subjects.Add(new Subject { abbr = "EGDT", name = "Engineering Graphics and Design Technology" });
            db.Subjects.Add(new Subject { abbr = "ENGH", name = "English - Basic Composition" });
            db.Subjects.Add(new Subject { abbr = "ENGL", name = "English" });
            db.Subjects.Add(new Subject { abbr = "ENGR", name = "Engineering" });
            db.Subjects.Add(new Subject { abbr = "ENST", name = "Environmental Studies" });
            db.Subjects.Add(new Subject { abbr = "ENVT", name = "Environmental Management" });
            db.Subjects.Add(new Subject { abbr = "ES", name = "Emergency Services" });
            db.Subjects.Add(new Subject { abbr = "ESAF", name = "Emergency Services - Aircraft Rescue Firefighting" });
            db.Subjects.Add(new Subject { abbr = "ESEC", name = "Emergency Services - Emergency Care" });
            db.Subjects.Add(new Subject { abbr = "ESFF", name = "Emergency Services - Firefighting" });
            db.Subjects.Add(new Subject { abbr = "ESFO", name = "Emergency Services - Fire Officer" });
            db.Subjects.Add(new Subject { abbr = "ESL", name = "English as a Second Language" });
            db.Subjects.Add(new Subject { abbr = "ESMG", name = "Emergency Services - Management" });
            db.Subjects.Add(new Subject { abbr = "ESWF", name = "Emergency Services - Wildland Firefighter" });
            db.Subjects.Add(new Subject { abbr = "EXSC", name = "Exercise Sience" });
            db.Subjects.Add(new Subject { abbr = "FAC", name = "Facilities Management" });
            db.Subjects.Add(new Subject { abbr = "FAMS", name = "Family Studies" });
            db.Subjects.Add(new Subject { abbr = "FAMT", name = "Fine Arts, Music, and Theater" });
            db.Subjects.Add(new Subject { abbr = "FIN", name = "Finance" });
            db.Subjects.Add(new Subject { abbr = "FREN", name = "French" });
            db.Subjects.Add(new Subject { abbr = "FSCI", name = "Forensic Science" });
            db.Subjects.Add(new Subject { abbr = "GEO", name = "Geology" });
            db.Subjects.Add(new Subject { abbr = "GEOG", name = "Geography" });
            db.Subjects.Add(new Subject { abbr = "GER", name = "German" });
            db.Subjects.Add(new Subject { abbr = "GIS", name = "Geographic Information Systems" });
            db.Subjects.Add(new Subject { abbr = "GRK", name = "Greek" });
            db.Subjects.Add(new Subject { abbr = "HIST", name = "History" });
            db.Subjects.Add(new Subject { abbr = "HLTH", name = "Community Health" });
            db.Subjects.Add(new Subject { abbr = "HM", name = "Hospitality Management" });
            db.Subjects.Add(new Subject { abbr = "HONR", name = "Honors" });
            db.Subjects.Add(new Subject { abbr = "HUM", name = "Humanities" });
            db.Subjects.Add(new Subject { abbr = "IDST", name = "Interdisciplinary Studies Program" });
            db.Subjects.Add(new Subject { abbr = "INFO", name = "Information Systems and Technology" });
            db.Subjects.Add(new Subject { abbr = "IS", name = "Integrated Studies" });
            db.Subjects.Add(new Subject { abbr = "IT", name = "Information Technology" });
            db.Subjects.Add(new Subject { abbr = "JPNS", name = "Japanese" });
            db.Subjects.Add(new Subject { abbr = "LANG", name = "Languages" });
            db.Subjects.Add(new Subject { abbr = "LATN", name = "Latin" });
            db.Subjects.Add(new Subject { abbr = "LEGL", name = "Legal Studies" });
            db.Subjects.Add(new Subject { abbr = "MAT", name = "Mathematics - Developmental" });
            db.Subjects.Add(new Subject { abbr = "MATH", name = "Mathematics" });
            db.Subjects.Add(new Subject { abbr = "MECH", name = "Mechatronics Engineering Technology" });
            db.Subjects.Add(new Subject { abbr = "METO", name = "Meteorology" });
            db.Subjects.Add(new Subject { abbr = "MGMT", name = "Business Management" });
            db.Subjects.Add(new Subject { abbr = "MICR", name = "Microbiology" });
            db.Subjects.Add(new Subject { abbr = "MILS", name = "Military Science" });
            db.Subjects.Add(new Subject { abbr = "MKTG", name = "Marketing" });
            db.Subjects.Add(new Subject { abbr = "MUSC", name = "Music" });
            db.Subjects.Add(new Subject { abbr = "NURS", name = "Nursing" });
            db.Subjects.Add(new Subject { abbr = "NUTR", name = "Nutrition" });
            db.Subjects.Add(new Subject { abbr = "PES", name = "Physical Education Sports" });
            db.Subjects.Add(new Subject { abbr = "PETE", name = "Physical Education Teacher Education" });
            db.Subjects.Add(new Subject { abbr = "PHIL", name = "Philosophy" });
            db.Subjects.Add(new Subject { abbr = "PHSC", name = "Physical Science" });
            db.Subjects.Add(new Subject { abbr = "PHYS", name = "Physics" });
            db.Subjects.Add(new Subject { abbr = "PJST", name = "Peace and Justice Studies" });
            db.Subjects.Add(new Subject { abbr = "POLS", name = "Political Science" });
            db.Subjects.Add(new Subject { abbr = "PORT", name = "Portuguese" });
            db.Subjects.Add(new Subject { abbr = "PSY", name = "Psychology" });
            db.Subjects.Add(new Subject { abbr = "REC", name = "Recreation" });
            db.Subjects.Add(new Subject { abbr = "RLST", name = "Religious Studies" });
            db.Subjects.Add(new Subject { abbr = "RUS", name = "Russian" });
            db.Subjects.Add(new Subject { abbr = "SOC", name = "Sociology" });
            db.Subjects.Add(new Subject { abbr = "SOSC", name = "Social Science" });
            db.Subjects.Add(new Subject { abbr = "SPAN", name = "Spanish" });
            db.Subjects.Add(new Subject { abbr = "SURV", name = "Land Surveying" });
            db.Subjects.Add(new Subject { abbr = "SW", name = "Social Work" });
            db.Subjects.Add(new Subject { abbr = "TECH", name = "Technology Management" });
            db.Subjects.Add(new Subject { abbr = "THEA", name = "Theatre" });
            db.Subjects.Add(new Subject { abbr = "UVST", name = "University Studies" });
            db.Subjects.Add(new Subject { abbr = "ZOOL", name = "Zoology" });
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                return;
            }

            /*-------------------------------------------*
             *                                           *
             *             Seed TEST DATA                *
             *                                           *
             *-------------------------------------------*
             */
            Subject subject1 = db.Subjects.First(s => s.abbr == "INFO");
            db.Courses.Add(new Course { number = 3410, title = "Data Warehousing", subject = subject1, subjectId = subject1.id });
            db.Courses.Add(new Course { number = 3420, title = "Web Application Systems", subject = subject1, subjectId = subject1.id });
            db.Courses.Add(new Course { number = 3120, title = "Information Systems Principles", subject = subject1, subjectId = subject1.id });
            string rp = Role.Professor.ToString();
            string rs = Role.Student.ToString();
            IdentityRole r1 = db.Roles.First(r => r.Name == rp);
            ApplicationUser u1 = db.Users.First(u => u.lastName == "Professor");
            IdentityRole r2 = db.Roles.First(r => r.Name == rs);
            ApplicationUser u2 = db.Users.First(u => u.lastName == "Student");
            DateTime b = new System.DateTime(2014, 1, 1).Date;
            DateTime e = new System.DateTime(2014, 5, 2).Date;

            Course course1 = db.Courses.Local.First(c => c.number == 3410);
            db.Sections.Add(
                new Section {
                    courseId = course1.id,
                    course = course1,
                    begin = b,
                    end = e,
                    code = "001"
                }
            );
            Section s1 = db.Sections.Local.First(s => s.course == course1 && s.begin == b && s.end == e);
            db.Registrations.Add(
                new Registration
                {
                    begin = b,
                    end = e,
                    roleId = r1.Id,
                    role = r1,
                    userId = u1.Id,
                    user = u1,
                    section = s1,
                    sectionId = s1.id
                }
            );
            db.Registrations.Add(
                new Registration
                {
                    begin = b,
                    end = e,
                    roleId = r2.Id,
                    role = r2,
                    userId = u2.Id,
                    user = u2,
                    section = s1,
                    sectionId = s1.id
                }
            );
            UserManager.AddToRole(u1.Id, r1.Name);
            UserManager.AddToRole(u2.Id, r2.Name);
            Course course2 = db.Courses.Local.First(c => c.number == 3120);
            db.Sections.Add(
                new Section
                {
                    courseId = course2.id,
                    course = course2,
                    begin = b,
                    end = e,
                    code = "004"
                }
            );
            Section s2 = db.Sections.Local.First(s => s.course == course2 && s.begin == b && s.end == e);
            db.Registrations.Add(
                new Registration
                {
                    begin = b,
                    end = e,
                    roleId = r1.Id,
                    role = r1,
                    userId = u1.Id,
                    user = u1,
                    section = s2,
                    sectionId = s2.id
                }
            );
            db.Registrations.Add(
                new Registration
                {
                    begin = b,
                    end = e,
                    roleId = r2.Id,
                    role = r2,
                    userId = u2.Id,
                    user = u2,
                    section = s2,
                    sectionId = s2.id
                }
            );

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                return;
            }


            base.Seed(db);
        }
    }
}