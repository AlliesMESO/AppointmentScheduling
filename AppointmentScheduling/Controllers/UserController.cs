using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.IO; //This line to include the System.IO namespace
using System.Linq;
using System.Net;
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

        [Authorize]
        public async Task<IActionResult> Profile()
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
                    Address = user.Address

                    // Populate other properties as needed
                };

                // Retrieve the user's profile data, including the profile picture path
                // Replace this with your logic to get the user's profile picture path
                userProfile.ProfilePicturePath = GetUserProfilePicturePath(user.Id);


                // Pass the userProfile object to the view
                return View(userProfile);
            }

            // Handle the case where the user is not authenticated
            // You can redirect them to the login page or perform some other action
            return RedirectToAction("Login", "Account"); // Redirect to login page as an example
        }

        [HttpPost]
        public IActionResult UploadProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                try
                {
                    // Generate a unique filename (e.g., using a GUID)
                    //var uniqueFileName = Guid.NewGuid().ToString() + "_" + profilePicture.FileName;

                    // Generate a unique filename (e.g., user_id_timestamp.jpg)
                    var userId = User.Identity.Name; // Replace with your user identifier
                    var fileName = $"{User.Identity.Name}_{DateTime.Now.Ticks}.jpg";


                    // Generate a unique directory name for the user
                    var userDirectoryName = $"{userId}_{DateTime.Now.Ticks}";

                    // Define the base directory where user-specific profile pictures will be stored
                    var baseProfilePicturesPath = Path.Combine(_webHostEnvironment.WebRootPath, "profile_pictures");

                    // Combine the base directory with the user-specific directory
                    var userDirectoryPath = Path.Combine(baseProfilePicturesPath, userDirectoryName);

                    // Create the user's directory if it doesn't exist
                    if (!Directory.Exists(userDirectoryPath))
                    {
                        Directory.CreateDirectory(userDirectoryPath);
                    }
                    // Upload the file to your chosen storage location (e.g., server or cloud)
                    // Update the database record to point to the new file (e.g., update the file path in the user's profile)


                    // Define the path where you want to save the uploaded file
                    //var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "profile_pictures", fileName);
                    // Define the full file path
                    var filePath = Path.Combine(userDirectoryPath, fileName);


                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        profilePicture.CopyTo(stream);
                    }

                    // Update the user's profile in the database with the new file path
                    // Replace with your user identifier
                    // UpdateUserProfilePicture(userId, filePath);

                    // Return a JSON response with the success status and image URL
                    return Json(new { success = true, imageUrl = $"/profile_pictures/{userDirectoryName}/{fileName}" });
                }
                catch (Exception ex)
                {
                    // Log the error or handle it appropriately
                    _logger.LogError(ex, "An error occurred while uploading the profile picture.");

                    // Handle the error and return an appropriate JSON response
                    return Json(new { success = false, errorMessage = "An error occurred while uploading the file." });
                }

            }
            return Json(new { success = false, errorMessage = "No File uploaded." });
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
            return "/path/to/default/profile-picture.jpg"; // Replace with your default image path
        }
    }
}   
