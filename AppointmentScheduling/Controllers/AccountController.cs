
using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Utility;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AppointmentScheduling.Controllers
{

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSenders _emailSender;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSenders emailSender)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);


                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        string callbackUrl = await SendEmailConfirmationTokenAsync(user, "Confirm your account-Resend");

                        ModelState.AddModelError("", "You must have a confirmed email to log on. "
                                             + "The confirmation token has been resent to your email account.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("ssuserName", user.Name);
                        //var username = httpcontext.session.getstring("ssusername");
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);

                    var callBackUrl = await SendEmailConfirmationTokenAsync(user, "Confirmation email");


                    //ViewBag.ErrorTitle = "Registration successful";
                    //ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                    //        "email, by clicking on the confirmation link we have emailed you";

                    // Redirect to another action that displays a success message
                    return RedirectToAction("RegistrationSuccess");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        private async Task<string> SendEmailConfirmationTokenAsync(ApplicationUser user, string subject)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = Url.Action("CompleteDetails", "User", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, subject, "Please complete your registration by <a href=\"" + callbackUrl + "\">clicking here</a>");
            return callbackUrl;
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        //Confirm-Email

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home"); // Redirect to an appropriate page if the parameters are missing
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home"); // Redirect to an appropriate page if the user is not found
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                // Redirect to the VerifyEmail action passing the userId and token
                return RedirectToAction("VerifyEmail", new { userId, code });
            }

            return RedirectToAction("Index", "Home"); // Redirect to an appropriate page if confirmation fails
        }



        [HttpGet]
        public IActionResult VerifyEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home"); // Redirect to an appropriate page if the parameters are missing
            }

            // Pass the userId and token to the view
            ViewData["UserId"] = userId;
            ViewData["Token"] = code;

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CompleteVerification(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    return RedirectToAction("WelcomePage"); // Redirect to the welcome page
                }
            }

            return RedirectToAction("Index", "Home"); // Redirect to an appropriate page if verification fails
        }

        [HttpGet]
        public IActionResult WelcomePage()
        {
            return View();
        }

    }


}
