using FluentValidation;
using Mapster;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.AppUserDtos;
using Mvc_LifeSure_DbFirst.Repositories.PolicyRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Services.AppUserServices
{
    public class AppUserService : IAppUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser, string> _userManager;
        private readonly IPolicyRepository _policyRepository;
        private readonly IValidator<UpdateAppUserDto> _updateValidator;

        public AppUserService(
            AppDbContext context,
            UserManager<AppUser, string> userManager,
            IPolicyRepository policyRepository,
            IValidator<UpdateAppUserDto> updateValidator)
        {
            _context = context;
            _userManager = userManager;
            _policyRepository = policyRepository;
            _updateValidator = updateValidator;
        }

        public async Task<List<ResultAppUserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var result = new List<ResultAppUserDto>();

            foreach (var user in users)
            {
                var policyCount = await _context.Policies.CountAsync(p => p.UserId == user.Id);
                var userDto = user.Adapt<ResultAppUserDto>();
                userDto.PolicyCount = policyCount;
                result.Add(userDto);
            }

            return result;
        }

        public async Task<ResultAppUserDto> GetUserByIdAsync(string id)
        {
            var user =  _context.Users.Find(id);
            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            var policyCount = await _context.Policies.CountAsync(p => p.UserId == user.Id);
            var userDto = user.Adapt<ResultAppUserDto>();
            userDto.PolicyCount = policyCount;

            return userDto;
        }

        public async Task<ResultAppUserDto> GetUserWithPoliciesAsync(string id)
        {
            var user = await _context.Users
                .Include(u => u.Policies)
                .Include(u => u.Policies.Select(p => p.InsurancePackage))
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            var userDto = user.Adapt<ResultAppUserDto>();
            userDto.PolicyCount = user.Policies?.Count ?? 0;

            return userDto;
        }

        public async Task<List<ResultAppUserDto>> GetUsersByCityAsync(string city)
        {
            var users = await _context.Users
                .Where(u => u.City == city)
                .ToListAsync();

            var result = new List<ResultAppUserDto>();

            foreach (var user in users)
            {
                var policyCount = await _context.Policies.CountAsync(p => p.UserId == user.Id);
                var userDto = user.Adapt<ResultAppUserDto>();
                userDto.PolicyCount = policyCount;
                result.Add(userDto);
            }

            return result;
        }

        public async Task UpdateUserAsync(UpdateAppUserDto updateDto)
        {
            // Validasyon
            await _updateValidator.ValidateAndThrowAsync(updateDto);

            var user = await _userManager.FindByIdAsync(updateDto.Id);
            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            // Güncelleme
            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.Email = updateDto.Email;
            user.UserName = updateDto.UserName;
            user.City = updateDto.City;
            user.BirthDate = updateDto.BirthDate;
            user.PhoneNumber = updateDto.PhoneNumber;
            user.IsActive = updateDto.IsActive;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors));
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            // Kullanıcının poliçelerini kontrol et
            var hasPolicies = await _context.Policies.AnyAsync(p => p.UserId == id);
            if (hasPolicies)
                throw new InvalidOperationException("Bu kullanıcının aktif poliçeleri bulunmaktadır. Önce poliçeleri silin veya başka kullanıcıya aktarın.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors));
        }

        public async Task<bool> ToggleUserStatusAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            user.IsActive = !user.IsActive;
            var result = await _userManager.UpdateAsync(user);

            return user.IsActive;
        }

        public async Task<int> GetTotalUserCountAsync()
        {
            return await _context.Users.CountAsync();
        }
    }
}