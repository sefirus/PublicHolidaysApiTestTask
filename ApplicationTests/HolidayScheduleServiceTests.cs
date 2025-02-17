using System.Linq.Expressions;
using Application.Services;
using ApplicationTests.Fixtures;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ApplicationTests;

public class HolidayScheduleServiceTests
{
    [Fact]
    public async Task ProcessHolidayChunkAsync_CountryExistsAndChunkNotProcessed_ShouldProcessAndSaveData()
    {
        // Arrange
        int testChunkSize = 3;
        int targetYear = 2020;
        // With chunks starting at 2000 and chunkSize = 3:
        // offset = 2020 - 2000 = 20; chunkIndex = 20/3 = 6 (integer division);
        // chunkStartYear = 2000 + 6*3 = 2018, chunkEndYear = 2018 + 3 - 1 = 2020.
        int expectedChunkStartYear = 2018;
        int expectedChunkEndYear = 2020;

        // In-memory configuration with ScheduleProcessingChunkSize = 3.
        var inMemorySettings = new Dictionary<string, string>
        {
            { "ScheduleProcessingChunkSize", testChunkSize.ToString() }
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Set up a Country record for Ukraine.
        var country = new Country
        {
            Id = Guid.NewGuid(),
            CountryCode3Digit = "UKR",
            CountryCode2Digit = "UA",
            FullName = "Ukraine",
            FromDate = new DateTime(2000, 1, 1),
            ToDate = new DateTime(2050, 12, 31)
        };

        // Use the fixture to get sample external holiday data.
        // (The fixture returns data for years 2020, 2021, 2022.)
        // In this test, only the data falling within 2018–2020 (i.e. 2020 data) will be processed.
        var externalHolidays = ExternalHolidayDataFixture.GetSampleData();

        // Create mocks for dependencies.
        var mockHolidayRepo = new Mock<IRepository<Holiday>>();
        var mockChunkRepo = new Mock<IRepository<ProcessedHolidaysChunk>>();
        var mockExternalApiService = new Mock<IExternalHolidayApiService>();
        var mockCountryService = new Mock<ICountryService>();

        // Setup: CountryService returns our Ukraine record.
        mockCountryService.Setup(cs => cs.GetCountryByCode(It.Is<string>(s => s.Equals("UKR", StringComparison.OrdinalIgnoreCase))))
                          .ReturnsAsync(country);

        // Setup: ProcessedHolidaysChunk repository returns an empty list (i.e. chunk not yet processed).
        mockChunkRepo.Setup(repo => repo.QueryAsync(
                It.IsAny<Expression<Func<ProcessedHolidaysChunk, bool>>>(),
                null, null, null, null, true))
            .ReturnsAsync(new List<ProcessedHolidaysChunk>());

        // Setup: External API service returns our fixture data when called with boundaries.
        mockExternalApiService.Setup(api => api.GetHolidays(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync(externalHolidays);

        // Setup: Holiday repository QueryAsync returns an empty list (no existing holidays).
        mockHolidayRepo.Setup(repo => repo.QueryAsync(
                It.IsAny<Expression<Func<Holiday, bool>>>(),
                null, null, null, null, false))
            .ReturnsAsync(new List<Holiday>());

        // For testing purposes, also set up Insert and SaveChangesAsync to complete.
        mockHolidayRepo.Setup(repo => repo.Insert(It.IsAny<Holiday>()))
                       .Returns(Task.CompletedTask);
        mockChunkRepo.Setup(repo => repo.Insert(It.IsAny<ProcessedHolidaysChunk>()))
                     .Returns(Task.CompletedTask);
        mockHolidayRepo.Setup(repo => repo.SaveChangesAsync())
                       .Returns(Task.CompletedTask);
        mockChunkRepo.Setup(repo => repo.SaveChangesAsync())
                     .Returns(Task.CompletedTask);

        // Create the service instance under test.
        var service = new HolidayScheduleService(
            mockHolidayRepo.Object,
            mockChunkRepo.Object,
            mockExternalApiService.Object,
            configuration,
            mockCountryService.Object);

        // Act
        await service.ProcessHolidayChunkAsync("UKR", targetYear, testChunkSize);

        // Assert

        // Verify that CalculateEffectiveChunkBoundaries yields expected boundaries.
        // This is indirectly verified by checking the external API call parameters.
        mockExternalApiService.Verify(api => api.GetHolidays(
            It.Is<string>(s => s.Equals("UKR", StringComparison.OrdinalIgnoreCase)),
            expectedChunkStartYear,
            expectedChunkEndYear),
            Times.Once);

        // Verify that the Holiday repository's Insert method is called at least once (for new Holiday records).
        mockHolidayRepo.Verify(repo => repo.Insert(It.IsAny<Holiday>()), Times.AtLeastOnce);

        // Verify that SaveChangesAsync is called on the Holiday repository.
        mockHolidayRepo.Verify(repo => repo.SaveChangesAsync(), Times.AtLeastOnce);

        // Verify that a ProcessedHolidaysChunk is inserted with the correct boundaries.
        mockChunkRepo.Verify(repo => repo.Insert(It.Is<ProcessedHolidaysChunk>(p =>
            p.ChunkStartYear == expectedChunkStartYear &&
            p.ChunkEndYear == expectedChunkEndYear &&
            p.CountryId == country.Id)),
            Times.Once);

        // Verify that SaveChangesAsync is called on the chunk repository.
        mockChunkRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
    
}