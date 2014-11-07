using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGrading.Models
{
    public class Submission
    {
        [Key]
        public virtual long id { get; set; }

        [Required]
        [ForeignKey("sectionAssignment")]
        [DisplayName("Assignment")]
        public virtual int sectionAssignmentId { get; set; }
        public virtual SectionAssignment sectionAssignment { get; set; }

        [Required]
        [ForeignKey("registration")]
        [DisplayName("Submitter")]
        public virtual long registrationId { get; set; }
        public virtual Registration registration { get; set; }
        
        [DisplayName("URL to submission")]
        [RegularExpression(@"[a-zA-Z0-9?.+&\\/:-]", ErrorMessage = "Enter a valid URL with valid characters.")]
        public virtual string assignmentFileLocation { get; set; }
        
        [DisplayName("Filename")]
        [RegularExpression(@"[a-zA-Z0-9.-][.][a-zA-Z0-9]{1,8}",ErrorMessage="Filename formats look like: example.zip")]
        public virtual string assignmentFilename { get; set; }

        [DisplayName("Text Submission")]
        public virtual string submission { get; set; }
        /*
         * 
        [DisplayName("Comments")]
        public virtual string professorCommentToGraders { get; set; }
        
        [DisplayName("Comments")]
        public virtual string submittersComments { get; set; }
        */
    }
}