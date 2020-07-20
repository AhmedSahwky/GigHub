using GigHub.Models;
using GigHub.Repositories;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        public HomeController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
        }
        public ActionResult Index(string SearchTerm = null)
        {
            var upcomingGigs = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCanceled);

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                upcomingGigs = upcomingGigs
                    .Where(g => g.Artist.Name.Contains(SearchTerm) ||
                    g.Genre.Name.Contains(SearchTerm) ||
                    g.Venue.Contains(SearchTerm));
            }

            var userid = User.Identity.GetUserId();
            var attendances = _attendanceRepository.GetFutureAttendances(userid)
                .ToLookup(a => a.GigId);

            var viewmodel = new GigViewModel
            {
                Gigs = upcomingGigs,
                Attendances = attendances
            };


            return View(viewmodel);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}