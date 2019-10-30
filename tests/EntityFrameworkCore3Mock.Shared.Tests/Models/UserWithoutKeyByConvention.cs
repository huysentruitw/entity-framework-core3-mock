using System;

namespace EntityFrameworkCore3Mock.Shared.Tests.Models
{
    public class UserWithoutKeyByConvention
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }
    }
}
