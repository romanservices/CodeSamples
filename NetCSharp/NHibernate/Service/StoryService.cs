using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using DotFramework.Domain;
using DotFramework.Domain.DataModels;
using DotFramework.Domain.Enumerable;
using DotFramework.Infrastructure;
using DotFramework.Services.PagedList;
using DotFramework.Services.Validation;

namespace DotFramework.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryAutoSaveRepository _storyAutoSaveRepository;
        private readonly IPurchasedStoryRepository _purchasedStoryRepository;
        private readonly ICoverArtRepository _coverArtRepository;

        private readonly IStoryContentRepository _storyContentRepository;
        public StoryService(IStoryRepository storyRepository,
            IStoryContentRepository storyContentRepository,
            IStoryAutoSaveRepository storyAutoSaveRepository,
            IPurchasedStoryRepository purchasedStoryRepository,
            ICoverArtRepository coverArtRepository)
        {
   
            _storyContentRepository = storyContentRepository;
            _storyRepository = storyRepository;
            _storyAutoSaveRepository = storyAutoSaveRepository;
            _purchasedStoryRepository = purchasedStoryRepository;
            _coverArtRepository = coverArtRepository;
        }

      

        public IPagedList<Story> GetStories(StoryFilter filter, int currentPage, int numPerPage)
        {
            int totalRecords = 0;
            if(!filter.PublishStatus.HasValue)
            {
                filter.PublishStatus = PublishStatus.Published;
            }
            List<Story> stories = _storyRepository.Search(filter, currentPage,
                numPerPage, ref totalRecords).ToList();
            return new PagedList<Story>(stories, currentPage, numPerPage, totalRecords);
        }

        public IList<Story> GetStories(StoryFilter filter)
        {
           
            if (!filter.PublishStatus.HasValue)
            {
                filter.PublishStatus = PublishStatus.Published;
            }
            List<Story> stories = _storyRepository.Search(filter).ToList();
            return new List<Story>(stories);
        }

        public IList<Story> GetStoriesQuery(StoryFilter filter)
        {
            if (!filter.PublishStatus.HasValue)
            {
                filter.PublishStatus = PublishStatus.Published;
            }
            List<Story> stories = _storyRepository.SearchQuery(filter).ToList();
            return new List<Story>(stories);
        }

        public IList<Story> GetTopStories(int number)
        {
            var stories = _storyRepository.FindAll().Where(x => x.StoryReviewAverage != null);
            return stories.OrderByDescending(x => x.StoryReviewAverage.SortingOrder).Take(number).ToList();
        }

        public IList<Story> GetNewTStories(int number)
        {
            return _storyRepository.FindAll().OrderByDescending(s => s.DatePublished).Take(number).ToList();
        }

        public IList<Story> GeTStoriesByUserID(int userID)
        {
            return _storyRepository.FindAll().Where(s => s.UserAuthor.Id == userID).ToList();
        }

        public Story GetStoryByID(int id)
        {
            return _storyRepository.FindOne(id);
        }

        public void SaveStory(Story story)
        {
            story.StringAuthor = story.UserAuthor.FullName;
            if(story.CoverArt != null)
            {
                _coverArtRepository.Save(story.CoverArt);    
            }
            _storyRepository.Save(story);
            _storyAutoSaveRepository.DeleteAll(story.UserAuthor.Id);
        }



        public int StoryCount()
        {
            return _storyRepository.FindAll().Count();
        }

        public IList<GenreStoryCount> GetGenres()
        {
            var gList = new List<GenreStoryCount>();
            
            foreach (Genre genre in Enum.GetValues(typeof(Genre)))
            {
                var count = _storyRepository.FindAll().Count(s => s.Genre == genre);
                var gsc = new GenreStoryCount
                              {
                                  Genre = genre,
                                  StoryCount = count
                              };
                gList.Add(gsc);
            }
            return gList;
        }

        public IList<GenreStoryCount> SearchGenre(string searchTerm)
        {
            var gList = new List<GenreStoryCount>();
            foreach (Genre genre in Enum.GetValues(typeof(Genre)))
            {
                var count = _storyRepository.FindAll().Count(s => s.Genre == genre);
                var gsc = new GenreStoryCount
                {
                    Genre = genre,
                    StoryCount = count
                };
                if(gsc.Genre.GetDescription().ToLower().Contains(searchTerm.ToLower()))
                gList.Add(gsc);
            }
            return gList;
        }


        public StoryContent GetStoryContentByID(int storyId)
        {
            return _storyContentRepository.FindAll().FirstOrDefault(c => c.Story.Id == storyId);
        }

        public void SaveStoryContent(StoryContent storyContent)
        {
            _storyContentRepository.Save(storyContent);
        }

        public StoryAutoSave GetAutoSave(int userId)
        {
            return _storyAutoSaveRepository.FindAll().OrderByDescending(s=>s.DraftDate).FirstOrDefault(s=>s.UserId == userId);
        }

        public void DeleteAutoSave(StoryAutoSave storyAutoSave)
        {
          _storyAutoSaveRepository.Delete(storyAutoSave);
        }

        public void SaveAutoSave(StoryAutoSave storyAutoSave)
        {
           var old =  _storyAutoSaveRepository.FindAll().Where(s => s.UserId == storyAutoSave.UserId);
            foreach (var o in old)
            {
                _storyAutoSaveRepository.Delete(o);
            }
            _storyAutoSaveRepository.Save(storyAutoSave);
        }

        public bool BuyStory(Story story, User user)
        {
            var purchased = new PurchasedStories
                                {
                                    Story = story,
                                    User = user
                                };

            try
            {
               _purchasedStoryRepository.Save(purchased);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
