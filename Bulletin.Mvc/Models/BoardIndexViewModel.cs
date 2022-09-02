using BulletinApp.Shared;  // Promos


namespace Bulletin.Mvc.Models
{
    public record BoardIndexViewModel
    (
        IList<Promo> Promos
    );
}
