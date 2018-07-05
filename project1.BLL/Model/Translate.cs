using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project1.BLL.Model
{
    public class Translate
    {
        public int id { get; set; }
        public string title { get; set; }
        public string decription { get; set; }
        public int src_lang { get; set; }
        public int dest_lang { get; set; }
        public System.DateTime created_date { get; set; }
        public System.DateTime end_date { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public int translate_status { get; set; }
        public string link_customer { get; set; }
        public string link_translator { get; set; }
        public Nullable<decimal> price { get; set; }
        public int id_customer { get; set; }
        public Nullable<int> id_translator { get; set; }
        public int id_type { get; set; }

        /// 
        public string typename { get; set; }

        public string srclang_name {get; set;}
        public string destlang_name { get; set; }
        public string status_name { get; set; }

        public string tr_userlname { get; set; }
        public string tr_userfname { get; set; }

        public string cs_userlname { get; set; }
        public string cs_userfname { get; set; }

        public Translate() {  }

    }
}