using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using project1.App_Start;
using project1.BLL.Interfaces;
using project1.BLL.Model;
using project1.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace project1.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        IUserService uservice;
        EmailService emailservice;

        public UserController(IUserService service)
        {
            uservice = service;
            emailservice  = new EmailService(service);         
        }


        [Authorize(Roles = "user,translator,administrator")]
        // GET: api/User
        public IHttpActionResult Get()
        {

            var users = uservice.GetAllUsers();
            if (users!=null)
            {
                if (users.ToArray().Any())
                {
                    return Ok(users.ToArray());
                }
            }
            return NotFound();
        }


        [Authorize(Roles = "user,translator,administrator")]
        // GET: api/User/5
        public IHttpActionResult Get(int id)
        {
          
            var user = uservice.GetUserById(id);
            if(user!=null)
            {
                return Ok(user);
            }
            return NotFound();
        }


        [Authorize(Roles = "user,translator,administrator")]
        [Route("api/user/email/{email}")]
        //GET: by email
        public IHttpActionResult GetByEmail(string email)
        {
            var user = uservice.GetUserByEmail(email);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }


        [AllowAnonymous]
        [Route("api/user/register")]
        // POST: api/User
        public async Task <IHttpActionResult> Post([FromBody]User value)
        {
            if (value != null)
            {
                int id = uservice.CreateUser(value);
                
                if (id > 0)
                {
                   value.id = id;
                   string token = emailservice.generateEmailToken(id.ToString(), value.password, value.email, DateTime.Now.Ticks);
                   var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = id, code = token }));
                   await emailservice.SendMail("Confirm your account on translate-project", "Please confirm your account by clicking <a href =\"" + callbackUrl + "\">here</a>",value.email);
                     //problema mozet bit v email, proverka na samoi pochte!     
                   return Created<User>(Url.Link("DefaultApi", new { controller = "user", id = id }), value);
                }
                else
                {
                    return Conflict(); // конфликт ??? нельзя создать если эмайл занет.
                }
            }
            return BadRequest();
           
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public IHttpActionResult ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("User Id and Code are required");                
            }
            bool valid = emailservice.IsEmailTokenValid(code);

            if (valid)
            {
                uservice.ConfirmEmail(userId, valid);
                return Ok("Your account has been verified!");
            }
            else
            {
                return BadRequest("Your token is invalid. Validation time is expired."); 
            }
        }


        [Authorize(Roles = "user,translator,administrator")]
        // PUT: api/User/5
        public IHttpActionResult Put(int id, [FromBody]User value)
        {
            if (id > 0)
            {
                bool upd = uservice.UpdateUser(id, value);
                return Ok(upd);
            }
            return BadRequest();
        }

        [Authorize(Roles = "administrator")]
        // DELETE: api/User/5
        public IHttpActionResult Delete(int id)
        {
            if (uservice.DeleteUser(id)) { return Ok(true); } else { return NotFound(); }
        }



    }
}
