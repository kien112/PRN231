using ScoreManagementClient.Dtos.Common;

namespace ScoreManagementClient.Dtos.ComponentScoreDto.Request
{
    public class CreateComponentScoreRequest
    {
        public string? Name { get; set; }
        public float? Percent { get; set; }
        public string? Description { get; set; }
        public int? SubjectId { get; set; }

        public List<ErrorMessage> ValidateInput()
        {
            var errors = new List<ErrorMessage>();

            if (String.IsNullOrEmpty(Name))
                errors.Add(new ErrorMessage
                {
                    Key = "Name",
                    Message = "Name is required!"
                });
            else if(Name.Length > 150)
                errors.Add(new ErrorMessage
                {
                    Key = "Name",
                    Message = "Length of Name must <= 150 characters!"
                });
            
            if (SubjectId == null)
                errors.Add(new ErrorMessage
                {
                    Key = "SubjectId",
                    Message = "SubjectId is required"
                });
            
            if (Percent == null)
                errors.Add(new ErrorMessage
                {
                    Key = "Percent",
                    Message = "Percent is required!"
                });
            else if(Percent <= 0 || Percent > 100)
                errors.Add(new ErrorMessage
                {
                    Key = "Percent",
                    Message = "Percent must be in (0, 100]!"
                });

            if (!String.IsNullOrEmpty(Description) && Description.Length > 250)
            {
                errors.Add(new ErrorMessage
                {
                    Key = "Description",
                    Message = "Length of Component Score's Description must <= 250!"
                });
            }

            return errors;
        }
    }
}
