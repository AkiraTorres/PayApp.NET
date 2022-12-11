using PayApp.Models;
using PayApp.Repositories;

namespace PayApp.Services;

public class UserService : EntityService<User, UserRepository>
{
    UserRepository Repo = new UserRepository();

    public UserService(UserRepository repo) : base(repo)
    {
        this.Repo = repo;
    }

    public bool Transference(string payeeCpf, Transaction transaction)
    {
        User? u = this.Repo.GetByCpfEmail(payeeCpf);
        User? payerUser = this.Repo.GetByCpfEmail(transaction.Cpf);


        if (u == null) throw new Exception("The destiny user don't exist");
        if (payerUser!.IsShopkeeper == true) throw new Exception("Shopkeepers can't send money, only receive.");

        if (UserAuthentication(transaction.Cpf, transaction.Password))
        {
            if (this.Repo.ValidateWalletMoney(transaction.Cpf, transaction.Value))
            {
                return this.Repo.Transference(payerUser, u, transaction.Value);
            }
            else
            {
                throw new Exception("Your account don't have the money needed for the withdraw.");
            }
        }
        else
        {
            throw new Exception("The informed password is wrong.");
        }
    }

    public string EncodeSHA512(string data)
    {
        var message = System.Text.Encoding.UTF8.GetBytes(data);
        using (var alg = System.Security.Cryptography.SHA512.Create())
        {
            string hex = "";

            var hashValue = alg.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }

    public bool UserAuthentication(string cpfEmail, string password)
    {
        String encodedPassword = EncodeSHA512(password);
        return this.Repo.UserAuthentication(cpfEmail, password);
    }

    public User Deposit(Transaction transaction)
    {
        if (UserAuthentication(transaction.Cpf, transaction.Password))
        {
            return this.Repo.Deposit(transaction.Cpf, transaction.Value);
        }
        else
        {
            throw new Exception("The informed password is wrong.");
        }
    }

    public User Withdraw(Transaction transaction)
    {
        if (UserAuthentication(transaction.Cpf, transaction.Password))
        {
            if (this.Repo.ValidateWalletMoney(transaction.Cpf, transaction.Value))
            {
                return this.Repo.Withdraw(transaction.Cpf, transaction.Value);
            }
            else
            {
                throw new Exception("Your account don't have the money needed for the withdraw.");
            }
        }
        else
        {
            throw new Exception("The informed password is wrong.");
        }
    }
}