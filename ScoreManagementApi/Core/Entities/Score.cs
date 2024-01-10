***REMOVED***using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
***REMOVED***
    public class Score
    ***REMOVED***
        public int Id ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("Student")]
        public string StudentId ***REMOVED*** get; set; ***REMOVED***
        public User Student ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("ComponentScore")]
        public int ComponentScoreId ***REMOVED*** get; set; ***REMOVED***
        public ComponentScore ComponentScore ***REMOVED*** get; set; ***REMOVED***

        public float? Mark ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
