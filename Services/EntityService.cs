using PayApp.Models;
using PayApp.Repositories;

namespace PayApp.Services;

public abstract class EntityService<T, R> where T : Entity where R : EntityRepository<T>
{
    private EntityRepository<T> Repo { get; set; }

    public EntityService(EntityRepository<T> repo)
    {
        this.Repo = repo;
    }

    public T? GetById(long id)
    {
        return this.Repo.GetById(id);
    }

    public List<T> ListAll()
    {
        return this.Repo.ListAll();
    }

    public T Store(T entity)
    {
        return this.Repo.Store(entity);
    }

    public T Edit(T t)
    {
        return this.Repo.Edit(t);
    }

    public bool Delete(long id)
    {
        return this.Repo.Delete(id);
    }
}