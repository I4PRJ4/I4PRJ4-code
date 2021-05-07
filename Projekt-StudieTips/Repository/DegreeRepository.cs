using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Data;

namespace Projekt_StudieTips.Repository
{
    public class DegreeRepository
    {
        private DatabaseContext _context;

        public DegreeRepository(DatabaseContext context)
        {
            _context = context;
        }
        public void AddDegree(string name)
        {
            if (!_context.Degrees.Any(s => s.DegreeName == name))
            {
                Degree degree = new Degree()
                    { DegreeName = name };


                _context.Degrees.Add(degree);
                _context.SaveChanges();
            }
        }
    }
}
