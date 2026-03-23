using Mvc_LifeSure_DbFirst.Dtos.ContactMessageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.ContactMessageServices
{
    public interface IContactMessageService
    {
        List<ResultContactMessageDto> GetAll();
        ResultContactMessageDto GetById(int id);
        void Create(CreateContactMessageDto createDto);
        void Delete(int id);
        Task ProcessMessageAsync(int id); // Kategorize et ve otomatik cevap gönder
        Task ReprocessMessageAsync(int id); // Yeniden işleme
        List<ResultContactMessageDto> GetUnrepliedMessages();
        Dictionary<string, int> GetMessageStatsByCategory();
    }
}
