using System.ComponentModel.DataAnnotations;

namespace MyWeather.Models
{
    /// <summary>
    /// My sample description
    /// </summary>
    public class SampleModel
    {
        /// <summary>
        /// My sample ID description
        /// </summary>
        /// <example>999</example>
        [Required(ErrorMessage = "{0} is required"), Range(1, 100, ErrorMessage = "ID must be between {1} and {2}")]
        public int Id { get; set;  }
        
        /// <summary>
        /// My sample DESCRIPTION description
        /// </summary>
        /// <example>my sample description</example>
        [MaxLength(100)]
        public string Description { get; set; }
    }
}