using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PayApp.Models;
using PayApp.Repositories;
using PayApp.Services;

namespace PayApp.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : EntityController<User, UserRepository, UserService>
{
    private UserService service = new UserService(new UserRepository());
    public UserController() : base()
    {
        this.Service = new UserService(new UserRepository());
    }

    [HttpPost]
    public IActionResult Store(User user)
    {
        if (user == null) return this.NotFound("An user needs to be send");
        user.Id = null;
        if (user.Password != null)
        {
            user.Password = service.EncodeSHA512(user.Password);
        }
        else
        {
            return this.NotFound("A password is mandatory.");
        }
        if (user.Name == null) return this.NotFound("A name is mandatory.");
        if (user.Email == null) return this.NotFound("A email is mandatory.");
        if (user.Cpf == null) return this.NotFound("A cpf is mandatory.");
        user.Wallet = user.Wallet == null ? 0 : user.Wallet;
        user.IsShopkeeper = user.IsShopkeeper == null ? false : user.IsShopkeeper;

        return this.Ok(service.Store(user));
    }

    [HttpPut]
    public IActionResult Edit(User user)
    {
        if (user == null) return this.NotFound("An user needs to be send");
        if (user.Id == null) return this.NotFound("The ID atribute is mandatory.");
        if (user.Name == null) return this.NotFound("A name is mandatory.");
        if (user.Email == null) return this.NotFound("A email is mandatory.");
        if (user.Cpf == null) return this.NotFound("A cpf is mandatory.");
        user.Wallet = user.Wallet == null ? 0 : user.Wallet;
        user.IsShopkeeper = user.IsShopkeeper == null ? false : user.IsShopkeeper;

        if (user.Password != null) user.Password = service.EncodeSHA512(user.Password);

        User a = service.Edit(user);
        if (a == null)
        {
            return this.NotFound("The user with the informed ID was not found.");
        }
        else
        {
            return this.Ok(a);
        }
    }
}