using System;

namespace EntityFrameworkCore3Mock.Shared.Tests.Models
{
    public class UserWithKeyByConvention
    {
        public Guid UserWithKeyByConventionId { get; set; }

        public string FullName { get; set; }
    }
}
