***REMOVED***using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
***REMOVED***
    public class ClassStudent
    ***REMOVED***
        public int Id ***REMOVED*** get; set; ***REMOVED***
        [ForeignKey("ClassRoom")]
        public int ClassRoomId ***REMOVED*** get; set; ***REMOVED***
        public ClassRoom ClassRoom ***REMOVED*** get; set; ***REMOVED***
        [ForeignKey("Student")]
        public string StudentId ***REMOVED*** get; set; ***REMOVED***
        public User Student ***REMOVED*** get; set; ***REMOVED***
        public DateTime JoinDate ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
