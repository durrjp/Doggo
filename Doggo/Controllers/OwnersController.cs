using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Doggo.Models;
using Doggo.Models.ViewModels;
using Doggo.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Doggo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly OwnerRepository _ownerRepo;
        private readonly DogRepository _dogRepo;
        private readonly NeighborhoodRepository _hoodRepo;
        private readonly WalkerRepository _walkerRepo;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnersController(IConfiguration config)
        {
            _ownerRepo = new OwnerRepository(config);
            _dogRepo = new DogRepository(config);
            _hoodRepo = new NeighborhoodRepository(config);
            _walkerRepo = new WalkerRepository(config);
    }
        // GET: OwnerController
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();
            foreach(Owner owner in owners)
            {
                owner.Neighborhood = _hoodRepo.GetNeighborhoodByOwner(owner.Id);
            }

            return View(owners);
        }

        // GET: OwnerController/Details/5
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = _ownerRepo.GetOwnerById(id),
                Dogs = _dogRepo.GetAllDogsWithOwner(owner.Id),
                Walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId)
        };

            return View(vm);
        }

        // GET: OwnerController/Create
        public ActionResult Create()
        {

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = _hoodRepo.GetAll()
            };

            return View(vm);
        }

        // POST: OwnerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OwnerFormViewModel vm)
        {
            try
            {
                _ownerRepo.AddOwner(vm.Owner);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                vm.Neighborhoods = _hoodRepo.GetAll();
                return View(vm);
            }
        }

        // GET: OwnerController/Edit/5
        public ActionResult Edit(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            if (owner == null)
            {
                return NotFound();
            }
            List<Neighborhood> neighborhoods = _hoodRepo.GetAll();
            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = owner,
                Neighborhoods = neighborhoods
            };
            return View(vm);

        }

        // POST: OwnerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }

        // GET: OwnerController/Delete/5
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            return View(owner);
        }

        // POST: OwnerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Login login)
        {
            Owner owner = _ownerRepo.GetOwnerByEmail(login.Email);

            if (owner == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
                new Claim(ClaimTypes.Email, owner.Email),
                new Claim(ClaimTypes.Role, "DogOwner"),
            };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Dog");
        }
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
