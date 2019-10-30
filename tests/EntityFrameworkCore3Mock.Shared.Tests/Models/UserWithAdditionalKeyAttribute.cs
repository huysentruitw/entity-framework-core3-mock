using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCore3Mock.Shared.Tests.Models
{
    public class UserWithAdditionalKeyAttribute : UserWithKeyAttribute
    {
        [Key, Column(Order = 1), MaxLength(50)]
        public string Tenant { get; set; }
    }
}
