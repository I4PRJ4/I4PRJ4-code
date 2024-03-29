﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagedList;
using Projekt_StudieTips.Repository;
using System;
using System.Threading.Tasks;

namespace Projekt_StudieTips.Controllers
{
    [Authorize(Policy = "IsModerator")]

    public class ModeratorController : Controller
    {

        private readonly ITipRepository _repository;


        public ModeratorController(ITipRepository repository)
        {
            _repository = repository;
        }


        // GET: ModeratorController
        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            var context = await _repository.GetUnmoderatedTips();

            ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_desc" : "date_asc";

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
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }


            return View(context.ToPagedList(pageNumber, pageSize));
        }



        public async Task<IActionResult> VerifyTip(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var tip = await _repository.GetTip(id);
            if (tip == null)
            {
                return NotFound();
            }

            tip.IsVerified = true;

            _repository.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DenyTip(int id)
        {

            await _repository.DeleteTip(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
