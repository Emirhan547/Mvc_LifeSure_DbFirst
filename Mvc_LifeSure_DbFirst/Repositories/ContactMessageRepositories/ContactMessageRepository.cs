using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.ContactMessageRepositories
{
    public class ContactMessageRepository : GenericRepository<ContactMessage>, IContactMessageRepository
    {
        private readonly AppDbContext _context;

        public ContactMessageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public List<ContactMessage> GetUnrepliedMessages()
        {
            return _context.ContactMessages
                .Where(x => !x.IsReplied)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        public List<ContactMessage> GetMessagesByCategory(string category)
        {
            return _context.ContactMessages
                .Where(x => x.Category == category)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        public List<IGrouping<string, ContactMessage>> GetMessagesGroupedByCategory()
        {
            return _context.ContactMessages
                .GroupBy(x => x.Category)
                .ToList();
        }
    }
}