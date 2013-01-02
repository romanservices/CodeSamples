using System.Collections.Generic;
using DotFramework.Domain;
using DotFramework.Domain.DataModels;
using DotFramework.Domain.Enumerable;
using DotFramework.Services.PagedList;

namespace DotFramework.Services
{
    public interface IStoryService
    {
        
        IPagedList<Story> GetStories(StoryFilter filter, int currentPage, int numPerPage);
        IList<Story> GetStories(StoryFilter filter);
        IList<Story> GetStoriesQuery(StoryFilter filter);
        IList<Story> GetTopStories(int number);
        IList<Story> GetNewTStories(int number);
        IList<Story> GeTStoriesByUserID(int userID);
        Story GetStoryByID(int id);
        void SaveStory(Story story);
        int StoryCount();

        //Genre
        IList<GenreStoryCount> GetGenres();
        IList<GenreStoryCount> SearchGenre(string genre); //StoryContent
        StoryContent GetStoryContentByID(int storyId);
        void SaveStoryContent(StoryContent storyContent);

        //StoryAutoSave
        StoryAutoSave GetAutoSave(int userId);
        void DeleteAutoSave(StoryAutoSave storyAutoSave);
        void SaveAutoSave(StoryAutoSave storyAutoSave);

        //Buy
        bool BuyStory(Story story, User user);
    }
}
