﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntityFrameworkCore3Mock.Tests.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace EntityFrameworkCore3Mock.NSubstitute.Tests
{
    [TestFixture]
    public class AutoMapperRelatedTests
    {
        public class UserModel
        {
            public string FullName { get; set; }

            public string Name { get; set; }
        }

        [Test]
        public async Task DbSetMock_AutoMapperProjectTo()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<User, UserModel>()
                    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.FullName));
            });

            var dbSetMock = new DbSetMock<User>(new[]
            {
                new User { Id = Guid.NewGuid(), FullName = "Fake Drake" },
                new User { Id = Guid.NewGuid(), FullName = "Jackira Spicy" }
            }, (x, _) => x.Id);
            var dbSet = dbSetMock.Object;

            Assert.That(await dbSet.CountAsync(), Is.EqualTo(2));

            var models = await dbSet
                .Where(u => u.FullName != null)
                .ProjectTo<UserModel>()
                .ToListAsync();

            Assert.That(models.Count, Is.EqualTo(2));
        }
    }
}
