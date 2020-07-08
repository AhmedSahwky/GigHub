﻿using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
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
            var gig = _context.Gigs.Single(g => g.Id == Id && g.ArtistId == userid);

            if (gig.IsCanceled)
                return NotFound();

            gig.IsCanceled = true;

            var notification = new Notification
            {
                DateTime = DateTime.Now,
                Gig = gig,
                Type = NotificationType.GigCanceld
            };
            var attendances = _context.Attendances.Where(a => a.GigId == gig.Id)
                .Select(a => a.Attendee)
                .ToList();
            foreach (var Attendee in attendances)
            {
                var usernotification = new UserNotification
                {
                    User = Attendee,
                    Notification = notification
                };
                _context.UserNotifications.Add(usernotification);
            }
            _context.SaveChanges();
            return Ok();
        }
    }
}
