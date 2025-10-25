using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservation.DataContext.Entities
{
    public class HostRequest : AbstractEntity
    {
        
        public int? UserId { get; set; }

        public User? User { get; set; } = null!;
        
        public int? PropertyId { get; set; }

        public Property? Property { get; set; } = null!;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public bool IsApproved { get; set; } = false;
    }
}
