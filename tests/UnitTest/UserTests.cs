using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Create()
        {
            try
            {
                var service = TestFactory.Instance.GetRequiredService<IUserStore>();

                var dto = new Shared.DTOs.UserDto()
                {
                    UserName = "dai.nguyen",
                    Email = "dai.nguyen@dsolution.com",
                    FirstName = "Dai",
                    LastName = "Nguyen",
                    Password = "Pa$$w0rd"
                };

                var action = AsyncHelper.RunSync(() => service.AddAsync(dto));

                Assert.IsTrue(action.Result != null);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
