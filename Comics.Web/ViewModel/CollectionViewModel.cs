using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Comics.Web.ViewModel
{
    public class CollectionViewModel
    {
        [MaxLength(31, ErrorMessage = "NameLength")]
        [Required(ErrorMessage = "NameRequired")]
        public string Name { get; set; }

        [MaxLength(1023, ErrorMessage = "DescriptionLength")]
        public string Desc { get; set; }

        [MaxLength(31, ErrorMessage = "ThemeLength")]
        [Required(ErrorMessage = "ThemeRequired")]
        public string Theme { get; set; }

        [Required(ErrorMessage = "ImageRequired")]
        public IFormFile Image { get; set; }
    }
}
