using System.Linq.Expressions;

namespace DigitalTwin.Common.Extensions;

public static class LinqQueryExtension
{
    public static Expression<Func<T, bool>> CombineAndQuery<T>(Expression<Func<T, bool>> filter1,
        Expression<Func<T, bool>> filter2)
    {
        // combine two predicates using and:
        // need to rewrite one of the lambdas, swapping in the parameter from the other
        var rewrittenBody1 = new ReplaceVisitor(
            filter1.Parameters[0], filter2.Parameters[0]).Visit(filter1.Body);
        var newFilter = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(rewrittenBody1!, filter2.Body), filter2.Parameters);
        return newFilter;
    }

    public static Expression<Func<T, bool>>? BuildQuery<T>(bool useAndCondition,
        params Expression<Func<T, bool>>[] queries)
    {
        Expression<Func<T, bool>>? result = null;

        if (queries.Length == 1)
            return queries[0];

        var firstQuery = queries[0];
        for (var i = 1; i < queries.Length; i++)
        {
            if (result != null)
            {
                var buildNewQuery = useAndCondition
                    ? CombineAndQuery(result, queries[i])
                    : CombineOrQuery(result, queries[i]);
                result = buildNewQuery;
            }
            else
                result = useAndCondition
                    ? CombineAndQuery(firstQuery, queries[i])
                    : CombineOrQuery(firstQuery, queries[i]);
        }

        return result;
    }

    public static Expression<Func<T, bool>> CombineOrQuery<T>(Expression<Func<T, bool>> filter1,
        Expression<Func<T, bool>> filter2)
    {
        // combine two predicates using or:
        // need to rewrite one of the lambdas, swapping in the parameter from the other
        var rewrittenBody1 = new ReplaceVisitor(
            filter1.Parameters[0], filter2.Parameters[0]).Visit(filter1.Body);
        var newFilter = Expression.Lambda<Func<T, bool>>(
            Expression.Or(rewrittenBody1!, filter2.Body), filter2.Parameters);
        return newFilter;
    }

    public static decimal? SumDecimalByProp<T>(this IEnumerable<T> data, Expression<Func<T, bool>> filter,
        Func<T, decimal> expression)
    {
        var func = filter.Compile();
        var enumerable = data as T[] ?? data.ToArray();
        return enumerable.Any(func) ? enumerable.Where(func).Sum(expression) : null;
    }
}

class ReplaceVisitor : ExpressionVisitor
{
    private readonly Expression _from;
    private readonly Expression? _to;

    public ReplaceVisitor(Expression from, Expression? to)
    {
        _from = from;
        _to = to;
    }

    public override Expression? Visit(Expression? node)
        => node == _from ? _to : base.Visit(node);
}