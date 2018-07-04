using System;
using System.Net.Http;
using System.Threading.Tasks;
using TardisBank.Client;
using Xunit;

namespace TardisBank.IntegrationTests
{
    public class ApiTests
    {
        const string baseUri = "http://localhost.fiddler:5000/";
        private TardisBankClient client = new TardisBankClient(new Uri(baseUri), new HttpClient());

        [Fact]
        public async Task Test1()
        {
            var result = await client.GetHome();
            Assert.Collection(result.Links, x => {});
        }
    }
}
