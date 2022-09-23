#nullable disable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RCL.Core.Authorization;

namespace RCL.Core.Api.RequestService.Test
{
    [TestClass]
    public class ApiRequestTest
    {
        private readonly IAuthTokenService _authTokenService;
        private readonly IOptions<ApiOptions> _apiOptions;

        private readonly DemoService _demoService;

        public ApiRequestTest()
        {
            _authTokenService = (IAuthTokenService)DependencyResolver
                .ServiceProvider().GetService(typeof(IAuthTokenService));

            _apiOptions = (IOptions<ApiOptions>)DependencyResolver
                .ServiceProvider().GetService<IOptions<ApiOptions>>();

            _demoService = new DemoService(_authTokenService, _apiOptions);
        }

        [TestMethod]
        public async Task RequestApiTest()
        {
            try
            {
                List<Booking> bookings = await _demoService.GetUserBookings("123");
                Assert.AreEqual(1, 1);
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }
    }

    public class DemoService : ApiRequestBase
    {
        public DemoService(IAuthTokenService authTokenService, 
            IOptions<ApiOptions> options) : base(authTokenService, options)
        {
        }

        public async Task<List<Booking>> GetUserBookings(string userId)
        {
            string uri = $"v1/demo/booking/userid/{userId}/getall";

            List<Booking> bookings = await GetListResultAsync<Booking>(uri);

            return bookings;
        }

    }

    public class Booking
    {
        public int Id { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RoomNumber { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
    }
}
