using DotFramework.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DotFramework.Infrastructure.NHibernateMaps
{
    public class StoryMap : IAutoMappingOverride<Story>
    {
        public void Override(AutoMapping<Story> mapping)
        {
            //Values
            mapping.Table("Story");
            mapping.Id(x => x.Id, "StoryID");
            mapping.Map(x => x.Title);
            mapping.Map(x => x.Synopsis);
            mapping.Map(x => x.StringAuthor).Nullable();
            mapping.Map(x => x.Price);
            mapping.Map(x => x.DateCreated).Not.Nullable();
            mapping.Map(x => x.DatePublished).Nullable();
            mapping.Map(x => x.DateRemoved).Nullable();
            //Enum
            mapping.Map(x => x.PublishStatus);
            mapping.Map(x => x.GenreCategory);
            mapping.Map(x => x.Rating);
            mapping.Map(x => x.Genre);
            mapping.References(x => x.UserAuthor);
            mapping.References(x => x.StoryContent).Cascade.SaveUpdate();
            //References
            //mapping.References(x => x.StoryContent).Fetch.Join()
             // .Column("StoryID")
             // .ReadOnly()
             // .NotFound
             // .Ignore()
              //.Cascade.All();  
            mapping.References(x => x.CoverArt).Column("CoverArtID").Fetch.Join().Cascade.None();
            /*mapping.References(x => x.CoverArt).LazyLoad()
              .Fetch.Join()
              .Column("CoverArtID")
              .ReadOnly()
              .NotFound
              .Ignore()
              .Cascade.None();*/
            mapping.References(x => x.AudioBook).Fetch.Join().LazyLoad()
              .Column("AudioBookID")
              .ReadOnly()
              .NotFound
              .Ignore()
              .Cascade.None();
            mapping.References(x => x.StoryReviewAverage)
              .Fetch.Join()
              .Column("StoryID")
              .ReadOnly()
              .NotFound
              .Ignore()
              .Cascade.None();   
            //HasMany

            //mapping.References(x => x.StoryContent).Column("StoryID").Fetch.Join().Cascade.All();
            mapping.HasMany(x => x.StoryBadges).KeyColumn("StoryID").Cascade.AllDeleteOrphan().LazyLoad();
            mapping.HasMany(x => x.Reviews).KeyColumn("StoryID").Inverse().Cascade.AllDeleteOrphan().LazyLoad();
            mapping.HasMany(x => x.Purchasers).KeyColumn("StoryID").Inverse().Cascade.AllDeleteOrphan().LazyLoad();

        }
    }
}
