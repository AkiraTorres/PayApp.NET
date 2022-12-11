using PayApp.Models;

namespace PayApp.Repositories;

public abstract class EntityRepository<T> where T : Entity
{
    protected readonly DataContext dc;

    public EntityRepository()
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"))
        .Build();
        dc = new DataContext(configuration);
    }

    public T? GetById(long? id)
    {
        try
        {
            return this.dc.Set<T>().Where(a => a.Id.Equals(id)).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public List<T> ListAll()
    {
        return this.dc.Set<T>().ToList();
    }

    public T Store(T entity)
    {
        try
        {
            this.dc.Set<T>().Add(entity);
            this.dc.SaveChanges();
            return entity;
            {
                throw new Exception("You must send a " + typeof(T).Name + ".");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public T Edit(T t)
    {
        try
        {
            T? entity = this.GetById(t.Id);
            if (entity != null)
            {
                var properties = entity.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    if (prop.GetValue(t) != null)
                    {
                        prop.SetValue(entity, prop.GetValue(t));
                    }
                }
                entity = this.dc.Set<T>().Update(entity).Entity;
                this.dc.SaveChanges();
                return entity;
            }
            else
            {
                throw new Exception(typeof(T).Name + " with the informed ID was not found.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public bool Delete(long id)
    {
        try
        {
            T? t = this.GetById(id);
            if (t != null)
            {
                this.dc.Set<T>().Remove(t);
                this.dc.SaveChanges();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}