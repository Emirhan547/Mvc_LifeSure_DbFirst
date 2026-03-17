using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Repositories.ContactMessageRepositories
{
    public interface IContactMessageRepository : IRepository<ContactMessage>
    {
        List<ContactMessage> GetUnrepliedMessages();
        List<ContactMessage> GetMessagesByCategory(string category);
        List<IGrouping<string, ContactMessage>> GetMessagesGroupedByCategory();
    }
}
