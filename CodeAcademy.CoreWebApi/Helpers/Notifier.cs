using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers
{
    public class Notifier
    {
        private IAppRepository _context;
        private IAuthRepository _auth;

        public Notifier(IAppRepository context, IAuthRepository auth)
        {
            _auth = auth;
            _context = context;
        }

        public async Task<List<Notification>> NewBook(Book newBook)
        {
            List<Notification> broadcast = new List<Notification>();
            List<AppIdentityUser> allteachers = await _auth.GetUsersByRole("Teacher");
            var subscribers = allteachers.Where(x => x.FacultyId == newBook.FacultyId).ToList();
            foreach (var teacher in subscribers)
            {
                broadcast.Add(new Notification
                {
                    ContentId = newBook.Id,
                    Sender = newBook.AppIdentityUser,
                    Subscriber = teacher,
                    Type = newBook.PostType,
                    Action = NotificationActions.Added,
                    State = NotificationStates.Neutral
                });
            }
            return broadcast;
        }

        public Notification Approved(Post post, Teacher sender)
        {
            if (post.PostType == "Book")
            {
                Notification bookNotification = new Notification
                {
                    ContentId = post.Id,
                    Sender = sender,
                    Subscriber = post.AppIdentityUser,
                    Type = post.PostType,
                    Action = NotificationActions.Approved,
                    State = NotificationStates.Positive,
                    Points = 20
                };
                return bookNotification;
            }
            Notification articleNotification = new Notification
            {
                ContentId = post.Id,
                Sender = sender,
                Subscriber = post.AppIdentityUser,
                Type = post.PostType,
                Action = NotificationActions.Approved,
                State = NotificationStates.Positive
            };
            return articleNotification;
        }

        public Notification Disapproved(Book book, Teacher sender, string reason)
        {
            Notification disapproveNotification = new Notification
            {
                ContentId = book.Id,
                Sender = sender,
                Subscriber = book.AppIdentityUser,
                Type = book.PostType,
                Action = NotificationActions.Dispproved,
                State = NotificationStates.Negative,
                Message = reason
            };
            return disapproveNotification;
        }

        public Notification Like(Like like, int points)
        {
            if (like.Post != null)
            {
                Notification postNotification = new Notification
                {
                    ContentId = like.Id,
                    Sender = like.AppIdentityUser,
                    Subscriber = like.Post.AppIdentityUser,
                    Type = like.Post.PostType,
                    Action = NotificationActions.Like,
                    State = NotificationStates.Positive,
                    Points = points
                };
                return postNotification;
            }
            Notification commentNotification = new Notification
            {
                ContentId = like.Id,
                Sender = like.AppIdentityUser,
                Subscriber = like.Comment.User,
                Type = "Comment",
                Action = NotificationActions.Like,
                State = NotificationStates.Positive,
                Points = points
            };
            return commentNotification;
        }

        public Notification Dislike(Like like, int points)
        {
            if (like.Post != null)
            {
                Notification postNotification = new Notification
                {
                    ContentId = like.Id,
                    Sender = like.AppIdentityUser,
                    Subscriber = like.Post.AppIdentityUser,
                    Type = like.Post.PostType,
                    Action = NotificationActions.Like,
                    State = NotificationStates.Negative,
                    Points = points
                };
                return postNotification;
            }
            Notification commentNotification = new Notification
            {
                ContentId = like.Id,
                Sender = like.AppIdentityUser,
                Subscriber = like.Comment.User,
                Type = "Comment",
                Action = NotificationActions.Like,
                State = NotificationStates.Negative,
                Points = points
            };
            return commentNotification;
        }

        public Notification Comment (Comment comment)
        {
            Notification notification = new Notification
            {
                Sender = comment.Post.AppIdentityUser,
                Subscriber = comment.User,
                ContentId = comment.Id,
                Type = "Comment",
                Action = NotificationActions.Added,
                State = NotificationStates.Positive
            };
            return notification;
        }

    }
}
