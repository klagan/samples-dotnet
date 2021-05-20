namespace MyWeather.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     My sample description
    /// </summary>
    public class SampleModel
    {
        /// <summary>
        /// this is a description for the *id* property of the **SampleModel** class.
        /// - one item
        /// - two item
        /// - three `item`
        /// </summary>
        /// <example>9991001999</example>
        [Required(ErrorMessage = "{0} is required")]
        [Range(1, 100, ErrorMessage = "ID must be between {1} and {2}")]
        public int Id { get; set; }

        /// <summary>
        ///     this is a description for the
        ///     <b>
        ///         <i>description</i>
        ///     </b>
        ///     property of the <b>SampleModel</b> class
        ///     <a href="#section/Authentication">go to authentication section (test)</a>
        /// </summary>
        /// <remarks>
        ///     we can add more content here for the
        ///     <b>
        ///         <i>description</i>
        ///     </b>
        /// </remarks>
        /// <example>example value for description goes here</example>
        [MaxLength(100)]
        public string Description { get; set; }
    }
}