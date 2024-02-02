***REMOVED***using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Dtos.User.Request;
using ScoreManagementApi.Core.Dtos.User.Response;
using ScoreManagementApi.Core.Entities;
using ScoreManagementApi.Core.OtherObjects;
using ScoreManagementApi.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

***REMOVED***
***REMOVED***
    public class UserService : IUserService
    ***REMOVED***

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
***REMOVED***

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        ***REMOVED***
            _userManager = userManager;
            _roleManager = roleManager;
***REMOVED***
***REMOVED***


        public async Task<ResponseData<UserResponse>> CreateUser(CreateUserRequest request)
        ***REMOVED***
            List<ErrorMessage> erorrs = new List<ErrorMessage>();
           
            //check user exist by email or username
            var isExistsUser = await _userManager.FindByNameAsync(request.UserName);

            if (isExistsUser != null)
                erorrs.Add(new ErrorMessage
                ***REMOVED***
                    Key = "UserName",
                    Message = "UserName is existed!"
        ***REMOVED***);


            if (!IsValidEmail(request.Email))
            ***REMOVED***
                erorrs.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Email",
                    Message = "Email is invalid!"
        ***REMOVED***);
    ***REMOVED***
            else
            ***REMOVED***
                isExistsUser = await _userManager.FindByEmailAsync(request.Email);

                if (isExistsUser != null)
                    erorrs.Add(new ErrorMessage
                    ***REMOVED***
                        Key = "Email",
                        Message = "Email is existed!"
            ***REMOVED***);
    ***REMOVED***


            if (erorrs != null && erorrs.Count > 0)
                return new ResponseData<UserResponse>
                ***REMOVED***
                    StatusCode = 400,
                    Erorrs = erorrs
    ***REMOVED***

            //create user and add role
            User newUser = new User()
            ***REMOVED***
                Email = request.Email,
                UserName = request.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                Active = true,
                FullName = request.FullName,
                Gender = request.Gender,
***REMOVED***

            var createUserResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!createUserResult.Succeeded)
            ***REMOVED***
                var errorString = "User Creation Failed Because: ";
                foreach (var error in createUserResult.Errors)
                ***REMOVED***
                    errorString += " # " + error.Description;
        ***REMOVED***
                return new ResponseData<UserResponse>
                ***REMOVED***
                    Message = errorString,
                    StatusCode = 400
    ***REMOVED***
    ***REMOVED***

            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.STUDENT);

            EmailServices emailServices = new EmailServices();
            await emailServices.SendAsync(new EmailMessage
            ***REMOVED***
                To = newUser.Email,
                Subject = "New Account",
                Content = "Your New Account in Score Management System is:\n" +
                $"Email: ***REMOVED***newUser.Email***REMOVED***\n" +
                $"Username: ***REMOVED***newUser.UserName***REMOVED***\n" +
                $"Password: ***REMOVED***request.Password***REMOVED***"
    ***REMOVED***);

            return new ResponseData<UserResponse>
            ***REMOVED***
                Message = "Ok",
                StatusCode = 200,
                Data = new UserResponse
                ***REMOVED***
                    Id = newUser.Id,
                    FullName = newUser.FullName,
                    Email = newUser.Email,
                    UserName = newUser.UserName,
                    Password = "******",
                    Gender = newUser.Gender,
                    Active = newUser.Active,
                    Roles = new List<string>
                    ***REMOVED***
                        StaticUserRoles.STUDENT
            ***REMOVED***
        ***REMOVED***
***REMOVED***
***REMOVED***

        public bool IsValidEmail(string email)
        ***REMOVED***
            string pattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z.]***REMOVED***2,10***REMOVED***$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
***REMOVED***

        public async Task<ResponseData<LoginResponse>> Login(LoginRequest request)
        ***REMOVED***
            var user = await _userManager.FindByNameAsync(request.UserNameOrEmail)
                ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);

            if (user is null)
                return new ResponseData<LoginResponse>
                ***REMOVED***
                    Message = "Invalid Credentials",
                    StatusCode = 400
    ***REMOVED***

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordCorrect)
                return new ResponseData<LoginResponse>
                ***REMOVED***
                    Message = "Invalid Credentials",
                    StatusCode = 400
    ***REMOVED***

            if (!user.Active)
                return new ResponseData<LoginResponse>
                ***REMOVED***
                    Message = "Your Account is InActive",
                    StatusCode = 400
    ***REMOVED***

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            ***REMOVED***
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
***REMOVED***

            foreach (var item in userRoles)
            ***REMOVED***
                authClaims.Add(new Claim(ClaimTypes.Role, item));
    ***REMOVED***

            var token = GenerateNewJsonWebToken(authClaims);

            return new ResponseData<LoginResponse>
            ***REMOVED***
                Message = "Ok",
                Data = new LoginResponse
                ***REMOVED***
                    FullName = user.FullName,
                    Token = token
        ***REMOVED***,
                StatusCode = 200
***REMOVED***
***REMOVED***

        public async Task<ResponseData<UserResponse>> UpdateUser(UpdateUserRequest request)
        ***REMOVED***
            var isExistsUser = await _userManager.FindByIdAsync(request.Id);

            if (isExistsUser == null)
            ***REMOVED***
                return new ResponseData<UserResponse>
                ***REMOVED***
                    Message = "User not found!",
                    StatusCode = 404
    ***REMOVED***
    ***REMOVED***

            List<ErrorMessage> erorrs = await ValidateUpdateUser(request, isExistsUser);

            if (erorrs != null && erorrs.Count > 0)
            ***REMOVED***
                return new ResponseData<UserResponse>
                ***REMOVED***
                    StatusCode = 400,
                    Erorrs = erorrs
    ***REMOVED***
    ***REMOVED***

            isExistsUser.Email = request.Email;
            isExistsUser.UserName = request.UserName;
            isExistsUser.Active = request.Active;
            isExistsUser.FullName = request.FullName;
            isExistsUser.Gender = request.Gender;

            if (request.Password != null)
            ***REMOVED***
                isExistsUser.PasswordHash = new PasswordHasher<User>().HashPassword(isExistsUser, request.Password);
    ***REMOVED***

            var updateUserResult = await _userManager.UpdateAsync(isExistsUser);

            if (!updateUserResult.Succeeded)
            ***REMOVED***
                var errorString = "User Update Failed Because: ";
                foreach (var error in updateUserResult.Errors)
                ***REMOVED***
                    errorString += " # " + error.Description;
        ***REMOVED***
                return new ResponseData<UserResponse>
                ***REMOVED***
                    Message = errorString,
                    StatusCode = 400
    ***REMOVED***
    ***REMOVED***

            return new ResponseData<UserResponse>
            ***REMOVED***
                Message = "Ok",
                StatusCode = 200,
                Data = new UserResponse
                ***REMOVED***
                    Id = isExistsUser.Id,
                    FullName = isExistsUser.FullName,
                    Email = isExistsUser.Email,
                    UserName = isExistsUser.UserName,
                    Password = "******",
                    Gender = isExistsUser.Gender,
                    Active = isExistsUser.Active
        ***REMOVED***
***REMOVED***
***REMOVED***


        private async Task<List<ErrorMessage>> ValidateUpdateUser(UpdateUserRequest request, User isExistsUser)
        ***REMOVED***
            List<ErrorMessage> erorrs = new List<ErrorMessage>();
            var isExistUserByUsernameOrEmail = await _userManager.FindByNameAsync(request.UserName);
            if (isExistUserByUsernameOrEmail != null
                && !isExistsUser.Id.Equals(isExistUserByUsernameOrEmail.Id))

                erorrs.Add(new ErrorMessage
                ***REMOVED***
                    Key = "UserName",
                    Message = "UserName is existed!"
        ***REMOVED***);

            if (!IsValidEmail(request.Email))
            ***REMOVED***
                erorrs.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Email",
                    Message = "Email is invalid!"
        ***REMOVED***);
    ***REMOVED***
            else
            ***REMOVED***
                isExistUserByUsernameOrEmail = await _userManager.FindByEmailAsync(request.Email);

                if (isExistUserByUsernameOrEmail != null
                    && !isExistsUser.Id.Equals(isExistUserByUsernameOrEmail.Id))

                    erorrs.Add(new ErrorMessage
                    ***REMOVED***
                        Key = "Email",
                        Message = "Email is existed!"
            ***REMOVED***);
    ***REMOVED***

            if (request.Password != null
                && (String.IsNullOrEmpty(request.Password) || request.Password.Length < 6))
            ***REMOVED***
                erorrs.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Password",
                    Message = "Length of Password must be 6 characters or more!"
        ***REMOVED***);
    ***REMOVED***

            return erorrs;
***REMOVED***

        public async Task<ResponseData<UpdateUserRole>> UpdateUserRole(UpdateUserRole request)
        ***REMOVED***
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            ***REMOVED***
                return new ResponseData<UpdateUserRole>
                ***REMOVED***
                    Message = "User is not existed!",
                    StatusCode = 404
    ***REMOVED***
    ***REMOVED***

            if (!await _roleManager.RoleExistsAsync(request.Role))
            ***REMOVED***
                return new ResponseData<UpdateUserRole>
                ***REMOVED***
                    Message = "Role is not existed!",
                    StatusCode = 404
    ***REMOVED***
    ***REMOVED***

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            await _userManager.AddToRoleAsync(user, request.Role);

            return new ResponseData<UpdateUserRole>
            ***REMOVED***
                Message = "Ok!",
                StatusCode = 200,
                Data = new UpdateUserRole
                ***REMOVED***
                    Id = request.Id,
                    Role = request.Role
        ***REMOVED***
***REMOVED***
***REMOVED***

        private string GenerateNewJsonWebToken(List<Claim> claims)
        ***REMOVED***
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidateIssuer"],
                    audience: _configuration["JWT:ValidateAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;

***REMOVED***

        public async Task<ResponseData<UserResponse>> GetUserById(string Id)
        ***REMOVED***
            var isExistsUser = await _userManager.FindByIdAsync(Id);
            if (isExistsUser == null)
                return new ResponseData<UserResponse>
                ***REMOVED***
                    Message = "User not found!",
                    StatusCode = 404
    ***REMOVED***

            var userRoles = await _userManager.GetRolesAsync(isExistsUser);
            List<string> roles = new List<string>();

            foreach (var item in userRoles)
            ***REMOVED***
                roles.Add(item);
    ***REMOVED***

            return new ResponseData<UserResponse>
            ***REMOVED***
                Message = "Ok",
                StatusCode = 200,
                Data = new UserResponse
                ***REMOVED***
                    Id = isExistsUser.Id,
                    FullName = isExistsUser.FullName,
                    Email = isExistsUser.Email,
                    UserName = isExistsUser.UserName,
                    Password = "******",
                    Gender = isExistsUser.Gender,
                    Active = isExistsUser.Active,
                    Roles = roles
        ***REMOVED***
***REMOVED***
***REMOVED***

        public async Task<ResponseData<SearchList<UserResponse>>> GetUsers(SearchUsers request)
        ***REMOVED***
            request.ValidateInput();

            var userIdInRole = request.Role == null 
                ? null : (await _userManager.GetUsersInRoleAsync(request.Role)).Select(u => u.Id).ToList();


            var userQuery = _userManager.Users
                .Where(u =>
                    (request.KeyWord == null || u.FullName.ToLower().Contains(request.KeyWord)
                        || u.Email.ToLower().Contains(request.KeyWord)
                        || u.UserName.ToLower().Contains(request.KeyWord))

                    && (request.Gender == null || u.Gender == request.Gender.Equals(StaticString.MALE))

                    && (userIdInRole == null || userIdInRole.Contains(u.Id))
                );

            Expression<Func<User, object>> keySelector = OrderByHelper.GetKeySelector<User>(request.SortBy);
            if (request.OrderBy == StaticString.DESC)
            ***REMOVED***
                userQuery = userQuery.OrderByDescending(keySelector);
    ***REMOVED***
            else
            ***REMOVED***
                userQuery = userQuery.OrderBy(keySelector);
    ***REMOVED***

            var totalElements = await userQuery.CountAsync();

            var users = await userQuery
                .Skip((int)(request.PageIndex * request.PageSize))
                .Take((int)request.PageSize)
                .ToListAsync();


            List<UserResponse> results = new List<UserResponse>();

            foreach (var user in users)
            ***REMOVED***
                var roles = await _userManager.GetRolesAsync(user);
                results.Add(new UserResponse
                ***REMOVED***
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = "******",
                    Gender = user.Gender,
                    Active = user.Active,
                    Roles = roles.ToList(),
        ***REMOVED***);
    ***REMOVED***

            return new ResponseData<SearchList<UserResponse>>
            ***REMOVED***
                Message = "Ok",
                StatusCode = 200,
                Data = new SearchList<UserResponse>
                ***REMOVED***
                    Result = results,
                    SortBy = request.SortBy,
                    OrderBy = request.OrderBy,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalElements = totalElements
        ***REMOVED***
***REMOVED***
***REMOVED***

        public async Task<ResponseData<string>> ForgotPassword(ForgotPasswordRequest request)
        ***REMOVED***
            if (!IsValidEmail(request.Email))***REMOVED***
                return new ResponseData<string>
                ***REMOVED***
                    Message = "Email is invalid!",
                    StatusCode = 400
    ***REMOVED***
    ***REMOVED***
        
            var existUser = await _userManager.FindByEmailAsync(request.Email);
            if (existUser == null)
            ***REMOVED***
                return new ResponseData<string>
                ***REMOVED***
                    Message = "Email is not exist!",
                    StatusCode = 404
    ***REMOVED***
    ***REMOVED***

            string newPassword = GenerateRandomString(10);

            existUser.PasswordHash = new PasswordHasher<User>().HashPassword(existUser, newPassword);
            await _userManager.UpdateAsync(existUser);

            EmailServices emailServices = new EmailServices();
            await emailServices.SendAsync(new EmailMessage
            ***REMOVED***
                To = existUser.Email,
                Subject = "Forgot Password",
                Content = $"Your new Password is: ***REMOVED***newPassword***REMOVED***"
    ***REMOVED***);

            return new ResponseData<string>
            ***REMOVED***
                StatusCode = 200,
                Message = "Ok"
***REMOVED***
***REMOVED***


        private string GenerateRandomString(int length)
        ***REMOVED***
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@!#$%^&*";
            var stringBuilder = new StringBuilder();
            
            for (int i = 0; i < length; i++)
            ***REMOVED***
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
    ***REMOVED***
            return stringBuilder.ToString();
***REMOVED***
***REMOVED***
***REMOVED***
