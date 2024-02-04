using Microsoft.AspNetCore.Identity;
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

namespace ScoreManagementApi.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        public async Task<ResponseData<UserResponse>> CreateUser(CreateUserRequest request)
        {
            List<ErrorMessage> erorrs = new List<ErrorMessage>();
           
            //check user exist by email or username
            var isExistsUser = await _userManager.FindByNameAsync(request.UserName);

            if (isExistsUser != null)
                erorrs.Add(new ErrorMessage
                {
                    Key = "UserName",
                    Message = "UserName is existed!"
                });


            if (!IsValidEmail(request.Email))
            {
                erorrs.Add(new ErrorMessage
                {
                    Key = "Email",
                    Message = "Email is invalid!"
                });
            }
            else
            {
                isExistsUser = await _userManager.FindByEmailAsync(request.Email);

                if (isExistsUser != null)
                    erorrs.Add(new ErrorMessage
                    {
                        Key = "Email",
                        Message = "Email is existed!"
                    });
            }


            if (erorrs != null && erorrs.Count > 0)
                return new ResponseData<UserResponse>
                {
                    StatusCode = 400,
                    Erorrs = erorrs
                };

            //create user and add role
            User newUser = new User()
            {
                Email = request.Email,
                UserName = request.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                Active = true,
                FullName = request.FullName,
                Gender = request.Gender,
            };

            var createUserResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Because: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new ResponseData<UserResponse>
                {
                    Message = errorString,
                    StatusCode = 400
                };
            }

            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.STUDENT);

            EmailServices emailServices = new EmailServices(_configuration);
            await emailServices.SendAsync(new EmailMessage
            {
                To = newUser.Email,
                Subject = "New Account",
                Content = "Your New Account in Score Management System is:\n" +
                $"Email: {newUser.Email}\n" +
                $"Username: {newUser.UserName}\n" +
                $"Password: {request.Password}"
            });

            return new ResponseData<UserResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new UserResponse
                {
                    Id = newUser.Id,
                    FullName = newUser.FullName,
                    Email = newUser.Email,
                    UserName = newUser.UserName,
                    Password = "******",
                    Gender = newUser.Gender,
                    Active = newUser.Active,
                    Roles = new List<string>
                    {
                        StaticUserRoles.STUDENT
                    }
                }
            };
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z.]{2,10}$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public async Task<ResponseData<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserNameOrEmail)
                ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);

            if (user is null)
                return new ResponseData<LoginResponse>
                {
                    Message = "Invalid Credentials",
                    StatusCode = 400
                };

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordCorrect)
                return new ResponseData<LoginResponse>
                {
                    Message = "Invalid Credentials",
                    StatusCode = 400
                };

            if (!user.Active)
                return new ResponseData<LoginResponse>
                {
                    Message = "Your Account is InActive",
                    StatusCode = 400
                };

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
            };

            foreach (var item in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return new ResponseData<LoginResponse>
            {
                Message = "Ok",
                Data = new LoginResponse
                {
                    FullName = user.FullName,
                    Token = token
                },
                StatusCode = 200
            };
        }

        public async Task<ResponseData<UserResponse>> UpdateUser(UpdateUserRequest request)
        {
            var isExistsUser = await _userManager.FindByIdAsync(request.Id);

            if (isExistsUser == null)
            {
                return new ResponseData<UserResponse>
                {
                    Message = "User not found!",
                    StatusCode = 404
                };
            }

            List<ErrorMessage> erorrs = await ValidateUpdateUser(request, isExistsUser);

            if (erorrs != null && erorrs.Count > 0)
            {
                return new ResponseData<UserResponse>
                {
                    StatusCode = 400,
                    Erorrs = erorrs
                };
            }

            isExistsUser.Email = request.Email;
            isExistsUser.UserName = request.UserName;
            isExistsUser.Active = request.Active;
            isExistsUser.FullName = request.FullName;
            isExistsUser.Gender = request.Gender;

            if (request.Password != null)
            {
                isExistsUser.PasswordHash = new PasswordHasher<User>().HashPassword(isExistsUser, request.Password);
            }

            var updateUserResult = await _userManager.UpdateAsync(isExistsUser);

            if (!updateUserResult.Succeeded)
            {
                var errorString = "User Update Failed Because: ";
                foreach (var error in updateUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new ResponseData<UserResponse>
                {
                    Message = errorString,
                    StatusCode = 400
                };
            }

            return new ResponseData<UserResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new UserResponse
                {
                    Id = isExistsUser.Id,
                    FullName = isExistsUser.FullName,
                    Email = isExistsUser.Email,
                    UserName = isExistsUser.UserName,
                    Password = "******",
                    Gender = isExistsUser.Gender,
                    Active = isExistsUser.Active
                }
            };
        }


        private async Task<List<ErrorMessage>> ValidateUpdateUser(UpdateUserRequest request, User isExistsUser)
        {
            List<ErrorMessage> erorrs = new List<ErrorMessage>();
            var isExistUserByUsernameOrEmail = await _userManager.FindByNameAsync(request.UserName);
            if (isExistUserByUsernameOrEmail != null
                && !isExistsUser.Id.Equals(isExistUserByUsernameOrEmail.Id))

                erorrs.Add(new ErrorMessage
                {
                    Key = "UserName",
                    Message = "UserName is existed!"
                });

            if (!IsValidEmail(request.Email))
            {
                erorrs.Add(new ErrorMessage
                {
                    Key = "Email",
                    Message = "Email is invalid!"
                });
            }
            else
            {
                isExistUserByUsernameOrEmail = await _userManager.FindByEmailAsync(request.Email);

                if (isExistUserByUsernameOrEmail != null
                    && !isExistsUser.Id.Equals(isExistUserByUsernameOrEmail.Id))

                    erorrs.Add(new ErrorMessage
                    {
                        Key = "Email",
                        Message = "Email is existed!"
                    });
            }

            if (request.Password != null
                && (String.IsNullOrEmpty(request.Password) || request.Password.Length < 6))
            {
                erorrs.Add(new ErrorMessage
                {
                    Key = "Password",
                    Message = "Length of Password must be 6 characters or more!"
                });
            }

            return erorrs;
        }

        public async Task<ResponseData<UpdateUserRole>> UpdateUserRole(UpdateUserRole request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                return new ResponseData<UpdateUserRole>
                {
                    Message = "User is not existed!",
                    StatusCode = 404
                };
            }

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                return new ResponseData<UpdateUserRole>
                {
                    Message = "Role is not existed!",
                    StatusCode = 404
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            await _userManager.AddToRoleAsync(user, request.Role);

            return new ResponseData<UpdateUserRole>
            {
                Message = "Ok!",
                StatusCode = 200,
                Data = new UpdateUserRole
                {
                    Id = request.Id,
                    Role = request.Role
                }
            };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
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

        }

        public async Task<ResponseData<UserResponse>> GetUserById(string Id)
        {
            var isExistsUser = await _userManager.FindByIdAsync(Id);
            if (isExistsUser == null)
                return new ResponseData<UserResponse>
                {
                    Message = "User not found!",
                    StatusCode = 404
                };

            var userRoles = await _userManager.GetRolesAsync(isExistsUser);
            List<string> roles = new List<string>();

            foreach (var item in userRoles)
            {
                roles.Add(item);
            }

            return new ResponseData<UserResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new UserResponse
                {
                    Id = isExistsUser.Id,
                    FullName = isExistsUser.FullName,
                    Email = isExistsUser.Email,
                    UserName = isExistsUser.UserName,
                    Password = "******",
                    Gender = isExistsUser.Gender,
                    Active = isExistsUser.Active,
                    Roles = roles
                }
            };
        }

        public async Task<ResponseData<SearchList<UserResponse>>> GetUsers(SearchUsers request)
        {
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
            {
                userQuery = userQuery.OrderByDescending(keySelector);
            }
            else
            {
                userQuery = userQuery.OrderBy(keySelector);
            }

            var totalElements = await userQuery.CountAsync();

            var users = await userQuery
                .Skip((int)(request.PageIndex * request.PageSize))
                .Take((int)request.PageSize)
                .ToListAsync();


            List<UserResponse> results = new List<UserResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                results.Add(new UserResponse
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = "******",
                    Gender = user.Gender,
                    Active = user.Active,
                    Roles = roles.ToList(),
                });
            }

            return new ResponseData<SearchList<UserResponse>>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new SearchList<UserResponse>
                {
                    Result = results,
                    SortBy = request.SortBy,
                    OrderBy = request.OrderBy,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalElements = totalElements
                }
            };
        }

        public async Task<ResponseData<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            if (!IsValidEmail(request.Email)){
                return new ResponseData<string>
                {
                    Message = "Email is invalid!",
                    StatusCode = 400
                };
            }
        
            var existUser = await _userManager.FindByEmailAsync(request.Email);
            if (existUser == null)
            {
                return new ResponseData<string>
                {
                    Message = "Email is not exist!",
                    StatusCode = 404
                };
            }

            string newPassword = GenerateRandomString(10);

            existUser.PasswordHash = new PasswordHasher<User>().HashPassword(existUser, newPassword);
            await _userManager.UpdateAsync(existUser);

            EmailServices emailServices = new EmailServices(_configuration);
            await emailServices.SendAsync(new EmailMessage
            {
                To = existUser.Email,
                Subject = "Forgot Password",
                Content = $"Your new Password is: {newPassword}"
            });

            return new ResponseData<string>
            {
                StatusCode = 200,
                Message = "Ok"
            };
        }


        private string GenerateRandomString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@!#$%^&*";
            var stringBuilder = new StringBuilder();
            
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }
            return stringBuilder.ToString();
        }
    }
}
