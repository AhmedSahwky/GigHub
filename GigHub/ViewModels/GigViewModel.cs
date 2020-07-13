using GigHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.ViewModels
{
    public class GigViewModel
    {
        public IEnumerable<Gig> Gigs { get; set; }
        public ILookup<int, Attendance> Attendances { get; set; }
    }
}