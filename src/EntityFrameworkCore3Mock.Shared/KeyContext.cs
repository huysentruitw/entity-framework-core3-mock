/*
 * Copyright 2017-2019 Wouter Huysentruit
 *
 * See LICENSE file.
 */

namespace EntityFrameworkCore3Mock
{
    public sealed class KeyContext
    {
        private long _nextIdentity = 1;

        public long NextIdentity => _nextIdentity++;
    }
}
