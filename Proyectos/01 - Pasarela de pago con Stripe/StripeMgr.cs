namespace StripePayment;

using System;
using Stripe;

class StripeMgr
{
    public StripeMgr(string secretKey)
    {
        try
        {
            StripeConfiguration.ApiKey = secretKey;

            // Verificar la conexión con la API de Stripe
            var accountService = new AccountService();
            var account = accountService.GetSelf();

            Console.WriteLine("Conexión exitosa a Stripe.");
            Console.WriteLine($"Cuenta Stripe ID: {account.Id}");
            Console.WriteLine($"Nombre de la cuenta: {account.Settings.Dashboard.DisplayName}");

        }
        catch (StripeException e)
        {
            throw new InvalidOperationException($"Error al conectarse a la API de Stripe: {e.Message}");
        }
    }

    public static bool VerifyEmail(string email)
    {
        try
        {
            var customerService = new CustomerService();
            var customers = customerService.List(new CustomerListOptions
            {
                Email = email,
                Limit = 1
            });

            return customers.Any();
        }
        catch (StripeException e)
        {
            throw new InvalidOperationException($"Error: {e.Message}");
        }
    }

    public static void CreateCustomer(string email, string name, string description, string phone)
    {
        try
        {
            if (VerifyEmail(email))
            {
                throw new InvalidOperationException($"El cliente con el email {email} ya existe.");
            }

            var options = new CustomerCreateOptions
            {
                Email = email,
                Name = name,
                Description = description,
                Phone = phone
            };

            var service = new CustomerService();
            var customer = service.Create(options);

            Console.WriteLine("Cliente creado exitosamente:");
            Console.WriteLine($"ID: {customer.Id}");
            Console.WriteLine($"Email: {customer.Email}");
            Console.WriteLine($"Nombre: {customer.Name}");
        }
        catch (StripeException e)
        {
            throw new InvalidOperationException($"Error: {e.Message}");
        }
    }

}
