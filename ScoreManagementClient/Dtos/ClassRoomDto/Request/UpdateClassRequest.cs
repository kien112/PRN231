using ScoreManagementClient.Dtos.Common;

namespace ScoreManagementClient.Dtos.ClassRoomDto.Request
{
    public class UpdateClassRequest : CreateClassRequest
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
                    Message = "Id is required!"
                });

            if(Active == null)
                Active = false;

            return errors;
        }
    }
}
