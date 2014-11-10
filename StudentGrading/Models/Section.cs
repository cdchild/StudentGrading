using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    public class Section
    {
        public virtual int id { get; set; }

        [Required]
        [DisplayName("Course")]
        [ForeignKey("course")]
        public virtual int courseId { get; set; }
        public virtual Course course { get; set; }

        [Required]
        [DisplayName("Number")]
        //[StringLength(5, MinimumLength = 2)]
        [RegularExpression(@"[a-zA-Z0-9]{2,5}", ErrorMessage = "Given the following:  INFO 3420- 601, the section number would be 601.")]
        [Description("Given the following:  INFO 3420- 601, the section number would be 601.")]
        public virtual string code { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [UIHint("ShortDate")]
        [DisplayName("Begin Date")]
        public virtual DateTime begin { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [UIHint("ShortDate")]
        [DisplayName("End Date")]
        public virtual DateTime end { get; set; }

        public virtual List<Registration> Registrations { get; set; }

        [DisplayName("Course Section")]
        [ReadOnly(true)]
        [ScaffoldColumn(false)]
        public string dispShort
        {
            get
            {
                return course.dispShort + "- " + code + " (" + begin.Month + "-" + begin.Year + ")";
            }
        }
    }
}