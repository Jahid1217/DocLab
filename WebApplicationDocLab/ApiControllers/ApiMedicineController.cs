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
    public class ApiMedicineController : ApiController
    {
        private DoctorLab _contextdb;
        public ApiMedicineController()
        {
            _contextdb = new DoctorLab();
        }

        [Route("api/Medicine")]
        [HttpGet]
        public IEnumerable<Medicine> GetTests()
        {
            return _contextdb.Medicines.ToList();
        }
        [Route("api/Medicine/{id}")]
        [HttpGet]
        public IHttpActionResult GetTest(int id)
        {
            var medicine = _contextdb.Medicines.Find(id);
            if (medicine == null)
            {
                return NotFound();
            }
            return Ok(medicine);
        }
        [Route("api/MedicineUpdate/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateTest(int id, Medicine medicine)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != medicine.Id)
                return BadRequest();
            var existingMedicine = _contextdb.Medicines.Find(id);
            if (existingMedicine == null)
                return NotFound();
            existingMedicine.MedicineName = medicine.MedicineName;
            existingMedicine.Description = medicine.Description;
            existingMedicine.MedicineCategory = medicine.MedicineCategory;
            existingMedicine.ImageUrl = medicine.ImageUrl;
            existingMedicine.Category = medicine.Category;
            existingMedicine.Created_at = DateTime.Now;
            _contextdb.SaveChanges();
            return Ok(existingMedicine);
        }
        [Route("api/MedicineDelete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteTest(int id)
        {
            var medicine = _contextdb.Medicines.Find(id);
            if (medicine == null)
            {
                return NotFound();
            }
            _contextdb.Medicines.Remove(medicine);
            _contextdb.SaveChanges();
            return Ok(medicine);
        }
        [Route("api/MedicineCreate")]
        [HttpPost]
        public IHttpActionResult AddMedicine(Medicine medicine)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _contextdb.Medicines.Add(medicine);
            _contextdb.SaveChanges();

            return Ok(medicine);
        }

    }
}
