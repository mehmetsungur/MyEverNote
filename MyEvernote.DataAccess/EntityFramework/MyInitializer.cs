using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MyEvernote.DataAccess.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            //Adding admin user...
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Mehmet",
                Surname = "Sungur",
                Email = "mehmetsungur90@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "msungur",
                Password = "1",
                ProfileImageFileName = "user_default.png",
                CreateOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifedUserName = "msungur"
            };
            context.EvernoteUsers.Add(admin);

            //Adding standart user...
            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Yetkin",
                Surname = "Demirci",
                Email = "yetkindemirci@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "ydemirci",
                Password = "1",
                ProfileImageFileName = "user_default.png",
                CreateOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifedUserName = "msungur"
            };
            context.EvernoteUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user{i}",
                    Password = "1",
                    ProfileImageFileName = "user_default.png",
                    CreateOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifedUserName = $"user{i}"
                };
                context.EvernoteUsers.Add(user);
            }
            context.SaveChanges();

            // User List for using
            List<EvernoteUser> userlist = context.EvernoteUsers.ToList();
            for (int i = 0; i < 10; i++)
            {
                //Adding fake categories...
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreateOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifedUserName = "msungur"
                };
                context.Categories.Add(cat);

                //Adding fake notes...
                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    EvernoteUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreateOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifedUserName = owner.UserName
                    };
                    cat.Notes.Add(note);

                    //Adding fake comments...
                    for (int j = 0; j < FakeData.NumberData.GetNumber(1, 5); j++)
                    {
                        EvernoteUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = comment_owner,
                            CreateOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifedUserName = comment_owner.UserName
                        };

                        note.Comments.Add(comment);
                    }

                    //Adding fake likes...
                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]
                        };

                        note.Likes.Add(liked);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}