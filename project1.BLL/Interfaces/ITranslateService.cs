using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project1.BLL.Model;

namespace project1.BLL.Interfaces
{
    public interface ITranslateService
    {
        Translate GetTransById(int id);
        IEnumerable<Translate> GetAllTranslates();
        IEnumerable<Translate> GetAllUserTranslates(int userid);
        int CreateTranslate(Translate newtrans);
        bool UpdateTrans(int id,Translate trans);
        bool DeleteTrans(int id);
    }
}
