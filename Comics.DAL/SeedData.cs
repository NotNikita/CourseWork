using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Comics.Domain;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Comics.DAL
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider service)
        {
            using (var db = new ComicsDbContext(
                service.GetRequiredService<
                    DbContextOptions<ComicsDbContext>>()))
            {
                if (db.Collections.Any())
                {
                    return;
                }

                else
                {
                    var curr_admin = db.Users.FirstOrDefault(u => u.UserName == "admin");
                    db.Comics.AddRange(
                        new Comic
                        {
                            Name= "The Amazing Spider-Man #56",
                            Tags="Spider-man,Marvel,Green Goblin",
                            User=curr_admin,
                            Img= "https://res.cloudinary.com/dynastycomic/image/upload/v1610635105/large-6407522_gkv4f1.jpg",
                            Price=5,
                            Created= DateTime.Now,
                            Publisher="Marvel",
                            Description= "“LAST REMAINS” TAKES ITS TOLL! You will never look at Norman or Harry Osborn the same again. We know SPIDER-MAN won’t.",
                            PageCount=44,
                            Format="Comic",
                            Released= new DateTime(2021, 1, 6)
                        },
                        new Comic
                        {
                            Name = "Star Wars: Darth Vader #9",
                            Tags = "Star Wars,Marvel,Darth Vader",
                            User = curr_admin,
                            Img = "https://res.cloudinary.com/dynastycomic/image/upload/v1610635326/large-8949575_b6bnp4.jpg",
                            Price = 4,
                            Created = DateTime.Now,
                            Publisher = "Marvel",
                            Description = "In his search for vengeance in the depths of MUSTAFAR, DARTH VADER has seized the mysterious key to the EMPEROR’S greatest secret. But the key itself needs a key — which only the deadly assassin OCHI OF BESTOON seems to have. Vader and Ochi are in for the fight of their lives with the fate of the Emperor in the balance — but how much of this is all PALPATINE’S plan? And what happens when the SITH LORD and the SITH ASSASSIN start to figure that plan out?",
                            PageCount = 28,
                            Format = "Comic",
                            Released = new DateTime(2021, 1, 13)
                        },
                        new Comic
                        {
                            Name = "Star Wars: Darth Vader #8",
                            Tags = "Star Wars,Marvel,Darth Vader",
                            User = curr_admin,
                            Img = "https://res.cloudinary.com/dynastycomic/image/upload/v1610635429/large-9410942_ytzstw.jpg",
                            Price = 4,
                            Created = DateTime.Now,
                            Publisher = "Marvel",
                            Description = "Hungry for vengeance after his brutal punishment at the hands of THE EMPEROR, can DARTH VADER uncover his master's secrets in the depths of MUSTAFAR? Stripped of his greatest weapons, can the dark lord survive the fire and the EYE? Or will he be overcome as the EYE turns Vader's every question back to his own terrible secrets?",
                            PageCount = 28,
                            Format = "Comic",
                            Released = new DateTime(2020, 12, 16)
                        },
                        new Comic
                        {
                            Name = "King in Black: Planet of the Symbiotes #1",
                            Tags = "Knull,Marvel,Earth-616",
                            User = curr_admin,
                            Img = "https://res.cloudinary.com/dynastycomic/image/upload/v1610643786/large-2541615_toxzo2.jpg",
                            Price = 4,
                            Created = DateTime.Now,
                            Publisher = "Marvel",
                            Description = "DARKNESS REIGNS OVER THE MARVEL UNIVERSE!",
                            PageCount = 28,
                            Format = "Comic",
                            Released = new DateTime(2021, 1, 13)
                        }

                        );

                    db.SaveChanges();

                    db.Collections.AddRange(
                        new Collection
                        {
                            Name = "Коллекция Комиксов 1",
                            Description = "Тестовая коллекция номер 1",
                            Theme = "Comics",
                            Items = new List<BaseItem> { db.Comics.FirstOrDefault(c => c.Id == 1), db.Comics.FirstOrDefault(c => c.Id == 2) , db.Comics.FirstOrDefault(c => c.Id == 3) },
                            UserId = curr_admin.Id,
                            User = curr_admin,
                            Img = "https://res.cloudinary.com/dynastycomic/image/upload/v1610635534/lf-109_ee7gpe.jpg"
                        },

                        new Collection
                        {
                            Name = "Коллекция Комиксов 2",
                            Description = "Тестовая коллекция номер 2",
                            Theme = "Comics",
                            Items = new List<BaseItem> { db.Comics.FirstOrDefault(c => c.Id == 3), db.Comics.FirstOrDefault(c => c.Id == 4) },
                            UserId = curr_admin.Id,
                            User = curr_admin,
                            Img = "https://res.cloudinary.com/dynastycomic/image/upload/v1610635643/large-4249891_atyuji.jpg"
                        }

                    );
                    db.SaveChanges();
                }
            }
        }
    }
}
