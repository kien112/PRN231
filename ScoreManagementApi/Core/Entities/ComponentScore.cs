***REMOVED***using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
***REMOVED***
    public class ComponentScore
    ***REMOVED***
        public int Id ***REMOVED*** get; set; ***REMOVED***
        [MaxLength(150)]
        public string Name ***REMOVED*** get; set; ***REMOVED***
        public float Percent ***REMOVED*** get; set; ***REMOVED***
        public bool Active ***REMOVED*** get; set; ***REMOVED***
        [MaxLength(250)]
        public string? Description ***REMOVED*** get; set; ***REMOVED***
        [ForeignKey("Subject")]
        public int SubjectId ***REMOVED*** get; set; ***REMOVED***
        public Subject Subject ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
