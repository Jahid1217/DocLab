using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplicationDocLab.Context;
using WebApplicationDocLab.Models;

namespace WebApplicationDocLab.ApiControllers
{
    [RoutePrefix("api/Profile")]

    public class ApiProfileController : ApiController
    {
        private DoctorLab _contextdb;

        public ApiProfileController()
        {
            _contextdb = new DoctorLab();
        }

        // GET api/Profile/{id}

        [HttpGet]
        [Route("{id:int}")] // Explicit route for GET
        public IHttpActionResult GetProfile(int id)
        {
            var profile = _contextdb.User_Infos.Find(id);
            if (profile == null)
            {
                return NotFound();
            }

            // Don't return password
            profile.Password = null;
            profile.ConfirmPassword = null;

            return Ok(profile);
        }

        // PUT api/ProfileUpdate/{id}

        [HttpPut]
        [Route("{id:int}")] // Explicit route for PUT
        public IHttpActionResult UpdateProfile(int id, User_Info profile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != profile.Id)
                return BadRequest("ID mismatch");

            var existingProfile = _contextdb.User_Infos.Find(id);
            if (existingProfile == null)
                return NotFound();

            // Manually validate date range
            if (profile.DateOfBirth < new DateTime(1900, 1, 1) ||
                profile.DateOfBirth > new DateTime(2100, 12, 31))
            {
                return BadRequest("Date of Birth must be between 1/1/1900 and 12/31/2100");
            }

            // Update fields
            existingProfile.DateOfBirth = profile.DateOfBirth;
            // ... other field updates

            try
            {
                _contextdb.SaveChanges();
                return Ok(existingProfile);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE api/ProfileDelete/{id}

        [HttpDelete]
        [Route("{id:int}")] // This makes it respond to DELETE api/Profile/{id}
        public IHttpActionResult DeleteProfile(int id)
        {
            var profile = _contextdb.User_Infos.Find(id);
            if (profile == null)
            {
                return NotFound();
            }

            _contextdb.User_Infos.Remove(profile);
            _contextdb.SaveChanges();
            return Ok(profile);
        }
    }
}