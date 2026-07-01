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
    public class ApiTextController : ApiController
    {
        private DoctorLab _contextdb;
        public ApiTextController()
        {
            _contextdb = new DoctorLab();
        }
        // GET: api/Text
        [Route("api/Text")]
        [HttpGet]
        public IEnumerable<Test> GetTests()
        {
            return _contextdb.Tests.ToList();
        }
        // GET: api/Text/5
        [Route("api/Text/{id}")]
        [HttpGet]
        public IHttpActionResult GetTest(int id)
        {
            var test = _contextdb.Tests.Find(id);
            if (test == null)
            {
                return NotFound();
            }
            return Ok(test);
        }
        // PUT: api/Text/5
        [Route("api/TextUpdate/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateTest(int id, Test test)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != test.Id)
                return BadRequest();
            var existingTest = _contextdb.Tests.Find(id);
            if (existingTest == null)
                return NotFound();
            existingTest.TestName = test.TestName;
            existingTest.Category = test.Category;
            existingTest.Created_at = DateTime.Now;
            _contextdb.SaveChanges();
            return Ok(existingTest);
        }
        // DELETE: api/Text/5
        [Route("api/TextDelete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteTest(int id)
        {
            var test = _contextdb.Tests.Find(id);
            if (test == null)
            {
                return NotFound();
            }
            _contextdb.Tests.Remove(test);
            _contextdb.SaveChanges();
            return Ok(test);
        }
        // GET: api/AddTest
        [Route("api/AddTest")]
        [HttpPost]
        public Test AddGames(Test test)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _contextdb.Tests.Add(test);
            _contextdb.SaveChanges();

            return test;
        }

    }
}
