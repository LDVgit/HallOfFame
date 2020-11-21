using Microsoft.AspNetCore.Mvc;
using HallOfFame.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HallOfFame.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _db;

        public ValuesController(DataContext db)
        {
            _db = db;
        }

        [HttpGet("persons")]
        public ActionResult<List<Person>> GetPersons()
        {
            return Ok(_db.Persons.Include(p => p.Skills));
        }

        [HttpGet("person/{id}")]
        public ActionResult<Person> GetPerson(long id)
        {
            var person = _db.Persons.Include(p => p.Skills).FirstOrDefault(p => p.Id == id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpPost("person")]
        public ActionResult CreatePerson(Person person)
        {
            _db.Persons.Add(person);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut("person/{id}")]
        public ActionResult UpdatePerson(long id, Person person)
        {
            if (!_db.Persons.Any(p => p.Id == id))
                return NotFound();

            if (id != person.Id)
                BadRequest();

            // Add new skills
            if (person.Skills.Where(s => s.Id == 0).Any())
                _db.Skills.AddRange(person.Skills.Where(s => s.Id == 0));

            // Update skills
            if (person.Skills.Where(s => s.Id != 0).Any())
                _db.Skills.UpdateRange(person.Skills.Where(s => s.Id != 0));

            // Delete skills
            var skills = _db.Skills.Where(s => s.PersonId == id).ToList();
            var skillsToDelete = skills.Where(s => !person.Skills.Contains(s));
            if (skillsToDelete.Any())
                _db.Skills.RemoveRange(skillsToDelete);

            _db.Entry(person).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok();
        }

        [HttpDelete("person/{id}")]
        public ActionResult RemovePerson(long id)
        {
            var person = _db.Persons.Find(id);
            if (person == null)
                return NotFound();

            _db.Persons.Remove(person);
            _db.SaveChanges();
            return Ok();
        }
    }
}
