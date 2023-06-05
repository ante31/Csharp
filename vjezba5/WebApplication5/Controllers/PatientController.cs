using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApplication3.Models;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientContext _dbContext;

        public PatientController(PatientContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            if (_dbContext.Patients == null)
            {
                return NotFound();
            }
            return await _dbContext.Patients.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            if (_dbContext.Patients == null)
            {
                return NotFound();
            }

            var patient = await _dbContext.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return patient;
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Regex.IsMatch(patient.Oib, @"^\d{11}$"))
            {
                ModelState.AddModelError("", "Oib must be a string of 11 digits.");
                return BadRequest(ModelState);
            }

            if (!Regex.IsMatch(patient.Mbo, @"^\d{8}$"))
            {
                ModelState.AddModelError("", "mbo must be a string of 8 digits.");
                return BadRequest(ModelState);
            }

            if (!patient.FirstAndLastName.Contains(" "))
            {
                ModelState.AddModelError("", "FirstAndLastName must contain a space.");
                return BadRequest(ModelState);
            }

            if (!DateTime.TryParse(patient.DateOfBirth.ToString(), out var dateOfBirth))
            {
                ModelState.AddModelError("", "DateOfBirth is not a valid date.");
                return BadRequest(ModelState);
            }

            if (!(patient.Gender.ToLower() == "male" || patient.Gender.ToLower() == "female"))
            {
                ModelState.AddModelError("", "Gender must be either 'male' or 'female'.");
                return BadRequest(ModelState);
            }

            _dbContext.Patients.Add(patient);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }


        [HttpPut]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if(id != patient.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Regex.IsMatch(patient.Oib, @"^\d{11}$"))
            {
                ModelState.AddModelError("", "Oib must be a string of 11 digits.");
                return BadRequest(ModelState);
            }

            if (!Regex.IsMatch(patient.Mbo, @"^\d{8}$"))
            {
                ModelState.AddModelError("", "mbo must be a string of 8 digits.");
                return BadRequest(ModelState);
            }

            if (!patient.FirstAndLastName.Contains(" "))
            {
                ModelState.AddModelError("", "FirstAndLastName must contain a space.");
                return BadRequest(ModelState);
            }

            if (!DateTime.TryParse(patient.DateOfBirth.ToString(), out var dateOfBirth))
            {
                ModelState.AddModelError("", "DateOfBirth is not a valid date.");
                return BadRequest(ModelState);
            }

            if (!(patient.Gender.ToLower() == "male" || patient.Gender.ToLower() == "female"))
            {
                ModelState.AddModelError("", "Gender must be either 'male' or 'female'.");
                return BadRequest(ModelState);
            }

            _dbContext.Entry(patient).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        private bool PatientAvailable(int id)
        {
            return (_dbContext.Patients?.Any(x => x.Id == id)).GetValueOrDefault();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            if(_dbContext.Patients == null)
            {
                return NotFound();
            }

            var patient = await _dbContext.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            _dbContext.Patients.Remove(patient);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}