namespace HeroForge_OnceAgain.Migrations
{
    using HeroForge_OnceAgain.Infrastructure.Database;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;    
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HeroForge_OnceAgain.Infrastructure.Database.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        public static class SeedHelper
        {
            public static void SeedData(ApplicationDbContext context)
            {
                var userStore = new UserStore<IdentityUser>(context);
                var userManager = new UserManager<IdentityUser>(userStore);

                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }

                var hasher = new PasswordHasher();
                string passwordHash = hasher.HashPassword("Password123!");

                var user = userManager.FindByName("admin");
                if (user == null)
                {
                    var newUser = new IdentityUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "admin",
                        
                        Email = "admin@example.com",
                        
                        EmailConfirmed = true,
                        PasswordHash = passwordHash,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0
                    };


                    var result = userManager.Create(newUser, "Password123!");

                    if (result.Succeeded)
                    {
                        userManager.AddToRole(newUser.Id, "Admin");
                    }
                    else
                    {
                        throw new Exception("Erro ao criar usuário admin: " + string.Join(", ", result.Errors));
                    }
                }
            }
        }


        protected override void Seed(ApplicationDbContext context)
        {
            // Criar UserManager e RoleManager
            var userStore = new UserStore<IdentityUser>(context);
            var userManager = new UserManager<IdentityUser>(userStore);

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            // Criar role "Admin" se não existir
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }

            // Verificar se o usuário já existe
            var user = userManager.FindByName("admin");
            if (user == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                var result = userManager.Create(newUser, "Password123!");

                if (result.Succeeded)
                {
                    userManager.AddToRole(newUser.Id, "Admin");
                }
                else
                {
                    throw new Exception("Erro ao criar usuário admin: " + string.Join(", ", result.Errors));
                }
            }
        }

    }
}
