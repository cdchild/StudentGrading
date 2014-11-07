using Microsoft.AspNet.Identity.EntityFramework;
using StudentGrading.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace StudentGrading.Models
{
    // In addition to the user fields included with this version of the identity model include an email, first name, and last name
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("UVID")]
        public override string UserName
        {
            get
            {
                return base.UserName;
            }
            set
            {
                base.UserName = value;
            }
        }

        public virtual List<IdentityUserRole> UserRoles
        {
            get
            {
                return (List<IdentityUserRole>)base.Roles;
            }
        }

        [DisplayName("First Name")]
        public virtual string firstName { get; set; }

        [DisplayName("Last Name")]
        public virtual string lastName { get; set; }
        
        [DisplayName("Email Address")]
        [EmailAddress]
        public virtual string email { get; set; }

        [DisplayName("User")]
        [ReadOnly(true)]
        [ScaffoldColumn(false)]
        public string dispFull
        {
            get
            {
                return lastName + ", " + firstName + " (" + UserName + ")";
            }
        }

        public UserViewModel ToUserViewModel()
        {
            return new UserViewModel
            {
                id = Id,
                UserName = UserName,
                Password = UserViewModel.maskedPassword,
                ConfirmPassword = UserViewModel.maskedPassword,
                firstName = firstName,
                lastName = lastName,
                email = email,
                IsRegistrar = UserRoles.Exists(u => u.UserId == Id && u.Role.Name == Role.Registrar.ToString()),
                UserRoles = UserRoles
            };
        }
    }
}