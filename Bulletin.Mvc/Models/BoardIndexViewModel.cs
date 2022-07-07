using BulletinApp.Shared;  // Users, Promos


namespace Bulletin.Mvc.Models
{
    public record BoardIndexViewModel
    (
        //int UserCount,
        //IList<User> Users,
        IList<Promo> Promos
    );
}
