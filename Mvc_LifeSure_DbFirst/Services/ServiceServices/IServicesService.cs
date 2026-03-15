using Mvc_LifeSure_DbFirst.Dtos.ServiceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.ServiceServices
{
    public interface IServicesService
    {
        List<ResultServicesDto> GetAll();
        UpdateServicesDto GetById(int id);
        void Update(UpdateServicesDto update);
        void Create(CreateServicesDto create);
        void Delete(int id);
    }
}
