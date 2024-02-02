***REMOVED***using ScoreManagementApi.Core.Entities;
using System.Linq.Expressions;

namespace ScoreManagementApi.Utils
***REMOVED***
    public static class OrderByHelper
    ***REMOVED***
        public static Expression<Func<T, object>> GetKeySelector<T>(string propertyName)
        ***REMOVED***
            var parameter = Expression.Parameter(typeof(T), "item");
            var property = Expression.Property(parameter, propertyName);
            var converted = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(converted, parameter);
***REMOVED***

***REMOVED***
***REMOVED***
