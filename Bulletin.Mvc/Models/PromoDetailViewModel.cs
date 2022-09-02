using BulletinApp.Shared;  //Promos

namespace Bulletin.Mvc.Models
{
    public record PromoDetailViewModel
    (
        IList<Promo> Promos
    );
}
