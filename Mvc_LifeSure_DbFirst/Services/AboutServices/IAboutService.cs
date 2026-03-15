using Mvc_LifeSure_DbFirst.Dtos.AboutDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.AboutServices
{
    public interface IAboutService
    {
        List<ResultAboutDto> GetAll();
        UpdateAboutDto GetById(int id);
        void Create(CreateAboutDto create);
        void Update(UpdateAboutDto update);
        void Delete(int id);
    }
}
