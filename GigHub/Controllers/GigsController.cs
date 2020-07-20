using GigHub.Models;
using GigHub.Persistence;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;
        public GigsController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }


        [HttpPost]
        public ActionResult Search(Gig gig)
        {
            return RedirectToAction("Index", "Home", new { SearchTerm = gig.SearchTerm });
        }

        [Authorize]

        public ActionResult Create()
        {
            var ViewModel = new GigFormViewModel
            {
                Genres = _unitOfWork.Genres.GetGenresList()
            };

            return View(ViewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _unitOfWork.Genres.GetGenresList();
                return View("Create", viewModel);
            }
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                Venue = viewModel.Venue,
                GenreId = viewModel.Genre
            };
            _unitOfWork.Gigs.Add(gig);
            _unitOfWork.Complete();
            return RedirectToAction("Mine", "Gigs");


        }


        public ActionResult Edit(int Id)
        {
            var gig = _unitOfWork.Gigs.GetGig(Id);

            if (gig == null)
            {
                return HttpNotFound();
            }
            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            var ViewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:MM"),
                Venue = gig.Venue,
                Genre = gig.GenreId
            };

            return View(ViewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GigFormViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                viewModel.Genres = _unitOfWork.Genres.GetGenresList();
                return View("Edit", viewModel);
            }
            var gig = _unitOfWork.Gigs.GetGigWithAttendees(viewModel.Id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);


            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");


        }
        public ActionResult Mine()
        {
            var userid = User.Identity.GetUserId();
            var gigs = _unitOfWork.Gigs.GetUpcomingGigsByArtist(userid);

            return View(gigs);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userid = User.Identity.GetUserId();


            var viewmodel = new GigViewModel
            {
                Gigs = _unitOfWork.Gigs.GetGigsUserAttending(userid),
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(userid)
               .ToLookup(a => a.GigId)
            };

            return View(viewmodel);
        }





        public ActionResult Details(int Id)
        {
            var gig = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == Id);

            if (gig == null)
                return HttpNotFound();

            var viewmodel = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                var userid = User.Identity.GetUserId();
                viewmodel.IsAttending = _context.Attendances
                    .Any(a => a.GigId == gig.Id && a.AttendeeId == userid);

                viewmodel.IsFollowing = _context.Follwoings
                    .Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userid);

            }
            return View("Details", viewmodel);
        }
    }
}