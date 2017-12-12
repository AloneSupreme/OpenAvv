using System.Collections.Generic;
using OpenAvv.Data.Models;
using OpenAvv.ViewModels;
using System.Security.Claims;

namespace OpenAvv.Data
{
    public interface IOpenAvvRepository
    {
        void AddNewStory(Story story);
        void DeleteStoryByStoryId(int storyId);
        IEnumerable<StoryVM> GetAllStories();
        StoryVM GetStoryByStoryId(int storyId);
        void UpdateStory(Story updatedStory);
        bool IsAuthor(ClaimsPrincipal signedInUser, int storyId);
    }
}