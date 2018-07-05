using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project1.BLL.Model
{
    public class TranslatorStatistics
    {
        public int translator_id { get; set; }


        public int translated { get; set; }
        public int inprogress { get; set; }
        public int expired { get; set; } // взятые в работу но вышел дедлайн

        public Decimal  totalgain { get; set; }
        public Decimal  mediumprice { get; set; }
        public Decimal  lostgain { get; set; }
        public Decimal futuregain { get; set; }

        public TranslatorStatistics(int id) { this.translator_id = id; }


    }
}