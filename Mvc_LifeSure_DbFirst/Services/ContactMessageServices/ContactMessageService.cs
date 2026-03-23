using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.ContactMessageDtos;
using Mvc_LifeSure_DbFirst.Repositories.ContactMessageRepositories;
using Mvc_LifeSure_DbFirst.Services.AIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Services.ContactMessageServices
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IContactMessageRepository _messageRepository;
        private readonly IValidator<CreateContactMessageDto> _createValidator;
        private readonly IHuggingFaceService _huggingFaceService;
        private readonly IChatGPTService _chatGPTService;
        private readonly IMailService _mailService;

        public ContactMessageService(
            IContactMessageRepository messageRepository,
            IValidator<CreateContactMessageDto> createValidator,
            IHuggingFaceService huggingFaceService,
            IChatGPTService chatGPTService,
            IMailService mailService)
        {
            _messageRepository = messageRepository;
            _createValidator = createValidator;
            _huggingFaceService = huggingFaceService;
            _chatGPTService = chatGPTService;
            _mailService = mailService;
        }

        public List<ResultContactMessageDto> GetAll()
        {
            var messages = _messageRepository.GetAll();
            return messages.Adapt<List<ResultContactMessageDto>>();
        }

        public ResultContactMessageDto GetById(int id)
        {
            var message = _messageRepository.GetById(id);
            if (message == null)
                throw new KeyNotFoundException("Mesaj bulunamadı");

            return message.Adapt<ResultContactMessageDto>();
        }

        public void Create(CreateContactMessageDto createDto)
        {
            _createValidator.ValidateAndThrow(createDto);

            var message = createDto.Adapt<ContactMessage>();
            message.CreatedAt = DateTime.Now;
            message.IsReplied = false;

            _messageRepository.Create(message);

            // Mesajı otomatik işleme koy
            Task.Run(async () => await ProcessMessageAsync(message.Id));
        }

        public void Delete(int id)
        {
            var message = _messageRepository.GetById(id);
            if (message == null)
                throw new KeyNotFoundException("Mesaj bulunamadı");

            _messageRepository.Delete(message);
        }

        public List<ResultContactMessageDto> GetUnrepliedMessages()
        {
            var messages = _messageRepository.GetUnrepliedMessages();
            return messages.Adapt<List<ResultContactMessageDto>>();
        }

        public Dictionary<string, int> GetMessageStatsByCategory()
        {
            var messages = _messageRepository.GetAll();
            return messages
                .GroupBy(m => m.Category != null ? m.Category : "Kategorisiz")
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task ProcessMessageAsync(int id)
        {
            try
            {
                var message = _messageRepository.GetById(id);
                if (message == null || message.IsReplied)
                    return;

                // 1. HuggingFace ile kategorize et
                var category = await _huggingFaceService.ClassifyMessageAsync(message.Message);
                message.Category = category;

                // 2. ChatGPT ile otomatik cevap oluştur
                var reply = await _chatGPTService.GenerateReplyAsync(message.Message, category, message.Name);
                message.AutoReply = reply;

                // 3. Mail ile gönder
                string subject;
                if (category == "Teşekkür") subject = "Teşekkür Mesajınız Hakkında";
                else if (category == "Şikayet") subject = "Şikayetiniz Hakkında";
                else if (category == "Rica") subject = "Talebiniz Hakkında";
                else if (category == "Bilgi Talebi") subject = "Bilgi Talebiniz Hakkında";
                else if (category == "Destek") subject = "Destek Talebiniz Hakkında";
                else if (category == "Geri Bildirim") subject = "Geri Bildiriminiz Hakkında";
                else subject = "Mesajınız Hakkında";

                await _mailService.SendEmailAsync(message.Email, subject, reply);

                message.IsReplied = true;
                _messageRepository.Update(message);
            }
            catch (Exception ex)
            {
                var message = _messageRepository.GetById(id);
                if (message != null)
                {
                    message.Category = "İşlenemedi";
                    message.AutoReply = "Üzgünüz, mesajınız işlenirken bir hata oluştu. En kısa sürede manuel olarak dönüş yapacağız.";
                    _messageRepository.Update(message);
                }
            }
        }

        public async Task ReprocessMessageAsync(int id)
        {
            var message = _messageRepository.GetById(id);
            if (message == null)
                throw new KeyNotFoundException("Mesaj bulunamadı");

            // Önce cevap ve kategori sıfırlansın
            message.IsReplied = false;
            message.AutoReply = null;
            message.Category = null;
            _messageRepository.Update(message);

            await ProcessMessageAsync(id);
        }
    }
}