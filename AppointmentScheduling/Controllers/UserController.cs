using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var users = _db.Users.ToList();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteDetails(UserDetailsViewModel model)
        {


            if (ModelState.IsValid)
            {
                //Retrive user by userId
                var user = await _db.Users.FindAsync(model.UserId);
                if (user != null)
                {
                    //Update the user details with model properties
                    user.PhoneNumber = model.PhoneNumber;
                    user.Age = model.Age;
                    user.Identification = model.Identification;
                    try
                    {
                        //Save the updated user details
                        _db.Users.Update(user);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("VerifyEmail", "Account", new { userId= user.Id, code = model.VerificationCode});

                    }
                    catch (System.Exception)
                    { 
                        ModelState.AddModelError("", "Erro");

                        throw;
                    }
        
                }
            }

            // If ModelState is invalid or user update failed, return to the view with errors
            return View(model);
        }


        [HttpGet]
        public IActionResult CompleteDetails(string userId, string code)
        {

            var  user =  _db.Users.FirstOrDefault(c => c.Id == userId);

            var model = new UserDetailsViewModel
            {
                UserId = userId,
                VerificationCode = code,
                Age = Convert.ToInt16(user?.Age),
                Identification = user?.Identification,
                 PhoneNumber = user?.PhoneNumber,
            };
           

            return View(model);
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
