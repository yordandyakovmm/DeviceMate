using DeviceMate.Models.Enums;

namespace DeviceMate.Objects.EmployeesInformation
{
   
    public class Employee
    {
        public string Name { get; set; }
        public string PictureResourceId { get; set; }
        public string PictureUrl { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public enTown Town { get; set; }
    }
}
