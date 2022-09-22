using RCL.Authorization.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Api.RequestService.Test.AccessToken
{
    [TestClass]
    public class AccessTokenTest
    {
        private readonly IAuthTokenService _authTokenService;

        public AccessTokenTest()
        {
            _authTokenService = (IAuthTokenService)DependencyResolver
                .ServiceProvider().GetService(typeof(IAuthTokenService));
        }

        [TestMethod]
        public async Task GetAuthTokenTest()
        {
            try
            {
                string resource = "6a8b4b39-c021-437c-b060-5a14a3fd65f3";
                AuthToken authToken = await _authTokenService.GetAuthTokenAsync(resource);
                Assert.AreNotEqual(string.Empty, authToken.access_token);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }
    }
}
