package Models;

/**
 * Created with IntelliJ IDEA.
 * User: Mark Kamberger
 * Date: 10/16/12
 * Time: 11:35 AM
 * To change this template use File | Settings | File Templates.
 */
public class StoryTitles {
    public String TitleID;
    public String StoryTitle;
    public String SubTitle;
    public String ClipCount;
    public String Image;
    public Boolean IsLocked; //dead
    public String Password;  //dead
    public String StoryType; //dead
    public int AverageRating;
    public int SortOrder;
    public int ReviewCount;
    public int PurchasedCount;
    public int VoteCount;
    public String Author;
    public String Price;
    public String PublishDate;
    public String CoverImageUrl;
    public boolean OwnStory;
    public boolean IsDraft;
    public int NextPage;
    public int PrevPage;
    public boolean HasNext;
    public boolean HasPrev;
    public int CurrentPage;
    public int NumPages;
    public int NumPerPage;
    public boolean HasMorePages;

    public StoryTitles() {

    }

    public StoryTitles(String TitleID, String StoryTitle, String SubTitle,
                       String ClipCount, String Image, boolean IsLocked,
                       String Password, String StoryType, int AverageRating,
                       int SortOrder, int ReviewCount, int PurchasedCount,
                       int VoteCount, String Author, String Price, String PublishDate,
                       String CoverImageUrl, boolean OwnStory, boolean IsDraft,
                        int NextPage,
             int PrevPage,
             boolean HasNext,
             boolean HasPrev,
             int CurrentPage,
             int NumPages,
             int NumPerPage,
             boolean HasMorePages) {
        this.ClipCount = ClipCount;
        this.Image = Image;
        this.TitleID = TitleID;
        this.StoryTitle = StoryTitle;
        this.SubTitle = SubTitle;
        this.IsLocked = IsLocked;
        this.Password = Password;
        this.StoryType = StoryType;
        this.AverageRating = AverageRating;
        this.SortOrder = SortOrder;
        this.ReviewCount = ReviewCount;
        this.PurchasedCount = PurchasedCount;
        this.VoteCount = VoteCount;
        this.Author = Author;
        this.Price = Price;
        this.PublishDate = PublishDate;
        this.CoverImageUrl = CoverImageUrl;
        this.OwnStory = OwnStory;
        this.IsDraft = IsDraft;
        this.NextPage = NextPage;
        this.PrevPage = PrevPage;
        this.HasNext = HasNext;
        this.HasPrev = HasPrev;
        this.CurrentPage = CurrentPage;
        this.NumPages = NumPages;
        this.NumPerPage = NumPerPage;
        this.HasMorePages = HasMorePages;
    }



}
