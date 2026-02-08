using Blogs.Query.Bootstrapper;
using Comments.Query.Bootstrapper;
using Emails.Query.Bootstrapper;
using Microsoft.Extensions.DependencyInjection;
using PostModule.Query.Bootstrapper;
using Query.Contract.Admin.Comment;
using Query.Contract.Admin.Email.EmailUser;
using Query.Contract.Admin.Email.MessageUser;
using Query.Contract.Admin.Seo;
using Query.Contract.Site.Page;
using Query.Contract.UI.Blogs;
using Query.Contract.UI.Comments;
using Query.Contract.UI.PostPackage;
using Query.Contract.UI.UserPanel;
using Query.Contract.UI.UserPanel.PostOrder;
using Query.Contract.UI.UserPanel.UserAddress;
using Query.Service.Admin.Comment;
using Query.Service.Admin.Email.EmailUser;
using Query.Service.Admin.Email.MessageUser;
using Query.Service.Admin.Seo;
using Query.Service.Site.Page;
using Query.Service.Ui.Blogs;
using Query.Service.Ui.Comments;
using Query.Service.Ui.PostPackages;
using Query.Service.Ui.UserPanel;
using Query.Service.Ui.UserPanel.PostOrder;
using Query.Service.Ui.UserPanel.UserAddress;
using Seos.Query.Bootstrapper;
using Site.Query.Bootstrapper;
using Transactions.Query.Bootstrapper;
using Users.Query.Bootstrapper;

namespace Query.Service
{
    public static class Module_Bootstrapper
    {
        public static void Config(IServiceCollection Services, string ConnectionString)
        {

            #region Modules

            Blog_Bootstrapper.Config(Services, ConnectionString);
            User_Bootstrapper.Config(Services, ConnectionString);
            Seo_Bootstrapper.Config(Services, ConnectionString);
            Site_Bootstrapper.Config(Services, ConnectionString);
            Comment_Bootstrapper.Config(Services, ConnectionString);
            Email_Bootstrapper.Config(Services, ConnectionString);
            Post_Bootstrapper.Config(Services, ConnectionString);
            Comment_Bootstrapper.Config(Services, ConnectionString);
            Transaction_Bootstrapper.Config(Services, ConnectionString);
            #endregion
            #region Admin

            Services.AddTransient<ICommentAdminQuery, CommentQueryService>();
            Services.AddTransient<IEmailAdminQuery, EmailAdminQuery>();
            Services.AddTransient<IMessageUserAdminQuery, MessageUserAdminQuery>();
            Services.AddTransient<ISeoAdminQuery, SeoAdminQuery>();
            #endregion
            #region Ui

            Services.AddTransient<IBlogUiQueryService, BlogUiQueryService>();
            Services.AddTransient<ICommentsUiQueryService, CommentsUiQueryService>();
            Services.AddTransient<ISitePageUiQueryService, SitePageUiQueryService>();
            Services.AddTransient<IPackageUiQueryService, PackagesUiQueryService>();
            Services.AddTransient<IUserPanelQueryService, UserPanelQueryService>();
            Services.AddTransient<IPostOrderQueryService, PostOrderQueryService>();
            Services.AddTransient<IUserAddressUiQueryService, UserAddressUiQueryService>();
            #endregion
        }
    }

}
