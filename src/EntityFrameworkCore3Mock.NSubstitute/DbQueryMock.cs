/*
 * Copyright 2017-2019 Wouter Huysentruit
 *
 * See LICENSE file.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace EntityFrameworkCore3Mock
{
    public class DbQueryMock<TEntity> : IDbQueryMock
        where TEntity : class
    {
        private readonly IEnumerable<TEntity> _entities;

        public DbQuery<TEntity> Object { get; }

        public DbQueryMock(IEnumerable<TEntity> entities, bool asyncQuerySupport = true)
        {
            _entities = (entities ?? Enumerable.Empty<TEntity>()).ToList();

            var data = _entities.AsQueryable();

            Object = Substitute.For<DbQuery<TEntity>, IQueryable<TEntity>, IAsyncEnumerable<TEntity>>();

            ((IQueryable<TEntity>)Object).Provider.Returns(asyncQuerySupport ? new DbAsyncQueryProvider<TEntity>(data.Provider) : data.Provider);
            Object.AsQueryable().Provider.Returns(asyncQuerySupport ? new DbAsyncQueryProvider<TEntity>(data.Provider) : data.Provider);
            Object.AsQueryable().Expression.Returns(data.Expression);
            Object.AsQueryable().ElementType.Returns(data.ElementType);
            ((IQueryable<TEntity>)Object).GetEnumerator().Returns(a => data.GetEnumerator());
            ((IEnumerable)Object).GetEnumerator().Returns(a => data.GetEnumerator());

            if (asyncQuerySupport)
            {
                ((IAsyncEnumerable<TEntity>)Object).GetEnumerator().Returns(a => new DbAsyncEnumerator<TEntity>(data.GetEnumerator()));
            }
        }
    }
}
