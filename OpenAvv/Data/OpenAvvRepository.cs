using Microsoft.AspNetCore.Identity;
using OpenAvv.Data.Models;
using OpenAvv.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace OpenAvv.Data
{
    public class OpenAvvRepository : IOpenAvvRepository
    {
        private readonly OpenAvvDbContext _ctx;
        private readonly UserManager<OpenAvvUser> _userManager;

        public OpenAvvRepository(OpenAvvDbContext ctx, UserManager<OpenAvvUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public IEnumerable<StoryVM> GetAllStories() //Returns list of stories with AuthorName instead of AuthorID
        {
            var records = (from story in _ctx.Stories
                         join user in _ctx.Users
                         on story.AuthorID equals user.Id
                         select new
                         {
                             ID = story.ID,
                             Title = story.Title,
                             AuthorName = user.FirstName,
                             AuthorId = user.Id,
                             Info = story.Info,
                             Description = story.Description,
                             DateCreated = story.DateCreated,
                             DateModified = story.DateModified
                         }).ToList();
            List<StoryVM> ListOfStoryVMs = new List<StoryVM>();
            foreach (var item in records) //retrieve each item and assign to model
            {
                ListOfStoryVMs.Add(new StoryVM()
                {
                    ID = item.ID,
                    Title = item.Title,
                    AuthorName = item.AuthorName,
                    AuthorId = item.AuthorId,
                    Info = item.Info,
                    Description = item.Description,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified
                });
            }
            return ListOfStoryVMs;
        }

        public void AddNewStory(Story story)
        {
            _ctx.Stories.Add(story);
            _ctx.SaveChanges();
        }

        public StoryVM GetStoryByStoryId(int storyId)
        {
            //retrive that record
            //Story record = (from story in _ctx.Stories
            //              where story.ID == storyId
            //              select story).First();
            //return record;
            var record = (from story in _ctx.Stories
                          join user in _ctx.Users
                          on story.AuthorID equals user.Id
                          where story.ID == storyId
                          select new
                          {
                              ID = story.ID,
                              Title = story.Title,
                              AuthorName = user.FirstName,
                              AuthorId = user.Id,
                              Info = story.Info,
                              Description = story.Description,
                              DateCreated = story.DateCreated,
                              DateModified = story.DateModified
                          }).First();
            //retrieve item and assign to model
            StoryVM storyViewModel = new StoryVM()
            {
                ID = record.ID,
                Title = record.Title,
                AuthorName = record.AuthorName,
                AuthorId = record.AuthorId,
                Info = record.Info,
                Description = record.Description,
                DateCreated = record.DateCreated,
                DateModified = record.DateModified
            };

            return storyViewModel;
        }

        public void UpdateStory(Story updatedStory)
        {
            //Updating the record
            Story record = (from story in _ctx.Stories
                          where story.ID == updatedStory.ID
                          select story).First();
            record.Info = updatedStory.Info;
            record.Title = updatedStory.Title;
            record.Description = updatedStory.Description;
            record.DateModified = DateTime.Now;

            _ctx.SaveChanges();
        }

        public void DeleteStoryByStoryId(int storyId)
        {
            Story record = (from story in _ctx.Stories
                          where story.ID == storyId
                          select story).First();
            _ctx.Stories.Remove(record);
            _ctx.SaveChanges();
        }

        public bool IsAuthor(ClaimsPrincipal signedInUser, int storyId)
        {
            var result = (from story in _ctx.Stories
                           join user in _ctx.Users
                           on story.AuthorID equals user.Id
                           where story.ID == storyId
                           select new {
                               AuthorId = user.Id
                           }).First();
            string AuthorId = result.AuthorId;

            string SignedInUserId = _userManager.GetUserId(signedInUser);
            return (AuthorId == SignedInUserId);
        }


    }
}