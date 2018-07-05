using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project1.BLL.Model
{
    public class Comment
    {
        public int id { get; set; }
        public System.DateTime created_date { get; set; }
        public string comment { get; set; }
        public int id_user { get; set; }
        public int id_translate { get; set; }

        // для того что бы не делать доп. запрос
        public string userfname { get; set; }
        public string userlname { get; set; }
        public string postion { get; set; }
    }
}