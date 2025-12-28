using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Mappings;
using AccountingSystem.Domain.Entities;
using AutoMapper;
using Xunit;

namespace AccountingSystem.Application.Tests
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            // הגדרת AutoMapper לטסט
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void MappingProfile_ShouldBeValid()
        {
            // בדיקה שכל המיפויים תקינים
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            // אם יש בעיה - זה יזרק Exception
            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMap_Company_To_CompanyDto()
        {
            // Arrange
            var company = new Company
            {
                Id = 1,
                Name = "חברת בדיקה בע\"מ",
                Taxid = "123456789",
                Email = "test@company.com",
                Firm = new Accountingfirm
                {
                    Id = 1,
                    Name = "משרד בדיקה"
                }
            };

            // Act
            var dto = _mapper.Map<CompanyDto>(company);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(company.Id, dto.Id);
            Assert.Equal(company.Name, dto.Name);
            Assert.Equal(company.Taxid, dto.TaxId);
            Assert.Equal("משרד בדיקה", dto.FirmName);  
        }

        [Fact]
        public void ShouldMap_CreateCompanyDto_To_Company()
        {
            // Arrange
            var createDto = new CreateCompanyDto
            {
                Name = "חברה חדשה בע\"מ",
                TaxId = "987654321",
                FirmId = 1,
                Email = "new@company.com"
            };

            // Act
            var company = _mapper.Map<Company>(createDto);

            // Assert
            Assert.NotNull(company);
            Assert.Equal(0, company.Id);  // צריך להיות 0 (Ignored)
            Assert.Equal(createDto.Name, company.Name);
            Assert.Equal(createDto.TaxId, company.Taxid);
            Assert.Equal(createDto.FirmId, company.Firmid);
        }

        [Fact]
        public void ShouldMap_Worker_To_WorkerDto_WithFullName()
        {
            // Arrange
            var worker = new Worker
            {
                Id = 1,
                Firstname = "יוסי",
                Lastname = "כהן",
                Email = "yossi@example.com",
                Firm = new Accountingfirm { Name = "משרד XYZ" },
                Role = new Role { Name = "Manager" }
            };

            // Act
            var dto = _mapper.Map<WorkerDto>(worker);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal("יוסי כהן", dto.FullName);  // בדיקת שילוב שדות!
            Assert.Equal("משרד XYZ", dto.FirmName);
            Assert.Equal("Manager", dto.RoleName);
        }
    }
}