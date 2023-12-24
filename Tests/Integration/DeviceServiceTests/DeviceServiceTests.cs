// using Microsoft.AspNetCore.Mvc.Testing;

// namespace DeviceServiceTests;

// public class DeviceServiceTests
//     : IClassFixture<CustomWebApplicationFactory<Program>>
// {
//     private const string DeviceUrl = "api/Device";

//     private readonly CustomWebApplicationFactory<Program> _factory;

//     public DeviceServiceTests(CustomWebApplicationFactory<Program> factory)
//     {
//         _factory = factory;
//     }

//     // [Fact]
//     public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
//     {
//         // Arrange
//         var client = _factory.CreateClient(
//             new WebApplicationFactoryClientOptions
//         {
//             AllowAutoRedirect = false
//         });

//         // Act
//         var response = await client.GetAsync(DeviceUrl);

//         // Assert
//         response.EnsureSuccessStatusCode(); // Status Code 200-299
//         Assert.Equal(
//             "application/json; charset=utf-8",
//             actual: response.Content.Headers.ContentType.ToString());
//     }
// }