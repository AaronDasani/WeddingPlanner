using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class WeddingGuest
    {
        [Key]
        public int association_id{get;set;}

        public int user_id{get;set;}
        public int wedding_id{get;set;}

        public User user{get;set;}
        public Wedding wedding{get;set;}
    }
}