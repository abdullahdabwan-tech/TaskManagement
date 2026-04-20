using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Domain;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data
{

    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.Migrate();

            // ================= ROLES =================
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                );
                context.SaveChanges();
            }

            var adminRole = context.Roles.First(r => r.Name == "Admin");
            var userRole = context.Roles.First(r => r.Name == "User");

            // ================= PERMISSIONS =================
            if (!context.Permissions.Any())
            {
                var permissions = new List<Permission>
            {
                new() { Name = "User.Create" },
                new() { Name = "User.Read" },
                new() { Name = "User.Update" },
                new() { Name = "User.Delete" },

                new() { Name = "Task.Create" },
                new() { Name = "Task.Read" },
                new() { Name = "Task.Update" },
                new() { Name = "Task.Delete" },

                new() { Name = "Comment.Create" },
                new() { Name = "Comment.Read" },
                new() { Name = "Comment.Update" },
                new() { Name = "Comment.Delete" },

                new() { Name = "Reaction.Create" },
                new() { Name = "Reaction.Read" },
                new() { Name = "Reaction.Update" },
                new() { Name = "Reaction.Delete" },

                new() { Name = "Category.Create" },
                new() { Name = "Category.Read" },
                new() { Name = "Category.Update" },
                new() { Name = "Category.Delete" },

                new() { Name = "Role.Create" },
                new() { Name = "Role.Read" },
                new() { Name = "Role.Update" },
                new() { Name = "Role.Delete" },
            };

                context.Permissions.AddRange(permissions);
                context.SaveChanges();
            }

            var permissionsList = context.Permissions.ToList();

            // ================= ROLE PERMISSIONS =================
            if (!context.RolePermissions.Any())
            {
                var adminPermissions = permissionsList
                    .Select(p => new RolePermission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = p.Id
                    });

                var userPermissions = permissionsList.Where(p =>
                    p.Name == "Task.Create" ||
                    p.Name == "Task.Read" ||
                    p.Name == "Task.Update" ||
                    p.Name == "Comment.Create" ||
                    p.Name == "Comment.Read" ||
                    p.Name == "Comment.Update" ||
                    p.Name == "Comment.Delete" ||
                    p.Name == "Reaction.Create" ||
                    p.Name == "Reaction.Read" ||
                    p.Name == "Reaction.Delete" ||
                    p.Name == "Category.Read"
                ).Select(p => new RolePermission
                {
                    RoleId = userRole.Id,
                    PermissionId = p.Id
                });

                context.RolePermissions.AddRange(adminPermissions);
                context.RolePermissions.AddRange(userPermissions);
            }

            // ================= REACTION TYPES =================
            if (!context.ReactionTypes.Any())
            {
                context.ReactionTypes.AddRange(
                    new ReactionType { Name = "Like" },
                    new ReactionType { Name = "Love" },
                    new ReactionType { Name = "Haha" },
                    new ReactionType { Name = "Wow" },
                    new ReactionType { Name = "Sad" },
                    new ReactionType { Name = "Angry" }
                );
            }

            // ================= CATEGORIES =================
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Work", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Personal", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Urgent", CreatedAt = DateTime.UtcNow }
                );
            }

            // ================= ADMIN USER =================
            if (!context.Users.Any(u => u.Email == "admin@gmail.com"))
            {
                var hasher = new PasswordHasher<User>();

                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    RoleId = adminRole.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                admin.PasswordHash = hasher.HashPassword(admin, "1234");

                context.Users.Add(admin);
            }

            context.SaveChanges();
        }
    }
}

