using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.BusinessLogicLayer.Concrete
{
    public class AuthRepository : IAuthRepository
    {
        private UserManager<AppIdentityUser> _usermanager;
        private RoleManager<AppIdentityRole> _roleManager;
        private AppIdentityDbContext _context;

        public AuthRepository(AppIdentityDbContext context,UserManager<AppIdentityUser> usermanager,RoleManager<AppIdentityRole> roleManager)
        {
            _context = context;
            _usermanager = usermanager;
            _roleManager = roleManager;
        }

        public async Task<AppIdentityUser> Login(string username, string password)
        {
            AppIdentityUser user = await _usermanager.FindByNameAsync(username);
            bool result = await _usermanager.CheckPasswordAsync(user, password);
            if (result == true)
                return user;
            else
                return null;
        }

        public async Task<AppIdentityUser> Register(AppIdentityUser user, string password)
        {
            IdentityResult result = await _usermanager.CreateAsync(user,password);
            if (result.Succeeded)
                return user;
            else
                return null;
        }

        public async Task<AppIdentityUser> FindByUsername(string userName)
        {
            AppIdentityUser user = await _usermanager.FindByNameAsync(userName);
            if (user != null)
                return user;
            else
                return null;

        }

        public async Task<AppIdentityRole> CreateRole(string roleName)
        {
            AppIdentityRole role = new AppIdentityRole() { Name = roleName };
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
                return role;
            else
                return null;
        }

        public List<AppIdentityRole> GetRoles()
        {
            List<AppIdentityRole> roles = _roleManager.Roles.ToList();
            return roles;
        }

        public async Task<AppIdentityRole> GetRoleByIdAsync(string id)
        {
            AppIdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
                return role;
            else
                return null;
        }

        public async Task<AppIdentityRole> GetRoleByNameAsync(string name)
        {
            AppIdentityRole role = await _roleManager.FindByNameAsync(name);
            if (role != null)
                return role;
            else
                return null;
        }

        public async Task<AppIdentityRole> DeleteRole(AppIdentityRole role)
        {
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return role;
                else
                    return null;
            }
            return null;
        }

        public async Task<AppIdentityRole> UpdateRole(AppIdentityRole role)
        {
            if (role != null)
            {
                IdentityResult result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                    return role;
                else
                    return null;
            }
            return null;
        }

        public async Task<AppIdentityUser> FindUserById(string id)
        {
            AppIdentityUser user = await _usermanager.FindByIdAsync(id);
            if (user != null)
                return user;
            else
                return null;
        }

        public async Task<List<AppIdentityUser>> GetUsersByRole(string roleName)
        {
            var users = await _usermanager.GetUsersInRoleAsync(roleName);
            return users.ToList();
        }

        public async Task<AppIdentityUser> FindUserByEmail(string email)
        {
            AppIdentityUser user = await _usermanager.FindByEmailAsync(email);
            if (user != null)
                return user;
            else
                return null;
        }

        public async Task<AppIdentityUser> AddUserToRole(AppIdentityUser user, string roleName)
        {
            if (user != null)
            {
                IdentityResult result = await _usermanager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return user;
                }
                return null;
            }
            return null;
        }

        public async Task<string> GenerateEmailConfirmToken(AppIdentityUser user)
        {
            var code = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
            if (!String.IsNullOrEmpty(code))
                return code;
            else
                return String.Empty;
        }

        public async Task<AppIdentityUser> UpdateUser(AppIdentityUser user)
        {
            IdentityResult result = await _usermanager.UpdateAsync(user);
            if (result.Succeeded)
                return user;
            else
                return null;
        }

        public async Task<AppIdentityUser> DeleteUser(AppIdentityUser user)
        {
            IdentityResult result = await _usermanager.DeleteAsync(user);
            if (result.Succeeded)
                return user;
            else
                return null;
        }

        public async Task<AppIdentityUser> ConfirmEmail(AppIdentityUser user, string code)
        {
            if (user != null && !String.IsNullOrEmpty(code))
            {
                IdentityResult result = await _usermanager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    return user;
                }
                return null;
            }
            return null;
        }
    }
}
