using ScoreManagementApi.Core.OtherObjects;
using System.Reflection;

namespace ScoreManagementApi.Core.Dtos.Common
{
    public class SearchList<T>
    {
        public List<T>? Result { get; set; }
        public int? TotalElements { get; set; }
        public string? OrderBy { get; set; }
        public string? SortBy { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public void ValidateInput()
        {
            if(OrderBy == null || (!OrderBy.Equals(StaticString.ASC) && !OrderBy.Equals(StaticString.DESC)))
            {
                OrderBy = StaticString.ASC;
            }

            if (SortBy != null)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                bool isValidProp = false;

                foreach (PropertyInfo prop in properties)
                {
                    if(String.Equals(prop.Name, SortBy, StringComparison.OrdinalIgnoreCase))
                    {
                        SortBy = prop.Name;
                        isValidProp = true;
                        break;
                    }
                }
                if (!isValidProp)
                    SortBy = "Id";
            }
            else
            {
                SortBy = "Id";
            }

            if(PageIndex == null || PageIndex < 0)
            {
                PageIndex = 0;
            }

            if(PageSize == null || PageSize <= 0)
            {
                PageSize = 5;
            }

        }
    }

}
