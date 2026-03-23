using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.TeamDtos;
using Mvc_LifeSure_DbFirst.Repositories.TeamRepositories;
using System.Collections.Generic;

namespace Mvc_LifeSure_DbFirst.Services.TeamServices
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<CreateTeamDto> _createValidator;
        private readonly IValidator<UpdateTeamDto> _updateValidator;

        public TeamService(
            ITeamRepository teamRepository,
            IValidator<CreateTeamDto> createValidator,
            IValidator<UpdateTeamDto> updateValidator)
        {
            _teamRepository = teamRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public List<ResultTeamDto> GetAll()
        {
            return _teamRepository.GetAll().Adapt<List<ResultTeamDto>>();
        }

        public UpdateTeamDto GetById(int id)
        {
            var team = _teamRepository.GetById(id)
                ?? throw new KeyNotFoundException($"Ekip üyesi bulunamadı (Id: {id})");
            return team.Adapt<UpdateTeamDto>();
        }

        public void Create(CreateTeamDto create)
        {
            _createValidator.ValidateAndThrow(create);
            _teamRepository.Create(create.Adapt<Team>());
        }

        public void Update(UpdateTeamDto update)
        {
            _updateValidator.ValidateAndThrow(update);
            var team = _teamRepository.GetById(update.Id)
                ?? throw new KeyNotFoundException($"Ekip üyesi bulunamadı (Id: {update.Id})");
            update.Adapt(team);
            _teamRepository.Update(team);
        }

        public void Delete(int id)
        {
            var team = _teamRepository.GetById(id)
                ?? throw new KeyNotFoundException($"Ekip üyesi bulunamadı (Id: {id})");
            _teamRepository.Delete(team);
        }
    }
}