using ScoreManagementClient.Dtos.Common;

namespace ScoreManagementClient.Dtos.SubjectDto.Request
{
    public class CreateSubjectRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<ErrorMessage> ValidateInput()
        {
            var erorrs = new List<ErrorMessage>();

            if(String.IsNullOrEmpty(Name))
            {
                erorrs.Add(new ErrorMessage
                {
                    Key = "Name",
                    Message = "Name of Subject is required!"
                });
            }else if(Name.Length > 150)
            {
                erorrs.Add(new ErrorMessage
                {
                    Key = "Name",
                    Message = "Length of Subject's Name must <= 150!"
                });
            }

            if(!String.IsNullOrEmpty(Description) && Description.Length > 250)
            {
                erorrs.Add(new ErrorMessage
                {
                    Key = "Description",
                    Message = "Length of Subject's Description must <= 250!"
                });
            }

            return erorrs;
        }
    }
}
