using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Moq;

namespace Services
{
    [TestFixture]
    public class UserServiceTests
    {

        #region Unit Test Helper Properties/Methods

        private ModelStateDictionary ModelState
        {
            get { return ModelState = new ModelStateDictionary(); }
            set { 
                // Method not used
            }
        }

        /// <summary>
        /// Returns valid user object for validation testing
        /// </summary>
        /// <returns></returns>
        private User GetValidUserObject()
        {
            return new User
            {
                AccessToken = "1234",
                CellPhone = "4105555555",
                Email = "test@mailinator.com",
                FacebookId = 123,
                FirstName = "John",
                HashedPassword = Encoding.ASCII.GetBytes("123"),
                LastName = "Doe",
                ResetPasswordToken = "123",
                Role = Role.Administrator,
                Salt = Encoding.ASCII.GetBytes("123")
            };
        }

        #endregion

        #region CRUD Methods

        [Test]
        public void GetUsers_Returns_List_Of_Users()
        {
            // Arrange
            var foundUser = new User { Email = "test@mailinator.com" };
            var userList = new List<User> { foundUser, foundUser }.AsQueryable();
            const int currentPage = 1;
            const int numPerPage = 10;
            int total = 0;
            var filter = new UserFilter();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.Search(filter, currentPage, numPerPage, ref total)).Returns(userList);

            
            var userService = ServiceMiniMart.CreateUserService(userRepository);
            
            // Act
            var returnedList = userService.GetUsers(filter, currentPage, numPerPage);

            // Assert
            Assert.AreEqual(userList.Count(), returnedList.Items.Count);
        }
        
        [Test]
        public void GetUserById_Returns_Matching_User()
        {
            // Arrange
            const int userId = 123;
            var foundUser = new User { Email = "test@mailinator.com" };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindOne(userId)).Returns(foundUser);

         

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var user = userService.GetUserById(userId);

            // Assert
            Assert.AreEqual(foundUser, user);

        }

        [Test]
        public void GetUserByEmail_Returns_Matching_User()
        {
            // Arrange
            const string email = "test@mailinator.com";
            var foundUser = new User { Email = email };
            var userList = new List<User> { foundUser }.AsQueryable();

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);

          

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var user = userService.GetUserByEmail(email);

            // Assert
            Assert.AreEqual(foundUser, user);
        }

        [Test]
        public void GetUserByFacebookId_Returns_Matching_User()
        {
            // Arrange
            const long facebookId = 123;
            var foundUser = new User { FacebookId = facebookId, Email = "test@mailinator.com" };
            var userList = new List<User> { foundUser }.AsQueryable();

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);


           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var user = userService.GetUserByFacebookId(facebookId);

            // Assert
            Assert.AreEqual(foundUser, user);
        }

        [Test]
        public void GetUserByFacebookResetToken_Returns_Matching_User()
        {
            // Arrange
            const string resetToken = "testtoken";
            var foundUser = new User { Email = "test@mailinator.com", ResetPasswordToken = resetToken };
            var userList = new List<User> { foundUser }.AsQueryable();

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);

          
           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var user = userService.GetUserByResetToken(resetToken);

            // Assert
            Assert.AreEqual(foundUser, user);
        }


        [Test]
        public void CreateUser_Saves_To_Repository_With_Phone()
        {
            // Arrange
            var newUser = new User { Email = "test@mailinator.com", CellPhone = "(410) 555-5555" };

            var userRepository = new Mock<IUserRepository>();

          

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            userService.CreateUser(newUser);

            // Assert
            userRepository.Verify(u => u.Save(newUser));
            Assert.Pass();
        }

        [Test]
        public void CreateUser_Saves_To_Repository_Without_Phone()
        {
            // Arrange
            var newUser = new User { Email = "test@mailinator.com" };

            var userRepository = new Mock<IUserRepository>();

            

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            userService.CreateUser(newUser);

            // Assert
            userRepository.Verify(u => u.Save(newUser));
            Assert.Pass();
        }

        [Test]
        public void UpdateUser_Hashes_New_Password()
        {
            // Arrange
            var updateUser = new User { Email = "test@mailinator.com" };
            var newPass = "test";

            var userRepository = new Mock<IUserRepository>();

           

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            userService.UpdateUser(updateUser, newPass);

            // Assert
            Assert.NotNull(updateUser.HashedPassword);
        }

        [Test]
        public void UpdateUser_Keeps_Old_Password()
        {
            // Arrange

            var userRepository = new Mock<IUserRepository>();
          
           var userService = ServiceMiniMart.CreateUserService(userRepository);
            var oldPass = Encoding.ASCII.GetBytes("oldpass");
            var updateUser = new User { Email = "test@mailinator.com", HashedPassword = oldPass };

            // Act
            userService.UpdateUser(updateUser);

            // Assert
            Assert.AreEqual(oldPass, updateUser.HashedPassword);
        }

        #endregion

        #region Validation

        [Test]
        public void ValidateUser_Returns_False_For_Missing_FirstName()
        {
            //Arrange
            User user = GetValidUserObject();
            user.FirstName = null;

            var validationDictionary = new ModelStateWrapper(ModelState);

            var userRepository = new Mock<IUserRepository>();
         

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            //Act
            userService.ValidateUser(user, validationDictionary);
            //Assert
            var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
            Assert.IsFalse(validationDictionary.IsValid, errorList);
        }

        [Test]
        public void ValidateUser_Returns_False_For_Missing_LastName()
        {
            //Arrange
            User user = GetValidUserObject();
            user.LastName = null;

            var validationDictionary = new ModelStateWrapper(ModelState);

            var userRepository = new Mock<IUserRepository>();
           

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            //Act
            userService.ValidateUser(user, validationDictionary);
            //Assert
            var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
            Assert.IsFalse(validationDictionary.IsValid, errorList);
        }

        [Test]
        public void ValidateUser_Returns_False_For_Missing_Email()
        {
            //Arrange
            User user = GetValidUserObject();
            user.Email = null;

            var validationDictionary = new ModelStateWrapper(ModelState);

            var userRepository = new Mock<IUserRepository>();
         

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            //Act
            userService.ValidateUser(user, validationDictionary);
            //Assert
            var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
            Assert.IsFalse(validationDictionary.IsValid, errorList);
        }

        [Test]
        public void ValidateUser_Returns_False_For_Invalid_Email()
        {
            //Arrange
            User user = GetValidUserObject();
            user.Email = "bademail";

            var validationDictionary = new ModelStateWrapper(ModelState);

            var userRepository = new Mock<IUserRepository>();
            

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            //Act
            userService.ValidateUser(user, validationDictionary);
            //Assert
            var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
            Assert.IsFalse(validationDictionary.IsValid, errorList);
        }

        [Test]
        public void ValidateUser_Returns_False_For_Used_Email()
        {
            //Arrange
            User user = GetValidUserObject();
            user.Email = "alreadyused@mailinator.com";

            var foundUser = new FakeUser { Email = user.Email };
            foundUser.SetId(3);

            var userList = new List<User> { foundUser }.AsQueryable();

            var validationDictionary = new ModelStateWrapper(ModelState);

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);

          

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            //Act
            userService.ValidateUser(user, validationDictionary);
            //Assert
            var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
            Assert.IsFalse(validationDictionary.IsValid, errorList);
        }

        [Test]
        public void ValidateUser_Returns_False_For_Missing_Password()
        {
            //Arrange
            User user = GetValidUserObject();
            user.HashedPassword = null;

            var validationDictionary = new ModelStateWrapper(ModelState);

            var userRepository = new Mock<IUserRepository>();
        

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            //Act
            userService.ValidateUser(user, validationDictionary);
            //Assert
            var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
            Assert.IsFalse(validationDictionary.IsValid, errorList);
        }

        [Test]
        public void ValidateUser_Returns_False_For_Invalid_Phone()
        {
            //Arrange
            User user = GetValidUserObject();
            user.CellPhone = "1234";

            var validationDictionary = new ModelStateWrapper(ModelState);

            var userRepository = new Mock<IUserRepository>();
          

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            //Act
            userService.ValidateUser(user, validationDictionary);
            //Assert
            var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
            Assert.IsFalse(validationDictionary.IsValid, errorList);
        }

        [Test]
        public void ValidateUser_Returns_True_For_Valid_User()
        {
            //Arrange
            var password = HashSalt.HashPassword("password", HashSalt.GenerateSalt());
            User user = GetValidUserObject();

            var validationDictionary = new ModelStateWrapper(ModelState);

            var userRepository = new Mock<IUserRepository>();
           

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            //Act

            userService.ValidateUser(user, validationDictionary);
            //Assert
            var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
            Assert.IsTrue(validationDictionary.IsValid, errorList);
        }

    

        #endregion

        #region Authentication

        [Test]
        public void Authenticate_False_With_Invalid_Email()
        {
            // Arrange
            const string userPass = "testpass";
            var testUser = new User { Email = "test@mailinator.com" };
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
          

           var userService = ServiceMiniMart.CreateUserService(userRepository);
            userService.GenerateUserPassword(testUser, userPass);

            // Act
            var result = userService.Authenticate("bad@email.com", userPass);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public void Authenticate_False_With_Invalid_Auth_Token()
        {
            const string authToken = "badToken";
            var testUser = new User { AuthToken = "GoodToken"};
            var userRepository = new Mock<IUserRepository>();
            var userList = new List<User> { testUser }.AsQueryable();
            userRepository.Setup(u => u.FindAll()).Returns(userList);


            var userService = ServiceMiniMart.CreateUserService(userRepository);


            // Act
            var result = userService.Authenticate(authToken);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public void Authenticate_True_With_Valid_Auth_Token()
        {
            const string authToken = "GoodToken";
            var testUser = new User { AuthToken = "GoodToken" };
            var userRepository = new Mock<IUserRepository>();
            var userList = new List<User> { testUser }.AsQueryable();
            userRepository.Setup(u => u.FindAll()).Returns(userList);


            var userService = ServiceMiniMart.CreateUserService(userRepository);


            // Act
            var result = userService.Authenticate(authToken);

            // Assert
            Assert.AreEqual(testUser,result);
        }
        [Test]
        public void Authenticate_False_With_Invalid_Password()
        {
            // Arrange
            const string userPass = "testpass";
            var testUser = new User { Email = "test@mailinator.com" };
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
            

           var userService = ServiceMiniMart.CreateUserService(userRepository);
            userService.GenerateUserPassword(testUser, userPass);

            // Act
            var result = userService.Authenticate(testUser.Email, "badpass");

            // Assert
            Assert.IsNull(result);
        }


        [Test]
        public void Authenticate_True_With_Valid_Credentials()
        {
            // Arrange
            const string userPass = "testpass";
            var testUser = new User { Email = "test@mailinator.com" };
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
          

           var userService = ServiceMiniMart.CreateUserService(userRepository);
            userService.GenerateUserPassword(testUser, userPass);

            // Act
            var result = userService.Authenticate(testUser.Email, userPass);

            // Assert
            Assert.AreEqual(testUser, result);
        }


        [Test]
        public void Authenticate_Adds_New_Device()
        {
            // Arrange
            const string userPass = "testpass";
            const string hardwareId = "H123";
            var testUser = new User { Email = "test@mailinator.com" };
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
         

           var userService = ServiceMiniMart.CreateUserService(userRepository);
            userService.GenerateUserPassword(testUser, userPass);

            // Act
            var result = userService.Authenticate(testUser.Email, userPass, hardwareId);

            // Assert
            Assert.AreEqual(1, testUser.Devices.Count);
        }

        [Test]
        public void Authenticate_Updates_Existing_Device_Token()
        {
            // Arrange
            const string userPass = "testpass";
            const string hardwareId = "H123";
            const string oldToken = "oldtoken";
            var testUser = new User { Email = "test@mailinator.com" };
            testUser.Devices.Add(new Device {HardwareId = hardwareId, AuthToken = oldToken});
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
            
           var userService = ServiceMiniMart.CreateUserService(userRepository);
            userService.GenerateUserPassword(testUser, userPass);

            // Act
            var result = userService.Authenticate(testUser.Email, userPass, hardwareId);

            // Assert
            Assert.AreEqual(1, testUser.Devices.Count);
            Assert.AreNotEqual(oldToken, testUser.Devices.FirstOrDefault().AuthToken);
        }

        [Test]
        public void Authenticate_Facebook_Returns_True_For_Valid_User()
        {
            // Arrange
            var profile = new UserProfile
            {
                FacebookId = 123
            };
            var testUser = new User { FacebookId = profile.FacebookId, Email = "test@mailinator.com" };
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
          

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var result = userService.AuthenticateFacebookUser(profile);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Authenticate_Facebook_Links_Existing_User_To_Profile()
        {
            // Arrange
            var profile = new UserProfile
            {
                FacebookId = 123,
                Email = "test@mailinator.com",
                AccessToken = "validaccess"
            };
            var testUser = new User { Email = "test@mailinator.com" };
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
            

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var result = userService.AuthenticateFacebookUser(profile);

            // Assert
            Assert.AreEqual(profile.AccessToken, testUser.AccessToken);
            Assert.IsTrue(result);
        }

        [Test]
        public void Authenticate_Facebook_Returns_False_For_Invalid_User()
        {
            // Arrange
            var profile = new UserProfile
            {
                FacebookId = 123
            };
            var testUser = new User { FacebookId = 456, Email = "test@mailinator.com" };
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
           

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var result = userService.AuthenticateFacebookUser(profile);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Generate_Facebook_Creates_User()
        {
            // Arrange
            var profile = new UserProfile
            {
                FacebookId = 123,
                Email = "fb@mailinator.com",
                FirstName = "John",
                LastName = "Doe"
            };
            var userRepository = new Mock<IUserRepository>();
           

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            User result = userService.RegisterFacebookUser(profile);

            // Assert
            Assert.AreEqual(profile.FacebookId, result.FacebookId);
            Assert.AreEqual(profile.Email, result.Email);
            Assert.AreEqual(profile.FirstName, result.FirstName);
            Assert.AreEqual(profile.LastName, result.LastName);
        }
        
        [Test]
        public void Generate_Forgot_Password_Sends_Mail_For_Valid_User()
        {
            // Arrange
            var profile = new UserProfile
            {
                FacebookId = 123,
                Email = "test@mailinator.com"
            };
            var testUser = new User { FacebookId = 456, Email = profile.Email };
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
          
           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var result = userService.GenerateForgotPasswordLink("", profile.Email);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Generate_Forgot_Password_Does_Not_Send_Mail_For_Invalid_User()
        {
            // Arrange
            var profile = new UserProfile
            {
                FacebookId = 123,
                Email = "test@mailinator.com"
            };
            var testUser = new User { FacebookId = 456, Email = "someotheremail@mailinator.com" };
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
          

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var result = userService.GenerateForgotPasswordLink("", profile.Email);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Reset_Password_Sends_Mail_For_Valid_User()
        {
            // Arrange
            var testUser = GetValidUserObject();
            testUser.ResetPasswordToken = "789";
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);
            

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var result = userService.ResetPassword(testUser.ResetPasswordToken, "newpass");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Reset_Password_Does_Not_Send_Mail_For_Invalid_Token()
        {
            // Arrange// Arrange
            var testUser = GetValidUserObject();
            testUser.ResetPasswordToken = "789";
            var userList = new List<User> { testUser }.AsQueryable();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.FindAll()).Returns(userList);


           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var result = userService.ResetPassword("9999", "newpass");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Generate_Password_Creates_Pass_With_Specified_Length()
        {

            // Arrange// Arrange
            const int passLength = 4;
            var userRepository = new Mock<IUserRepository>();
           

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            var result = userService.GenerateRandomPassword(passLength);

            // Assert
            Assert.AreEqual(passLength, result.Length);
        }

        [Test]
        public void Generate_User_Password_Creates_New_Hashed_Password()
        {

            // Arrange// Arrange
            User user = GetValidUserObject();
            byte[] originalHash = user.HashedPassword;
            var userRepository = new Mock<IUserRepository>();
           

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            userService.GenerateUserPassword(user, "newpass"); ;

            // Assert
            Assert.AreNotEqual(originalHash, user.HashedPassword);
        }

        [Test]
        public void Generate_User_Password_Creates_New_Salt()
        {

            // Arrange// Arrange
            User user = GetValidUserObject();
            byte[] originalSalt = user.HashedPassword;
            var userRepository = new Mock<IUserRepository>();
           

           var userService = ServiceMiniMart.CreateUserService(userRepository);

            // Act
            userService.GenerateUserPassword(user, "newpass"); ;

            // Assert
            Assert.AreNotEqual(originalSalt, user.Salt);
        }

        #endregion

        #region Sample Integration Test
        [TestFixture]
        public class VehicleIntegrationTest : BaseIntegrationTest
        {

            /// <summary>
            /// Gets the vehicles paged list_ valid.
            /// </summary>
            [Test]
            public void GetVehiclesPagedList_Valid()
            {
                var vehicleService = ServiceMiniMart.CreateVehicleService();
                var vehicleFilter = new VehicleFilter();
                // vehicleFilter.VehicleId = 1;
                var v = vehicleService.GetVehicles(vehicleFilter, 0, 10);
                Assert.Greater(v.Items.Count, 0);
            }
            /// <summary>
            /// Gets the vehicles paged list_ in valid.
            /// </summary>
            [Test]
            public void GetVehiclesPagedList_InValid()
            {
                var vehicleService = ServiceMiniMart.CreateVehicleService();
                var vehicleFilter = new VehicleFilter();
                // vehicleFilter.VehicleId = -1;
                var v = vehicleService.GetVehicles(vehicleFilter, 0, 10);
                Assert.Less(v.Items.Count, 1);
            }
            /// <summary>
            /// Validates the vehicle_is invalid.
            /// </summary>
            [Test]
            public void ValidateVehicle_isInvalid()
            {
                var vehicleService = ServiceMiniMart.CreateVehicleService();
                var validationDictionary = new ModelStateWrapper(ModelState);
                var vehicle = new Vehicle
                {

                };
                vehicleService.ValidateVehicle(vehicle, validationDictionary);
                var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
                Assert.IsFalse(validationDictionary.IsValid, errorList);
            }
            /// <summary>
            /// Validates the vehicle_is valid.
            /// </summary>
            [Test]
            public void ValidateVehicle_isValid()
            {
                var vehicleService = ServiceMiniMart.CreateVehicleService();
                var validationDictionary = new ModelStateWrapper(ModelState);
                vehicleService.ValidateVehicle(GetValidVehicle(), validationDictionary);
                var errorList = validationDictionary.Errors.Aggregate(string.Empty, (current, error) => current + error.Message + "; ");
                Assert.IsTrue(validationDictionary.IsValid, errorList);
            }
            /// <summary>
            /// Saves a vehicle.
            /// </summary>
            [Test]
            public void SaveVehicle()
            {
                var results = new List<string>();
                var success = true;
                using (var session = NHibernateSession.Current)
                using (var trans = session.BeginTransaction())
                {
                    var vehicle = GetValidVehicle();
                    session.Save(vehicle);
                    if (vehicle.Id == 0)
                    {
                        results.Add("Failed to save vehicle");
                        success = false;
                    }
                    if (success)
                    {
                        results.Add("Success");
                    }
                    trans.Rollback();
                }

                Assert.IsTrue(success, results.Aggregate((i, j) => i + j));


            }


        }
        #endregion

    }
}
