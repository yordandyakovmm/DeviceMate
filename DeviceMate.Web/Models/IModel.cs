using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Web.Models
{
    public interface IModel
    {
        void Init();
    }

    public interface IModel<T>
    {
        void Init(T data);
    }
}
