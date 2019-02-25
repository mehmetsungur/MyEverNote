using MyEvernote.Common;
using MyEvernote.Entities;
using MyEvernote.WebApp.Models;

namespace MyEvernote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUserName()
        {
            EvernoteUser user = CurrentSession.User;

            if (user != null)
                return user.UserName;
            else
                return "system";
        }
    }
}