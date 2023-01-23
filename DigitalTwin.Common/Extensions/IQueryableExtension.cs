using System.Linq.Expressions;
using System.Reflection;
using DigitalTwin.Common.Enums;
using DigitalTwin.Common.Utilities;

namespace DigitalTwin.Common.Extensions
{
    public static class IQueryableExtension
    {
        private const string ContainsMethodName = "Contains";
        private const string ToLowerMethodName = "ToLower";


        /// <summary>
        /// BuildSearchBy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="keyWords"></param>
        /// <param name="onProperties"></param>
        /// <returns></returns>
        public static IQueryable<T> BuildSearchBy<T>(this IQueryable<T> queryable, string keyWords, IEnumerable<string> onProperties)
        {
            var query = queryable;
            if (string.IsNullOrEmpty(keyWords) ||
                onProperties == null || !onProperties.Any())
            {
                return query;
            }
            var lambda = ToSearchLambdaExpress<T>(keyWords, onProperties);
            if (lambda != null)
            {
                query = query.Where(lambda);
            }
            return query;
        }

        /// <summary>
        /// BuildFilterby
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="filterOptions"></param>
        /// <returns></returns>
        public static IQueryable<T> BuildFilterBy<T>(this IQueryable<T> queryable, IEnumerable<FilterOptions> filterOptions)
        {
            var query = queryable;
            if (filterOptions == null)
            {
                return query;
            }
            foreach (var filterOption in filterOptions)
            {
                query = query.BuildFilterBy(filterOption);
            }
            return query;
        }

        /// <summary>
        /// BuildFilterby
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="filterOption"></param>
        /// <returns></returns>
        public static IQueryable<T> BuildFilterBy<T>(this IQueryable<T> queryable, FilterOptions filterOption)
        {
            var query = queryable;
            if (filterOption == null ||
                string.IsNullOrWhiteSpace(filterOption.PropertyName) ||
                filterOption.FilterValues == null)
            {
                return query;
            }
            var lambda = filterOption.ToLambdaExpress<T>();
            if (lambda != null)
            {
                query = query.Where(lambda);
            }
            return query;
        }

        /// <summary>
        /// BuildOrderby
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="option"></param>
        /// <param name="columnSortFieldTwo"></param>
        /// <returns></returns>
        public static IQueryable<T> BuildOrderBy<T>(this IQueryable<T> queryable, SortingOptions option, List<SortingOptions>? sortingOptionsDefault = null)
            where T : class
        {
            if (string.IsNullOrEmpty(option.Column))
            {
                return queryable;
            }

            IOrderedQueryable<T> query = queryable.ApplyOrderBy(option);

            if (sortingOptionsDefault != null)
            {
                foreach (var sortOption in sortingOptionsDefault)
                {
                    query = query.ApplyThenBy(sortOption);
                }
            }

            return query;
        }

        private static IOrderedQueryable<T> ApplyOrderBy<T>(this IQueryable<T> queryable, SortingOptions option)
        {
            var lambda = option.ToLambdaExpress<T>();
            return option.SortType switch
            {
                ESortingTypes.Ascending => queryable.OrderBy(lambda),
                ESortingTypes.Descending => queryable.OrderByDescending(lambda),
                _ => queryable.OrderBy(lambda),
            };
        }

        private static IOrderedQueryable<T> ApplyThenBy<T>(this IOrderedQueryable<T> queryable, SortingOptions option)
        {
            var lambda = option.ToLambdaExpress<T>();
            return option.SortType switch
            {
                ESortingTypes.Ascending => queryable.ThenBy(lambda),
                ESortingTypes.Descending => queryable.ThenByDescending(lambda),
                _ => queryable.ThenBy(lambda),
            };
        }

        private static Expression<Func<T, object>> ToLambdaExpress<T>(this SortingOptions option)
        {
            var param = Expression.Parameter(typeof(T), "param");
            var property = Expression.Convert(Expression.PropertyOrField(param, option.Column), typeof(object));
            return Expression.Lambda<Func<T, object>>(property, new ParameterExpression[] { param });
        }

        private static Expression<Func<T, bool>>? ToLambdaExpress<T>(this FilterOptions option)
        {
            var param = Expression.Parameter(typeof(T), "entity");
            var filterProperty = Expression.PropertyOrField(param, option.PropertyName);
            (ConstantExpression filterValue,
            MethodInfo? containsMethod) =
            TypeCorrection(option.FilterValues, filterProperty);
            if (containsMethod != null)
            {

                var checkContains = Expression.Call(filterValue, containsMethod, filterProperty);
                return Expression.Lambda<Func<T, bool>>(checkContains, new ParameterExpression[] { param });
            }
            return null;
        }

        private static Expression<Func<T, bool>>? ToSearchLambdaExpress<T>(string keyWords, IEnumerable<string> onProperties)
        {
            var param = Expression.Parameter(typeof(T), "entity");
            List<Expression> orExpression = new List<Expression>();

            foreach (var property in onProperties)
            {
                var searchProperty = Expression.PropertyOrField(param, property);
                (ConstantExpression? searchValue,
                MethodInfo? normalizeMethod,
                MethodInfo? containsMethod) =
                TypeCorection(keyWords.ToLower(), searchProperty);
                if (searchValue == null)
                {
                    continue;
                }
                Expression compareExpression;
                if (containsMethod != null)
                {
                    compareExpression = normalizeMethod != null ?
                        Expression.Call(Expression.Call(searchProperty, normalizeMethod), containsMethod, searchValue) :
                        Expression.Call(searchProperty, containsMethod, searchValue);
                }
                else
                {
                    compareExpression = normalizeMethod != null ?
                        Expression.Equal(searchValue, Expression.Call(searchProperty, normalizeMethod)) :
                        Expression.Equal(searchValue, searchProperty);
                }
                orExpression.Add(compareExpression);
            }
            if (!orExpression.Any())
            {
                return null;
            }
            if (orExpression.Count == 1)
            {
                return Expression.Lambda<Func<T, bool>>(orExpression.First(), new ParameterExpression[] { param });
            }
            Expression finalExpression = orExpression.First();
            foreach (var comparer in orExpression.Skip(1))
            {
                finalExpression = Expression.Or(finalExpression, comparer);
            }
            return Expression.Lambda<Func<T, bool>>(finalExpression, new ParameterExpression[] { param });
        }
 
        private static (ConstantExpression returnValue, MethodInfo? containsMethod)
            TypeCorrection(IEnumerable<string> filterValues, MemberExpression filterProperty)
        {
            ConstantExpression returnValue;
            MethodInfo? containsMethod;
            if (filterProperty.Type == typeof(Guid) )
            {
                var typeValue = filterValues
                    .Select(v =>
                    {
                        if (Guid.TryParse(v, out Guid result))
                        {
                            return result;
                        }
                        return Guid.Empty;
                    })
                    .Where(v => v != Guid.Empty)
                    .ToList();

                returnValue = Expression.Constant(typeValue);
                containsMethod = typeValue.GetType().GetMethod(ContainsMethodName, new Type[] {
                    filterProperty.Type
                });
                return (returnValue, containsMethod);
            }
            if(filterProperty.Type == typeof(Guid?))
            {
                var typeValue = filterValues
                    .Select(v =>
                    {
                        if (Guid.TryParse(v, out Guid result))
                        {
                            return result;
                        }
                        return (Guid?)null;
                    })
                    .Where(v => v != null)
                    .ToList();

                returnValue = Expression.Constant(typeValue);
                containsMethod = typeValue.GetType().GetMethod(ContainsMethodName, new Type[] {
                    filterProperty.Type
                });
                return (returnValue, containsMethod);
            }    
            if (filterProperty.Type == typeof(int) || filterProperty.Type == typeof(int?))
            {
                var typeValue = filterValues
                    .Select(v =>
                    {
                        if (int.TryParse(v, out int result))
                        {
                            return result;
                        }
                        return (int?)null;
                    })
                    .Where(v => v != null)
                    .ToList();

                returnValue = Expression.Constant(typeValue);
                containsMethod = typeValue.GetType().GetMethod(ContainsMethodName, new Type[] {
                    filterProperty.Type
                });
                return (returnValue, containsMethod);
            }
            if (filterProperty.Type == typeof(float) || filterProperty.Type == typeof(float?))
            {
                var typeValue = filterValues
                    .Select(v =>
                    {
                        if (float.TryParse(v, out float result))
                        {
                            return result;
                        }
                        return (float?)null;
                    })
                    .Where(v => v != null)
                    .ToList();

                returnValue = Expression.Constant(typeValue);
                containsMethod = typeValue.GetType().GetMethod(ContainsMethodName, new Type[] {
                    filterProperty.Type
                });
                return (returnValue, containsMethod);
            }
            if (filterProperty.Type == typeof(double) || filterProperty.Type == typeof(double?))
            {
                var typeValue = filterValues
                    .Select(v =>
                    {
                        if (double.TryParse(v, out double result))
                        {
                            return result;
                        }
                        return (double?)null;
                    })
                    .Where(v => v != null)
                    .ToList();

                returnValue = Expression.Constant(typeValue);
                containsMethod = typeValue.GetType().GetMethod(ContainsMethodName, new Type[] {
                    filterProperty.Type
                });
                return (returnValue, containsMethod);
            }

            if (filterProperty.Type != typeof(string))
            {
                throw new NotImplementedException();
            }

            returnValue = Expression.Constant(filterValues);
            containsMethod = filterValues.GetType().GetMethod(ContainsMethodName, new Type[] {
                filterProperty.Type
            });

            return (returnValue, containsMethod);
        }

        private static (ConstantExpression? returnValue,
            MethodInfo? normalizeMethod,
            MethodInfo? containsMethod
            )
            TypeCorection(string searchValue, MemberExpression searchProperty)
        {
            ConstantExpression returnValue;
            MethodInfo? normalizeMethod;
            MethodInfo? containsMethod;
            if (searchProperty.Type == typeof(Guid) || searchProperty.Type == typeof(Guid?))
            {
                if (Guid.TryParse(searchValue, out Guid result))
                {
                    returnValue = Expression.Constant(result);
                    return (returnValue, null, null);
                }
                return (null, null, null);
            }
            if (searchProperty.Type == typeof(int) || searchProperty.Type == typeof(int?))
            {
                if (int.TryParse(searchValue, out int result))
                {
                    returnValue = Expression.Constant(result);
                    return (returnValue, null, null);
                }
                return (null, null, null);
            }
            if (searchProperty.Type == typeof(float) || searchProperty.Type == typeof(float?))
            {
                if (float.TryParse(searchValue, out float result))
                {
                    returnValue = Expression.Constant(result);
                    return (returnValue, null, null);
                }
                return (null, null, null);
            }
            if (searchProperty.Type == typeof(double) || searchProperty.Type == typeof(double?))
            {
                if (double.TryParse(searchValue, out double result))
                {
                    returnValue = Expression.Constant(result);
                    return (returnValue, null, null);
                }
                return (null, null, null);
            }

            if (searchProperty.Type != typeof(string))
            {
                throw new NotImplementedException();
            }

            returnValue = Expression.Constant(searchValue);
            normalizeMethod = searchValue.GetType().GetMethod(ToLowerMethodName, Type.EmptyTypes);
            containsMethod = searchValue.GetType().GetMethod(ContainsMethodName, new Type[] {
                searchValue.GetType()
            });

            return (returnValue, normalizeMethod, containsMethod);
        }
    }
}
