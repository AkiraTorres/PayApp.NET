using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PayApp.Models;
using PayApp.Repositories;
using PayApp.Services;

namespace PayApp.Controllers;

[Route("api/transference")]
[ApiController]
public class TransactionController : ControllerBase
{
    private UserService service = new UserService(new UserRepository());

    [HttpPost]
    public IActionResult Withdraw(Transaction model)
    {
        return this.Ok(this.service.Withdraw(model));
    }

    [HttpPut]
    public IActionResult Deposit(Transaction model)
    {
        return this.Ok(this.service.Deposit(model));
    }

    [HttpPut("{destiny_cpf}")]
    public IActionResult Transference(string destiny_cpf, Transaction model)
    {
        bool result = this.service.Transference(destiny_cpf, model);
        if (result)
        {
            return this.Ok("Transference realized successfully.");
        }
        else
        {
            return this.NotFound("An error ocourred in this transference.");
        }
    }
}