using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project1.BLL.Model
{
    public class AdminStatistics
    {
        public int totalusers { get; set; }
        public int totaltranslators { get; set; }
        public int totaladmins { get; set; }
        public int notredeemedusers { get; set; }


        public int translated { get; set; }
        public int posted { get; set; }
        public int inprogress { get; set; }
        public int expired { get; set; }

        public AdminStatistics() { }

    }
}