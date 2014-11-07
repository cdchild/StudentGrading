using Microsoft.AspNet.Identity.EntityFramework;

namespace StudentGrading.Models
{
    public class ApplicationRole : IdentityRole
    {

        private string p;

        public ApplicationRole()
        {
            base.MemberwiseClone();
        }
        public ApplicationRole(string p)
        {
            this.p = base.Name;
        }
    }
}