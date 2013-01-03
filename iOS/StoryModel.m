//
// Created by markkamberger on 12/17/12.
//
// To change the template use AppCode | Preferences | File Templates.
//


#import "StoryModel.h"


@interface StoryModel ()
@property(readwrite, nonatomic) BOOL *Own;
@property(readwrite, nonatomic) NSString *StoryTitle;
@property(readwrite, nonatomic) NSString *TitleID;
@property(readwrite, nonatomic) NSString *Synopsis;
@property(readwrite, nonatomic) NSString *CoverArtUrl;
@property(readwrite, nonatomic) NSString *IsDraft;
@property(readwrite, nonatomic) NSString *AverageRating;
@property(readwrite, nonatomic) NSString *SortOrder;
@property(readwrite, nonatomic) NSString *ReviewCount;
@property(readwrite, nonatomic) NSString *PurchasedCount;
@property(readwrite, nonatomic) NSString *Author;
@property(readwrite, nonatomic) NSString *PublishDate;
@property(readwrite, nonatomic) NSString *Price;
@property(readwrite, nonatomic) NSString *Genre;
@property(readwrite, nonatomic) NSString *VoteCount;
@property (readwrite, nonatomic)NSString *AccountBalance;
@property (readwrite, nonatomic)NSString *StoryPrice;


@end

@implementation StoryModel {

}

@end