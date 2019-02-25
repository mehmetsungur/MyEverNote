using MyEvernote.Business.Abstract;
using MyEvernote.Business.Results;
using MyEvernote.Common.Helpers;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using System;

namespace MyEvernote.Business
{
    public class EvernoteUserManager : ManagerBase<EvernoteUser>
    {
        public BusinessResult<EvernoteUser> RegisterUser(RegisterViewModal data)
        {
            EvernoteUser user = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessResult<EvernoteUser> res = new BusinessResult<EvernoteUser>();
            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email Adres Kayıtlı");
                }
            }
            else
            {
                int dbResult = base.Insert(new EvernoteUser()
                {
                    UserName = data.UserName,
                    Email = data.Email,
                    Password = data.Password,
                    ProfileImageFileName = "user_default.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                });
                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.Email && x.UserName == data.UserName);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = ($"Merhaba {res.Result.UserName};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.");
                    MailHelper.SendMail(body, res.Result.Email, "MyEvernote Hesap Aktifleştirme");
                }
            }
            return res;
        }

        public BusinessResult<EvernoteUser> GetUserById(int id)
        {
            BusinessResult<EvernoteUser> res = new BusinessResult<EvernoteUser>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "User Not Found");
            }

            return res;
        }

        public BusinessResult<EvernoteUser> LoginUser(LogInViewModal data)
        {
            BusinessResult<EvernoteUser> res = new BusinessResult<EvernoteUser>();
            res.Result = Find(x => x.UserName == data.UserName && x.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmedi.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen Email adresinizi kontrol ediniz.");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameorPassWrong, "Kullanıcı Adı yada Şifre Uyuşmuyor");
            }

            return res;
        }

        public BusinessResult<EvernoteUser> UpdateProfile(EvernoteUser data)
        {
            EvernoteUser db_user = Find(x => x.Id != data.Id && (x.UserName == data.UserName || x.Email == data.Email));
            BusinessResult<EvernoteUser> res = new BusinessResult<EvernoteUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email Adresi Kayıtlı");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;

            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi");
            }
            return res;
        }

        public BusinessResult<EvernoteUser> RemoveUserById(int id)
        {
            BusinessResult<EvernoteUser> res = new BusinessResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFound, "Kullanıcı Bulunamadı");
            }

            return res;
        }

        public BusinessResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessResult<EvernoteUser> res = new BusinessResult<EvernoteUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAllReadyActive, "Kullanıcı zaten aktifleştirildi.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aftifleştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }

        public new BusinessResult<EvernoteUser> Insert(EvernoteUser data)
        {
            EvernoteUser user = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessResult<EvernoteUser> res = new BusinessResult<EvernoteUser>();

            res.Result = data;

            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email Adres Kayıtlı");
                }
            }
            else
            {
                res.Result.ProfileImageFileName = "user_default.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserIsNotInserted, "Kullanıcı Eklenemedi");
                }
            }
            return res;
        }

        public new BusinessResult<EvernoteUser> Update(EvernoteUser data)
        {
            EvernoteUser db_user = Find(x => x.Id != data.Id && (x.UserName == data.UserName || x.Email == data.Email));
            BusinessResult<EvernoteUser> res = new BusinessResult<EvernoteUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email Adresi Kayıtlı");
                }
                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi");
            }
            return res;
        }
    }
}