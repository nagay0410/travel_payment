using Application.Common.Mappings;
using Application.Trips.Queries.GetTripById;
using Application.Trips.Queries;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Trips.Queries.GetTripById;

/// <summary>
/// 旅行詳細取得機能の単体テスト（TDD: Red段階）。
/// </summary>
public class GetTripByIdQueryTests
{
    private readonly Mock<ITripRepository> _tripRepositoryMock;
    private readonly AutoMapper.IMapper _mapper;

    public GetTripByIdQueryTests()
    {
        _tripRepositoryMock = new Mock<ITripRepository>();
        
        var config = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_ReturnTrip_When_TripExists()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var trip = Trip.Create(tripId, "test trip", DateTime.Today, DateTime.Today.AddDays(1), Guid.NewGuid());
        _tripRepositoryMock.Setup(x => x.GetByIdAsync(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trip);

        var query = new GetTripByIdQuery(tripId);
        var handler = new GetTripByIdQueryHandler(_tripRepositoryMock.Object, _mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(tripId);
        result.TripName.Should().Be("test trip");
    }
}
