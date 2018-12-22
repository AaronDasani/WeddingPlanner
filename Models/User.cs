using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User
    {
        [Key]
        public int user_id{get;set;}

        [Required]
        [MinLength(2,ErrorMessage="First Name should be atleast 2 chracters")]
        [Display(Name="First Name")]
        public string firstname{get;set;}

        [Required]
        [MinLength(2,ErrorMessage="Last Name should be atleast 2 chracters")]
        [Display(Name="Last Name")]
        public string lastname{get;set;}

        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string email{get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string password{get;set;}

        [Required]
        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("password",ErrorMessage="Confirm Password does not match")]
        public string confirmPassword{get;set;}

        public DateTime created_at{get;set;}=DateTime.Now;
        public DateTime updated_at{get;set;}=DateTime.Now;

        public List<WeddingGuest> weddingInvites{get;set;}

    }
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string LoginEmail{get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string LoginPassword{get;set;}
    }

    public class RegisterNLogin
    {
        public User registerUser{get;set;}
        public LoginUser loginUser{get;set;}
    }
}