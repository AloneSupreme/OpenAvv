using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenAvv.Data.Models;
using OpenAvv.Data;
using OpenAvv.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace OpenAvv.Controllers
{
    [Authorize]
    public class StoriesController : Controller
    {
        private readonly IOpenAvvRepository _repository;
        private readonly UserManager<OpenAvvUser> _userManager;

        public StoriesController(IOpenAvvRepository repository, UserManager<OpenAvvUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<StoryVM> model = _repository.GetAllStories();
            return View(model);
        }

    
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Story formStoryModel)
        {
            if (ModelState.IsValid)
            {
                formStoryModel.AuthorID = _userManager.GetUserId(User);
                formStoryModel.DateCreated = DateTime.Now;

                _repository.AddNewStory(formStoryModel);

                ViewData["Message"] = "Success";
                ModelState.Clear();
            }
            else
            {
                ViewData["Message"] = "Error";
            }
            return View();
        }

        
        [HttpGet]
        public IActionResult Edit(int id)
        {

            if (_repository.IsAuthor(User, id)) { //Checking whether the author have permission to edit the story
                StoryVM model = _repository.GetStoryByStoryId(id);
                return View(model);
            }
            ViewData["AllowAuthorError"] = "You are not allowed to access that URL"; //Otherwise returning to Index page
            IEnumerable<StoryVM> IndexModel = _repository.GetAllStories();
            return View("Index",IndexModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Story updatedStory)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateStory(updatedStory);
                ViewData["Message"] = "Success";
                ModelState.Clear();
            }
            else
            {
                ViewData["Message"] = "Error";
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (_repository.IsAuthor(User, id)) //Checking whether the author have permission to edit the story
            {
                _repository.DeleteStoryByStoryId(id);
                return RedirectToAction("Index");
            }
            ViewData["AllowAuthorError"] = "You are not allowed to access that URL";//Otherwise returning to Index page
            IEnumerable<StoryVM> IndexModel = _repository.GetAllStories();
            return View("Index", IndexModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            StoryVM model = _repository.GetStoryByStoryId(id);
            return View(model);   
        }
        

    }
}