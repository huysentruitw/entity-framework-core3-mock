/*
 * Copyright 2017-2019 Wouter Huysentruit
 *
 * See LICENSE file.
 */

using System;

namespace EntityFrameworkCore3Mock
{
    public sealed class SavedChangesEventArgs<TEntity> : EventArgs
        where TEntity : class
    {
        public UpdatedEntityInfo<TEntity>[] UpdatedEntities { get; set; }
    }

    public sealed class UpdatedEntityInfo<TEntity>
        where TEntity : class
    {
        public TEntity Entity { get; set; }

        public UpdatePropertyInfo[] UpdatedProperties { get; set; }
    }

    public sealed class UpdatePropertyInfo
    {
        public string Name { get; set; }

        public object Original { get; set; }

        public object New { get; set; }
    }
}
