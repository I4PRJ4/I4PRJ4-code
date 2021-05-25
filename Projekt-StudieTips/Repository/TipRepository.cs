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
    public class TipRepository
    {
        public DatabaseContext Context { get; set; }

        public TipRepository(DatabaseContext context)
        {
            Context = context;
        }


        public async Task<List<Tip>> GetTips(int? id, string sortOrder)
        {
            var context = Context.Tips
                .Include(t => t.Course)
                .Where(t => t.CourseId == id & t.IsVerified == true);

            switch (sortOrder)
            {
                case "date_asc":
                    context = context.OrderBy(t => t.Date);
                    break;
                case "date_desc":
                    context = context.OrderByDescending(t => t.Date);
                    break;
                default:
                    context = context.OrderByDescending(t => t.Date);
                    break;
            }

            return await context.ToListAsync();
        }

        public async Task<Course> GetCourse(int? id)
        {
            return await Context.Courses.Where(c => c.CourseId == id).FirstAsync();
        }

        public async Task<List<Tip>> GetTipsWithinSearchTerm(SearchDto search)
        {
            return await Context.Tips
                .Include(t => t.Course)
                .Where(t => (t.Headline.Contains(search.SearchTerm) || t.Text.Contains(search.SearchTerm) ||
                             t.Course.CourseName.Contains(search.SearchTerm)) & t.IsVerified == true).ToListAsync();
        }

        public async Task<Tip> GetTipDetails(int? id)
        {
            return await Context.Tips
                .Include(t => t.Course)
                .FirstOrDefaultAsync(m => m.TipId == id);

        }

        public bool TipExists(int id)
        {
            return Context.Tips.Any(e => e.TipId == id);
        }
    }
}