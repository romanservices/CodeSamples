using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using DotFramework.Domain;
using DotFramework.Domain.Enumerable;
using DotFramework.Domain.StoryBook;
using DotFramework.Infrastructure.UserRepository;
using DotFramework.Services.Validation;

namespace DotFramework.Services.StoryBookService
{
    public class StoryBookService :IStoryBookService
    {
        #region Fields

        private readonly ITitleRepository _titleRepository;
        private readonly IStoryClipRepository _storyClipRepository;
        #endregion
        #region Constructor
        public StoryBookService(ITitleRepository titleRepository, IStoryClipRepository storyClipRepository)
        {
            _storyClipRepository = storyClipRepository;
            _titleRepository = titleRepository;
        }
        #endregion
        #region Service Methods
        public void CreateNewTitle(Title title)
        {
            title.StoryStatus = StoryStatus.Open;
            title.DateCreated = DateTime.UtcNow;
           _titleRepository.Save(title);
        }

        public IList<Title> GetTitlesByUserId(int userId)
        {
            return _titleRepository.FindAll().Where(t => t.User.Id == userId).ToList();
        }

        public IList<Title> GetTitles(User user)
        {
            var allTitles = _titleRepository.FindAll();
            var titles = new List<Title>();
            foreach (var title in allTitles)
            {
                if(title.PublishStatus == PublishStatus.Private && title.User == user)
                {
                   titles.Add(title);
                }
                if(title.PublishStatus == PublishStatus.Group)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.Public)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.ReadOnly)
                {
                    titles.Add(title);
                }
              

            }
            return titles;
        }

        public IList<Title> GetTop10Titles(User user)
        {
            var Titles = _titleRepository.FindAll().Where(x=>x.TitleReviewAverage != null);
            var allTitles = Titles.OrderByDescending(x => x.TitleReviewAverage.SortingOrder).Take(10);
            var titles = new List<Title>();
            foreach (var title in allTitles)
            {
                if (title.PublishStatus == PublishStatus.Private && title.User == user)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.Group)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.Public)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.ReadOnly)
                {
                    titles.Add(title);
                }


            }
            return titles;
        }

        public IList<Title> GetTitlesByGenre(Genre category, PublishStatus publishStatus, User user)
        {
            var allTitles = _titleRepository.FindAll().Where(x => x.Genre == category).Where(x=>x.PublishStatus == publishStatus).ToList();
               var titles = new List<Title>();
            foreach (var title in allTitles)
            {
                if(title.PublishStatus == PublishStatus.Private && title.User == user)
                {
                   titles.Add(title);
                }
                if(title.PublishStatus == PublishStatus.Group)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.Public)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.ReadOnly)
                {
                    titles.Add(title);
                }
              

            }
            return titles;
        }

        public IList<Title> GetTitlesByPublication(PublishStatus publishStatus, User user)
        {
            var allTitles = _titleRepository.FindAll().Where(x => x.PublishStatus == publishStatus).ToList();
            var titles = new List<Title>();
            foreach (var title in allTitles)
            {
                if (title.PublishStatus == PublishStatus.Private && title.User == user)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.Group)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.Public)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.ReadOnly)
                {
                    titles.Add(title);
                }


            }
            return titles;
        }

        public IList<Title> GetMyTitlesByGenre(Genre category, PublishStatus publishStatus, User user)
        {
            var allTitles = _titleRepository.FindAll().Where(x => x.Genre == category).Where(x => x.PublishStatus == publishStatus).ToList();
            var titles = new List<Title>();
            foreach (var title in allTitles)
            {
                if (title.PublishStatus == PublishStatus.Private && title.User == user)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.Group && title.User == user)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.Public && title.User == user)
                {
                    titles.Add(title);
                }
                if (title.PublishStatus == PublishStatus.ReadOnly && title.User == user)
                {
                    titles.Add(title);
                }


            }
            return titles;
        }

        public IList<GenreModel> GetMyStories(User user)
        {
            var models = new List<GenreModel>();

            foreach (var genre in Enum.GetValues(typeof(Genre)).Cast<Genre>())
            {
                var allTitles = _titleRepository.FindAll().Where(x => x.Genre == genre);
                var titles = new List<Title>();
                foreach (var title in allTitles)
                {
                    if (title.PublishStatus == PublishStatus.Private && title.User == user)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Group && title.User == user)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Public && title.User == user)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.ReadOnly && title.User == user)
                    {
                        titles.Add(title);
                    }


                }

                var model = new GenreModel
                {
                    Genre = genre.ToString(),
                    Description = genre.GetDescription(),
                    Count = titles.Count

                };
                models.Add(model);

            }
            return models;
        }

        public Title GetTitleByID(int id)
        {
            return _titleRepository.FindOne(id);
        }

        public IList<GenreModel> GetGenre(User user)
        {
            var models = new List<GenreModel>();

            foreach (var genre in Enum.GetValues(typeof(Genre)).Cast<Genre>())
            {
                var allTitles = _titleRepository.FindAll().Where(x => x.Genre == genre);
                var titles = new List<Title>();
                foreach (var title in allTitles)
                {
                    if (title.PublishStatus == PublishStatus.Private && title.User == user)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Group)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Public)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.ReadOnly)
                    {
                        titles.Add(title);
                    }


                }
                
                var model = new GenreModel
                                {
                                    Genre = genre.ToString(),
                                    Description = genre.GetDescription(),
                                    Count = titles.Count
                                    
                                };
                models.Add(model);

            }
            return models;
        }

        public IList<PublishStatusModel> GetPublishStatusByGenre(Genre category, User user)
        {
            var models = new List<PublishStatusModel>();

            foreach (var publishStatus in Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>())
            {
                var allTitles = _titleRepository.FindAll().Where(x=>x.Genre == category).Where(x => x.PublishStatus == publishStatus);
                var titles = new List<Title>();
                foreach (var title in allTitles)
                {
                    if (title.PublishStatus == PublishStatus.Private && title.User == user)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Group)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Public)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.ReadOnly)
                    {
                        titles.Add(title);
                    }


                }
                var model = new PublishStatusModel
                {
                    PublishStatus = publishStatus.ToString(),
                    Description = publishStatus.GetDescription(),
                    Count = titles.Count

                };
                models.Add(model);

            }
            return models;
        }

        public IList<PublishStatusModel> GetPublishStatus(User user)
        {
            var models = new List<PublishStatusModel>();

            foreach (var publishStatus in Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>())
            {
                var allTitles = _titleRepository.FindAll().Where(x => x.PublishStatus == publishStatus);
                var titles = new List<Title>();
                foreach (var title in allTitles)
                {
                    if (title.PublishStatus == PublishStatus.Private && title.User == user)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Group)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Public)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.ReadOnly)
                    {
                        titles.Add(title);
                    }


                }
                var model = new PublishStatusModel
                {
                    PublishStatus = publishStatus.ToString(),
                    Description = publishStatus.GetDescription(),
                    Count = titles.Count

                };
                models.Add(model);

            }
            return models;
        }

        public IList<PublishStatusModel> GetMyPublishStatusByGenre(Genre category, User user)
        {
            var models = new List<PublishStatusModel>();

            foreach (var publishStatus in Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>())
            {
                var allTitles = _titleRepository.FindAll().Where(x => x.Genre == category).Where(x => x.PublishStatus == publishStatus);
                var titles = new List<Title>();
                foreach (var title in allTitles)
                {
                    if (title.PublishStatus == PublishStatus.Private && title.User == user)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Group && title.User == user)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.Public && title.User == user)
                    {
                        titles.Add(title);
                    }
                    if (title.PublishStatus == PublishStatus.ReadOnly && title.User == user)
                    {
                        titles.Add(title);
                    }


                }
                var model = new PublishStatusModel
                {
                    PublishStatus = publishStatus.ToString(),
                    Description = publishStatus.GetDescription(),
                    Count = titles.Count

                };
                models.Add(model);

            }
            return models;
        }

        public IList<StoryClip> GetStoryClipsByID(int titleID)
        {
            return _storyClipRepository.FindAll()
                .Where(x => x.Title.Id == titleID)
                .Where(x=>x.Included)
                .OrderBy(x=>x.Id)
                .ToList();
        }

        public void AddStoryClip(StoryClip storyClip)
        {
            _storyClipRepository.Save(storyClip);
        }

        public void UpdateTitle(Title title)
        {
            _titleRepository.Save(title);
        }

        public bool ValidateCanAddClip(StoryClip clip, string deviceID, IValidationDictionary validationDictionary)
        {

            if (String.IsNullOrEmpty(clip.Clip))
            {
                validationDictionary.AddError("Clip", "Text can not be blank");
            }
            if (clip.Title.StoryStatus == StoryStatus.Open)
            {
                validationDictionary.AddError("Status", "Story is not ready to be added to");
            }
            if (String.IsNullOrEmpty(deviceID))
            {
                validationDictionary.AddError("Device", "DeviceID is blank");
            }
            if (deviceID != clip.Title.DeviceID)
            {
                validationDictionary.AddError("Device", "DeviceID does not match");
            }
            return validationDictionary.IsValid;
        }

        public bool ValidateTitle(Title title, IValidationDictionary validationDictionary)
        {
            if (String.IsNullOrEmpty(title.StoryTitle))
            {
                validationDictionary.AddError("Title", "Title is required");
            }
            return validationDictionary.IsValid;
        }

        public bool ValidatePassword(Title title, string password, IValidationDictionary validationDictionary)
        {
            if (String.IsNullOrEmpty(password))
            {
                validationDictionary.AddError("Password", "This story is password protected");
            }

            if (!String.IsNullOrEmpty(password))
            {
                if(password != title.Password)
                validationDictionary.AddError("Password", "Wrong password!");
            }
            return validationDictionary.IsValid;
        }

        #endregion
    }
}
