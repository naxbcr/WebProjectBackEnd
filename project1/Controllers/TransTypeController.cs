using project1.BLL.Interfaces;
using project1.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace project1.Controllers
{
    [Authorize]
    public class TransTypeController : ApiController
    {
        ITypeService ttservice;

        public TransTypeController(ITypeService service)
        {
            ttservice = service; 
        }
        // GET: api/TransType
        public IHttpActionResult Get()
        {
            var ttypes = ttservice.GetAllTypes();
            if (ttypes != null)
            {
                if (ttypes.ToArray().Any())
                {
                    return Ok(ttypes.ToArray());
                }
            }
            return NotFound();
        }

        // GET: api/TransType/5
        public IHttpActionResult Get(int id)
        {
            var ttype = ttservice.GetTypeById(id);
            if (ttype != null)
            {
                return Ok(ttype);
            }
            return NotFound();
        }

    }
}
