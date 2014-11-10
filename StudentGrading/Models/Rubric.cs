using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    public class Rubric
    {
        [Key]
        public virtual int id { get; set; }

        [Required]
        [ForeignKey("owningUser")]
        [DisplayName("Owner")]
        public virtual string owningUserId { get; set; }
        public virtual ApplicationUser owningUser { get; set; }

        [Required]
        [DisplayName("Name")]
        public virtual string name { get; set; }

        [DisplayName("Aspects")]
        public virtual List<RubricAspect> aspects { get; set; }

        [DisplayName("Global")]
        public virtual bool global { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime created { get; set; }
    }
}