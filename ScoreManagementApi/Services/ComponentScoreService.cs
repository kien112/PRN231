using Microsoft.EntityFrameworkCore;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Request;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Response;
using ScoreManagementApi.Core.Dtos.SubjectDto;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Entities;
using ScoreManagementApi.Core.OtherObjects;
using ScoreManagementApi.Utils;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ScoreManagementApi.Services
{
    public class ComponentScoreService : IComponentScoreService
    {

        private readonly ApplicationDbContext _context;

        public ComponentScoreService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ResponseData<ComponentScoreResponse>> CreateComponentScore(UserTiny? user,
            CreateComponentScoreRequest request)
        {
            if(user == null)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            var errors = request.ValidateInput();
            if(errors != null && errors.Count > 0)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Errors = errors,
                    StatusCode = 400
                };
            }

            var isExistSubject = await _context.Subjects.FindAsync(request.SubjectId);
            if (isExistSubject == null)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "Subject Not Found!",
                    StatusCode = 404
                };
            }

            var componentScoreExistByName = await _context.ComponentScores
                .SingleOrDefaultAsync(c => c.Name.Equals(request.Name) && c.SubjectId == request.SubjectId);

            if(componentScoreExistByName != null)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "Name of Component Score is existed!",
                    StatusCode = 400
                };
            }

            var totalPercent = _context.ComponentScores
                .Where(c => c.SubjectId == request.SubjectId)
                .Select(x => x.Percent)
                .ToList().Sum();

            if(totalPercent == 100)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "Total percent of this subject is fully 100%!",
                    StatusCode = 400
                };
            }

            if(totalPercent + request.Percent > 100)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = $"Please enter percent <= {100 - totalPercent}!",
                    StatusCode = 400
                };
            }

            ComponentScore componentScore = new ComponentScore
            {
                Name = request.Name,
                Description = request.Description,
                Percent = (float) request.Percent,
                SubjectId = (int) request.SubjectId,
                Active = true
            };

            _context.ComponentScores.Add(componentScore);
            await _context.SaveChangesAsync();


            return new ResponseData<ComponentScoreResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new ComponentScoreResponse
                {
                    Id = componentScore.Id,
                    Name = componentScore.Name,
                    Description = componentScore.Description,
                    Percent = (float) componentScore.Percent,
                    Active = componentScore.Active,
                    Subject = new SubjectTiny
                    {
                        Id = isExistSubject.Id,
                        Name = isExistSubject.Name
                    }
                }
            };
        }

        public async Task<ResponseData<int?>> DeleteComponentScore(UserTiny? user, int id)
        {

            if (user == null)
            {
                return new ResponseData<int?>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            var isExistComponentScore = await _context.ComponentScores
                .SingleOrDefaultAsync(x => x.Id == id);
            
            if (isExistComponentScore == null)
            {
                return new ResponseData<int?>
                {
                    Message = "Component Score Not Found!",
                    StatusCode = 404
                };
            }

            var scoresByComponentScoreId = await _context.Scores
                .Where(x => x.ComponentScoreId == id)
                .ToListAsync();

            _context.Scores.RemoveRange(scoresByComponentScoreId);
            _context.ComponentScores.Remove(isExistComponentScore);
            await _context.SaveChangesAsync();


            return new ResponseData<int?>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = id
            };
        }

        public async Task<ResponseData<ComponentScoreResponse>> GetComponentScoreById(UserTiny? user, int id)
        {
            if (user == null)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            var isExistComponentScore = await _context.ComponentScores
                .Include(x => x.Subject)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (isExistComponentScore == null)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "Component Score Not Found!",
                    StatusCode = 404
                };
            }

            SubjectTiny subject = new SubjectTiny();
            if(isExistComponentScore.Subject != null)
            {
                subject.Id = isExistComponentScore.Subject.Id;
                subject.Name = isExistComponentScore.Subject.Name;
            }

            return new ResponseData<ComponentScoreResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new ComponentScoreResponse 
                {
                    Id = isExistComponentScore.Id, 
                    Name = isExistComponentScore.Name,  
                    Description = isExistComponentScore.Description,
                    Percent = isExistComponentScore.Percent,
                    Active = isExistComponentScore.Active,
                    Subject = subject
                }
            };
        }

        public async Task<ResponseData<SearchList<ComponentScoreResponse>>> SearchComponentScore(UserTiny? user,
            SearchComponentScores request)
        {
            if (user == null)
            {
                return new ResponseData<SearchList<ComponentScoreResponse>>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            request.ValidateInput();

            var componentScoreQuery = _context.ComponentScores
                .Include(c => c.Subject)
                .Where(c =>
                    (request.Name == null || c.Name.ToLower().Contains(request.Name))
                    && (request.Percent == null || c.Percent <= request.Percent)
                    && (request.SubjectId == null || c.SubjectId == request.SubjectId)
                    && (request.Active == request.Active)
                );

            Expression<Func<ComponentScore, object>> keySelector = OrderByHelper.GetKeySelector<ComponentScore>(request.SortBy);
            if (request.OrderBy == StaticString.DESC)
            {
                componentScoreQuery = componentScoreQuery.OrderByDescending(keySelector);
            }
            else
            {
                componentScoreQuery = componentScoreQuery.OrderBy(keySelector);
            }

            var totalElements = await componentScoreQuery.CountAsync();

            var componentScores = await componentScoreQuery
                .Skip((int)(request.PageIndex * request.PageSize))
                .Take((int)request.PageSize)
                .ToListAsync();

            List<ComponentScoreResponse> responses = new List<ComponentScoreResponse>();
            foreach (var c in componentScores)
            {
                SubjectTiny subject = new SubjectTiny();
                if(c.Subject != null)
                {
                    subject.Id = c.Subject.Id;
                    subject.Name = c.Subject.Name;
                }

                responses.Add(new ComponentScoreResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Percent = c.Percent,
                    Active = c.Active,
                    Subject = subject
                });
            }


            return new ResponseData<SearchList<ComponentScoreResponse>>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new SearchList<ComponentScoreResponse>
                {
                    Result = responses,
                    OrderBy = request.OrderBy,
                    SortBy = request.SortBy,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalElements = totalElements
                }
            };
        }

        public async Task<ResponseData<ComponentScoreResponse>> UpdateComponentScore(UserTiny? user,
            UpdateComponentScoreRequest request)
        {
            if (user == null)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            var errors = request.ValidateInput();
            if (errors != null && errors.Count > 0)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Errors = errors,
                    StatusCode = 400
                };
            }

            var isExistComponentScore = await _context.ComponentScores.FindAsync(request.Id);
            if (isExistComponentScore == null)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "Component Score Not Found!",
                    StatusCode = 404
                };
            }


            var isExistSubject = await _context.Subjects.FindAsync(request.SubjectId);
            if (isExistSubject == null)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "Subject Not Found!",
                    StatusCode = 404
                };
            }

            var componentScoreExistByName = await _context.ComponentScores
                .SingleOrDefaultAsync(c => 
                    c.Name.Equals(request.Name) 
                    && c.SubjectId == request.SubjectId
                    && c.Id != request.Id);

            if (componentScoreExistByName != null)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = "Name of Component Score is existed!",
                    StatusCode = 400
                };
            }

            var totalPercent = _context.ComponentScores
                .Where(c => 
                    c.SubjectId == request.SubjectId
                    && c.Id != request.Id   
                    && c.Active == true
                )
                .Select(x => x.Percent)
                .ToList().Sum();

            if (totalPercent + request.Percent > 100)
            {
                return new ResponseData<ComponentScoreResponse>
                {
                    Message = $"Please enter percent <= {100 - totalPercent}!",
                    StatusCode = 400
                };
            }

            isExistComponentScore.Name = request.Name;
            isExistComponentScore.SubjectId = (int)request.SubjectId;
            isExistComponentScore.Active = (bool)request.Active;
            isExistComponentScore.Description = request.Description;
            isExistComponentScore.Percent = (float)request.Percent;

            _context.ComponentScores.Update(isExistComponentScore);
            await _context.SaveChangesAsync();


            return new ResponseData<ComponentScoreResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new ComponentScoreResponse
                {
                    Id = isExistComponentScore.Id,
                    Name = isExistComponentScore.Name,
                    Description = isExistComponentScore.Description,
                    Percent = (float)isExistComponentScore.Percent,
                    Active = isExistComponentScore.Active,
                    Subject = new SubjectTiny
                    {
                        Id = isExistSubject.Id,
                        Name = isExistSubject.Name
                    }
                }
            };
        }



    }
}
