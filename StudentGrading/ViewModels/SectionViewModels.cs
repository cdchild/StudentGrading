using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using StudentGrading.Models;

namespace StudentGrading.ViewModels
{
    public class AddSectionViewModel
    {
        public virtual int id { get; set; }

        [Required]
        [DisplayName("Course")]
        public virtual int courseId { get; set; }

        public virtual Course course { get; set; }

        [Required]
        [DisplayName("Number")]
        //[StringLength(5, MinimumLength = 2)]
        [RegularExpression(@"[a-zA-Z0-9]{2,5}", ErrorMessage = "Given the following:  INFO 3420- 601; the section number would be 601.")]
        [Description("Given the following:  INFO 3420- 601; the section number would be 601.")]
        public virtual string code { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Begin Date")]
        public virtual DateTime begin { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public virtual DateTime end { get; set; }
        
        [DisplayName("Professor")]
        [RegularExpression(@"[a-f\d]{8}-([a-f\d]{4}-){3}[a-f\d]{12}")]
        public virtual string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
    public class SectionViewModel
    {
        public virtual int id { get; set; }

        [Required]
        [DisplayName("Course")]
        public virtual int courseId { get; set; }

        public virtual Course course { get; set; }

        [Required]
        [DisplayName("Number")]
        //[StringLength(5, MinimumLength = 2)]
        [RegularExpression(@"[a-zA-Z0-9]{2,5}", ErrorMessage = "Given the following:  INFO 3420- 601; the section number would be 601.")]
        [Description("Given the following:  INFO 3420- 601; the section number would be 601.")]
        public virtual string code { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Begin Date")]
        public virtual DateTime begin { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public virtual DateTime end { get; set; }
        
        public virtual List<Registration> Registrations { get; set; }
    }
}