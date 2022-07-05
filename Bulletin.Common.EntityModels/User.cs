using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BulletinApp.Shared
{
    [Index("LastName", Name = "LastName")]
    [Index("State", Name = "State")]
    public partial class User
    {
        public User()
        {
            Promos = new HashSet<Promo>();
        }

        [Key]
        [Required]
        public int UserId { get; set; }
        
        [StringLength(30)]
        [Required]
        public string LastName { get; set; } = null!;
        [StringLength(20)]
        [Required]
        public string FirstName { get; set; } = null!;
        [StringLength(20)]
        public string? PreferredName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SignUpDate { get; set; }
        [StringLength(60)]
        public string? Address { get; set; }
        [StringLength(15)]
        public string? City { get; set; }
        [StringLength(3)]
        public string? State { get; set; }
        [StringLength(10)]
        public string? PostalCode { get; set; }
        [StringLength(50)]
        public string? SchoolEmail { get; set; }
        [StringLength(255)]
        public string? Hyperlink { get; set; }
        [StringLength(24)]
        public string? Phone { get; set; }
        public byte[]? Photo { get; set; }
        [StringLength(300)]
        public string? Notes { get; set; }
        [StringLength(255)]
        public string? PhotoPath { get; set; }

        //one-to-many relationship with Promos
        [InverseProperty("User")]
        public virtual ICollection<Promo> Promos { get; set; }
    }
}
