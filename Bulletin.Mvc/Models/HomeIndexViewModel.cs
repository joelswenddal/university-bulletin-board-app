using BulletinApp.Shared;  // Promos


namespace Bulletin.Mvc.Models
{
    public record HomeIndexViewModel
    (
        IList<BulletinApp.Shared.Category> Categories
    );
}
