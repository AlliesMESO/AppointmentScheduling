using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO; //This line to include the System.IO namespace
using System.Linq;
using System.Threading.Tasks;
using AppointmentScheduling.Repositories; // Adjust to your actual namespace


namespace AppointmentScheduling.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager; // Declare _userManager
        private readonly IWebHostEnvironment _webHostEnvironment; // Include the IWebHostEnvironment
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;

        //Constructor-Accepting specified dependencies
        public UserController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment,
                ILogger<UserController> logger, // Inject ILogger
                IUserRepository userRepository)

        {
            _db = db;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger; // Assign the injected logger to the private field
            _userRepository = userRepository;

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
                    //user.Identification = model.Identification;
                    user.Gender = model.Gender; // Update the gender property
                    user.Workplace = model.Workplace;
                    user.Address = model.Address; // Update the Address property
                    try
                    {
                        //Save the updated user details
                        _db.Users.Update(user);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("VerifyEmail", "Account", new { userId = user.Id, code = model.VerificationCode });

                    }
                    catch (System.Exception)
                    {
                        ModelState.AddModelError("", "Error");

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

            var user = _db.Users.FirstOrDefault(c => c.Id == userId);

            var model = new UserDetailsViewModel
            {
                UserId = userId,
                VerificationCode = code,
                Age = Convert.ToInt16(user?.Age),
                //Identification = user?.Identification,
                PhoneNumber = user?.PhoneNumber,
                Gender = user?.Gender // Set the UserGender property
            };


            return View(model);
        }


        // Add a method to retrieve the user's profile picture path
        private string GetUserProfilePicturePath(string userId)
        {
            // Replace this with your logic to fetch the user's profile picture path from your data source
            // For example, if you store the path in the user's database record, fetch it here
            var user = _userRepository.GetUserById(userId); // Replace with your actual repository or data access code

            // Assuming your user model has a ProfilePicturePath property
            if (user != null)
            {
                return user.ProfilePicturePath;
            }

            // Handle the case where the user's profile picture path is not found
            // You can return a default path or empty string
            return "/profilepictures/default-profile-picture.jpg"; // replace with your default image path
        }

        [Authorize] //Authorize attribute
        public async Task<IActionResult> Profile()
        {
            try
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);



                // Check if the user is authenticated

                if (user != null)
                {
                    // Create an instance of UserProfileVM and populate it with the user's data
                    // Populate the UserProfileVM model here

                    var userProfile = new UserProfileVM
                    {
                        Name = user.Name,  // Replace with the property that holds the user's full name
                        Email = user.Email,    // Replace with the property that holds the user's email
                        PhoneNumber = user.PhoneNumber,
                        Age = user.Age, // Populate Age property
                        Gender = user.Gender, // Populate Gender property
                        Workplace = user.Workplace,
                        Address = user.Address,
                        ProfilePicturePath = GetUserProfilePicturePath(user.Id) // Include the profile picture path
                                                                                // Populate other properties as needed
                    };
                    // Pass the userProfile object to the view

                    return View(userProfile);
                }
                else
                {
                    return RedirectToAction("Login", "Account");

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the user's profile.");
                return RedirectToAction("Login", "Account");
            }
              
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            try
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);

                if (user != null && profilePicture != null && profilePicture.Length > 0)
                {
                    // Generate a unique filename (e.g., using a GUID)
                    var fileName = $"{user.Id}_{DateTime.Now.Ticks}.jpg";

                    // Generate a unique directory name for the user
                    var userDirectoryName = $"{user.Id}_{DateTime.Now.Ticks}";

                    // Define the base directory where user-specific profile pictures will be stored
                    var baseProfilePicturesPath = Path.Combine(_webHostEnvironment.WebRootPath, "profilepictures");

                    // Combine the base directory with the user-specific directory
                    var userDirectoryPath = Path.Combine(baseProfilePicturesPath, userDirectoryName);

                    // Create the user's directory if it doesn't exist
                    if (!Directory.Exists(userDirectoryPath))
                    {
                        Directory.CreateDirectory(userDirectoryPath);
                    }

                    // Define the full file path
                    var filePath = Path.Combine(userDirectoryPath, fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePicture.CopyToAsync(stream);
                    }

                    // Update the user's profile in the database with the new file path
                    user.ProfilePicturePath = $"/profilepictures/{userDirectoryName}/{fileName}";

                    // Save the changes to the database
                    await _userManager.UpdateAsync(user); // Update the user using UserManager

                    // Redirect to the ProfileContent action
                    return RedirectToAction("Profile");
                }
                else
                {
                    return Json(new { success = false, errorMessage = "No File uploaded or user not authenticated." });
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                _logger.LogError(ex, "An error occurred while uploading the profile picture.");

                // Handle the error and return an appropriate JSON response
                return Json(new { success = false, errorMessage = "An error occurred while uploading the file." });
            }
        
        }


    }
}   
