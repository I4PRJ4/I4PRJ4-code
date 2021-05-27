using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Data;
using Projekt_StudieTips.Models;

namespace Projekt_StudieTips.Repository
{
    public interface ITipRepository
    {

        Task<Tip> GetTip(int? id);

        Task<List<Tip>> GetTips(int? id, string sortOrder);

        Task<Course> GetCourse(int? id);

        Task<List<Tip>> GetTipsWithinSearchTerm(SearchDto search);

        Task<Tip> GetTipDetails(int? id);

        bool TipExists(int id);

        Task<List<Tip>> GetUnmoderatedTips();
        void SaveChanges();
        Task DeleteTip(int id);
        Task AddTip(Tip tip);
        Task UpdateTip(Tip tip);
    }
}