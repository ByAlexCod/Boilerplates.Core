using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using ProjectBpl.DAL.CoreServices;
using ProjectBpl.DAL.Entities;

namespace ProjectBpl.Tests
{
    public class UserDalTest
    {
        UserService service = new UserService();
        



        [Test]
        public async Task TestUserCreate()
        {
            var user = await TestUtils.CreateBaseUser(service);
            Assert.AreEqual(user.Email, TestUtils.DEFAULT_EMAIL);
            Assert.IsTrue(user.UserId >= 0);
            Assert.AreEqual(user.Username, TestUtils.DEFAULT_USERNAME);
        }

        [Test]
        public async Task TestGetUserByEmail()
        {
            var baseUser = await TestUtils.CreateBaseUser(service);
            var userByEmail = await service.GetUserByEmail(baseUser.Email);
            Assert.IsNotNull(userByEmail);
            Assert.AreEqual(baseUser.Email, userByEmail.Email);
        }

        [Test]
        public async Task TestCheckPassword()
        {
            await TestUtils.CreateBaseUser(service);
            var isPasswordValid = await service.CheckPassword(TestUtils.DEFAULT_EMAIL, TestUtils.DEFAULT_PASSWORD);
            var isPasswordUsernameValid = await service.CheckPassword(TestUtils.DEFAULT_USERNAME, TestUtils.DEFAULT_PASSWORD);
            Assert.IsTrue(isPasswordValid);
            Assert.IsTrue(isPasswordUsernameValid);
        }

        [Test]
        public async Task TestPasswordUpdate()
        {
            var user = await TestUtils.CreateBaseUser(service);
            var isPasswordValid = await service.CheckPassword(TestUtils.DEFAULT_EMAIL, TestUtils.DEFAULT_PASSWORD);
            var isPasswordUsernameValid = await service.CheckPassword(TestUtils.DEFAULT_USERNAME, TestUtils.DEFAULT_PASSWORD);
            Assert.IsTrue(isPasswordValid);
            Assert.IsTrue(isPasswordUsernameValid);
            await service.UpdatePassword(user.UserId, "abcd1234*");
            Assert.IsTrue(await service.CheckPassword(TestUtils.DEFAULT_EMAIL, "abcd1234*"));
        }

        [TearDown]
        public async Task TearDown()
        {
            await service.DeleteUserByEmail(TestUtils.DEFAULT_EMAIL);
        }
    }
}
