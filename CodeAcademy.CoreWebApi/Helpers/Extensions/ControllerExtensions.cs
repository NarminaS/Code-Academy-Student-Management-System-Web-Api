using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Helpers.Logging;
using CodeAcademy.CoreWebApi.Helpers.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task SendConfirmaitionMail(this ControllerBase controller, AppIdentityUser user,IAuthRepository repository, IUrlHelper urlHelper, Logger logger)
        {
            var _code = await repository.GenerateEmailConfirmToken(user);
            var callbackUrl = urlHelper.Action("ConfirmEmail",
                                               "Account",
                                               new { userId = user.Id, code = _code },
                                               protocol: controller.HttpContext.Request.Scheme);
            await new EmailService(logger).SendEmailAsync(user.Name, user.Email, $"CodeAcademy - {user.Name} - confirmation",
                                                    $"Confirm your registration via this link: <a href='{callbackUrl}'>link</a>", controller.Request.Path);
        }

        public static AppIdentityUser GetLoggedUser(this ControllerBase controller, IAuthRepository authRepo, IAppRepository appRepo)
        {
            var bearerToken = controller.Request.Headers["Authorization"].ToString();
            //var token = controller.Request.Headers["token"].ToString();
            string token = bearerToken.Substring(7);

            AppIdentityUser user = authRepo.GetUserFromToken(token);

            if (user != null)
            {
                if (user.UserType == "Student")
                {
                    Student student = appRepo.GetByIdAsync<Student>(x => x.Id == user.Id).Result;
                    Group group = appRepo.GetByIdAsync<Group>(x => x.Id == student.GroupId).Result;
                    Photo studPhoto =  appRepo.GetPhoto(student.PhotoId).Result;
                    student.FacultyId = group.FacultyId;
                    student.Photo = studPhoto;
                    student.Group = group;
                    return student;
                }
                if (user.UserType == "Teacher")
                {
                    Teacher teacher = appRepo.GetByIdAsync<Teacher>(x => x.Id == user.Id).Result;
                    Photo userPhoto = appRepo.GetPhoto(user.PhotoId).Result;
                    Faculty faculty = appRepo.GetByIdAsync<Faculty>(x => x.Id == teacher.FacultyId).Result;
                    teacher.Faculty = faculty;
                    teacher.Photo = userPhoto;
                    return teacher;
                }
                return user;
            }
            return null;
        }

        public static bool ValidRoleForAction(this ControllerBase controller, IAppRepository appRepo, IAuthRepository auth, params string[] roleNames)
        {
            AppIdentityUser user = controller.GetLoggedUser(auth, appRepo);
            foreach (string role in roleNames)
            {
                bool isValid = auth.CheckUserRole(user, role).Result;
                if (isValid == false)
                {
                    continue;
                }

                if (isValid == true)
                    return true;
            }        
            return false;
        }

        public static dynamic GetBaseData(this ControllerBase controller, IAppRepository appRepo, IAuthRepository auth)
        {
            return new { controller.GetLoggedUser(auth, appRepo).Email, controller.Request.Path };
        }
    }
}
