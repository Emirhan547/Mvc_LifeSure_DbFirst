using Mvc_LifeSure_DbFirst.Dtos.AppUserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.AppUserServices
{
    public interface IAppUserService
    {
        Task<ResultAppUserDto> GetUserByIdAsync(string id);
        Task<List<ResultAppUserDto>> GetAllUsersAsync();
        Task<List<ResultAppUserDto>> GetUsersByCityAsync(string city);
        Task<ResultAppUserDto> GetUserWithPoliciesAsync(string id);
        Task UpdateUserAsync(UpdateAppUserDto updateDto);
        Task DeleteUserAsync(string id);
        Task<bool> ToggleUserStatusAsync(string id);
        Task<int> GetTotalUserCountAsync();
    }
}
