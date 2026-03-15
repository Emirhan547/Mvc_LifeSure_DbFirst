using Mvc_LifeSure_DbFirst.Dtos.FeatureDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.FeatureServices
{
    public interface IFeatureService
    {
        void Create(CreateFeatureDto createFeatureDto);
        List<ResultFeatureDto> GetAll();
        UpdateFeatureDto GetById(int id);
        void Delete(int id);
        void Update(UpdateFeatureDto updateFeatureDto);
    }
}
