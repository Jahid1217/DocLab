using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplicationDocLab.Context;
using WebApplicationDocLab.Models;

namespace WebApplicationDocLab.ApiControllers
{
    public class ApiDoctorTypeController : ApiController
    {
        private readonly DoctorLab _contextdb;

        public ApiDoctorTypeController()
        {
            _contextdb = new DoctorLab();
        }

        // GET: api/Type
        [Route("api/Type")]
        [HttpGet]
        public IEnumerable<Doctor_Type> GetDoctorTypes()
        {
            return _contextdb.Doctor_Types.ToList();
        }

        // GET: api/Type/5
        [Route("api/Type/{id}")]
        [HttpGet]
        public IHttpActionResult GetDoctorType(int id)
        {
            var doctorType = _contextdb.Doctor_Types.Find(id);
            if (doctorType == null)
            {
                return NotFound();
            }
            return Ok(doctorType);
        }

        // PUT: api/TypeUpdate/5
        [Route("api/TypeUpdate/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateDoctorType(int id, Doctor_Type doctor_Type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != doctor_Type.Id)
                return BadRequest("ID mismatch.");

            var existingType = _contextdb.Doctor_Types.Find(id);
            if (existingType == null)
                return NotFound();

            existingType.TypeName = doctor_Type.TypeName;
            _contextdb.SaveChanges();

            return Ok(existingType);
        }

        // DELETE: api/TypeDelete/5
        [Route("api/TypeDelete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteDoctorType(int id)
        {
            var doctorType = _contextdb.Doctor_Types.Find(id);
            if (doctorType == null)
            {
                return NotFound();
            }

            _contextdb.Doctor_Types.Remove(doctorType);
            _contextdb.SaveChanges();

            return Ok(doctorType);
        }

        // POST: api/AddType
        [Route("api/AddType")]
        [HttpPost]
        public IHttpActionResult AddDoctorType(Doctor_Type doctor_Type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _contextdb.Doctor_Types.Add(doctor_Type);
            _contextdb.SaveChanges();

            return Ok(doctor_Type);
        }
        //search doctor type by name
        [Route("api/SearchType/{name}")]
        [HttpGet]
        public IHttpActionResult SearchDoctorType(string name)
        {
            var doctorTypes = _contextdb.Doctor_Types
                .Where(dt => dt.TypeName.Contains(name))
                .ToList();
            if (doctorTypes.Count == 0)
            {
                return NotFound();
            }
            return Ok(doctorTypes);
        }
    }
}
