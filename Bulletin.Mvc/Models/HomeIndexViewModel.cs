using BulletinApp.Shared;  // Users, Promos


namespace Bulletin.Mvc.Models
{
    public record HomeIndexViewModel
    (
        //int UserCount,
        //IList<User> Users,
        IList<Promo> Promos
    );
}
