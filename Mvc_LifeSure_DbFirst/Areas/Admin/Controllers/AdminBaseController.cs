using Microsoft.AspNet.Identity;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBaseController : Controller
    {
        protected readonly IAdminLogService _logService;

        public AdminBaseController(IAdminLogService logService)
        {
            _logService = logService;
        }

        protected void LogAction(string action, string actionType, string tableName, int? recordId = null)
        {
            var userId = User.Identity.GetUserId();
            _logService.LogAdminAction(userId, action, actionType, tableName, recordId);
        }
    }
}