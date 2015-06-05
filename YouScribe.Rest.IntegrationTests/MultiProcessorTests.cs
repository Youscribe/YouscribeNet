using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YouScribe.Rest.IntegrationTests.Helpers;

namespace YouScribe.Rest.IntegrationTests
{
    public class MultiProcessorTests
    {
        [Fact]
        public void WhenDoingMultipleRequestInParallel_ThenItsOk()
        {
            Parallel.ForEach(Enumerable.Range(0, 100), new ParallelOptions() { MaxDegreeOfParallelism = 100 }, (c) =>
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                using (var dClient = client.clientFactory())
                {
                    var id = Guid.NewGuid().ToString();
                    dClient.Client.BaseClient.DefaultRequestHeaders.Add("test", id);
                    dClient.Client.GetAsync(TestHelpers.BaseUrl);
                    Assert.Equal(id, dClient.Client.BaseClient.DefaultRequestHeaders.GetValues("test").First());
                }
            });
        }
    }
}
