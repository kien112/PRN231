using Microsoft.EntityFrameworkCore;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Response;
using ScoreManagementApi.Core.Dtos.SubjectDto;
using ScoreManagementApi.Core.Dtos.SubjectDto.Request;
using ScoreManagementApi.Core.Dtos.SubjectDto.Response;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Dtos.User.Response;
using ScoreManagementApi.Core.Entities;
using ScoreManagementApi.Core.OtherObjects;
using ScoreManagementApi.Utils;
using System.Linq.Expressions;

namespace ScoreManagementApi.Services
{
    public class SubjectService : ISubjectService
    {

        private readonly ApplicationDbContext _context;

        public SubjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<SubjectResponse>> CreateSubject(UserTiny? user, CreateSubjectRequest request)
        {
            if (user == null)
                return new ResponseData<SubjectResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };

            List<ErrorMessage> errors = request.ValidateInput();

            if(errors != null && errors.Count > 0)
            {
                return new ResponseData<SubjectResponse>
                {
                    Errors = errors,
                    StatusCode = 400
                };
            }

            var subjectExistByName = await _context.Subjects.SingleOrDefaultAsync(s => s.Name.Equals(request.Name));
            if (subjectExistByName != null)
            {
                return new ResponseData<SubjectResponse>
                {
                    Message = "Subject Name is existed!",
                    StatusCode = 400
                };
            }

            Subject subject = new Subject
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                CreatorId = user.Id,
                Active = true,
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return new ResponseData<SubjectResponse>
            {
                Data = new SubjectResponse
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    Description = subject.Description,
                    CreatedAt = subject.CreatedAt,
                    Creator = user,
                    Active = subject.Active,
                    ComponentScores = null
                },
                Message = "Ok",
                StatusCode = 200
            };

        }

        public async Task<ResponseData<SubjectResponse>> GetSubjectById(UserTiny? user, int? id)
        {
            if (user == null)
                return new ResponseData<SubjectResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };

            var existSubject = await _context.Subjects
                .Include(x => x.ComponentScores)
                .Include(x => x.Creator)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(existSubject == null)
            {
                return new ResponseData<SubjectResponse>
                {
                    Message = "Subject Not Found!",
                    StatusCode = 404
                };
            }

            List<ComponentScoreResponse> componentScoreResponses = new List<ComponentScoreResponse>();
            if(existSubject.ComponentScores != null)
            {
                foreach (var item in existSubject.ComponentScores)
                {
                    componentScoreResponses.Add(new ComponentScoreResponse
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Percent = item.Percent,
                        Active = item.Active,
                        Subject = null
                    });
                }
            }

            UserTiny? creator = new UserTiny();
            if(existSubject.Creator != null)
            {
                creator.Id = existSubject.Creator.Id;
                creator.FullName = existSubject.Creator.FullName;
            }

            return new ResponseData<SubjectResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new SubjectResponse
                {
                    Id = existSubject.Id,
                    Name = existSubject.Name,
                    Description = existSubject.Description,
                    Active = existSubject.Active,
                    ComponentScores = componentScoreResponses,
                    CreatedAt = existSubject.CreatedAt,
                    Creator = creator
                }
            };
        }

        public async Task<ResponseData<SearchList<SubjectResponse>>> SearchSubjects(UserTiny? user, SearchSubject request)
        {
            request.ValidateInput();

            if (user == null)
                return new ResponseData<SearchList<SubjectResponse>>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };

            List<int> subjectIds = new List<int>();
            //get current subject of current user
            if (request.IsCurrentSubject == true)
            {
                var classRoomIds = await _context.ClassStudents
                    .Include(c => c.ClassRoom)
                    .Where(c => c.StudentId.Equals(user.Id) && c.ClassRoom.Active == true)
                    .Select(c => c.ClassRoomId)
                    .ToListAsync();
                
                if(classRoomIds != null && classRoomIds.Count > 0)
                {
                    subjectIds = await _context.ClassRooms
                        .Where(c => classRoomIds.Contains(c.Id))
                        .Select(c => c.SubjectId)
                        .ToListAsync();
                }
            }

            var subjectsQuery = _context.Subjects
                .Include(s => s.ComponentScores)
                .Include(s => s.Creator)
                .Where(s =>
                    (request.Name == null || s.Name.ToLower().Contains(request.Name))
                    && (request.Active == null || s.Active == request.Active)
                    && (request.IsCurrentSubject == false || subjectIds.Contains(s.Id))
                );

            Expression<Func<Subject, object>> keySelector = OrderByHelper.GetKeySelector<Subject>(request.SortBy);
            if (request.OrderBy == StaticString.DESC)
            {
                subjectsQuery = subjectsQuery.OrderByDescending(keySelector);
            }
            else
            {
                subjectsQuery = subjectsQuery.OrderBy(keySelector);
            }

            var totalElements = await subjectsQuery.CountAsync();

            var subjects = await subjectsQuery
                .Skip((int)(request.PageIndex * request.PageSize))
                .Take((int)request.PageSize)
                .ToListAsync();

            List<SubjectResponse> results = new List<SubjectResponse>();
            foreach (var item in subjects)
            {
                UserTiny creator = new UserTiny();
                if(item.Creator != null)
                    creator = new UserTiny
                    {
                        Id = item.Creator.Id,
                        FullName = item.Creator.FullName,
                    };

                List<ComponentScoreResponse> componentScoreResponses = new List<ComponentScoreResponse>();
                if(item.ComponentScores != null)
                {
                    foreach (var c in item.ComponentScores)
                    {
                        componentScoreResponses.Add(new ComponentScoreResponse
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            Percent = c.Percent,
                            Active = c.Active,
                            Subject = null
                        });
                    }
                }

                results.Add(new SubjectResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Active = item.Active,
                    CreatedAt = item.CreatedAt,
                    Creator = creator,
                    ComponentScores = componentScoreResponses
                });
            }


            return new ResponseData<SearchList<SubjectResponse>>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new SearchList<SubjectResponse>
                {
                    TotalElements = totalElements,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                    OrderBy = request.OrderBy,
                    Result = results
                }
            };
        }

        public async Task<ResponseData<SubjectResponse>> UpdateSubject(UserTiny? user, UpdateSubjectRequest request)
        {
            if (user == null)
                return new ResponseData<SubjectResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };

            var existSubject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (existSubject == null)
                return new ResponseData<SubjectResponse>
                {
                    Message = "Subject Not Found!",
                    StatusCode = 404
                };

            List<ErrorMessage> errors = request.ValidateInput();

            if (errors != null && errors.Count > 0)
            {
                return new ResponseData<SubjectResponse>
                {
                    Errors = errors,
                    StatusCode = 400
                };
            }

            var subjectExistByName = await _context.Subjects.SingleOrDefaultAsync(s => s.Name.Equals(request.Name));
            if (subjectExistByName != null && existSubject.Id != subjectExistByName.Id)
            {
                return new ResponseData<SubjectResponse>
                {
                    Message = "Subject Name is existed!",
                    StatusCode = 400
                };
            }

            existSubject.Name = request.Name;
            existSubject.Description = request.Description;
            existSubject.Active = (bool)request.Active;


            _context.Subjects.Update(existSubject);
            await _context.SaveChangesAsync();

            return new ResponseData<SubjectResponse>
            {
                Data = new SubjectResponse
                {
                    Id = existSubject.Id,
                    Name = existSubject.Name,
                    Description = existSubject.Description,
                    CreatedAt = existSubject.CreatedAt,
                    Creator = user,
                    Active = existSubject.Active,
                    ComponentScores = null
                },
                Message = "Ok",
                StatusCode = 200
            };

        }
    }
}
