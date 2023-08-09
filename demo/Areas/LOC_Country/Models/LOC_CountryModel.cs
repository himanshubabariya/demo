using System.ComponentModel.DataAnnotations;

namespace demo.Areas.LOC_Country.Models
{
    public class LOC_CountryModel
    {
        public int? CountryID { get; set; }

        [Required]
        [StringLength(20,MinimumLength = 3)]
        public string CountryName { get; set; }
        
        [Required]
        public string CountryCode { get; set; }

        public DateTime CountryAddDate { get; set; }

        public DateTime CountryModifiedDate { get; set; }

        public IFormFile File { get; set; }
        public string PhotoPath { get; set; }
    }
    public class LOC_CountryDropDownModel
    {
        public int CountryID { get; set; }

        public string? CountryName { get; set; }

        
    }
}
