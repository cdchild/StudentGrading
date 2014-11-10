using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    public class Registration
    {
        [Key]
        public virtual long id { get; set; }

        [Required]
        [ForeignKey("user")]
        [DisplayName("User")]
        [RegularExpression(@"[a-f\d]{8}-([a-f\d]{4}-){3}[a-f\d]{12}")]
        public virtual string userId { get; set; }
        [DisplayName("User")]
        public virtual ApplicationUser user { get; set; }

        [Required]
        [ForeignKey("section")]
        [DisplayName("Class/Section")]
        public virtual int sectionId { get; set; }
        public virtual Section section { get; set; }

        [Required]
        [ForeignKey("role")]
        [DisplayName("Role")]
        public virtual string roleId { get; set; }
        public virtual IdentityRole role { get; set; }

        [Required]
        [DisplayName("Begin")]
        [DataType(DataType.Date)]
        public virtual DateTime begin { get; set; }

        [Required]
        [DisplayName("End")]
        [DataType(DataType.Date)]
        public virtual DateTime end { get; set; }
    }
}