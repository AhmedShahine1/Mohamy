using AutoMapper;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.BusinessLayer.Services;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.RepositoryLayer.Interfaces;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace TMohamy
{
    public class MainConsultingServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileHandling> _fileHandlingMock;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly MainConsultingService _service;

        public MainConsultingServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _fileHandlingMock = new Mock<IFileHandling>();
            _accountServiceMock = new Mock<IAccountService>();
            _service = new MainConsultingService(_unitOfWorkMock.Object, _mapperMock.Object, _fileHandlingMock.Object, _accountServiceMock.Object);
        }

        [Theory]
        [MemberData(nameof(GetAllAsyncTestData))]
        public async Task GetAllAsync_ShouldReturnExpectedListOfMainConsultingDTO(
            List<mainConsulting> mainConsultings,
            List<MainConsultingDTO> expectedDtos)
        {
            // Arrange
            _unitOfWorkMock
                .Setup(u => u.MainConsultingRepository.GetAllAsync(
                    It.IsAny<Func<IQueryable<mainConsulting>, IIncludableQueryable<mainConsulting, object>>>(),
                    It.IsAny<Func<IQueryable<mainConsulting>, IOrderedQueryable<mainConsulting>>>()))
                .ReturnsAsync(mainConsultings);

            _fileHandlingMock.Setup(f => f.GetFile(It.IsAny<string>()))
                .ReturnsAsync("iconUrl");

            _mapperMock.Setup(m => m.Map<ICollection<SubConsultingDTO>>(It.IsAny<ICollection<subConsulting>>()))
                .Returns((ICollection<subConsulting> subConsultings) =>
                    subConsultings.Select(sub => new SubConsultingDTO
                    {
                        Id = sub.Id,
                        Name = sub.Name,
                        Description = sub.Description,
                        IconUrl = "iconUrl"
                    }).ToList());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(expectedDtos.Count);

            for (int i = 0; i < result.Count; i++)
            {
                result[i].Id.Should().Be(expectedDtos[i].Id);
                result[i].Name.Should().Be(expectedDtos[i].Name);
                result[i].Description.Should().Be(expectedDtos[i].Description);
                result[i].IconUrl.Should().Be(expectedDtos[i].IconUrl);
            }
        }

        // Test data provider
        public static IEnumerable<object[]> GetAllAsyncTestData()
        {
            yield return new object[]
            {
        new List<mainConsulting>
        {
            new mainConsulting
            {
                Id = "1",
                Name = "Consulting 1",
                Description = "Description 1",
                iconId = "icon1",
                SubConsultings = new List<subConsulting>
                {
                    new subConsulting { Id = "1", Name = "Sub 1", Description = "Sub Desc 1", iconId = "icon2" }
                }
            }
        },
        new List<MainConsultingDTO>
        {
            new MainConsultingDTO
            {
                Id = "1",
                Name = "Consulting 1",
                Description = "Description 1",
                IconUrl = "iconUrl",
                SubConsultings = new List<SubConsultingDTO>
                {
                    new SubConsultingDTO { Id = "1", Name = "Sub 1", Description = "Sub Desc 1", IconUrl = "iconUrl" }
                }
            }
        }
            };

            yield return new object[]
            {
        new List<mainConsulting>(),
        new List<MainConsultingDTO>()
            };
        }

        [Theory]
        [InlineData("1", "Consulting 1", "Description 1", "icon1", true)] // Entity exists
        [InlineData("2", null, null, null, false)] // Entity does not exist
        public async Task GetByIdAsync_ShouldHandleVariousCases(
            string id,
            string expectedName,
            string expectedDescription,
            string expectedIconId,
            bool exists)
        {
            // Arrange
            var mainConsulting = exists
                ? new mainConsulting
                {
                    Id = id,
                    Name = expectedName,
                    Description = expectedDescription,
                    iconId = expectedIconId,
                    SubConsultings = new List<subConsulting>
                    {
                new subConsulting { Id = "1", Name = "Sub 1", Description = "Sub Desc 1", iconId = "icon2" }
                    }
                }
                : null;

            _unitOfWorkMock
                .Setup(u => u.MainConsultingRepository.FindAsync(
                    It.IsAny<Expression<Func<mainConsulting, bool>>>(),
                    It.IsAny<Func<IQueryable<mainConsulting>, IIncludableQueryable<mainConsulting, object>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(mainConsulting);

            _fileHandlingMock.Setup(f => f.GetFile(It.IsAny<string>())).ReturnsAsync("iconUrl");

            _mapperMock.Setup(m => m.Map<ICollection<SubConsultingDTO>>(It.IsAny<ICollection<subConsulting>>()))
                .Returns(new List<SubConsultingDTO>());

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            if (exists)
            {
                result.Should().NotBeNull();
                result.Id.Should().Be(id);
                result.Name.Should().Be(expectedName);
                result.Description.Should().Be(expectedDescription);
                result.IconUrl.Should().Be("iconUrl");
            }
            else
            {
                result.Should().BeNull();
            }
        }

        [Theory]
        [InlineData("1", "To Delete", false, 1)] // ID exists, IsDeleted is false, save succeeds
        [InlineData("2", "Non-existent", true, 0)] // ID does not exist, save fails
        public async Task DeleteAsync_ShouldHandleVariousCases(
            string id,
            string name,
            bool isDeleted,
            int expectedResult)
        {
            // Arrange
            var mainConsulting = isDeleted ? null : new mainConsulting { Id = id, Name = name, IsDeleted = false };

            _unitOfWorkMock.Setup(u => u.MainConsultingRepository.GetByIdAsync(id)).ReturnsAsync(mainConsulting);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            result.Should().Be(expectedResult);

            if (mainConsulting != null)
            {
                mainConsulting.IsDeleted.Should().BeTrue();
            }
        }


    }
}
