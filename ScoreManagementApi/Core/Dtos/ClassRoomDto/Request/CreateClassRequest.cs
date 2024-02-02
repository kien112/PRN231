***REMOVED***using ScoreManagementApi.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ClassRoomDto.Request
***REMOVED***
    public class CreateClassRequest
    ***REMOVED***
        public string Name ***REMOVED*** get; set; ***REMOVED***
        public string? TeacherId ***REMOVED*** get; set; ***REMOVED***
        public int? SubjectId ***REMOVED*** get; set; ***REMOVED***

        public List<ErrorMessage> ValidateInput()
        ***REMOVED***
            var errors = new List<ErrorMessage>();

            if (String.IsNullOrEmpty(Name))
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Name",
                    Message = "Class Name is required!"
        ***REMOVED***);
            else if(Name.Length > 150)
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Name",
                    Message = "Length of Class Name must <= 150 characters!"
        ***REMOVED***);

            if(SubjectId == null)
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "SubjectId",
                    Message = "SubjectId is required!"
        ***REMOVED***);


            return errors;
***REMOVED***
***REMOVED***
***REMOVED***
