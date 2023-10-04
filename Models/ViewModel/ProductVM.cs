using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyTest.Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}
