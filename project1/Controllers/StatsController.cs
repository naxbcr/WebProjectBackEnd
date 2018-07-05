using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using project1.BLL.Interfaces;
using project1.BLL.Model;
using project1.BLL.Services;

namespace project1.Controllers
{
    [Authorize]
    public class StatsController : ApiController
    {
        IStatistics aservice;

        public StatsController(IStatistics service)
        {
            aservice = service;
        }
        // GET: api/Stats
        [Route("api/Stats/admin")]
        [Authorize(Roles = "administrator")]
        public async Task<IHttpActionResult> GetAdminStats()
        {
            return Ok(await aservice.getAdminStatistics());
        }

        // GET: api/Stats/5
        [Route("api/Stats/translator/{id}")]
        [Authorize(Roles = "translator")]
        public async Task<IHttpActionResult> GetTranslatorStats(int id)
        {
            return Ok(await aservice.getTranslatorStatistics(id));
        }


    }
}
