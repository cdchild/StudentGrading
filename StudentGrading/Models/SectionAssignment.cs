using StudentGrading.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    public class SectionAssignment
    {
        [Key]
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
        public virtual DateTime? dueDate { get; set; }

        [Required]
        [DisplayName("Grading Start")]
        public virtual DateTime gradingStart { get; set; }

        [Required]
        [DisplayName("Grading End")]
        public virtual DateTime gradingEnd { get; set; }

        public virtual List<Submission> Submissions { get; set; }

        public SectionAssignmentViewModel ToSectionAssignmentViewModel()
        {
            return new SectionAssignmentViewModel
            {
                id = id,
                assignment = assignment,
                assignmentId = assignmentId,
                dueDate = dueDate,
                dueTime = dueDate,
                gradingEnd = gradingEnd,
                gradingEndTime = gradingEnd,
                gradingStart = gradingStart,
                gradingStartTime = gradingStart,
                section = section,
                sectionId = sectionId
            };
        }
    }
}