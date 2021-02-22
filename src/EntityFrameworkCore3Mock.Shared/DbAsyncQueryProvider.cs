﻿/*
 * Copyright 2017-2019 Wouter Huysentruit
 *
 * See LICENSE file.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EntityFrameworkCore3Mock
{
    public class DbAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public DbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            if (expression is MethodCallExpression m)
            {
                var resultType = m.Method.ReturnType; // it should be IQueryable<T>
                var tElement = resultType.GetGenericArguments().First();
                var queryType = typeof(DbAsyncEnumerable<>).MakeGenericType(tElement);
                return (IQueryable)Activator.CreateInstance(queryType, expression);
            }

            return new DbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<T> CreateQuery<T>(Expression expression)
        {
            return new DbAsyncEnumerable<T>(expression);
        }

        public object Execute(Expression expression)
        {
            return CompileExpressionItem<object>(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return CompileExpressionItem<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];

#if NETSTANDARD2_1
            var executeMethodInfo = typeof(IQueryProvider)
                .GetMethod(
                    name: nameof(IQueryProvider.Execute),
                    genericParameterCount: 1,
                    types: new[] { typeof(Expression) })
                ?? throw new InvalidOperationException("Execute method not found");
#else
            var executeMethodInfo = typeof(IQueryProvider)
                .GetMethods()
                .FirstOrDefault(x => x.IsGenericMethod &&
                                     x.Name == nameof(IQueryProvider.Execute) &&
                                     x.GetGenericArguments().Length == 1)
                ?? throw new InvalidOperationException("Execute method not found");
#endif
            var executionResult = executeMethodInfo
                .MakeGenericMethod(expectedResultType)
                .Invoke(this, new[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                .MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult });
        }

        private static T CompileExpressionItem<T>(Expression expression)
            => Expression.Lambda<Func<T>>(
                body: new Visitor().Visit(expression) ?? throw new InvalidOperationException("Visitor returns null"),
                parameters: (IEnumerable<ParameterExpression>) null)
            .Compile()();

        private class Visitor : ExpressionVisitor { }
    }
}
