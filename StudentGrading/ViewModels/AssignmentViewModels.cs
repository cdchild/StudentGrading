using StudentGrading.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.ViewModels
{
    public class AddAssignmentViewModel
    {
        [Key]
        public virtual int id { get; set; }

        [Required]
        [ForeignKey("rubric")]
        [DisplayName("Rubric")]
        public virtual int rubricId { get; set; }
        public virtual Rubric rubric { get; set; }

        [Required]
        [ForeignKey("section")]
        [DisplayName("Section")]
        public virtual int sectionId { get; set; }
        public virtual Section section { get; set; }

        [DisplayName("Due Date")]
        [UIHint("Date")]
        [DataType(DataType.Date)]
        [Description("Can be left blank if not necessary for rating.")]
        public virtual DateTime? dueDate { get; set; }
        [UIHint("Time")]
        [DataType(DataType.Time)]
        public virtual DateTime? dueTime { get; set; }

        [Required]
        [DisplayName("Grading Start")]
        [UIHint("Date3")]
        [DataType(DataType.Date)]
        public virtual DateTime gradingStart { get; set; }
        [Required]
        [UIHint("Time")]
        [DataType(DataType.Time)]
        public virtual DateTime gradingStartTime { get; set; }

        [Required]
        [DisplayName("Grading End")]
        [UIHint("Date3")]
        [DataType(DataType.Date)]
        public virtual DateTime gradingEnd { get; set; }
        [Required]
        [UIHint("Time")]
        [DataType(DataType.Time)]
        public virtual DateTime gradingEndTime { get; set; }

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
    }

    public class AssignmentViewModel
    {

        public virtual int id { get; set; }

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

        [DisplayName("Sections")]
        [Description("Sections this assignment is given.")]
        public virtual List<SectionAssignment> sectionAssignments { get; set; }

    }
}