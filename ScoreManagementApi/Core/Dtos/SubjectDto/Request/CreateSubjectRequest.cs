***REMOVED***using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ScoreManagementApi.Core.Entities;
using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.SubjectDto.Request
***REMOVED***
    public class CreateSubjectRequest
    ***REMOVED***
        public string? Name ***REMOVED*** get; set; ***REMOVED***
        public string? Description ***REMOVED*** get; set; ***REMOVED***

        public List<ErrorMessage> ValidateInput()
        ***REMOVED***
            var erorrs = new List<ErrorMessage>();

            if(String.IsNullOrEmpty(Name))
            ***REMOVED***
                erorrs.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Name",
                    Message = "Name of Subject is required!"
        ***REMOVED***);
    ***REMOVED***else if(Name.Length > 150)
            ***REMOVED***
                erorrs.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Name",
                    Message = "Length of Subject's Name must <= 150!"
        ***REMOVED***);
    ***REMOVED***

            if(!String.IsNullOrEmpty(Description) && Description.Length > 250)
            ***REMOVED***
                erorrs.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Description",
                    Message = "Length of Subject's Description must <= 250!"
        ***REMOVED***);
    ***REMOVED***

            return erorrs;
***REMOVED***
***REMOVED***
***REMOVED***
