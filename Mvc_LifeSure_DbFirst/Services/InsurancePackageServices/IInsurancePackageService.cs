using Mvc_LifeSure_DbFirst.Dtos.InsurancePackageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.InsurancePackageServices
{
    public interface IInsurancePackageService
    {
        List<ResultInsurancePackageDto> GetAll();
        ResultInsurancePackageDto GetById(int id);
        ResultInsurancePackageDto GetPackageWithPolicies(int id);
        List<ResultInsurancePackageDto> GetActivePackages();
        void Create(CreateInsurancePackageDto createDto);
        void Update(UpdateInsurancePackageDto updateDto);
        void Delete(int id);
        void TogglePackageStatus(int id);
    }
}