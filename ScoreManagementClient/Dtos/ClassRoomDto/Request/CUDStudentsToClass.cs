using ScoreManagementClient.Dtos.Common;

namespace ScoreManagementClient.Dtos.ClassRoomDto.Request
{
    public class CUDStudentsToClass
    {
        public int? ClassId { get; set; }
        public List<string>? StudentIds { get; set; }

    }
}
