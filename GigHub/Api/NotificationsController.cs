using AutoMapper;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly ApplicationDbContext _context;


        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userid = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == userid && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserDto>();
                cfg.CreateMap<Gig, GigDto>();
                cfg.CreateMap<Notification, NotificationDto>();

            });




            return notifications.Select(new Mapper(config).Map<Notification, NotificationDto>);
        }
        //    return notifications.Select(n => new NotificationDto()
        //    {
        //        //manullay mapping
        //    //    DateTime = n.DateTime,
        //    //    Gig = new GigDto
        //    //    {
        //    //        Artist = new UserDto
        //    //        {
        //    //            Id = n.Gig.Artist.Id,
        //    //            Name = n.Gig.Artist.Name
        //    //        },
        //    //        DateTime = n.Gig.DateTime,
        //    //        Id = n.Gig.Id,
        //    //        IsCanceled = n.Gig.IsCanceled,
        //    //        Venue = n.Gig.Venue
        //    //    },
        //    //    OriginalDateTime = n.OriginalDateTime,
        //    //    OriginalVenue = n.OriginalVenue,
        //    //    Type = n.Type
        //    //});

        //}
    }
}
