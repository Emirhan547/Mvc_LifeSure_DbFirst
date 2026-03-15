using Mvc_LifeSure_DbFirst.Dtos.SliderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.SliderServices
{
    public interface ISliderService
    {
        void Create(CreateSliderDto create);
        void Update(UpdateSliderDto update);
        List<ResultSliderDto> GetAll();
        UpdateSliderDto GetById(int id);
        void Delete(int id);
    }
}
