using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using project1.BLL.Interfaces;
using project1.BLL.Model;
using project1.BLL.Services;

namespace project1.Controllers
{
    [Authorize]
    public class CommentController : ApiController
    {
        ICommentService cservice;

        public CommentController(ICommentService service)
        {
            cservice = service;
        }

        // GET: api/Comment
        //public IEnumerable<string> Get()
        //{
        //    nenado
        //}

        // GET: api/Comment/5
        public IHttpActionResult Get(int id)
        {
            var com = cservice.GetCommentById(id);
            if (com != null)
            {
                return Ok(com);
            }
            return NotFound();
        }

        [Route("api/comment/translate/{id}/all")]
        public IHttpActionResult GetAllForTranslate(int id)
        {
            var com = cservice.GetAllComentsOfTranslate(id);
            if (com != null)
            {
                if (com.ToArray().Any())
                {
                    return Ok(com.ToArray());
                }
            }
            return NotFound();
        }

        [Route("api/comment/user/{id}/all")]
        public IHttpActionResult GetAllOfUser(int id)
        {
            var com = cservice.GetAllComentsOfUser(id);
            if (com != null)
            {
                if (com.ToArray().Any())
                {
                    return Ok(com.ToArray());
                }
            }
            return NotFound();
        }


        // POST: api/Comment
        public IHttpActionResult Post([FromBody]Comment value)
        {
            if (value != null)
            {
                int id = cservice.CreateComment(value);
                if (id > 0)
                {
                    value.id = id;
                    return Created<Comment>(Request.RequestUri + id.ToString(), value);
                }
                else
                {
                    return Conflict(); // конфликт ??? перевода такого перевода нет?
                }
            }
            return BadRequest();
        }

        //// PUT: api/Comment/5
        //public void Put(int id, [FromBody]string value)
        //{
              //  нужно ли вообще обновлять коммент если обновлять там можно всего 1 поле текст ???
        //}

        // DELETE: api/Comment/5
        public IHttpActionResult Delete(int id)
        {
            if (cservice.DeleteComment(id)) { return Ok(true); } else { return NotFound(); }
        }


    }
}
