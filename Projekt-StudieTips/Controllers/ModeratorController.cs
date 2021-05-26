using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Data;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Repository;
using PagedList;

namespace Projekt_StudieTips.Controllers
{
    [Authorize(Policy = "IsModerator")]
    public class ModeratorController : Controller
    {

        private readonly TipRepository _repository;


        public ModeratorController(TipRepository repository)
        {
            _repository = repository;
        }


        // GET: ModeratorController
        public async Task<IActionResult> Index()
        {



            var context = await _repository.GetUnmoderatedTips();

           
            
                try
                {
                    if (context.Count == 0)
                    {
                        ViewBag.CourseName = "Der er ingen tips at verificere.";
                    }
                }
                catch (InvalidOperationException)
                {
                    return NotFound();
                }

            return View(context);
        }


        
        public async Task<IActionResult> VerifyTip(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var tip = await _repository.Context.Tips.FindAsync(id);
            if (tip == null)
            {
                return NotFound();
            }

            tip.IsVerified = true;

            _repository.Context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
       
        public async Task<IActionResult> DenyTip(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Tip tipToDelete = new() { TipId = (int)id };
            _repository.Context.Attach(tipToDelete);
            _repository.Context.Remove(tipToDelete);

            _repository.Context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
