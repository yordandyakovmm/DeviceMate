using DeviceMate.Objects.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class AdminUserProxy
    {
        public int? Id { get; set; }

        [StringLength(100, ErrorMessage = "The maximum length is 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The email field is required.")]
        [StringLength(100, ErrorMessage = "The maximum length is 100 characters.")]
        [EmailAddress(ErrorMessage = "The email is invalid.")]
        [UniqueEmailAttribute]
        public string Email { get; set; }

        public string PictureUrl { get; set; }

        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "You should select status.")]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "You should select team.")]
        public int? TeamId { get; set; }
    }    
}