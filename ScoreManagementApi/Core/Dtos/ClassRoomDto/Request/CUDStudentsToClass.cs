using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ClassRoomDto.Request
{
    public class CUDStudentsToClass
    {
        public int? ClassId { get; set; }
        public List<string>? StudentIds { get; set; }

    }
}
