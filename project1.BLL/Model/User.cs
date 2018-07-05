using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace project1.BLL.Model
{
    public class User 
    {
        public int id { get; set; }
        public int id_position { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password { get; set; }
        public bool emailconfirm { get; set; }


        public string postionname { get; set; }

   
    }
}