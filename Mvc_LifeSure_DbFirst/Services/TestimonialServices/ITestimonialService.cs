using Mvc_LifeSure_DbFirst.Dtos.TestimonialDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.TestimonialServices
{
    public interface ITestimonialService
    {
        List<ResultTestimonialDto> GetAll();
        UpdateTestimonialDto GetById(int id);
        void Create(CreateTestimonialDto create);
        void Update(UpdateTestimonialDto update);
        void Delete(int id);
    }
}
