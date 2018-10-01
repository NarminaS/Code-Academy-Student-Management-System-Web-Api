using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.NotifyHelpers    
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

        public Notification SharePost(Post post , int point)
        {
            Notification notification = new Notification
            {
                Sender = post.AppIdentityUser,
                Subscriber = post.AppIdentityUser,
                Point = point,
                TotalPoints = post.AppIdentityUser.Point,
                Type = post.PostType,
                Action = NotificationActions.Added,
                State = NotificationStates.Positive,
                ContentId = post.Id
            };
            return notification;
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
                    Point = 20,
                    TotalPoints = post.AppIdentityUser.Point
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
                State = NotificationStates.Positive,
                Point = 20,
                TotalPoints = post.AppIdentityUser.Point
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
                    ContentId = like.Post.Id,
                    Sender = like.AppIdentityUser,
                    Subscriber = like.Post.AppIdentityUser,
                    Type = like.Post.PostType,
                    Action = NotificationActions.Like,
                    State = NotificationStates.Positive,
                    Point = points,
                    TotalPoints = like.Post.AppIdentityUser.Point
                };
                return postNotification;
            }
            Notification commentNotification = new Notification
            {
                ContentId = like.Comment.Id,
                Sender = like.AppIdentityUser,
                Subscriber = like.Comment.User,
                Type = "Comment",
                Action = NotificationActions.Like,
                State = NotificationStates.Positive,
                Point = points,
                TotalPoints = like.Comment.User.Point
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
                    Point = points,
                    TotalPoints = like.Post.AppIdentityUser.Point
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
                Point = points,
                TotalPoints = like.Comment.User.Point
            };
            return commentNotification;
        }

        public Notification Comment (Comment comment)
        {
            Notification notification = new Notification
            {
                Sender = comment.User,
                Subscriber = comment.Post.AppIdentityUser,
                ContentId = comment.Id,
                Type = "Comment",
                Action = NotificationActions.Added,
                State = NotificationStates.Positive,
                Point = 5,
                TotalPoints = comment.User.Point
            };
            return notification;
        }

        public async Task<Notification> Reply(Comment answer)
        {
            Comment parent = await _context.GetComment(answer.ParentId.Value);
            Notification notification = new Notification
            {
                Sender = answer.User,
                Subscriber = parent.User,
                ContentId = answer.Id,
                Point = 5,
                TotalPoints = parent.User.Point,
                Type = "Answer",
                Action = NotificationActions.Reply,
                State = NotificationStates.Neutral
            };
            return notification;
        }   
    }
}
