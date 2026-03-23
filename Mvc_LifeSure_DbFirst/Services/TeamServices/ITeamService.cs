using Mvc_LifeSure_DbFirst.Dtos.TeamDtos;
using System.Collections.Generic;

namespace Mvc_LifeSure_DbFirst.Services.TeamServices
{
    public interface ITeamService
    {
        List<ResultTeamDto> GetAll();
        UpdateTeamDto GetById(int id);  
        void Create(CreateTeamDto create);
        void Update(UpdateTeamDto update);
        void Delete(int id);
    }
}
