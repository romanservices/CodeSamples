//
//  GlobalVariables.m
//  StoryHound
//
//  Created by Mark Kamberger on 12/13/12.
//  Copyright (c) 2012 Mark Kamberger. All rights reserved.
//

#import "GlobalVariables.h"
#import "KeychainItemWrapper.h"

@implementation GlobalVariables
@synthesize apiLogin;
@synthesize getUserDetails;
@synthesize isSessionActive;
@synthesize registerUser;
@synthesize topStories;
@synthesize baseUrl;
@synthesize newestStories;
@synthesize genres;
@synthesize storyDetails;
@synthesize getStoryContent;
@synthesize buyStoryNow;
@synthesize getMyPurchasedStories;
@synthesize getMyWrittenStories;

//
@synthesize password;
@synthesize userEmail;
@synthesize connectionError;
@synthesize successFail;
@synthesize loginFail;
@synthesize getStoriesFail;
@synthesize buyFail;

+(id)sharedInstance{
    static GlobalVariables *sharedMyInstance = nil;
    static dispatch_once_t onceToken;
    NSString *url = @"http://192.168.0.111";
    dispatch_once(&onceToken, ^{
        sharedMyInstance = [[self alloc]init];
        KeychainItemWrapper *keychain = [[KeychainItemWrapper alloc] initWithIdentifier:@"StoryHound" accessGroup:nil];
        //Authentication
        sharedMyInstance.userEmail = [keychain objectForKey:(__bridge id)kSecAttrAccount];
        //URLs
        sharedMyInstance.password = [keychain objectForKey:(__bridge id)kSecValueData];
        sharedMyInstance.apiLogin = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/login/login"]];
        sharedMyInstance.getUserDetails = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/user/get"]];
        sharedMyInstance.isSessionActive = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/login/AmILoggedIn"]];
        sharedMyInstance.registerUser = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/user/create"]];
        sharedMyInstance.topStories = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/Story/GetTopStories"]];
        sharedMyInstance.newestStories = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/Story/GetNewStories"]];
        sharedMyInstance.genres = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/Genre/GetGenres"]];
        sharedMyInstance.storyDetails = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/Story/GetDetails"]];
        sharedMyInstance.getStoryContent = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/Story/GetStoryListContent"]];
        sharedMyInstance.buyStoryNow = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/Story/Buy"]];
        sharedMyInstance.getMyWrittenStories = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/Story/GetMyCreatedStories"]];
        sharedMyInstance.getMyPurchasedStories = [[NSURL alloc] initWithString:[url stringByAppendingString:@"/api/Story/GetMyOwnedStories"]];
        //Strings
        sharedMyInstance.baseUrl = url;
        sharedMyInstance.connectionError = @"Can't seem to connect to the server, try again later";
        sharedMyInstance.successFail = @"Odd...Something went wrong, try again";
        sharedMyInstance.loginFail = @"Login failed, check your username and password";
        sharedMyInstance.getStoriesFail = @"Can't seem to find what i was looking for, try gain";
        sharedMyInstance.buyFail = @"Odd...something went wrong while trying to buy this story, try again";



    });
    return sharedMyInstance;
}


@end
