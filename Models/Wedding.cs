using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int wedding_id{get;set;}

        [Required]
        [MinLength(2,ErrorMessage="Name should be atleast 2 chracters")]
        [Display(Name="First Wedder Name")]
        public string wedder_one{get;set;}

        [Required]
        [MinLength(2,ErrorMessage="Name should be atleast 2 chracters")]
        [Display(Name="Second Wedder Name")]
        public string wedder_two{get;set;}

        [Required]
        [DataType(DataType.Date,ErrorMessage="Invalid Date")]
        [Display(Name="Lucky Wedding Date")]
        public DateTime weddingDate{get;set;}

        [Required]
        [MinLength(2,ErrorMessage="Invalid Address")] 
        [Display(Name="Wedding Address")]
        public string weddingAdress{get;set;}
        public DateTime created_at{get;set;}=DateTime.Now;
        public DateTime updated_at{get;set;}=DateTime.Now;

        public int user_id{get;set;}
        public User creator{get;set;}

        public List<WeddingGuest> guests{get;set;}

    }
}