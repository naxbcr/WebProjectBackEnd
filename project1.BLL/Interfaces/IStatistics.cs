﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project1.BLL.Model;

namespace project1.BLL.Interfaces
{
    public interface IStatistics
    {
       Task<AdminStatistics> getAdminStatistics();
        Task<TranslatorStatistics> getTranslatorStatistics(int id);
    }
}
