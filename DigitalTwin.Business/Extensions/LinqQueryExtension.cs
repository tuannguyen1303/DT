using System.Linq.Expressions;

namespace DigitalTwin.Business.Extensions;

public static class LinqQueryExtension
{
    public static Expression<Func<T, bool>> CombineAndQuery<T>(Expression<Func<T, bool>> filter1,
        Expression<Func<T, bool>> filter2)
    {
        // combine two predicates:
        // need to rewrite one of the lambdas, swapping in the parameter from the other
        var rewrittenBody1 = new ReplaceVisitor(
            filter1.Parameters[0], filter2.Parameters[0]).Visit(filter1.Body);
        var newFilter = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(rewrittenBody1!, filter2.Body), filter2.Parameters);
        return newFilter;
    }
    
    public static Expression<Func<T, bool>> CombineOrQuery<T>(Expression<Func<T, bool>> filter1,
        Expression<Func<T, bool>> filter2)
    {
        // combine two predicates:
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
        return data.Any(func) ? data.Where(func).Sum(expression) : null;
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
    {
        return node == _from ? _to : base.Visit(node);
    }
}