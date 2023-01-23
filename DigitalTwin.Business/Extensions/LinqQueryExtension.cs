using System.Linq.Expressions;

namespace DigitalTwin.Business.Extensions;

public static class LinqQueryExtension
{
    public static Expression<Func<T, bool>> BuildQuery<T>(Expression<Func<T, bool>> filter1)
        where T : class
    {
        Expression<Func<T, bool>> expression = filter1;
        return expression;
    }

    public static Expression<Func<T, bool>> AndQuery<T>(Expression<Func<T, bool>> filter1,
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