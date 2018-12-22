using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Http;


namespace WeddingPlanner
{
    public class WeddingPlannerController:Controller
    {
        private WeddingPlannerContext dbContext;
        public WeddingPlannerController(WeddingPlannerContext context)
        {
            dbContext=context;
        }

        private void CleanUp()
        {
            var expireWedding=dbContext.Weddings.Where(w=>w.weddingDate<DateTime.Now);
            if (expireWedding.Count()>0)
            {
                foreach (var wedding in expireWedding)
                {
                    dbContext.Weddings.Remove(wedding);
                }
                dbContext.SaveChanges();
            }
            HttpContext.Session.SetString("cleanUpDay",DateTime.Now.ToString());
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View("index");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            var user_id=HttpContext.Session.GetInt32("user_id");
            if (user_id==null){return RedirectToAction("Index");}


            var currentUserName=dbContext.Users.FirstOrDefault(u=>u.user_id==user_id);

            var allWedding=dbContext.Weddings.Include(w=>w.guests).ToList();

           
            if (HttpContext.Session.GetString("cleanUpDay")==null)
            {
                CleanUp();
            }
            else{
                DateTime cleanUpDay;
                int day;
                if (DateTime.TryParse(HttpContext.Session.GetString("cleanUpDay"),out cleanUpDay))
                {
                    day=(DateTime.Now - cleanUpDay).Days;
                    if (day>1)
                    {
                        CleanUp();
                    }
                }
            }
            

            ViewBag.currentUser=new Dictionary<string,object>(){
                {"user_id",user_id},
                {"user_name",currentUserName.firstname}
            };
            return View("dashboard",allWedding);
        }

        [HttpGet("plan")]
        public IActionResult PlanWedding()
        {
            var user_id=HttpContext.Session.GetInt32("user_id");
            if (user_id==null){return RedirectToAction("Index");}
            return View("PlanWedding");
        }

        [HttpGet("wedding/{wedding_id}")]
        public IActionResult Wedding(int wedding_id)
        {

            var wedding=dbContext.Weddings.Include(w=>w.guests)
                                            .ThenInclude(g=>g.user)
                                            .FirstOrDefault(w=>w.wedding_id==wedding_id);

            if (wedding==null){ return RedirectToAction("Dashboard");}

            return View("wedding",wedding);
        }


        [HttpGet("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        // ---------Form Processing ------------------------

        [HttpPost("registration")]
        public IActionResult Register_process(RegisterNLogin model){
            var modelInfo=model.registerUser;

            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(theuser=>theuser.email==modelInfo.email))
                {
                    ModelState.AddModelError("registerUser.email","Email already Exist");
                    return View("registration");
                }

                var hasher=new PasswordHasher<User>();
                modelInfo.password=hasher.HashPassword(modelInfo,modelInfo.password);
                dbContext.Users.Add(modelInfo);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("user_id",modelInfo.user_id);

                return RedirectToAction("Dashboard");
            }

            return View("index");
        }


        [HttpPost("login")]
        public IActionResult Login_process(RegisterNLogin model){
            var modelInfo=model.loginUser;

            if (ModelState.IsValid)
            {
                var databaseUser=dbContext.Users.FirstOrDefault(theloginUser=>theloginUser.email==modelInfo.LoginEmail);
                if (databaseUser!=null)
                {
                    var hasher=new PasswordHasher<LoginUser>();
                    var result=hasher.VerifyHashedPassword(modelInfo,databaseUser.password,modelInfo.LoginPassword);
                    if (result!=0)
                    {
                        HttpContext.Session.SetInt32("user_id",databaseUser.user_id);
                        return RedirectToAction("Dashboard");
                    }

                    ModelState.AddModelError("loginUser.LoginEmail","Invalid Email or Password");
                    return View("index");

                }

                ModelState.AddModelError("loginUser.LoginEmail","Invalid Email or Password");
                return View("index");
                
            }

            return View("index");
        }


        [HttpPost("createWedding")]
        public IActionResult createWedding(Wedding weddingModel)
        {
            if (ModelState.IsValid)
            {
               
                if(weddingModel.weddingDate<DateTime.Now)
                {
                    ModelState.AddModelError("weddingDate",errorMessage:"Sorry, wedding date is invalid ");
                    return View("PlanWedding");
                }
                
                var user_id=HttpContext.Session.GetInt32("user_id");
                if (user_id==null){return RedirectToAction("Logout");}

                weddingModel.user_id=(int)user_id;
                dbContext.Weddings.Add(weddingModel);
                dbContext.SaveChanges();
                Console.WriteLine("successsssss");
                return RedirectToAction("PlanWedding");
            }
            
            Console.WriteLine("FAILLLLLLLLLLLLLLL");
            return View("PlanWedding");
        }

        // Accepting invites Process
        [HttpGet("accept/{user_id}/{wedding_id}")]
        public IActionResult AcceptingInvites(int user_id,int wedding_id)
        {
            var existingGuest=dbContext.WeddingGuests.Where(wg=>wg.user_id==user_id).FirstOrDefault(wg=>wg.wedding_id==wedding_id);
            if (existingGuest!=null)
            {
                Console.WriteLine("*************");
                Console.WriteLine("existing guest FOUND<<<<<<<<------");
                Console.WriteLine("*************");
                return RedirectToAction("dashboard");
            }
            else
            {
                Console.WriteLine("*************");
                Console.WriteLine("existing guest NOT NOT FOUND<<<<<<<<------");
                Console.WriteLine("*************");
            }
            var WeddingGuestModel=new WeddingGuest();
            WeddingGuestModel.user_id=user_id;
            WeddingGuestModel.wedding_id=wedding_id;
            dbContext.WeddingGuests.Add(WeddingGuestModel);
            dbContext.SaveChanges();

            var newWedding=dbContext.Weddings.Include(w=>w.guests).FirstOrDefault(w=>w.wedding_id==wedding_id);
            var weddingInfo=new Dictionary<string,int>()
            {
                {"guestCount",newWedding.guests.Count},
                {"wedding_id",wedding_id},
                {"user_id",user_id}
            };
            return Json(weddingInfo);
        }

        // UnAccepting invites Process
        [HttpGet("unAccept/{user_id}/{wedding_id}")]
        public IActionResult UnAcceptingInvites(int user_id,int wedding_id)
        {
            var retrieveAssociation=dbContext.WeddingGuests.Where(a=>a.user_id==user_id)
                                                            .FirstOrDefault(a=>a.wedding_id==wedding_id);
            
            dbContext.WeddingGuests.Remove(retrieveAssociation);
            dbContext.SaveChanges();
            var newWedding=dbContext.Weddings.Include(w=>w.guests).FirstOrDefault(w=>w.wedding_id==wedding_id);
            var weddingInfo=new Dictionary<string,int>()
            {
                {"guestCount",newWedding.guests.Count},
                {"wedding_id",wedding_id},
                {"user_id",user_id}
            };
            return Json(weddingInfo);

        }

        [HttpGet("cancelWedding/{wedding_id}")]
        public IActionResult CancelWedding(int wedding_id)
        {
            var retrieveWedding=dbContext.Weddings.FirstOrDefault(w=>w.wedding_id==wedding_id);
            dbContext.Weddings.Remove(retrieveWedding);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}


