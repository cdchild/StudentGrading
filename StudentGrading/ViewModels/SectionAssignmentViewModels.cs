using StudentGrading.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentGrading.ViewModels
{
    public class SectionAssignmentViewModel
    {
        public virtual int id { get; set; }

        [Required]
        [ForeignKey("assignment")]
        [DisplayName("Assignment")]
        public virtual int assignmentId { get; set; }
        public virtual Assignment assignment { get; set; }

        [Required]
        [ForeignKey("section")]
        [DisplayName("Section")]
        public virtual int sectionId { get; set; }
        public virtual Section section { get; set; }

        [DisplayName("Due Date")]
        [Description("Can be left blank if not necessary for rating.")]
        [UIHint("Date")]
        public virtual DateTime? dueDate { get; set; }

        [UIHint("Time")]
        [DisplayName("Due Time")]
        public virtual DateTime? dueTime { get; set; }

        [Required]
        [DisplayName("Grading Start Date")]
        [UIHint("Date")]
        public virtual DateTime gradingStart { get; set; }

        [Required]
        [DisplayName("Grading Start Time")]
        [UIHint("Time")]
        public virtual DateTime gradingStartTime { get; set; }

        [Required]
        [UIHint("Date")]
        [DisplayName("Grading End Date")]
        public virtual DateTime gradingEnd { get; set; }

        [Required]
        [DisplayName("Grading End Time")]
        [UIHint("Time")]
        public virtual DateTime gradingEndTime { get; set; }
    }
}