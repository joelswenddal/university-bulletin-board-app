using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BulletinApp.Shared
{
    [Index("CategoryName", Name = "CategoryName")]
    [Index("CategoryName", Name = "UQ__Categori__8517B2E006E77A55", IsUnique = true)]
    public partial class Category
    {
        public Category()
        {
            Promos = new HashSet<Promo>();
        }

        [Key]
        public int CategoryId { get; set; }
        [StringLength(30)]
        public string CategoryName { get; set; } = null!;
        [StringLength(300)]
        public string? Description { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Categories")]
        public virtual ICollection<Promo> Promos { get; set; }
    }
}
