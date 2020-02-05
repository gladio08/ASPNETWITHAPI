using API1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API1.Controllers
{
    public class RolesController : ApiController
    {
        ApplicationDbContext myContext = new ApplicationDbContext();

        [HttpGet]
        public IQueryable<Roles> GetRoles()
        {
            return myContext.Roles;
        }

        //[ResponseType(typeof(Roles))]
        [HttpGet]//Mendeklarasikan bahwa bagian ini merupakan GET
        public IHttpActionResult GetRoles(int Id)
        {
            Roles role = myContext.Roles.Find(Id);
            if(role != null)
            {
                return Ok(role);
            }
            return NotFound();
        }

        //[ResponseType(typeof(Roles))]
        [HttpPost]
        public IHttpActionResult Post(Roles role)
        {
            if (!string.IsNullOrWhiteSpace(role.Name))//Bila data inputan Null
            {
                myContext.Roles.Add(role);
                var result = myContext.SaveChanges();
                if (result > 0)
                {
                    return Ok(role);
                }
                //return CreatedAtRoute("DefaultApi", new { Id = role.Id }, role);
            }
            return BadRequest();
        }

        /*[ResponseType(typeof(void))]*///void tdk mengembalikan nilai
        [HttpPut]
        public IHttpActionResult Put(Roles role, int Id)
        {
            var put = myContext.Roles.Find(Id);
            //var put = GetRoles(Id);
            if (put != null)
            {
                //put.Name = role.Name;
                if (!string.IsNullOrWhiteSpace(role.Name))
                {
                    put.Name = role.Name;
                    myContext.Entry(put).State = EntityState.Modified;
                    var result = myContext.SaveChanges();
                    if (result > 0)
                    {
                        return Ok(role);
                    }
                }
                return BadRequest();
            }
            return NotFound();
            //return StatusCode(HttpStatusCode.NoContent);
        }

        //[ResponseType(typeof(Roles))]
        [HttpDelete]
        public IHttpActionResult Delete(Roles role, int Id)
        {
            var del = myContext.Roles.Find(Id);
            if (del != null)
            {
                myContext.Roles.Remove(del);
                var result = myContext.SaveChanges();
                if(result > 0)
                {
                    return Ok(del);
                }
            }
            return NotFound();
        }
    }
}
