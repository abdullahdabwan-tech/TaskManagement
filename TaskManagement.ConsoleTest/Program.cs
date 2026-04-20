using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using TaskManagement.Domain;
using TaskManagement.Infrastructure;

class Program
{
    public static string Hash(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }
    private static void Main(string[] args)
    {

        // 3) إنشاء PasswordHasher
        var hasher = new PasswordHasher<User>();

        // 4) إنشاء المستخدم

        var user = new User
        {
            Id = 1,
            UserName = "alia",
            Email = "alia@test.com",
        };
        // 5) تشفير كلمة المرور
        user.PasswordHash = hasher.HashPassword(user, "1234");

        Console.WriteLine(user.PasswordHash);
        // 6) إضافة المستخدم للقاعدة

        Console.WriteLine("User created successfully!");
    }
}