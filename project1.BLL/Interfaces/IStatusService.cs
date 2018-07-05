using project1.BLL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1.BLL.Interfaces
{
    public interface IStatusService
    {
        Status GetById(int id);
        IEnumerable<Status> GetAll();
    }
}
