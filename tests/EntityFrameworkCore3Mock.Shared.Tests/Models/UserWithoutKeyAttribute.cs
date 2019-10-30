using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCore3Mock.Shared.Tests.Models
{
    public class UserWithoutKeyAttribute
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }
    }
}
