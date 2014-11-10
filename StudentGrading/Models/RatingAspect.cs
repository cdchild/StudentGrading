using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    //The columns of a ruberic
    //Each ruberic item/aspect can have multiple choices for every given assignment
    //This is the object to define exactly how an assignment can be rated
    public class RatingAspect
    {
        [Key]
        public virtual int id { get; set; }

        [Required]
        [ForeignKey("rubricAspect")]
        [DisplayName("Subject")]
        public virtual int rubricAspectId { get; set; }
        public virtual RubricAspect rubricAspect { get; set; }

        [Required]
        [DisplayName("Description")]
        [Description("This should describe and contain qualifiers to help a grader easily identify which rating to choose for this aspect.")]
        public virtual string qualifyingDescription { get; set; }

        [Required]
        [DisplayName("Points")]
        public virtual double points { get; set; }
    }
}