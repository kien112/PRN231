***REMOVED***namespace ScoreManagementApi.Core.Dtos.Common
***REMOVED***
    public class ResponseData<T>
    ***REMOVED***
        public T Data ***REMOVED*** get; set; ***REMOVED***
        public string Message ***REMOVED*** get; set; ***REMOVED***
        public int StatusCode ***REMOVED*** get; set; ***REMOVED***
        public List<ErrorMessage> Erorrs ***REMOVED*** get; set; ***REMOVED***
***REMOVED***

    public class ErrorMessage
    ***REMOVED***
        public string Key ***REMOVED*** get; set; ***REMOVED***
        public string Message ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
