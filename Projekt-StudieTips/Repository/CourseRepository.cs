using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Data;
using Projekt_StudieTips.Models;

namespace Projekt_StudieTips.Repository
{
    public class CourseRepository
    {

        public DatabaseContext Context { get; set; }

        public CourseRepository(DatabaseContext context)
        {
            Context = context;
        }
    }
}
