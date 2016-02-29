using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DeviceMate.Objects.Helpers
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            UserRepo userRepo = new UserRepo(new DeviceContext());
            AdminUserProxy adminUser = validationContext.ObjectInstance as AdminUserProxy;

            if (adminUser.Id.HasValue)
            {
                string currentAdminUserEmail = userRepo.Get(u => u.Id == adminUser.Id.Value).Select(u => u.Email).FirstOrDefault();
                if (currentAdminUserEmail != adminUser.Email)
                {
                    return this.IsEmailUnique(userRepo, adminUser.Email);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return this.IsEmailUnique(userRepo, adminUser.Email);
            }
        }

        private ValidationResult IsEmailUnique(UserRepo userRepo, string email)
        {
            if (!userRepo.Get(u => u.Email == email).Any())
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("The email is already taken.");
            }
        }
    }
}
