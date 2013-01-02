using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotFramework.Domain;
using DotFramework.Domain.Enumerable;
using DotFramework.Domain.StoryBook;
using DotFramework.Services.Validation;

namespace DotFramework.Services.StoryBookService
{
    public interface IStoryBookService
    {
        void CreateNewTitle(Title title);
        IList<Title> GetTitlesByUserId(int userId);
        IList<Title> GetTitles(User user);
        IList<Title> GetTop10Titles(User user);
        IList<Title> GetTitlesByGenre(Genre category, PublishStatus publishStatus, User user);
        IList<Title> GetTitlesByPublication(PublishStatus publishStatus, User user);
        IList<Title> GetMyTitlesByGenre(Genre category, PublishStatus publishStatus, User user);
        IList<GenreModel> GetMyStories( User user);
        Title GetTitleByID(int id);
        IList<GenreModel> GetGenre(User user);
        IList<PublishStatusModel> GetPublishStatusByGenre(Genre category, User user);
        IList<PublishStatusModel> GetPublishStatus(User user);
        IList<PublishStatusModel> GetMyPublishStatusByGenre(Genre category, User user);
        IList<StoryClip> GetStoryClipsByID(int titleID);
        void AddStoryClip(StoryClip storyClip);
        void UpdateTitle(Title title);
        bool ValidateCanAddClip(StoryClip clip, string deviceID, IValidationDictionary validationDictionary);
        bool ValidateTitle(Title title,IValidationDictionary validationDictionary);
        bool ValidatePassword(Title title, string password, IValidationDictionary validationDictionary);
    }
}
