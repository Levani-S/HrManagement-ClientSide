using System.ComponentModel.DataAnnotations;

namespace HRManagementClientSide.Models
{
    public class GenderModel
    {
        [Key]
        public Guid GenderId { get; set; }

        public string GenderName { get; set; }
    }
}
