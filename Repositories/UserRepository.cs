using PayApp.Models;

namespace PayApp.Repositories;

public class UserRepository : EntityRepository<User>
{
    private DataContext dbc;

    public UserRepository()
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"))
        .Build();
        dbc = new DataContext(configuration);
    }

    public bool Transference(User payerUser, User payeeUser, int value)
    {
        try
        {
            payerUser.Wallet -= value;
            payeeUser.Wallet += value;
            payerUser = this.dc.Set<User>().Update(payerUser).Entity;
            payeeUser = this.dc.Set<User>().Update(payeeUser).Entity;
            this.dbc.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public User? GetByCpfEmail(string cpfEmail)
    {
        return dbc.Set<User>().Where(a => a.Email.Equals(cpfEmail) || a.Cpf.Equals(cpfEmail)).FirstOrDefault();
    }

    public User Deposit(string cpfEmail, int value)
    {
        try
        {
            User? u = this.GetByCpfEmail(cpfEmail);

            if (u != null)
            {
                u.Wallet += value;
                u = this.dc.Set<User>().Update(u).Entity;
                this.dc.SaveChanges();
                return u;
            }
            else
            {
                throw new Exception("User with the informed ID was not found.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public User Withdraw(string cpfEmail, int value)
    {
        try
        {
            User? u = this.GetByCpfEmail(cpfEmail);

            if (u != null)
            {
                u.Wallet -= value;
                u = this.dbc.Set<User>().Update(u).Entity;
                this.dbc.SaveChanges();
                return u;
            }
            else
            {
                throw new Exception("User with the informed ID was not found.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public bool UserAuthentication(string cpfEmail, string encodedPassword)
    {
        try
        {
            User? u = this.GetByCpfEmail(cpfEmail);

            if (u != null)
            {
                if (encodedPassword == u.Password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("User with the informed CPF/Email was not found.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public bool ValidateWalletMoney(string cpf, int value)
    {
        User? u = this.GetByCpfEmail(cpf);
        if (u.Wallet > value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}