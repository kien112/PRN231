using ScoreManagementClient.Dtos.Common;
using ScoreManagementClient.Dtos.SubjectDto.Response;

namespace ScoreManagementClient.Dtos.SubjectDto
{
    public class SearchSubject : SearchList<SubjectResponse>
    {
        public string? Name { get; set; }
        public bool? Active { get; set; }
        public bool? IsCurrentSubject { get; set; }

        public new void ValidateInput()
        {
            base.ValidateInput();

            if (!String.IsNullOrEmpty(Name))
            {
                Name = Name.Trim().ToLower();
            }

            if(IsCurrentSubject == null)
                IsCurrentSubject = false;

        }
    }
}
