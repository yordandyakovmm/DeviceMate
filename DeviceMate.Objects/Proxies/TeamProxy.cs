using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
        public class TeamProxy
        {
            public int? Id { get; set; }

            [Required(ErrorMessage = "Team Name is required.")]
            public string Name { get; set; }

        }
    
}
