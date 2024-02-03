using ScoreManagementApi.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ClassRoomDto.Request
{
    public class CreateClassRequest
    {
        public string Name { get; set; }
        public string? TeacherId { get; set; }
        public int? SubjectId { get; set; }

        public List<ErrorMessage> ValidateInput()
        {
            var errors = new List<ErrorMessage>();

            if (String.IsNullOrEmpty(Name))
                errors.Add(new ErrorMessage
                {
                    Key = "Name",
                    Message = "Class Name is required!"
                });
            else if(Name.Length > 150)
                errors.Add(new ErrorMessage
                {
                    Key = "Name",
                    Message = "Length of Class Name must <= 150 characters!"
                });

            if(SubjectId == null)
                errors.Add(new ErrorMessage
                {
                    Key = "SubjectId",
                    Message = "SubjectId is required!"
                });


            return errors;
        }
    }
}
