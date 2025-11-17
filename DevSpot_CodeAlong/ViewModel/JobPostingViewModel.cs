
using System.ComponentModel.DataAnnotations;
//Klass för att Modelstate ska vara valid då Rquired fields inte fylls i formulär vid Create
namespace DevSpot_CodeAlong.ViewModel
{
    public class JobPostingViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]

        public string Description { get; set; }

        [Required]

        public string Company { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
