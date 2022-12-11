using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PayApp.Models;
using PayApp.Repositories;
using PayApp.Services;

namespace PayApp.Controllers;

[ApiController]
public abstract class EntityController<T, R, S> : ControllerBase where T : Entity
    where S : EntityService<T, R> where R : EntityRepository<T>
{
    protected S Service { get; set; }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        T? entity = Service.GetById(id);

        if (entity != null)
        {
            return this.Ok(entity);
        }
        else
        {
            return this.NotFound("Type of " + typeof(T).Name + " not found.");
        }
    }

    [HttpGet]
    public IActionResult ListAll()
    {
        return this.Ok(Service.ListAll());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        if (Service.Delete(id))
        {
            return this.Ok(typeof(T).Name + " with the informed ID was deleted successfully.");
        }
        else
        {
            return this.NotFound(typeof(T).Name + " with the informed ID was not found.");
        }
    }
}