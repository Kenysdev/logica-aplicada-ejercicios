namespace StripeManager;

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

    public static string CreateCustomer(string email, string name, string description, string phone)
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

            return customer.Id;
        }
        catch (StripeException e)
        {
            throw new InvalidOperationException($"Error: {e.Message}");
        }
    }

    public static string CreateProduct(string name, string description, long price)
    {
        try
        {
            var options = new ProductCreateOptions
            {
                Name = name,
                Description = description,
                DefaultPriceData = new ProductDefaultPriceDataOptions
                {
                    UnitAmount = price,
                    Currency = "usd"
                }
            };

            var service = new ProductService();
            var product = service.Create(options);

            Console.WriteLine("Producto creado exitosamente:");
            Console.WriteLine($"ID: {product.Id}");
            Console.WriteLine($"Nombre: {product.Name}");
            Console.WriteLine($"Descripción: {product.Description}");

            return product.Id;
        }
        catch (StripeException e)
        {
            throw new InvalidOperationException($"Error: {e.Message}");
        }
    }

    public static string CreatePaymentMethod(string cardToken)
    {
        try
        {
            var options = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Token = cardToken
                }
            };

            var service = new PaymentMethodService();
            var paymentMethod = service.Create(options);

            Console.WriteLine("Método de pago creado exitosamente:");
            Console.WriteLine($"ID: {paymentMethod.Id}");
            Console.WriteLine($"Tipo: {paymentMethod.Type}");
            Console.WriteLine($"Marca: {paymentMethod.Card.Brand}");

            return paymentMethod.Id;
        }
        catch (StripeException e)
        {
            throw new InvalidOperationException($"Error: {e.Message}");
        }
    }

    public static void AttachPaymentMethod(string customerId, string paymentMethodId)
    {
        try
        {
            var service = new PaymentMethodService();
            var paymentMethod = service.Attach(paymentMethodId, new PaymentMethodAttachOptions
            {
                Customer = customerId
            });

            Console.WriteLine("Método de pago asociado al cliente exitosamente:");
            Console.WriteLine($"ID: {paymentMethod.Id}");
            Console.WriteLine($"Tipo: {paymentMethod.Type}");
            Console.WriteLine($"Marca: {paymentMethod.Card.Brand}");
        }
        catch (StripeException e)
        {
            throw new InvalidOperationException($"Error: {e.Message}");
        }
    }

}
