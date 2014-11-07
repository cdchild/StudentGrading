using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    public class Course
    {

        public virtual int id { get; set; }

        [Required]
        [ForeignKey("subject")]
        [DisplayName("Subject")]
        public virtual short subjectId { get; set; }
        public virtual Subject subject { get; set; }

        // Web Applications I, Principles of Information Systems, Macro-Economics, Accounting II
        [Required]
        [DisplayName("Title")]
        public virtual string title { get; set; }

        // 3420, 1000, 1050, 2200...
        [Required]
        [DisplayName("Course #")]
        [Description("Given the following:  INFO 3420- 601, the course number would be 3420.")]
        public virtual short number { get; set; }

        // property to display the course in short format, ex: INFO 3420
        [DisplayName("Subject Course")]
        [ReadOnly(true)]
        [ScaffoldColumn(false)]
        public string dispShort
        {
            get
            {
                return subject.abbr + " " + number.ToString();
            }
        }

        // property to display the course in short format, ex: INFO 3420
        [DisplayName("Subject Course")]
        [ReadOnly(true)]
        [ScaffoldColumn(false)]
        public string dispShortWithTitle
        {
            get
            {
                return dispShort + ": "  + title;
            }
        }
        
        public List<Section> Sections { get; set; }
    }
}