using Mvc_LifeSure_DbFirst.Dtos.ServiceDtos;
using Mvc_LifeSure_DbFirst.Dtos.TeamDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.TeamServices
{
    public interface ITeamService
    {
        List<ResultTeamDto> GetAll();
        void Create(CreateTeamDto create);
        void Update(UpdateTeamDto update);
        void Delete(int id);
        UpdateServicesDto GetById(int id);
    }
}
