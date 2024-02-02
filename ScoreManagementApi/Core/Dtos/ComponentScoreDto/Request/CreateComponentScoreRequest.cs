***REMOVED***using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ComponentScoreDto.Request
***REMOVED***
    public class CreateComponentScoreRequest
    ***REMOVED***
        public string? Name ***REMOVED*** get; set; ***REMOVED***
        public float? Percent ***REMOVED*** get; set; ***REMOVED***
        public string? Description ***REMOVED*** get; set; ***REMOVED***
        public int? SubjectId ***REMOVED*** get; set; ***REMOVED***

        public List<ErrorMessage> ValidateInput()
        ***REMOVED***
            var errors = new List<ErrorMessage>();

            if (String.IsNullOrEmpty(Name))
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Name",
                    Message = "Name is required!"
        ***REMOVED***);
            else if(Name.Length > 150)
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Name",
                    Message = "Length of Name must <= 150 characters!"
        ***REMOVED***);
            
            if (SubjectId == null)
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "SubjectId",
                    Message = "SubjectId is required"
        ***REMOVED***);
            
            if (Percent == null)
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Percent",
                    Message = "Percent is required!"
        ***REMOVED***);
            else if(Percent <= 0 || Percent > 100)
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Percent",
                    Message = "Percent must be in (0, 100]!"
        ***REMOVED***);

            if (!String.IsNullOrEmpty(Description) && Description.Length > 250)
            ***REMOVED***
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Description",
                    Message = "Length of Component Score's Description must <= 250!"
        ***REMOVED***);
    ***REMOVED***

            return errors;
***REMOVED***
***REMOVED***
***REMOVED***
