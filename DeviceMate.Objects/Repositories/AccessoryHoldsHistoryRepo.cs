using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class AccessoryHoldsHistoryRepo : BaseRepo<AccessoryHoldsHistory>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public AccessoryHoldsHistoryRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion
    }
}
