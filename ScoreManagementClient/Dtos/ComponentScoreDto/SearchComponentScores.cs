﻿using ScoreManagementClient.Dtos.Common;
using ScoreManagementClient.Dtos.ComponentScoreDto.Response;

namespace ScoreManagementClient.Dtos.ComponentScoreDto
{
    public class SearchComponentScores : SearchList<ComponentScoreResponse>
    {
        public string? Name { get; set; }
        public float? Percent { get; set; }
        public bool? Active { get; set; }
        public int? SubjectId { get; set; }

        public new void ValidateInput()
        {
            base.ValidateInput();

            if(!String.IsNullOrEmpty(Name))
                Name = Name.Trim().ToLower();
            
            if (Active == null)
                Active = false;

        }
    }
}
