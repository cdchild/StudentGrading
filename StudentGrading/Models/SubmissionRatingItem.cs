using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    public class SubmissionRatingItem
    {
        [Key]
        public virtual long id { get; set; }

        //the grader/rater registration
        [Required]
        [ForeignKey("registration")]
        [DisplayName("Grader")]
        public virtual long registrationId { get; set; }
        public virtual Registration registration { get; set; }

        //the submission rated
        [Required]
        [ForeignKey("submission")]
        [DisplayName("Submission")]
        public virtual long submissionId { get; set; }
        public virtual Submission submission { get; set; }

        //the particular rating chosen, defining how many points are earned for this aspect of the ruberic, by default the points are tied to the rating
        [Required]
        [ForeignKey("ratingAspect")]
        [DisplayName("Rating")]
        public virtual int ratingAspectId { get; set; }
        public virtual RatingAspect ratingAspect { get; set; }

        //although it may be desireable to override or manually key in the points for this particular aspect of the ruberic
        [DisplayName("Manual Points")]
        public virtual double? ratingPointsOverride { get; set; }

        //comments if allowed by assignment
        [DisplayName("Comment")]
        [Description("Please limit comments to directly answering these two questions for this aspect:  1. What specifically was done well? 2. What could/should have been done to get the best rating?")]
        public virtual string comment { get; set; }
    }
}