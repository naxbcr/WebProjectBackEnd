using project1.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace project1.Controllers
{
    [Authorize]
    public class LanguageController : ApiController
    {
        ILanguageService laservice;
        public LanguageController(ILanguageService service)
        {
            laservice = service;
        }

        // GET: api/Language
        public IHttpActionResult Get()
        {
            var ttypes = laservice.GetAll();
            if (ttypes != null)
            {
                if (ttypes.ToArray().Any())
                {
                    return Ok(ttypes.ToArray());
                }
            }
            return NotFound();
        }

        // GET: api/Language/5
        public IHttpActionResult Get(int id)
        {
            var ttype = laservice.GetLanguagenById(id);
            if (ttype != null)
            {
                return Ok(ttype);
            }
            return NotFound();
        }



    }
}
