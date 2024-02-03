using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.SubjectDto.Request
{
    public class UpdateSubjectRequest : CreateSubjectRequest
    {
        public int? Id { get; set; }
        public bool? Active { get; set; }

        public new List<ErrorMessage> ValidateInput()
        {
            var errors = base.ValidateInput();

            if (Id == null)
                errors.Add(new ErrorMessage
                {
                    Key = "Id",
                    Message = "Subject Id is required!"
                });

            if (Active == null)
                Active = false;
            
            return errors;
        }
    }
}
