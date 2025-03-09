namespace StripeManager;

using System;
using Stripe;

class StripeMgr
{
    public static void Connect(string secretKey)
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

            var customerService = new CustomerService();
            customerService.Update(customerId, new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethodId
                }
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

    public class ProductDetails
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public long Price { get; set; }
        public required string Currency { get; set; }
    }

    public static ProductDetails GetProduct(string productId)
    {
        try
        {
            var service = new ProductService();
            var product = service.Get(productId);

            if (product == null)
            {
                throw new InvalidOperationException("El producto o el precio por defecto no se encontraron.");
            }

            var priceService = new PriceService();
            var price = priceService.Get(product.DefaultPriceId);

            return new ProductDetails
            {
                Name = product.Name,
                Description = product.Description,
                Price = price.UnitAmount ?? 0,
                Currency = price.Currency
            };
        }
        catch (StripeException e)
        {
            throw new InvalidOperationException($"Error: {e.Message}");
        }
    }

    public static string BuyProduct(string customerId, string productId, int quantity)
    {
        try
        {
            var productDetails = GetProduct(productId);

            var customerService = new CustomerService();
            var customer = customerService.Get(customerId);

            if (customer.InvoiceSettings.DefaultPaymentMethodId == null)
            {
                throw new InvalidOperationException("El cliente no tiene un método de pago predeterminado.");
            }

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = productDetails.Price * quantity,
                Currency = productDetails.Currency,
                Customer = customerId,
                PaymentMethod = customer.InvoiceSettings.DefaultPaymentMethodId,
                PaymentMethodTypes = new List<string> { "card" },
                Description = $"Compra de {quantity} x {productDetails.Name} (ID: {productId})",
                Confirm = true,
                OffSession = true
            });

            Console.WriteLine("Producto comprado exitosamente:");
            Console.WriteLine($"ID: {paymentIntent.Id}");
            Console.WriteLine($"Estado: {paymentIntent.Status}");

            return paymentIntent.Id;
        }
        catch (StripeException e)
        {
            throw new InvalidOperationException($"Error: {e.Message}");
        }
    }
}
