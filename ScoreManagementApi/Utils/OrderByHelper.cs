using ScoreManagementApi.Core.Entities;
using System.Linq.Expressions;

namespace ScoreManagementApi.Utils
{
    public static class OrderByHelper
    {
        public static Expression<Func<T, object>> GetKeySelector<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "item");
            var property = Expression.Property(parameter, propertyName);
            var converted = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(converted, parameter);
        }

    }
}
