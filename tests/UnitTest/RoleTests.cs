using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class RoleTests
    {
        [TestMethod]
        public void Create()
        {
            try
            {
                var service = TestFactory.Instance.GetRequiredService<IRoleStore>();

                var dto = new Shared.DTOs.RoleDto()
                {
                    Name = "Admin",
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
