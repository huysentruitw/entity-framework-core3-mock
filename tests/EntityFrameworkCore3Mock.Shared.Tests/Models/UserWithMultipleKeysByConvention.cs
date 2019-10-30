using System;

namespace EntityFrameworkCore3Mock.Shared.Tests.Models
{
    public class UserWithMultipleKeysByConvention
    {
        public Guid Id { get; set; }

        public Guid UserWithMultipleKeysByConventionId { get; set; }

        public string FullName { get; set; }
    }
}
