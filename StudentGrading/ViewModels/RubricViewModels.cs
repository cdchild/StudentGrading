using StudentGrading.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentGrading.ViewModels
{
    public class AddRubricViewModel
    {
        [Required]
        [DisplayName("Name")]
        public virtual string name { get; set; }
        
        [DisplayName("Available Globally")]
        public virtual bool global { get; set; }

        [DisplayName("First Criteria")]
        public virtual string aspect { get; set; }

    }
}