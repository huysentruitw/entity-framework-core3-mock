using System;

namespace EntityFrameworkCore3Mock.Tests.Models
{
    public class NoKeyModel
    {
        public Guid ModelId { get; set; }

        public string Value { get; set; }
    }
}
