using System;

namespace EntityFrameworkCore3Mock.Shared.Tests.Models
{
    public class UserWithIdProperty
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }
    }
}
