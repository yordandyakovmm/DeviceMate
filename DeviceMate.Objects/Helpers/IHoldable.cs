using DeviceMate.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Helpers
{
    public interface IHoldable
    {
        HashSet<string> GetNumbers();
        Hold GetHoldByNumber(string number);
        string GetNumberByHoldId(int id);
    }
}