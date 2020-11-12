using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HallOfFame.Models;
using HallOfFame.Services;

namespace HallOfFame.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly PeopleService _peopleService;

        public PeopleController(DataContext context, PeopleService peopleService)
        {
            _context = context;
            _peopleService = peopleService;
        }

        [HttpGet("persons")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            var persons = await _peopleService.GetPersons();
            return Ok(persons);
        }

        [HttpGet("person/{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _peopleService.GetPerson(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpPut("person/{id}")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            try
            {
                await _peopleService.UpdatePerson(id, person);
            }
            catch(ArgumentNullException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_peopleService.ExistsPerson(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost("person")]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            await _peopleService.CreatePerson(person);
            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        [HttpDelete("person/{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            try
            {
                await _peopleService.RemovePerson(id);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
