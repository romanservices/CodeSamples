//
//  LoginController.m
//  StoryHound
//
//  Created by Mark Kamberger on 12/10/12.
//  Copyright (c) 2012 Mark Kamberger. All rights reserved.
//

#import "LoginController.h"
#import "RegisterController.h"
#import "ASIHTTPRequest.h"
#import "ASIFormDataRequest.h"
#import "SBJson.h"
#import "AlertMessageHelper.h"
#import "GlobalVariables.h"
#import "HomeViewController.h"
#import "KeychainItemWrapper.h"


#include <CommonCrypto/CommonDigest.h>

#define kSalt @"adlfu3489tyh2jnkLIUGI&%EV(&0982cbgrykxjnk8855"
@interface LoginController ()

@end

@implementation LoginController
@synthesize tf_password;
@synthesize tf_email;
@synthesize btn_login;
@synthesize uiActivityView;




- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    uiActivityView  = [[UIActivityIndicatorView alloc]initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray];
    uiActivityView.frame = CGRectMake(0.0, 0.0, 40.0, 40.0);
    uiActivityView.center = self.view.center;
    [tf_email becomeFirstResponder];
    [self.view addSubview:uiActivityView];
    [uiActivityView bringSubviewToFront:self.view];
    [UIApplication sharedApplication].networkActivityIndicatorVisible = TRUE;
    // Do any additional setup after loading the view from its nib.

}

- (void)viewDidUnload
{

    [self setTf_email:nil];
    [self setTf_password:nil];
    [super viewDidUnload];
    
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
	return YES;
}



- (IBAction)btn_RegisterClick:(id)sender {
    RegisterController * registerController =[[RegisterController alloc]init];
    [self.navigationController pushViewController:registerController animated:true];
}
- (IBAction)btn_loginClick:(id)sender {
    [uiActivityView startAnimating];
    if(tf_email.text.length == 0 || tf_password.text.length == 0){
          [AlertMessageHelper showMessage:@"Missing Information" :@"You need to enter both your email and password" :@"OK"];
    }
    NSURL *url = [[GlobalVariables sharedInstance]apiLogin];
    ASIFormDataRequest *request = [ASIFormDataRequest requestWithURL:url];
    [request setUseKeychainPersistence:true];
    [request setTimeOutSeconds:20];
    [request setPostValue:tf_email.text forKey:@"Email"];
    [request setPostValue:tf_password.text forKey:@"Password"];
    [request setDelegate:self];
    [request startAsynchronous];
}
-(void)requestFinished:(ASIHTTPRequest *) request{
  if(request.responseStatusCode == 400){
     [AlertMessageHelper showMessage:@"Code 400" :@"Unable to contact server" :@"OK"];
    }
    NSString *responseString = [request responseString];
    NSDictionary *json = [responseString JSONValue];
    NSString *success = [json objectForKey:@"Success"];
    BOOL *b = success.boolValue;
    if(b){
        KeychainItemWrapper *keychain = [[KeychainItemWrapper alloc] initWithIdentifier:@"StoryHound" accessGroup:nil];
        [keychain setObject:tf_email.text forKey:(__bridge id)kSecAttrAccount];
        [keychain setObject:tf_password.text forKey:(__bridge id)kSecValueData];
        HomeViewController *home = [[HomeViewController alloc]init];
        [self.navigationController pushViewController:home animated:true];
    }
    else{
        [AlertMessageHelper showMessage:@"Login Failed":[json objectForKey:@"Message"] :@"OK"];
    }
    [uiActivityView stopAnimating];
}
-(void)requestFailed:(ASIHTTPRequest *)request{
    NSError *error = [request error];
    [uiActivityView stopAnimating];
    [AlertMessageHelper showMessage:@"Bad Request" :[request responseString] :@"OK"];
}
@end
