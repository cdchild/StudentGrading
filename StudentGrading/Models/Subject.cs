using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    public class Subject
    {
        [Key]
        public virtual short id { get; set; }

        //ex: Information Systems, Computer Science, Accounting...
        [Required]
        [DisplayName("Name")]
        public virtual string name { get; set; }

        // ex: INFO, COMP, CS, ACC, BIOL, MATH...
        //[Required]
        [DisplayName("Code")]
        [RegularExpression(@"[A-Z]{2,6}", ErrorMessage="Only (between 2 and 6) capital letters can compose a subject code.")]
        [StringLength(6, MinimumLength = 2)]
        [Description("Given the following:  INFO 3420- 601  the subject code would be: INFO.")]
        public virtual string abbr { get; set; }

        public string dispFull
        {
            get
            {
                return abbr + "- " + name;
            }
        }

        public virtual List<Course> Courses { get; set; }
    }
}