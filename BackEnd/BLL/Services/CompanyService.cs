using AutoMapper;
using BLL.DTO;
using DAL.Entity;
using DAL.Repository;

namespace BLL.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<List<CompanyDto>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return _mapper.Map<List<CompanyDto>>(companies);
        }

        public async Task<CompanyDto?> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return null;
            }
            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<List<CompanyDto>> GetByUserIdAsync(int userId)
        {
            var companies = await _companyRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<CompanyDto>>(companies);
        }

        public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto)
        {
            var company = new CompanyEntity
            {
                Name = dto.Name,
                Description = dto.Description,
                Website = dto.Website,
                Location = dto.Location,
                LogoUrl = dto.LogoUrl,
                UserId = dto.UserId
            };
            var createdCompany = await _companyRepository.CreateAsync(company);
            return _mapper.Map<CompanyDto>(createdCompany);
        }

        public async Task<bool> UpdateAsync(int id, UpdateCompanyDto dto)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return false;
            }
            company.Name = dto.Name;
            company.Description = dto.Description;
            company.Website = dto.Website;
            company.Location = dto.Location;
            company.LogoUrl = dto.LogoUrl;
            await _companyRepository.UpdateAsync(company);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return false;
            }
            var companyEntity = new CompanyEntity
            {
                Id = company.Id
            };
            await _companyRepository.DeleteAsync(companyEntity);
            return true;
        }
    }
}
