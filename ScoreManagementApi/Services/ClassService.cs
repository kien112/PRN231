using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.ClassRoomDto;
using ScoreManagementApi.Core.Dtos.ClassRoomDto.Request;
using ScoreManagementApi.Core.Dtos.ClassRoomDto.Response;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.SubjectDto;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Entities;
using ScoreManagementApi.Core.OtherObjects;
using ScoreManagementApi.Utils;
using System.Linq.Expressions;

namespace ScoreManagementApi.Services
{
    public class ClassService : IClassService
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ClassService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<ResponseData<ClassResponse>> CreateClass(UserTiny? user, CreateClassRequest request)
        {

            if(user == null)
            {
                return new ResponseData<ClassResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            var errors = request.ValidateInput();

            if(errors != null && errors.Count > 0)
            {
                return new ResponseData<ClassResponse>
                {
                    StatusCode = 400,
                    Errors = errors
                };
            }

            var isExistSubject = await _context.Subjects.FindAsync(request.SubjectId);
            
            if(isExistSubject == null)
            {
                return new ResponseData<ClassResponse>
                {
                    Message = "Subject not found!",
                    StatusCode = 400
                };
            }

            UserTiny teacher = new UserTiny();

            if(request.TeacherId != null)
            {
                var teachers = (await _userManager.GetUsersInRoleAsync(StaticUserRoles.TEACHER))
                    .Select(u => u.Id).ToList();

                var isExistTeacher = await _context.Users
                    .SingleOrDefaultAsync(x => x.Id.Equals(request.TeacherId)
                        && teachers.Contains(x.Id));

                if (isExistTeacher == null)
                {
                    return new ResponseData<ClassResponse>
                    {
                        Message = "Teacher not found!",
                        StatusCode = 400
                    };
                }

                teacher.Id = isExistTeacher.Id;
                teacher.FullName = isExistTeacher.FullName;
            }

            var classExistByName = await _context.ClassRooms.SingleOrDefaultAsync(c => c.Name.Equals(request.Name));

            if(classExistByName != null)
            {
                return new ResponseData<ClassResponse>
                {
                    Message = "Class Name is existed!",
                    StatusCode = 400
                };
            }

            ClassRoom classRoom = new ClassRoom
            {
                Name = request.Name,
                TeacherId = request.TeacherId,
                SubjectId = (int)request.SubjectId,
                Active = true,
                CreatedAt = DateTime.Now,
                CreatorId = user.Id
            };

            _context.ClassRooms.Add(classRoom);
            await _context.SaveChangesAsync();


            return new ResponseData<ClassResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new ClassResponse
                {
                    Id = classRoom.Id,
                    Name = classRoom.Name,
                    Active = classRoom.Active,
                    Teacher = teacher,
                    Subject = new SubjectTiny
                    {
                        Id = isExistSubject.Id,
                        Name = isExistSubject.Name,
                    },
                    CreatedAt = classRoom.CreatedAt,
                    Creator = user
                }
            };
        }

        public async Task<ResponseData<ClassResponse>> GetClassRoomById(UserTiny? user, int id)
        {
            if (user == null)
            {
                return new ResponseData<ClassResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            var isExistClass = await _context.ClassRooms
                .Include(x => x.ClassStudents)
                .Include(x => x.Teacher)
                .Include(x => x.Subject)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (isExistClass == null)
                return new ResponseData<ClassResponse>
                {
                    Message = "Class not found",
                    StatusCode = 404
                };


            List<ClassStudentResponse> students = await GetStudentsInClass(id);

            UserTiny teacher = new UserTiny();
            if(isExistClass.Teacher != null)
            {
                teacher.Id = isExistClass.Teacher.Id;
                teacher.FullName = isExistClass.Teacher.FullName;
            }

            return new ResponseData<ClassResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new ClassResponse
                {
                    Id = isExistClass.Id,
                    Name = isExistClass.Name,
                    Active = isExistClass.Active,
                    Subject = new SubjectTiny
                    {
                        Id = isExistClass.Subject.Id,
                        Name = isExistClass.Subject.Name
                    },
                    Teacher = teacher,
                    ClassStudents = students
                }
            };
        }

        private async Task<List<ClassStudentResponse>> GetStudentsInClass(int id)
        {
            var studentsInClass = await _context.ClassStudents
                .Include(x => x.Student)
                .Where(x => x.ClassRoomId == id)
                .ToListAsync();

            List<ClassStudentResponse> students = new List<ClassStudentResponse>();

            if (studentsInClass != null)
                foreach (var sc in studentsInClass)
                {
                    students.Add(new ClassStudentResponse
                    {
                        Id = sc.Id,
                        Student = new UserTiny
                        {
                            Id = sc.Student.Id,
                            FullName = sc.Student.FullName
                        },
                        JoinDate = sc.JoinDate
                    });
                }

            return students;
        }

        public async Task<ResponseData<SearchList<ClassResponse>>> SearchClassRoom(UserTiny? user, SearchClassRoom request)
        {
            if (user == null)
                return new ResponseData<SearchList<ClassResponse>>
                {
                    Message = "UnAuth",
                    StatusCode = 401,
                };

            request.ValidateInput();

            var classRoomQuery = _context.ClassRooms
                .Include(x => x.ClassStudents)
                .Include(x => x.Teacher)
                .Include(x => x.Subject)
                .Where(x => 
                    (request.Name == null || x.Name.ToLower().Contains(request.Name))
                    && (request.Active == null || x.Active == request.Active)
                    && (request.IsCurrentClass == false || (x.TeacherId != null && x.TeacherId.Equals(user.Id)))
                    && (request.TeacherId == null || (x.TeacherId != null && x.TeacherId.Equals(request.TeacherId)))
                    && (request.TeacherName == null || (x.Teacher != null && x.Teacher.FullName.ToLower().Contains(request.TeacherName)))
                    && (request.SubjectId == null || (x.Subject != null && x.Subject.Id == request.SubjectId))
                    && (request.SubjectName == null || (x.Subject != null && x.Subject.Name.ToLower().Contains(request.SubjectName)))
                );

            Expression<Func<ClassRoom, object>> keySelector = OrderByHelper.GetKeySelector<ClassRoom>(request.SortBy);
            if (request.OrderBy == StaticString.DESC)
            {
                classRoomQuery = classRoomQuery.OrderByDescending(keySelector);
            }
            else
            {
                classRoomQuery = classRoomQuery.OrderBy(keySelector);
            }

            var totalElements = await classRoomQuery.CountAsync();

            var classRooms = await classRoomQuery
                .Skip((int)(request.PageIndex * request.PageSize))
                .Take((int)request.PageSize)
                .ToListAsync();

            List<ClassResponse> results = new List<ClassResponse>();

            foreach (var item in classRooms)
            {
                UserTiny teacher = new UserTiny();
                if(item.Teacher != null)
                {
                    teacher.Id = item.Teacher.Id;
                    teacher.FullName = item.Teacher.FullName;
                }

                List<ClassStudentResponse> students = await GetStudentsInClass(item.Id);

                results.Add(new ClassResponse
                {
                    Teacher = teacher,
                    Id = item.Id,
                    Name = item.Name,
                    Active = item.Active,
                    CreatedAt = item.CreatedAt,
                    Creator = new UserTiny 
                    { 
                        Id = item.CreatorId,
                        FullName = item.Creator.FullName
                    },
                    ClassStudents = students,
                    Subject = new SubjectTiny
                    {
                        Id = item.SubjectId,
                        Name = item.Subject.Name
                    }
                });
            }


            return new ResponseData<SearchList<ClassResponse>>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new SearchList<ClassResponse>
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

        public async Task<ResponseData<ClassResponse>> UpdateClassRoom(UserTiny? user, UpdateClassRequest request)
        {
            if (user == null)
            {
                return new ResponseData<ClassResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            var errors = request.ValidateInput();

            if (errors != null && errors.Count > 0)
            {
                return new ResponseData<ClassResponse>
                {
                    StatusCode = 400,
                    Errors = errors
                };
            }

            var isExistClass = await _context.ClassRooms.FindAsync(request.Id);

            if (isExistClass == null)
            {
                return new ResponseData<ClassResponse>
                {
                    Message = "Class not found!",
                    StatusCode = 400
                };
            }

            var isExistSubject = await _context.Subjects.FindAsync(request.SubjectId);

            if (isExistSubject == null)
            {
                return new ResponseData<ClassResponse>
                {
                    Message = "Subject not found!",
                    StatusCode = 400
                };
            }

            UserTiny teacher = new UserTiny();

            if (request.TeacherId != null)
            {
                var teachers = (await _userManager.GetUsersInRoleAsync(StaticUserRoles.TEACHER))
                    .Select(u => u.Id).ToList();

                var isExistTeacher = await _context.Users
                    .SingleOrDefaultAsync(x => x.Id.Equals(request.TeacherId)
                        && teachers.Contains(x.Id));

                if (isExistTeacher == null)
                {
                    return new ResponseData<ClassResponse>
                    {
                        Message = "Teacher not found!",
                        StatusCode = 400
                    };
                }

                teacher.Id = isExistTeacher.Id;
                teacher.FullName = isExistTeacher.FullName;
            }

            var classExistByName = await _context.ClassRooms
                .SingleOrDefaultAsync(c => c.Name.Equals(request.Name) && c.Id != isExistClass.Id);

            if (classExistByName != null)
            {
                return new ResponseData<ClassResponse>
                {
                    Message = "Class Name is existed!",
                    StatusCode = 400
                };
            }

            if(request.SubjectId != request.SubjectId)
            {
                var studentsInClass = await _context.ClassStudents
                    .Where(x => x.ClassRoomId == isExistClass.Id)
                    .ToListAsync();

                _context.ClassStudents.RemoveRange(studentsInClass);
            }

            isExistClass.Name = request.Name;
            isExistClass.Active = (bool)request.Active;
            isExistClass.SubjectId = (int)request.SubjectId;
            isExistClass.TeacherId = request.TeacherId;

            _context.ClassRooms.Update(isExistClass);
            await _context.SaveChangesAsync();

            return new ResponseData<ClassResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new ClassResponse
                {
                    Id = isExistClass.Id,
                    Name = isExistClass.Name,
                    Active = isExistClass.Active,
                    Subject = new SubjectTiny
                    {
                        Id = isExistSubject.Id,
                        Name = isExistSubject.Name
                    },
                    Teacher = teacher
                }
            };
        }

        public async Task<ResponseData<ClassResponse>> CUDStudentsToClassRoom(UserTiny? user, CUDStudentsToClass request)
        {
            if (user == null)
                return new ResponseData<ClassResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401,
                };

            var isExistClass = await _context.ClassRooms
                .Include(x => x.ClassStudents)
                .SingleOrDefaultAsync(x => x.Id == request.ClassId);
            if(isExistClass == null)
            {
                return new ResponseData<ClassResponse>
                {
                    Message = "Class not found!",
                    StatusCode = 404,
                };
            }

            if(request.StudentIds == null || request.StudentIds.Count == 0)
            {
                if(isExistClass.ClassStudents != null)
                {
                    _context.ClassStudents.RemoveRange(isExistClass.ClassStudents);
                    await _context.SaveChangesAsync();
                }
            }
            else if(request.StudentIds.Count > 0)
            {
                var studentIds = (await _userManager.GetUsersInRoleAsync(StaticUserRoles.STUDENT))
                    .Select(x => x.Id)
                    .ToList();

                request.StudentIds = request.StudentIds
                    .Where(x => studentIds.Contains(x))
                    .ToList();

                var classStudentIds = isExistClass.ClassStudents
                    .Select(x => x.StudentId).ToList();

                //remove student
                var studentsToRemove = isExistClass.ClassStudents
                    .Where(x => !request.StudentIds.Contains(x.StudentId))
                    .ToList();
                _context.RemoveRange(studentsToRemove);

                //add student
                var newStudents = request.StudentIds
                    .Where(x => classStudentIds == null
                        || classStudentIds.Count == 0
                        || !classStudentIds.Contains(x)).ToList();
                foreach (var item in newStudents)
                {
                    _context.ClassStudents.Add(new ClassStudent
                    {
                        StudentId = item,
                        ClassRoomId = (int)request.ClassId,
                        JoinDate = DateTime.Now
                    });
                }
                await _context.SaveChangesAsync();
            }

            return new ResponseData<ClassResponse>
            {
                Message = "Ok",
                StatusCode = 200
            };
        }

    }
}
