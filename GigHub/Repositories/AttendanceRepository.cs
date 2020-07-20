using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Repositories
{
    public class AttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Attendance> GetFutureAttendances(string userid)
        {
            return _context.Attendances
                           .Where(a => a.AttendeeId == userid && a.Gig.DateTime > DateTime.Now)
                           .ToList();
        }

        public bool GetAttendance(int id, string userid)
        {
            return _context.Attendances.Any(a => a.GigId == id && a.AttendeeId == userid);
        }
    }
}