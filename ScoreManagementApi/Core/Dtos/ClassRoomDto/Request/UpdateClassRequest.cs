***REMOVED***using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ClassRoomDto.Request
***REMOVED***
    public class UpdateClassRequest : CreateClassRequest
    ***REMOVED***
        public int? Id ***REMOVED*** get; set; ***REMOVED***
        public bool? Active ***REMOVED*** get; set; ***REMOVED***

        public new List<ErrorMessage> ValidateInput()
        ***REMOVED***
            var errors = base.ValidateInput();

            if (Id == null)
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Id",
                    Message = "Id is required!"
        ***REMOVED***);

            if(Active == null)
                Active = false;

            return errors;
***REMOVED***
***REMOVED***
***REMOVED***
