﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }
        public bool IsCanceled { get; private set; }

        [Required]
        public string ArtistId { get; set; }

        public ApplicationUser Artist { get; set; }
        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        [Required]
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }
        public string SearchTerm { get; set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }
        public void Canceled()
        {
            IsCanceled = true;

            var notification = Notification.GigCanceled(this);


            foreach (var Attendee in Attendances.Select(a => a.Attendee))
            {
                Attendee.Notify(notification);

            }
        }

        public void Modify(DateTime dateTime, string venue, byte genre)
        {
            var notification = Notification.GigUpdated(this, dateTime, venue);


            Venue = Venue;
            DateTime = dateTime;
            GenreId = genre;

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}