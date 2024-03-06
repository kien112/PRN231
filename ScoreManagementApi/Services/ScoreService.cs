﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto;
using ScoreManagementApi.Core.Dtos.ScoreDto.Request;
using ScoreManagementApi.Core.Dtos.ScoreDto.Response;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Entities;
using ScoreManagementApi.Core.OtherObjects;

namespace ScoreManagementApi.Services
{
    public class ScoreService : IScoreService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ScoreService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ResponseData<ScoreResponse>> CUDScore(UserTiny? user, List<CUDScoreRequest> request)
        {
            if (user == null)
            {
                return new ResponseData<ScoreResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            var validScores = new List<CUDScoreRequest>();
            var errors = new List<ErrorMessage>();
            foreach (var r in request)
            {
                var error = r.ValidateInput();
                if (error != null)
                {
                    errors.Add(error);
                }
                else
                {
                    var existUser = await _context.Users.FindAsync(r.StudentId);
                    bool isValidData = true;
                    
                    if (existUser == null)
                    {
                        errors.Add(new ErrorMessage
                        {
                            Key = $"StudentId: {r.StudentId}",
                            Message = "Student Id is not existed!"
                        });
                        isValidData = false;
                    }
                    else
                    {
                        var isRoleStudent = await _userManager.GetRolesAsync(existUser);
                        if(isRoleStudent == null || isRoleStudent.Count == 0
                            || !isRoleStudent.Contains(StaticUserRoles.STUDENT))
                        {
                            errors.Add(new ErrorMessage
                            {
                                Key = $"UserId: {r.StudentId}",
                                Message = "This User Id is not a student!"
                            });
                            isValidData = false;
                        }
                    }

                    var existComponentScore = await _context.ComponentScores.FindAsync(r.ComponentScoreId);
                    if (existComponentScore == null)
                    {
                        errors.Add(new ErrorMessage
                        {
                            Key = $"ComponentScoreId: {r.ComponentScoreId}",
                            Message = "Component Score Id is not existed!"
                        });
                        isValidData = false;
                    }

                    if(isValidData)
                        validScores.Add(r);
                }
            }

            if(validScores.Count > 0)
            {
                foreach (var sc in validScores)
                {
                    var existScore = await _context.Scores
                            .SingleOrDefaultAsync(x => x.StudentId == sc.StudentId
                                && x.ComponentScoreId == sc.ComponentScoreId);
                    if (existScore == null)
                    {
                        _context.Scores.Add(new Score
                        {
                            StudentId = sc.StudentId,
                            ComponentScoreId = (int)sc.ComponentScoreId,
                            Mark = sc.Mark,
                        });
                    }
                    else
                    {
                        existScore.Mark = sc.Mark;
                        _context.Scores.Update(existScore);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return new ResponseData<ScoreResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Errors = errors
            };
        }

        public async Task<ResponseData<ExportScoreResponse>> ExportScore(UserTiny? user, int? classId)
        {
            if(user == null)
            {
                return new ResponseData<ExportScoreResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401,
                };
            }

            if(classId == null)
            {
                return new ResponseData<ExportScoreResponse>
                {
                    Message = "ClassId is required!",
                    StatusCode = 400
                };
            }

            var existClass = await _context.ClassRooms
                .Include(x => x.ClassStudents)
                .SingleOrDefaultAsync(x => x.Id == classId);
            if (existClass == null)
            {
                return new ResponseData<ExportScoreResponse>
                {
                    Message = "Class not found!",
                    StatusCode = 404
                };
            }
            else if ((existClass.TeacherId == null || !existClass.TeacherId.Equals(user.Id))
                && user.Role.Equals(StaticUserRoles.TEACHER))
            {
                return new ResponseData<ExportScoreResponse>
                {
                    Message = "You are not allow to access to score of this class!",
                    StatusCode = 403
                };
            }
            else if(existClass.Active == false)
            {
                return new ResponseData<ExportScoreResponse>
                {
                    Message = "Class is inactive!",
                    StatusCode = 400
                };
            }


            var components = await _context.ComponentScores
                .Where(x => x.SubjectId == existClass.SubjectId)
                .ToListAsync();

            var students = existClass.ClassStudents
                .Select(x => x.StudentId)
                .ToList();


            return new ResponseData<ExportScoreResponse>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new ExportScoreResponse
                {
                    Bytes = CreateExcelFile(components, students),
                    FileName = $"Scores_{existClass.Name}.xlsx"
                }
            };

        }

        public async Task<ResponseData<ScoreResponse>> ImportScore(UserTiny? user, ImportScoresRequest request)
        {
            if(user == null)
            {
                return new ResponseData<ScoreResponse>
                {
                    Message = "UnAuth",
                    StatusCode = 401,
                };
            }

            var errors = request.ValidateInput();

            if(errors != null && errors.Count > 0)
            {
                return new ResponseData<ScoreResponse>
                {
                    Errors = errors,
                    StatusCode = 400
                };
            }

            var existClass = await _context.ClassRooms
                .Include(x => x.Subject)
                .Include(x => x.ClassStudents)
                .SingleOrDefaultAsync(x => x.Id == request.ClassId);
            if(existClass == null)
            {
                return new ResponseData<ScoreResponse>
                {
                    Message = "Class not found!",
                    StatusCode = 404,
                };
            }
            else if ((existClass.TeacherId == null || !existClass.TeacherId.Equals(user.Id))
                && user.Role.Equals(StaticUserRoles.TEACHER))
            {
                return new ResponseData<ScoreResponse>
                {
                    Message = "You are not allow to access to score of this class!",
                    StatusCode = 403
                };
            }
            else if(existClass.Active == false)
            {
                return new ResponseData<ScoreResponse>
                {
                    Message = "Class is inactive!",
                    StatusCode = 404,
                };
            }

            using (var stream = new MemoryStream())
            {
                await request.ExcelFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets[0];

                    var rowCount = workSheet.Dimension.Rows;
                    var colCount = workSheet.Dimension.Columns;

                    var components = _context.ComponentScores
                        .Where(c => c.SubjectId == existClass.SubjectId
                            && c.Active == true
                        )
                        .ToList();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string? id = workSheet.Cells[row, 1].Text;

                        if (id != null)
                        {
                            Dictionary<string, float> dic = new Dictionary<string, float>();
                            for (int col = 3; col <= colCount; col++)
                            {
                                string header = workSheet.Cells[1, col].Text;
                                float cellValue = -1;

                                try
                                {
                                    cellValue = float.Parse(workSheet.Cells[row, col].Text);
                                    if (cellValue < 0 || cellValue > 10)
                                    {
                                        cellValue = -1;
                                    }
                                }
                                catch (Exception ex) { }

                                dic.Add(header, cellValue);
                            }
                            foreach (var item in dic)
                            {
                                var component = components.FirstOrDefault(c => c.Name.Equals(item.Key));
                                var existStudent = existClass.ClassStudents
                                    .SingleOrDefault(x => x.StudentId == id);

                                if (item.Value != -1 && component != null && existStudent != null)
                                {
                                    Score? score = _context.Scores
                                        .SingleOrDefault(s => s.StudentId == existStudent.StudentId
                                            && s.ComponentScoreId == component.Id
                                        );
                                    if (score != null)
                                    {
                                        if (score.Mark != item.Value)
                                        {
                                            score.Mark = item.Value;
                                            _context.Update(score);
                                        }
                                    }
                                    else
                                    {
                                        _context.Scores.Add(
                                            new Score
                                            {
                                                Mark = item.Value,
                                                ComponentScoreId = component.Id,
                                                StudentId = existStudent.StudentId
                                            }
                                        );
                                    }
                                }
                            }
                            await _context.SaveChangesAsync();
                        }

                    }
                }
            }

            return new ResponseData<ScoreResponse>
            {
                Message = "Ok",
                StatusCode = 200
            };
        }

        public async Task<ResponseData<SearchList<ScoreResponse>>> SearchScore(UserTiny? user, SearchScoreRequest request)
        {
            if(user == null)
            {
                return new ResponseData<SearchList<ScoreResponse>>
                {
                    Message = "UnAuth",
                    StatusCode = 401
                };
            }

            request.ValidateInput();

            var isExistClass = await _context.ClassRooms.FindAsync(request.ClassId);
            if(isExistClass == null)
            {
                return new ResponseData<SearchList<ScoreResponse>>
                {
                    Message = "Class Not Found!",
                    StatusCode = 404
                };
            }
            else if (isExistClass.Active == false)
            {
                return new ResponseData<SearchList<ScoreResponse>>
                {
                    Message = "Class Is InActive!",
                    StatusCode = 400
                };
            }
            else if ((isExistClass.TeacherId == null || !isExistClass.TeacherId.Equals(user.Id))
                && user.Role.Equals(StaticUserRoles.TEACHER))
            {
                return new ResponseData<SearchList<ScoreResponse>>
                {
                    Message = "You are not allow to access to score of this class!",
                    StatusCode = 403
                };
            }

            var isExistSubject = await _context.Subjects.FindAsync(isExistClass.SubjectId);
            if(isExistSubject == null)
            {
                return new ResponseData<SearchList<ScoreResponse>>
                {
                    Message = "Subject Not Found!",
                    StatusCode = 404
                };
            }
            else if(isExistSubject.Active == false)
            {
                return new ResponseData<SearchList<ScoreResponse>>
                {
                    Message = "Subject Is InActive!",
                    StatusCode = 400
                };
            }

            var studentIdsInClass = await _context.ClassStudents
                .Include(x => x.Student)
                .Where(x => 
                    x.ClassRoomId == request.ClassId
                    && (request.StudentName == null || x.Student.FullName.ToLower().Contains(request.StudentName))
                )
                .Select(x => new UserTiny
                {
                    Id = x.Student.Id,
                    FullName = x.Student.FullName
                })
                .ToListAsync();

            var componentScores = await _context.ComponentScores
                .Where(x => 
                    x.SubjectId == isExistSubject.Id
                    && (request.ComponentScoreId == null || x.Id == request.ComponentScoreId)    
                )
                .Select(x => new ComponentScoreTiny 
                { 
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            List<ScoreResponse> scores = new List<ScoreResponse>();
            
            foreach (var student in studentIdsInClass)
            {
                foreach (var cs in componentScores)
                {
                    var existScore = _context.Scores
                        .SingleOrDefault(x => 
                            x.StudentId.Equals(student.Id) 
                            && x.ComponentScoreId == cs.Id);

                    if(existScore != null)
                    {
                        scores.Add(new ScoreResponse
                        {
                            Id = existScore.Id,
                            Mark = existScore.Mark,
                            Student = student,
                            ComponentScore = cs
                        });
                    }
                    else
                    {
                        scores.Add(new ScoreResponse
                        {
                            Student = student,
                            ComponentScore = cs
                        });
                    }
                }
            }


            return new ResponseData<SearchList<ScoreResponse>>
            {
                Message = "Ok",
                StatusCode = 200,
                Data = new SearchList<ScoreResponse>
                {
                    Result = scores
                }
            };
        }

        private byte[] CreateExcelFile(List<ComponentScore> components, List<string> students)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Scores");

                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "Name";
                int columnIndex = 3;
                foreach (var item in components)
                {
                    string columnName = GetColumnName(columnIndex++);
                    worksheet.Cells[$"{columnName}1"].Value = item.Name;
                }
                worksheet.Cells[$"{GetColumnName(columnIndex++)}1"].Value = "Total";

                for (int i = 0; i < students.Count; i++)
                {
                    int col = 1;
                    float? total = 0;

                    var student = _context.Users.Find(students[i]);
                    worksheet.Cells[i + 2, col++].Value = student.Id;
                    worksheet.Cells[i + 2, col++].Value = student.FullName;
                    
                    for (int j = 0; j < components.Count; j++)
                    {
                        var score = _context.Scores
                            .SingleOrDefault(s => s.ComponentScoreId == components[j].Id
                                && s.StudentId == student.Id
                            );
                        if (score != null)
                        {
                            worksheet.Cells[i + 2, col].Value = score.Mark == null ? DBNull.Value : score.Mark;
                            total += score.Mark * components[j].Percent / 100;
                        }
                        col++;
                    }
                    worksheet.Cells[i + 2, col++].Value = total == null ? DBNull.Value : total;
                }

                using (var memoryStream = new MemoryStream())
                {
                    package.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        private string GetColumnName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = string.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }
    }
}