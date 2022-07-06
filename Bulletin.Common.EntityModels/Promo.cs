using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BulletinApp.Shared
{
    [Index("ContactName", Name = "ContactName")]
    [Index("PromoType", Name = "PromoType")]
    public partial class Promo
    {
        public Promo()
        {
            Categories = new HashSet<Category>();
        }

        [Key]
        public int PromoId { get; set; }
        public int? UserId { get; set; }
        [StringLength(40)]
        public string? ContactName { get; set; }
        [StringLength(10)]
        public string? PromoType { get; set; }
        [Column(TypeName = "date")]
        public DateTime PostDate { get; set; }

        [StringLength(100)]
        public string? Headline { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        [StringLength(200)]
        public string? ContactInfo { get; set; }
        public byte[]? Photo { get; set; }
        [StringLength(255)]
        public string? PhotoPath { get; set; }
        [StringLength(255)]
        public string? Hyperlink { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Promos")]
        public virtual User? User { get; set; }

        [ForeignKey("PromoId")]
        [InverseProperty("Promos")]
        public virtual ICollection<Category> Categories { get; set; }
    }
}
