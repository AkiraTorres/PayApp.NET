namespace PayApp.Models;

public class Transaction
{
    public string Cpf { get; set; }
    public int Value { get; set; }
    public string Password { get; set; }

    public Transaction(string cpf, int value, string password)
    {
        Cpf = cpf;
        Value = value;
        Password = password;
    }
}