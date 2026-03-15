using Mvc_LifeSure_DbFirst.Dtos.AboutDtos;
using Mvc_LifeSure_DbFirst.Dtos.BlogDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.BlogServices
{
    public interface IBlogService
    {
        List<ResultBlogDto> GetAll();
        UpdateBlogDto GetById (int id);
        void Create(CreateBlogDto create);
        void Update(UpdateBlogDto update);
        void Delete(int id);
    }
}
