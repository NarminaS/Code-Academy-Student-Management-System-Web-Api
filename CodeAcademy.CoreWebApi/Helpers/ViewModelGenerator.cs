using CodeAcademy.CoreWebApi.BusinessLogicLayer.Abstract;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.DataTransferObject.ToView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers
{
    public class ViewModelGenerator
    {
        private IAppRepository _context;
        private IAuthRepository _auth;
        public ViewModelGenerator(IAppRepository context, IAuthRepository auth)
        {
            _auth = auth;
            _context = context;
        }

        public List<PostViewModel> CreatePostListViewModel(List<Post> posts)    
        {
            List<PostViewModel> postViewModels = new List<PostViewModel>();

            List<ArticleViewModel> articleViewModels = new List<ArticleViewModel>();
            List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
            List<LinkViewModel> linkViewModels = new List<LinkViewModel>();
            List<BookViewModel> bookViewModels = new List<BookViewModel>();

            foreach (var post in posts)
            {
                switch (post.PostType)
                {
                    case "Article":
                        articleViewModels.Add(new ArticleViewModel(post as Article));
                        break;
                    case "Question":
                        Question q = post as Question;
                        QuestionViewModel viewModel = new QuestionViewModel(q);
                        if (q.Photo != null)
                        {
                            viewModel.Photo = q.Photo.Url;
                        }
                        questionViewModels.Add(viewModel);
                        break;
                    case "Link":
                        linkViewModels.Add(new LinkViewModel(post as Link));
                        break;
                    case "Book":
                        bookViewModels.Add(new BookViewModel(post as Book));
                        break;
                    default:
                        break;
                }
            }

            postViewModels.AddRange(articleViewModels);
            postViewModels.AddRange(bookViewModels);
            postViewModels.AddRange(questionViewModels);
            postViewModels.AddRange(linkViewModels);

            foreach (var vm in postViewModels)
            {
                if (vm.UserType == "Student")
                {
                    vm.GroupName = _context.GetUserGroup(vm.UserId);
                }
            }

            return postViewModels;
        }
    }
}
