using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract
{
    public interface IAuthRepository
    {
        Task<AppIdentityUser> Register(AppIdentityUser user, string password);
        Task<AppIdentityUser> AddUserToRole(AppIdentityUser user, string roleName);
        Task<AppIdentityUser> ConfirmEmail(AppIdentityUser user, string code);
        Task<string> GenerateEmailConfirmToken(AppIdentityUser user);
        Task<AppIdentityUser> Login(string username, string password);
        Task<AppIdentityUser> UpdateUser(AppIdentityUser user);
        Task<AppIdentityUser> DeleteUser(AppIdentityUser user);
        Task<AppIdentityUser> FindByUsername(string userName);
        Task<AppIdentityUser> FindUserById(string id);
        Task<AppIdentityUser> FindUserByEmail(string email);
        Task<List<AppIdentityUser>> GetUsersByRole(string roleName);
        Task<AppIdentityRole> CreateRole(string roleName);
        List<AppIdentityRole> GetRoles();
        Task<AppIdentityRole> GetRoleByIdAsync(string id);
        Task<AppIdentityRole> GetRoleByNameAsync(string name);
        Task<AppIdentityRole> DeleteRole(AppIdentityRole role);
        Task<AppIdentityRole> UpdateRole(AppIdentityRole role);
    }
}
