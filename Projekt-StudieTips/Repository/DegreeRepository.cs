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
        public DatabaseContext Context { get; set; }

        public DegreeRepository(DatabaseContext context)
        {
            Context = context;
        }
        
    }
}
