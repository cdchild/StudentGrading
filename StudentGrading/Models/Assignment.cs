using StudentGrading.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    public class Assignment
    {
        [Key]
        public virtual int id { get; set; }

        [Required]
        [ForeignKey("owningUser")]
        [DisplayName("Owner")]
        public virtual string owningUserId { get; set; }
        public virtual ApplicationUser owningUser { get; set; }

        [Required]
        [ForeignKey("course")]
        [DisplayName("Course")]
        public virtual int courseId { get; set; }
        public virtual Course course { get; set; }

        [Required]
        [ForeignKey("rubric")]
        [DisplayName("Rubric")]
        public virtual int rubricId { get; set; }
        public virtual Rubric rubric { get; set; }

        [DisplayName("Sections")]
        [Description("Sections this assignment is given.")]
        public virtual List<SectionAssignment> sectionAssignments { get; set; }

        [Required]
        [DisplayName("Name")]
        public virtual string name { get; set; }

        [DisplayName("Description")]
        public virtual string description { get; set; }

        [Required]
        [DisplayName("Points Possible")]
        public virtual double ptsPossible { get; set; }

        [DisplayName("Rater Visible")]
        [DefaultValue(false)]
        [Description("Allows the grader/rater name to be shown to the student.")]
        public virtual bool studentGraderVisible { get; set; }

        [DisplayName("Rubric Rating Overrides")]
        [DefaultValue(false)]
        [Description("Allows the grader to specify the exact points earned instead of the points defined in the rubric aspect rating.")]
        public virtual bool allowGraderOverride { get; set; }

        
        [DisplayName("Allow Comments")]
        [DefaultValue(false)]
        [Description("Allows the grader/rater to comment per ruberic aspect.")]
        public virtual bool allowComments { get; set; }

        public AssignmentViewModel ToAssignmentViewModel()
        {
            return new AssignmentViewModel
            {
                id = id,
                name = name,
                ptsPossible = ptsPossible,
                courseId = courseId,
                course = course,
                rubricId = rubricId,
                rubric = rubric,
                allowComments = allowComments,
                allowGraderOverride = allowComments,
                studentGraderVisible = studentGraderVisible,
                description = description,
                sectionAssignments = sectionAssignments
            };
        }
    }
}