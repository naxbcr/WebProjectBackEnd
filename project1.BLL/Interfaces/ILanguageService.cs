using project1.BLL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1.BLL.Interfaces
{
    public interface ILanguageService
    {
        Language GetLanguagenById(int id);
        IEnumerable<Language> GetAll();
    }
}
