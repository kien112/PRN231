***REMOVED***using ScoreManagementApi.Core.OtherObjects;
using System.Reflection;

namespace ScoreManagementApi.Core.Dtos.Common
***REMOVED***
    public class SearchList<T>
    ***REMOVED***
        public List<T>? Result ***REMOVED*** get; set; ***REMOVED***
        public int? TotalElements ***REMOVED*** get; set; ***REMOVED***
        public string? OrderBy ***REMOVED*** get; set; ***REMOVED***
        public string? SortBy ***REMOVED*** get; set; ***REMOVED***
        public int? PageIndex ***REMOVED*** get; set; ***REMOVED***
        public int? PageSize ***REMOVED*** get; set; ***REMOVED***

        public void ValidateInput()
        ***REMOVED***
            if(OrderBy == null || (!OrderBy.Equals(StaticString.ASC) && !OrderBy.Equals(StaticString.DESC)))
            ***REMOVED***
                OrderBy = StaticString.ASC;
    ***REMOVED***

            if (SortBy != null)
            ***REMOVED***
                PropertyInfo[] properties = typeof(T).GetProperties();
                bool isValidProp = false;

                foreach (PropertyInfo prop in properties)
                ***REMOVED***
                    if(String.Equals(prop.Name, SortBy, StringComparison.OrdinalIgnoreCase))
                    ***REMOVED***
                        SortBy = prop.Name;
                        isValidProp = true;
                        break;
            ***REMOVED***
        ***REMOVED***
                if (!isValidProp)
                    SortBy = "Id";
    ***REMOVED***
            else
            ***REMOVED***
                SortBy = "Id";
    ***REMOVED***

            if(PageIndex == null || PageIndex < 0)
            ***REMOVED***
                PageIndex = 0;
    ***REMOVED***

            if(PageSize == null || PageSize <= 0)
            ***REMOVED***
                PageSize = 5;
    ***REMOVED***

***REMOVED***
***REMOVED***

***REMOVED***
