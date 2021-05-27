using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt_StudieTips.Models;

namespace Projekt_StudieTips.Repository
{
    public interface IDegreeRepository
    {
        List<Degree> GetDegrees();
        Degree FindDegree(int? id);

        Task AddDegree(Degree degree);

        Task UpdateDegree(Degree degree);

        Task RemoveDegree(Degree degree);

        bool DegreeExists(int id);
    }
}
