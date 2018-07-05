using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using project1.BLL.Interfaces;
using project1.BLL.Model;
using project1.BLL.Services;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace project1.Controllers
{
    [Authorize]
    public class TranslateController : ApiController
    {
        ITranslateService tservice;

        public TranslateController(ITranslateService service)
        {
            tservice = service;
        }

        // GET: api/Translate
        public IHttpActionResult Get()
        {
            var trans = tservice.GetAllTranslates().OrderByDescending(x => x.created_date);
            if (trans != null)
            {
                if (trans.ToArray().Any())
                {
                    return Ok(trans.ToArray());
                }
            }
            return NotFound();
        }

        [Route("api/translate/user/{id}/all")]
        public IHttpActionResult GetAllOfUser(int id)
        {
            var trans = tservice.GetAllUserTranslates(id).OrderByDescending(x => x.created_date);
            if (trans != null)
            {
                if (trans.ToArray().Any())
                {
                    return Ok(trans.ToArray());
                }
            }
            return NotFound();
        }
        // GET: api/Translate/5
       public IHttpActionResult Get(int id)
        {
            var trans = tservice.GetTransById(id);
            if (trans != null)
            {
                return Ok(trans);
            }
            return NotFound();
        }
        [Route("api/translate/avaliable")]
        [Authorize(Roles = "translator")]
        public IHttpActionResult GetAllAvaliable()
        {
            var trans = tservice.GetAllTranslates().Where(x =>x.id_translator==null).OrderByDescending(x => x.created_date).ToList();
            //trans.OrderByDescending(x => x.created_date);
            if (trans != null)
            {
                return Ok(trans);
            }
            return NotFound();
        }

        [Route("api/translate/translator/{id}")]
        [Authorize(Roles = "translator")]
        public IHttpActionResult GetTranslateByTranslator(int id)
        {
            var trans = tservice.GetAllTranslates().Where(x => x.id_translator == id).OrderByDescending(x => x.created_date).ToList();
            if (trans != null)
            {
                return Ok(trans);
            }
            return NotFound();
        }

        // POST: api/Translate
        public IHttpActionResult Post([FromBody]Translate value)
        {
            if (value != null & value.link_customer !=null)
            {
                int pricecount = DocReader.ReadDoc(value.link_customer);
                Debug.WriteLine("Testprice: " + pricecount); //koli4estvo slov nahodit
                value.price = Convert.ToDecimal(pricecount);
                Debug.WriteLine("Test value price: " + value.price);

                int id = tservice.CreateTranslate(value); // создаем перевод уже с ценой!!!!!!!!!!

                if (id > 0)
                {
                    value.id = id;
                    return Created<Translate>(Request.RequestUri + id.ToString(), value);
                }
                else
                {
                    return Conflict(); // конфликт ??? нельзя создать если эмайл занет.
                }
            }
            return BadRequest("Neverniy format dannyh! Proverj celostnostj poley.");
        }

        // PUT: api/Translate/5
        public IHttpActionResult Put(int id, [FromBody]Translate value)
        {
            if (id > 0)
            {
                bool upd = tservice.UpdateTrans(id, value);
                return Ok(upd);
            }
            return BadRequest();
        }

        [Authorize(Roles = "administrator")]
        // DELETE: api/Translate/5
        public IHttpActionResult Delete(int id)
        {
            if (tservice.DeleteTrans(id)) { return Ok(true); } else { return NotFound(); }
        }
    }
}
