using System.Security.Claims;
using KidsLogic.Models.DataBase;
using KidsLogic.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KidsLogic.Controllers;

public class AccountController : Controller
{
    private readonly DataBaseContext _dataBase;
    private readonly EmailService _emailService;

    public AccountController(EmailService emailService, DataBaseContext dataBase)
    {
        _dataBase = dataBase;
        _emailService = emailService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDTO loginData)
    {
        if (ModelState.IsValid)
        {
            User? user = await _dataBase.Users
                .Where(u => u.Email == loginData.Email && u.Password == loginData.Password)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                ModelState.AddModelError("", "Неправильно введен логин или пароль.");
            }
            else
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Email, loginData.Email),
                    new(ClaimTypes.Name, user.FirstName),
                    new(ClaimTypes.Surname, user.LastName),
                    new(ClaimTypes.Uri, user.AvatarUri),
                    new(ClaimTypes.Sid, user.Id.ToString())
                };
                ClaimsIdentity claimsIdentity = new(claims, "Cookies");

                await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
                return Redirect("/");
            }
        }

        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterDTO registrationData)
    {
        if (ModelState.IsValid)
        {
            User? user = await _dataBase.Users
                .Where(u => u.Email == registrationData.Email)
                .FirstOrDefaultAsync();

            if (user is not null)
            {
                ModelState.AddModelError("Email", "Пользователь с данным адресом уже зарегистрирован.");
            }
            else
            {
                string password = PasswordGenerator.Generate();
                string name = registrationData.FirstName + " " + registrationData.LastName;
                
                await _emailService.SendRegistrationPasswordAsync(registrationData.Email, name, password);

                User newUser = new()
                {
                    Email = registrationData.Email,
                    FirstName = registrationData.FirstName,
                    LastName = registrationData.LastName,
                    Password = password
                };

                await _dataBase.Users.AddAsync(newUser);
                await _dataBase.SaveChangesAsync();

                ViewData["Email"] = registrationData.Email;
                return View("RegisterNotification");
            }
        }

        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
}