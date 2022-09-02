using BulletinApp.Shared;  //Promos
using System;
using System.Collections.Generic;


namespace Bulletin.Mvc.Models.DataViewModels
{
    public class PromoCategoriesData
    {
       public int CategoryId { get; set; }
       public string? CategoryName { get; set; }
       public bool Associated { get; set; }
    }
}
