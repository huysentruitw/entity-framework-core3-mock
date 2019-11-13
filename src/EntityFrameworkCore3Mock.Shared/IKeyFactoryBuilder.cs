/*
 * Copyright 2017-2019 Wouter Huysentruit
 *
 * See LICENSE file.
 */

using System;

namespace EntityFrameworkCore3Mock
{
    public interface IKeyFactoryBuilder
    {
        Func<T, KeyContext, object> BuildKeyFactory<T>();
    }
}
