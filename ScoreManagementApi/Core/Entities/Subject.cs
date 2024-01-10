***REMOVED***using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
***REMOVED***
    public class Subject
    ***REMOVED***

        public int Id ***REMOVED*** get; set; ***REMOVED***

        [MaxLength(150)]
        [Required]
        public string Name ***REMOVED*** get; set; ***REMOVED*** = null!;
        
        [MaxLength(250)]
        public string? Description ***REMOVED*** get; set; ***REMOVED***
        public bool Active ***REMOVED*** get; set; ***REMOVED***
        public DateTime CreatedAt ***REMOVED*** get; set; ***REMOVED***
        [ForeignKey("Creator")]
        public string? CreatorId ***REMOVED*** get; set; ***REMOVED***
        public User? Creator ***REMOVED*** get; set; ***REMOVED***

        public virtual List<ClassRoom>? ClassRooms ***REMOVED*** get; set; ***REMOVED***
        public virtual List<ComponentScore>? ComponentScores ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
