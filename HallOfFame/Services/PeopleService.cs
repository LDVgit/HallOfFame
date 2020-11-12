using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HallOfFame.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HallOfFame.Services
{
    public class PeopleService
    {
        private readonly DataContext _context;

        public PeopleService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetPersons()
        {
            return await _context.Persons.Include(p => p.Skills).ToListAsync();
        }

        public async Task<Person> GetPerson(long id)
        {
            return await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdatePerson(long id, Person person)
        {
            if (id != person.Id)
                throw new ArgumentNullException();

            // Add new skills
            if (person.Skills.Where(s => s.Id == 0).Any())
                await _context.Skills.AddRangeAsync(person.Skills.Where(s => s.Id == 0));

            // Update skills
            if (person.Skills.Where(s => s.Id != 0).Any())
                _context.Skills.UpdateRange(person.Skills.Where(s => s.Id != 0));

            // Delete skills
            var skills = await _context.Skills.Where(s => s.PersonId == id).ToListAsync();
            var skillsToDelete = skills.Where(s => !person.Skills.Contains(s));
            if (skillsToDelete.Any())
                _context.Skills.RemoveRange(skillsToDelete);

            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CreatePerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePerson(long id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
                throw new NullReferenceException();

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }

        public bool ExistsPerson(long id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
