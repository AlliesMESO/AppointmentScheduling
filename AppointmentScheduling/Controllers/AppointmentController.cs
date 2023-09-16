using AppointmentScheduling.Models;
using AppointmentScheduling.Services;
using AppointmentScheduling.Utility;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {

        private readonly IAppointmentService _appointmentService;
        private readonly ApplicationDbContext _db; // Inject your ApplicationDbContext

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
            // Other properties if needed

        }


        //[Authorize(Roles = Helper.Admin)]
        public IActionResult Index()
        {

            ViewBag.Duration = Helper.GetTimeDropDown();
            ViewBag.DoctorList = _appointmentService.GetDoctorList();
            ViewBag.PatientList = _appointmentService.GetPatientList();
       
            return View();
        }

        // Other actions and methods...

        //// Add a method to check if a user has active appointments

        //// Your HasActiveAppointments method
        //private bool HasActiveAppointments(string userId)
        //{
        //    // Assuming you want to check if there are any active appointments 
        //    // for the user where the user's ID matches either DoctorId or PatientId
        //    var hasActiveAppointments = _db.Appointments.Any(a => (a.DoctorId == userId || a.PatientId == userId) && a.Status == "Active");
        //    return hasActiveAppointments;

        //}
    }
}
