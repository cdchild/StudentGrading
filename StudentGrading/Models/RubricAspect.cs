using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    //the rows of a ruberic, defining the things in an assignment to be rated
    public class RubricAspect
    {
        [Key]
        public virtual int id { get; set; }

        [Required]
        [DisplayName("Rubric")]
        [ForeignKey("rubric")]
        public virtual int rubricId { get; set; }
        public virtual Rubric rubric { get; set; }

        [Required]
        [DisplayName("Order")]
        public virtual short order { get; set; }

        [Required]
        [DisplayName("Aspect")]
        public virtual string aspect { get; set; }

        [DisplayName("Ratings")]
        public virtual List<RatingAspect> ratingAspects { get; set; }
    }
}