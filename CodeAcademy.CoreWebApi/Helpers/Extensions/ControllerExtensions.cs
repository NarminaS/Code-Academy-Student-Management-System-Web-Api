using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Entities;
using CodeAcademy.CoreWebApi.Helpers.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task SendConfirmaitionMail(this ControllerBase controller, AppIdentityUser user,IAuthRepository repository, IUrlHelper urlHelper)
        {

            var _code = await repository.GenerateEmailConfirmToken(user);
            var callbackUrl = urlHelper.Action("ConfirmEmail",
                                               "Account",
                                               new { userId = user.Id, code = _code },
                                               protocol: controller.HttpContext.Request.Scheme);
            await new EmailService().SendEmailAsync(user.Name, user.Email, $"CodeAcademy - {user.Name} - confirmation",
                                                    $"Confirm your registration via this link: <a href='{callbackUrl}'>link</a>");


        }

        public static AppIdentityUser GetLoggedUser(this ControllerBase controller, IAuthRepository authRepo, IAppRepository appRepo)
        {
            var email = controller.HttpContext.User.Identity.Name;
            AppIdentityUser user = authRepo.FindUserByEmail(email).Result;

            if (user != null)
            {
                if (user.UserType == "Student")
                {
                    Student student = appRepo.GetByIdAsync<Student>(x => x.Id == user.Id).Result;
                    Group group = appRepo.GetByIdAsync<Group>(x => x.Id == student.GroupId).Result;
                    Photo studPhoto = appRepo.GetPhoto(student.PhotoId);
                    student.FacultyId = group.FacultyId;
                    student.Photo = studPhoto;
                    return student;
                }
                if (user.UserType == "Teacher")
                {
                    Teacher teacher = appRepo.GetByIdAsync<Teacher>(x => x.Id == user.Id).Result;
                    Photo userPhoto = appRepo.GetPhoto(user.PhotoId);
                    teacher.Photo = userPhoto;
                    return teacher;
                }
                return user;
            }
            return null;
        }
    }
}
