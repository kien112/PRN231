using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ComponentScoreDto.Request
{
    public class UpdateComponentScoreRequest : CreateComponentScoreRequest
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
                    Message = "Id of Component Score is required!"
                });

            if (Active == null)
                Active = false;

            return errors;
        }
    }
}
