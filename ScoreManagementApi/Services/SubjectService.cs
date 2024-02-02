***REMOVED***using Microsoft.EntityFrameworkCore;
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

***REMOVED***
***REMOVED***
    public class SubjectService : ISubjectService
    ***REMOVED***

        private readonly ApplicationDbContext _context;

        public SubjectService(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
***REMOVED***

        public async Task<ResponseData<SubjectResponse>> CreateSubject(UserTiny? user, CreateSubjectRequest request)
        ***REMOVED***
            if (user == null)
                return new ResponseData<SubjectResponse>
                ***REMOVED***
                    Message = "UnAuth",
                    StatusCode = 401
    ***REMOVED***

            List<ErrorMessage> errors = request.ValidateInput();

            if(errors != null && errors.Count > 0)
            ***REMOVED***
                return new ResponseData<SubjectResponse>
                ***REMOVED***
                    Erorrs = errors,
                    StatusCode = 400
    ***REMOVED***
    ***REMOVED***

            var subjectExistByName = await _context.Subjects.SingleOrDefaultAsync(s => s.Name.Equals(request.Name));
            if (subjectExistByName != null)
            ***REMOVED***
                return new ResponseData<SubjectResponse>
                ***REMOVED***
                    Message = "Subject Name is existed!",
                    StatusCode = 400
    ***REMOVED***
    ***REMOVED***

            Subject subject = new Subject
            ***REMOVED***
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                CreatorId = user.Id,
                Active = true,
***REMOVED***

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return new ResponseData<SubjectResponse>
            ***REMOVED***
                Data = new SubjectResponse
                ***REMOVED***
                    Id = subject.Id,
                    Name = subject.Name,
                    Description = subject.Description,
                    CreatedAt = subject.CreatedAt,
                    Creator = user,
                    Active = subject.Active,
                    ComponentScores = null
        ***REMOVED***,
                Message = "Ok",
                StatusCode = 200
***REMOVED***

***REMOVED***

        public async Task<ResponseData<SubjectResponse>> GetSubjectById(UserTiny? user, int? id)
        ***REMOVED***
            if (user == null)
                return new ResponseData<SubjectResponse>
                ***REMOVED***
                    Message = "UnAuth",
                    StatusCode = 401
    ***REMOVED***

            var existSubject = await _context.Subjects
                .Include(x => x.ComponentScores)
                .Include(x => x.Creator)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(existSubject == null)
            ***REMOVED***
                return new ResponseData<SubjectResponse>
                ***REMOVED***
                    Message = "Subject Not Found!",
                    StatusCode = 404
    ***REMOVED***
    ***REMOVED***

            List<ComponentScoreResponse> componentScoreResponses = new List<ComponentScoreResponse>();
            if(existSubject.ComponentScores != null)
            ***REMOVED***
                foreach (var item in existSubject.ComponentScores)
                ***REMOVED***
                    componentScoreResponses.Add(new ComponentScoreResponse
                    ***REMOVED***
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Percent = item.Percent,
                        Active = item.Active,
                        Subject = null
            ***REMOVED***);
        ***REMOVED***
    ***REMOVED***

            UserTiny? creator = new UserTiny();
            if(existSubject.Creator != null)
            ***REMOVED***
                creator.Id = existSubject.Creator.Id;
                creator.FullName = existSubject.Creator.FullName;
    ***REMOVED***

            return new ResponseData<SubjectResponse>
            ***REMOVED***
                Message = "Ok",
                StatusCode = 200,
                Data = new SubjectResponse
                ***REMOVED***
                    Id = existSubject.Id,
                    Name = existSubject.Name,
                    Description = existSubject.Description,
                    Active = existSubject.Active,
                    ComponentScores = componentScoreResponses,
                    CreatedAt = existSubject.CreatedAt,
                    Creator = creator
        ***REMOVED***
***REMOVED***
***REMOVED***

        public async Task<ResponseData<SearchList<SubjectResponse>>> SearchSubjects(UserTiny? user, SearchSubject request)
        ***REMOVED***
            request.ValidateInput();

            if (user == null)
                return new ResponseData<SearchList<SubjectResponse>>
                ***REMOVED***
                    Message = "UnAuth",
                    StatusCode = 401
    ***REMOVED***

            List<int> subjectIds = new List<int>();
            //get current subject of current user
            if (request.IsCurrentSubject == true)
            ***REMOVED***
                var classRoomIds = await _context.ClassStudents
                    .Include(c => c.ClassRoom)
                    .Where(c => c.StudentId.Equals(user.Id) && c.ClassRoom.Active == true)
                    .Select(c => c.ClassRoomId)
                    .ToListAsync();
                
                if(classRoomIds != null && classRoomIds.Count > 0)
                ***REMOVED***
                    subjectIds = await _context.ClassRooms
                        .Where(c => classRoomIds.Contains(c.Id))
                        .Select(c => c.SubjectId)
                        .ToListAsync();
        ***REMOVED***
    ***REMOVED***

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
            ***REMOVED***
                subjectsQuery = subjectsQuery.OrderByDescending(keySelector);
    ***REMOVED***
            else
            ***REMOVED***
                subjectsQuery = subjectsQuery.OrderBy(keySelector);
    ***REMOVED***

            var totalElements = await subjectsQuery.CountAsync();

            var subjects = await subjectsQuery
                .Skip((int)(request.PageIndex * request.PageSize))
                .Take((int)request.PageSize)
                .ToListAsync();

            List<SubjectResponse> results = new List<SubjectResponse>();
            foreach (var item in subjects)
            ***REMOVED***
                UserTiny creator = new UserTiny();
                if(item.Creator != null)
                    creator = new UserTiny
                    ***REMOVED***
                        Id = item.Creator.Id,
                        FullName = item.Creator.FullName,
        ***REMOVED***

                List<ComponentScoreResponse> componentScoreResponses = new List<ComponentScoreResponse>();
                if(item.ComponentScores != null)
                ***REMOVED***
                    foreach (var c in item.ComponentScores)
                    ***REMOVED***
                        componentScoreResponses.Add(new ComponentScoreResponse
                        ***REMOVED***
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            Percent = c.Percent,
                            Active = c.Active,
                            Subject = null
                ***REMOVED***);
            ***REMOVED***
        ***REMOVED***

                results.Add(new SubjectResponse
                ***REMOVED***
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    CreatedAt = item.CreatedAt,
                    Creator = creator,
                    ComponentScores = componentScoreResponses
        ***REMOVED***);
    ***REMOVED***


            return new ResponseData<SearchList<SubjectResponse>>
            ***REMOVED***
                Message = "Ok",
                StatusCode = 200,
                Data = new SearchList<SubjectResponse>
                ***REMOVED***
                    TotalElements = totalElements,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                    OrderBy = request.OrderBy,
                    Result = results
        ***REMOVED***
***REMOVED***
***REMOVED***

        public async Task<ResponseData<SubjectResponse>> UpdateSubject(UserTiny? user, UpdateSubjectRequest request)
        ***REMOVED***
            if (user == null)
                return new ResponseData<SubjectResponse>
                ***REMOVED***
                    Message = "UnAuth",
                    StatusCode = 401
    ***REMOVED***

            var existSubject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (existSubject == null)
                return new ResponseData<SubjectResponse>
                ***REMOVED***
                    Message = "Subject Not Found!",
                    StatusCode = 404
    ***REMOVED***

            List<ErrorMessage> errors = request.ValidateInput();

            if (errors != null && errors.Count > 0)
            ***REMOVED***
                return new ResponseData<SubjectResponse>
                ***REMOVED***
                    Erorrs = errors,
                    StatusCode = 400
    ***REMOVED***
    ***REMOVED***

            var subjectExistByName = await _context.Subjects.SingleOrDefaultAsync(s => s.Name.Equals(request.Name));
            if (subjectExistByName != null && existSubject.Id != subjectExistByName.Id)
            ***REMOVED***
                return new ResponseData<SubjectResponse>
                ***REMOVED***
                    Message = "Subject Name is existed!",
                    StatusCode = 400
    ***REMOVED***
    ***REMOVED***

            existSubject.Name = request.Name;
            existSubject.Description = request.Description;
            existSubject.Active = (bool)request.Active;


            _context.Subjects.Update(existSubject);
            await _context.SaveChangesAsync();

            return new ResponseData<SubjectResponse>
            ***REMOVED***
                Data = new SubjectResponse
                ***REMOVED***
                    Id = existSubject.Id,
                    Name = existSubject.Name,
                    Description = existSubject.Description,
                    CreatedAt = existSubject.CreatedAt,
                    Creator = user,
                    Active = existSubject.Active,
                    ComponentScores = null
        ***REMOVED***,
                Message = "Ok",
                StatusCode = 200
***REMOVED***

***REMOVED***
***REMOVED***
***REMOVED***
