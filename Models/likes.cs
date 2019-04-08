using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace BeltExam.Models
{
    public class Like
    {
        [Key]
        public int likeid {get;set;}

        public int userid {get;set;}
        [ForeignKey("userid")]
        public User User {get;set;}
        public int postid {get;set;}
        [ForeignKey("postid")]
        public Post Post {get;set;}
        
       
    }
}