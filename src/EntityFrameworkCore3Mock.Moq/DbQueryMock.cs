/*
 * Copyright 2017-2019 Wouter Huysentruit
 *
 * See LICENSE file.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EntityFrameworkCore3Mock
{
    public class DbQueryMock<TEntity> : Mock<DbQuery<TEntity>>, IDbQueryMock
        where TEntity : class
    {
        private readonly IEnumerable<TEntity> _entities;

        public DbQueryMock(IEnumerable<TEntity> entities, bool asyncQuerySupport = true)
        {
            _entities = (entities ?? Enumerable.Empty<TEntity>()).ToList();

            var data = _entities.AsQueryable();
            As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(asyncQuerySupport ? new DbAsyncQueryProvider<TEntity>(data.Provider) : data.Provider);
            As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(() => data.GetEnumerator());
            As<IEnumerable>().Setup(x => x.GetEnumerator()).Returns(() => data.GetEnumerator());

            if (asyncQuerySupport)
            {
                As<IAsyncEnumerable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(() => new DbAsyncEnumerator<TEntity>(data.GetEnumerator()));
            }
        }
    }
}
