using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BeltExam.Models;
using System.Linq;
namespace BeltExam.Models
{
    public class User
    {
        [Key]
        public int userid {get;set;}

        [Required]
        [MinLength(2)]
        public string name {get;set;}
        [Required]
        [MinLength(2)]
        public string alias {get;set;}

        [EmailAddress]
        [Required]
        public string email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [NotMapped]
        [Compare("password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string passwordConfirm { get; set; }
        
        public List<Like> Likes {get;set;}
        public User(){
            Like Likes = new Like();
        }
    }
    public class LoginUser
    {
        [EmailAddress]
        [Required]
        public string email2 { get; set; }
        [Required]
        public string password { get; set; }
    }
    
     public class ModelBundle
    {
        public User UserModel { get; set; }
        public LoginUser LoginModels { get; set; }
    }
    

}
