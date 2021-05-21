using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Data;

namespace Projekt_StudieTips.Repository
{
    public class DegreeRepository
    {
        public DatabaseContext Context { get; set; }

        public DegreeRepository(DatabaseContext context)
        {
            Context = context;
        }

        public List<Degree> GetDegrees()
        {
            return Context.Degrees.ToList();
        }

        public Degree FindDegree(int? id)
        {
            return Context.Degrees.First(m => m.DegreeId == id);
        }

        public async Task AddDegree(Degree degree)
        {
            Context.Add(degree);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateDegree(Degree degree)
        {
            Context.Update(degree);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveDegree(Degree degree)
        {
            Context.Degrees.Remove(degree);
            await Context.SaveChangesAsync();
        }

        public bool DegreeExist(int id)
        {
            return Context.Degrees.Any(e => e.DegreeId == id);
        }
    }
}
