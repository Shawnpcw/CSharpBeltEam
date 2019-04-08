using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BeltExam.Models{
    public class Post{
        public int postid { get; set; }
        [MinLength(2)]
        [MaxLength(45)]
        [Required]
        public string text { get; set; }
        public int posterid { get; set; }
        [ForeignKey("posterid")]
        public User poster {get;set;}
        public List<Like> Likes { get; set; }
    }
}