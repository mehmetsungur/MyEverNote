using MyEvernote.Business;
using MyEvernote.Business.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;
using MyEvernote.WebApp.ViewModals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();

        public ActionResult Index()
        {
            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Note> notes = noteManager.ListQueryable()
                .Where(x => x.IsDraft == false && x.CategoryId == id)
                .OrderByDescending(x => x.ModifiedOn).ToList();

            return View("Index", notes);
        }

        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            BusinessResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModal ErrnotifyObj = new ErrorViewModal()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }

            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            BusinessResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModal ErrnotifyObj = new ErrorViewModal()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }

            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifedUserName");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }

                BusinessResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModal errorNotifyObj = new ErrorViewModal()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                CurrentSession.Set<EvernoteUser>("login", res.Result);
                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile(EvernoteUser user)
        {
            BusinessResult<EvernoteUser> res = evernoteUserManager.RemoveUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModal errorNotifyObj = new ErrorViewModal()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LogInViewModal modal)
        {
            if (ModelState.IsValid)
            {
                BusinessResult<EvernoteUser> res = evernoteUserManager.LoginUser(modal);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(modal);
                }

                CurrentSession.Set<EvernoteUser>("login", res.Result);
                return RedirectToAction("Index");
            }

            return View(modal);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModal modal)
        {
            if (ModelState.IsValid)
            {
                BusinessResult<EvernoteUser> res = evernoteUserManager.RegisterUser(modal);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(modal);
                }
                OKViewModal notifyObj = new OKViewModal()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObj.Items.Add("Please, Your Email sending Activation Mail must access.");

                return View("Ok", notifyObj);
            }
            return View(modal);
        }

        public ActionResult UserActivate(Guid id)
        {
            BusinessResult<EvernoteUser> res = evernoteUserManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModal ErrnotifyObj = new ErrorViewModal()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }

            OKViewModal OknotifyObj = new OKViewModal()
            {
                Title = "Hesap Aktifleştirildi.",
                RedirectingUrl = "/Home/LogIn"
            };
            OknotifyObj.Items.Add("Your Account activated. You can do shared and like.");
            return View("Ok", OknotifyObj);
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}