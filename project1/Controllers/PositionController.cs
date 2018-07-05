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
    public class PositionController : ApiController
    {
        IPositionService pservice;

        public PositionController(IPositionService service)
        {
            pservice = service; 
        }

        // GET: api/Position
        public IHttpActionResult Get()
        {
            var positions = pservice.GetAll();
            if (positions != null)
            {
                if (positions.ToArray().Any())
                {
                    return Ok(positions.ToArray());
                }
            }
            return NotFound();
        }

        // GET: api/Position/5
        public IHttpActionResult Get(int id)
        {
            var position = pservice.GetPositionById(id);
            if (position != null)
            {
                return Ok(position);
            }
            return NotFound();
        }

    
    }
}
