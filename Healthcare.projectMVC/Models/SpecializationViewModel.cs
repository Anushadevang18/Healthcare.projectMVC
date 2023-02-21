using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Healthcare.projectMVC.Models
{
    public class SpecializationViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Plese provide the specialization")]
        [Display(Name = "Specialization")]
        public string SpecializationName { get; set; }
    }
}