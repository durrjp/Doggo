using Doggo.Models;
using Doggo.Models.ViewModels;
using Doggo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;

namespace Doggo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly WalkerRepository _walkerRepo;
        private readonly NeighborhoodRepository _hoodRepo;
        private readonly WalksRepository _walksRepo;
        private readonly OwnerRepository _ownerRepo;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkersController(IConfiguration config)
        {
            _walkerRepo = new WalkerRepository(config);
            _hoodRepo = new NeighborhoodRepository(config);
            _walksRepo = new WalksRepository(config);
            _ownerRepo = new OwnerRepository(config);
        }
        // GET: WalkersController
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();
            List<Walker> walkers = new List<Walker>();
            
            if(ownerId == 0)
            {
                walkers = _walkerRepo.GetAllWalkers();
                return View(walkers);
            }
            else
            {
                Owner currentUser = _ownerRepo.GetOwnerById(ownerId);
                walkers = _walkerRepo.GetWalkersInNeighborhood(currentUser.NeighborhoodId);
                return View(walkers);
            }
        }

        // GET: WalkersController/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            walker.Neighborhood = _hoodRepo.GetNeighborhoodByWalker(id);
            List<Walks> walks = _walksRepo.GetWalksByWalker(walker.Id);
            int totalWalksDuration = 0;
            foreach(Walks walk in walks)
            {
                walk.Duration = walk.Duration / 60;
                totalWalksDuration += walk.Duration;
            }



            if (walker == null)
            {
                return NotFound();
            }
            WalkerViewModel vm = new WalkerViewModel()
            {
                Walker = walker,
                Walks = walks,
                TotalWalksDuration = totalWalksDuration
            };

            return View(vm);

        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(id != null)
            {
                return int.Parse(id);
            }
            else
            {
                return 0;
            }
        }
    }
}
