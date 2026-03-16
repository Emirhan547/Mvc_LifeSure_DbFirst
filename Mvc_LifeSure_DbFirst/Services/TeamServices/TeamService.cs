using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.ServiceDtos;
using Mvc_LifeSure_DbFirst.Dtos.TeamDtos;
using Mvc_LifeSure_DbFirst.Repositories.TeamRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Services.TeamServices
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repository;
        private readonly IValidator<CreateTeamDto> _createValidators;
        private readonly IValidator<UpdateTeamDto> _updateValidators;

        public TeamService(ITeamRepository repository, IValidator<CreateTeamDto> createValidators, IValidator<UpdateTeamDto> updateValidators)
        {
            _repository = repository;
            _createValidators = createValidators;
            _updateValidators = updateValidators;
        }

        public void Create(CreateTeamDto create)
        {
           _createValidators.ValidateAndThrow(create);
            var mappedTeams = create.Adapt<Team>();
            _repository.Create(mappedTeams);
        }

        public void Delete(int id)
        {
            var teams = _repository.GetById(id);
            if(teams == null)
            {
                throw new KeyNotFoundException("Team Not Found");
            }
            _repository.Delete(teams);
        }

        public List<ResultTeamDto> GetAll()
        {
            var teams=_repository.GetAll();
            return teams.Adapt<List<ResultTeamDto>>();
        }

        public UpdateServicesDto GetById(int id)
        {
            var teams = _repository.GetById(id);
            if( teams == null)
            {
                throw new KeyNotFoundException("Team Not Found");
            }
            return teams.Adapt<UpdateServicesDto>();
        }

        public void Update(UpdateTeamDto update)
        {
            _updateValidators.ValidateAndThrow(update);
            var teams=_repository.GetById(update.Id);
            if(teams ==null)
            {
                throw new KeyNotFoundException("Team Not Found");
            }
            update.Adapt(teams);
            _repository.Update(teams);
        }
    }
}