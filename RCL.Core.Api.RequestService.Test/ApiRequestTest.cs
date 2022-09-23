#nullable disable

using RCL.Core.Authorization;

namespace RCL.Core.Api.RequestService.Test
{
    [TestClass]
    public class ApiRequestTest
    {
        private readonly IAuthTokenService _authTokenService;

        private readonly DemoService _demoService;

        public ApiRequestTest()
        {
            _authTokenService = (IAuthTokenService)DependencyResolver
                .ServiceProvider().GetService(typeof(IAuthTokenService));

            _demoService = new DemoService(_authTokenService);
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
        private const string _apiEndpoint = "https://my-api-endpoint.com";
        private const string _resource = "111111111";

        public DemoService(IAuthTokenService authTokenService) : base(authTokenService)
        {
        }

        public async Task<List<Booking>> GetUserBookings(string userId)
        {
            string uri = $"{_apiEndpoint}/v1/demo/booking/userid/{userId}/getall";

            List<Booking> bookings = await GetListResultAsync<Booking>(uri,_resource);

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
