using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.BusinessLayer.Services;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.RepositoryLayer.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace TMohamy
{
    public class SubConsultingServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileHandling> _fileHandlingMock;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IMainConsultingService> _mainConsultingServiceMock;
        private readonly SubConsultingService _service;

        public SubConsultingServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _fileHandlingMock = new Mock<IFileHandling>();
            _accountServiceMock = new Mock<IAccountService>();
            _mainConsultingServiceMock = new Mock<IMainConsultingService>();
            _service = new SubConsultingService(_unitOfWorkMock.Object, _mapperMock.Object, _fileHandlingMock.Object, _accountServiceMock.Object, _mainConsultingServiceMock.Object);
        }

        [Theory]
        [InlineData("1", "SubConsulting 1", "Description 1", "iconUrl")]
        public async Task GetAllAsync_ShouldReturnListOfSubConsultingDTO(string id, string name, string description, string iconUrl)
        {
            // Arrange
            var subConsultings = new List<subConsulting>
            {
                new subConsulting
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    MainConsultingId = "1",
                    iconId = "icon1"
                }
            };

            _unitOfWorkMock
                .Setup(u => u.SubConsultingRepository.GetAllAsync(It.IsAny<Func<IQueryable<subConsulting>, IIncludableQueryable<subConsulting, object>>>(), It.IsAny<Func<IQueryable<subConsulting>, IOrderedQueryable<subConsulting>>>()))
                .ReturnsAsync(subConsultings);

            _fileHandlingMock.Setup(f => f.GetFile(It.IsAny<string>())).ReturnsAsync(iconUrl);

            _mainConsultingServiceMock.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new MainConsultingDTO());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(id);
            result[0].Name.Should().Be(name);
            result[0].Description.Should().Be(description);
            result[0].IconUrl.Should().Be(iconUrl);
        }

        [Theory]
        [InlineData("1", "SubConsulting 1", "Description 1", "iconUrl", true)] // id, name, description, iconUrl, isFound
        [InlineData("1", "", "", "", false)] // id, name, description, iconUrl, isFound
        public async Task GetByIdAsync_ShouldReturnSubConsultingDTOOrNull_BasedOnExistence(
            string id, string name, string description, string iconUrl, bool isFound)
        {
            // Arrange
            subConsulting subConsulting = null;

            if (isFound)
            {
                subConsulting = new subConsulting
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    MainConsultingId = "1",
                    iconId = "icon1"
                };

                _unitOfWorkMock
                    .Setup(u => u.SubConsultingRepository.FindAsync(It.IsAny<Expression<Func<subConsulting, bool>>>(),
                                                                   It.IsAny<Func<IQueryable<subConsulting>, IIncludableQueryable<subConsulting, object>>>(),
                                                                   It.IsAny<bool>()))
                    .ReturnsAsync(subConsulting);

                _fileHandlingMock.Setup(f => f.GetFile(It.IsAny<string>())).ReturnsAsync(iconUrl);
                _mainConsultingServiceMock.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new MainConsultingDTO());
            }
            else
            {
                _unitOfWorkMock
                    .Setup(u => u.SubConsultingRepository.FindAsync(It.IsAny<Expression<Func<subConsulting, bool>>>(),
                                                                   It.IsAny<Func<IQueryable<subConsulting>, IIncludableQueryable<subConsulting, object>>>(),
                                                                   It.IsAny<bool>()))
                    .ReturnsAsync((subConsulting)null);
            }

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            if (isFound)
            {
                result.Should().NotBeNull();
                result.Id.Should().Be(id);
                result.Name.Should().Be(name);
                result.Description.Should().Be(description);
                result.IconUrl.Should().Be(iconUrl);
            }
            else
            {
                result.Should().BeNull();
            }
        }


        [Theory]
        [InlineData("1", true, 1)] // id, isSubConsultingFound, expectedResult
        [InlineData("1", false, 0)] // id, isSubConsultingFound, expectedResult
        public async Task DeleteAsync_ShouldDeleteOrReturnZero_BasedOnExistence(string id, bool isSubConsultingFound, int expectedResult)
        {
            // Arrange
            subConsulting subConsulting = null;
            if (isSubConsultingFound)
            {
                subConsulting = new subConsulting
                {
                    Id = id,
                    Name = "SubConsulting 1",
                    IsDeleted = false
                };

                _unitOfWorkMock
                    .Setup(u => u.SubConsultingRepository.GetByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync(subConsulting);

                _unitOfWorkMock
                    .Setup(u => u.SaveChangesAsync())
                    .ReturnsAsync(1); // 1 indicates successful deletion
            }
            else
            {
                _unitOfWorkMock
                    .Setup(u => u.SubConsultingRepository.GetByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync((subConsulting)null);
            }

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            result.Should().Be(expectedResult);
            if (isSubConsultingFound)
            {
                subConsulting.IsDeleted.Should().BeTrue();
            }
        }

    }
}
