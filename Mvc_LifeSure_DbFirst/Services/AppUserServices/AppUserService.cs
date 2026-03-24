using FluentValidation;
using Mapster;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Data.Identity;
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
        private readonly IValidator<UpdateAppUserDto> _updateValidator;

        public AppUserService(
     AppDbContext context,
            IValidator<UpdateAppUserDto> updateValidator,
            AppUserManager userManager)
        {
            _context = context; 
            _userManager = userManager;
            _updateValidator = updateValidator;
        }

        public async Task<List<ResultAppUserDto>> GetAllUsersAsync()
        {
            return await ProjectUsers(_context.Users)
                .ToListAsync();
        }

        public async Task<ResultAppUserDto> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
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
     .Include("Policies.InsurancePackage")
     .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            var userDto = user.Adapt<ResultAppUserDto>();
            userDto.PolicyCount = user.Policies?.Count ?? 0;

            return userDto;
        }

        public async Task<List<ResultAppUserDto>> GetUsersByCityAsync(string city)
        {
            return await ProjectUsers(_context.Users.Where(user => user.City == city))
                .ToListAsync();

           
          
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
            EnsureIdentityResult(result);
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
            EnsureIdentityResult(result);
        }

        public async Task<bool> ToggleUserStatusAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            user.IsActive = !user.IsActive;
            var result = await _userManager.UpdateAsync(user);
            EnsureIdentityResult(result);

            return user.IsActive;
        }

        public async Task<int> GetTotalUserCountAsync()
        {
            return await _context.Users.CountAsync();
        }
        public async Task<IList<string>> GetUserRolesAsync(string id)
        {
            var user = await EnsureUserExistsAsync(id);
            return await _userManager.GetRolesAsync(user.Id);
        }

        public async Task AddUserToRoleAsync(string userId, string role)
        {
            await EnsureRoleAssignmentInputAsync(userId, role);
            var result = await _userManager.AddToRoleAsync(userId, role);
            EnsureIdentityResult(result);
        }

        public async Task RemoveUserFromRoleAsync(string userId, string role)
        {
            await EnsureRoleAssignmentInputAsync(userId, role);
            var result = await _userManager.RemoveFromRoleAsync(userId, role);
            EnsureIdentityResult(result);
        }

        private IQueryable<ResultAppUserDto> ProjectUsers(IQueryable<AppUser> users)
        {
            return users.Select(user => new ResultAppUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                PolicyCount = _context.Policies.Count(policy => policy.UserId == user.Id)
            });
        }

        private async Task<AppUser> EnsureUserExistsAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            return user;
        }

        private async Task EnsureRoleAssignmentInputAsync(string userId, string role)
        {
            await EnsureUserExistsAsync(userId);

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Geçersiz rol bilgisi.");
        }

        private static void EnsureIdentityResult(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors));
        }
    }
}