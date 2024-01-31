using System.Runtime.InteropServices;
using FluentAssertions;
using Moq;
using Utils.Daily;
using Utils.Providers;

namespace UtilsTests;

public class RandomDailyTests
{
    private readonly Mock<IDateTimeProvider> dateTimeProviderMock;

    private readonly Mock<IFileProvider> fileProviderMock;

    private readonly Mock<IPathProvider> pathProviderMock;

    private readonly Mock<IDirectoryProvider> directoryProviderMock;

    public RandomDailyTests()
    {
        dateTimeProviderMock = new Mock<IDateTimeProvider>();
        fileProviderMock = new Mock<IFileProvider>();
        pathProviderMock = new Mock<IPathProvider>();
        directoryProviderMock = new Mock<IDirectoryProvider>();
    }

    [Theory]
    [InlineData(2024, 1, 31)]
    [InlineData(2024, 2, 9)]
    [InlineData(2543, 10, 27)]
    public void GetFileNameTest_Daily(int year, int month, int day)
    {
        var now = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
        dateTimeProviderMock.Setup(p => p.Now).Returns(now);
        var randomDaily = new RandomDaily(new DailyCliOptions()
        {
            ConfigFile = "config.json"
        }, new RandomDailyConfiguration(),
        dateTimeProviderMock.Object,
        pathProviderMock.Object,
        fileProviderMock.Object,
        directoryProviderMock.Object);
        var fileName = randomDaily.GetFileName(FeedbackArchive.Daily);
        fileName.Should().NotBeNullOrEmpty();
        fileName.Should().Contain(now.ToString("yyyy_MM_dd"));
    }

    [Theory]
    [InlineData(2024, 1, 31, 2024, 1, 29)]
    [InlineData(2024, 2, 9, 2024, 2, 5)]
    [InlineData(2004, 1, 1, 2003, 12, 29)]
    [InlineData(2001, 1, 1, 2001, 01, 01)]
    public void GetFileNameTest_Weekly(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
    {
        var now = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
        dateTimeProviderMock.Setup(p => p.Now).Returns(now);
        var randomDaily = new RandomDaily(new DailyCliOptions()
        {
            ConfigFile = "config.json"
        }, new RandomDailyConfiguration(),
        dateTimeProviderMock.Object,
        pathProviderMock.Object,
        fileProviderMock.Object,
        directoryProviderMock.Object);
        var fileName = randomDaily.GetFileName(FeedbackArchive.Weekly);
        var expected = new DateTime(expectedYear, expectedMonth, expectedDay, 0, 0, 0, DateTimeKind.Local);
        fileName.Should().NotBeNullOrEmpty();
        fileName.Should().Contain(expected.ToString("yyyy_MM_dd"));
    }
}