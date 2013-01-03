//
//  GlobalVariables.h
//  StoryHound
//
//  Created by Mark Kamberger on 12/13/12.
//  Copyright (c) 2012 Mark Kamberger. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface GlobalVariables : NSObject
{
    NSURL *apiLogin;
    NSURL *getUserDetails;
    NSURL *isSessionActive;
    NSURL *registerUser;
    NSURL *topStories;
    NSURL *newestStories;
    NSURL *genres;
    NSURL *storyDetails;
    NSURL *getStoryContent;
    NSURL *buyStoryNow;
    NSURL *getMyWrittenStories;
    NSURL *getMyPurchasedStories;

    
    //
    NSString *userEmail;
    NSString *password;
    NSString *baseUrl;
    NSString *connectionError;
    NSString *successFail;
    NSString *loginFail;
    NSString *buyFail;
    NSString *getStoriesFail;

}
@property (nonatomic,retain)NSURL*apiLogin;
@property (nonatomic,retain)NSURL*getUserDetails;
@property (nonatomic, retain)NSURL *isSessionActive;
@property (nonatomic, retain)NSURL *registerUser;
@property (nonatomic, retain)NSURL *topStories;
@property (nonatomic, retain)NSURL *newestStories;
@property (nonatomic, retain)NSURL *genres;
@property (nonatomic, retain)NSURL *storyDetails;
@property (nonatomic, retain)NSURL *getStoryContent;
@property (nonatomic, retain)NSURL *buyStoryNow;
@property (nonatomic, retain)NSURL *getMyPurchasedStories;
@property (nonatomic, retain)NSURL *getMyWrittenStories;
//
@property (nonatomic, retain)NSString *userEmail;
@property (nonatomic, retain)NSString *password;
@property (nonatomic, retain)NSString *baseUrl;
@property (nonatomic, retain)NSString *connectionError;
@property (nonatomic, retain)NSString *successFail;
@property (nonatomic, retain)NSString *loginFail;
@property (nonatomic, retain)NSString *buyFail;
@property (nonatomic, retain)NSString *getStoriesFail;


+(id)sharedInstance;


@end
