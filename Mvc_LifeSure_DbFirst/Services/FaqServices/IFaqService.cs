using Mvc_LifeSure_DbFirst.Dtos.FaqDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.FaqServices
{
    public interface IFaqService
    {
        List<ResultFaqDto> GetAll();
        UpdateFaqDto GetById(int id);
        void Create(CreateFaqDto create);
        void Update(UpdateFaqDto update);
        void Delete(int id);
    }
}
