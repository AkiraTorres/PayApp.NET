using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PayApp.Models;

public class User : Entity
{
    public String? Name { get; set; }
    public String? Email { get; set; }
    public String? Cpf { get; set; }
    public String? Password { get; set; }
    public int? Wallet { get; set; }
    [Column("is_shopkeeper")]
    public bool? IsShopkeeper { get; set; }

    [JsonConstructor]
    public User() { }

    // [JsonConstructor]
    public User(long id, string name, string email, string cpf, string password, int wallet, bool? is_shopkeeper)
    {
        Id = id;
        Name = name;
        Email = email;
        Cpf = cpf;
        Password = password;
        Wallet = wallet;
        IsShopkeeper = is_shopkeeper;
    }
}