using System;
using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class HoldProxy
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "You should select a team.")]
        public int TeamId { get; set; }

        public DateTime HoldDate { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "You should fill in a name.")]
        public string HoldedItemName { get; set; }

        public bool IsBusy { get; set; }

        [Required(ErrorMessage = "You should select a town.")]
        public int TownId { get; set; }
    }
}
