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
    public class StatusController : ApiController
    {
        IStatusService sservice;
        public StatusController(IStatusService service)
        {
            sservice = service;
        }

        // GET: api/Status
        public IHttpActionResult Get()
        {
            var ttypes = sservice.GetAll();
            if (ttypes != null)
            {
                if (ttypes.ToArray().Any())
                {
                    return Ok(ttypes.ToArray());
                }
            }
            return NotFound();
        }

        // GET: api/Status/5
        public IHttpActionResult Get(int id)
        {
            var ttype = sservice.GetById(id);
            if (ttype != null)
            {
                return Ok(ttype);
            }
            return NotFound();
        }

    }
}
