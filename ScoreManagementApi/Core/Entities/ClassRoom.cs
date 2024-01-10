***REMOVED***using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
***REMOVED***
    public class ClassRoom
    ***REMOVED***
        public int Id ***REMOVED*** get; set; ***REMOVED***
        
        [MaxLength(150)]
        [Required]
        public string Name ***REMOVED*** get; set; ***REMOVED***
        
        public bool Active ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("Teacher")]
        public string? TeacherId ***REMOVED*** get; set; ***REMOVED***
        public User? Teacher ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("Creator")]
        public string? CreatorId ***REMOVED*** get; set; ***REMOVED***
        public User? Creator ***REMOVED*** get; set; ***REMOVED***
        public DateTime CreatedAt ***REMOVED*** get; set; ***REMOVED***
        public int SubjectId ***REMOVED*** get; set; ***REMOVED***
        public virtual Subject Subject ***REMOVED*** get; set; ***REMOVED***
        public virtual List<ClassStudent> ClassStudents ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
