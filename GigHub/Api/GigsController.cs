using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int Id)
        {
            var userid = User.Identity.GetUserId();
            var gig = _context.Gigs.Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == Id && g.ArtistId == userid);

            if (gig.IsCanceled)
                return NotFound();

            gig.Canceled();

            _context.SaveChanges();

            return Ok();
        }
    }
}
