namespace StripeProgram;

using DotNetEnv;
using StripeManager;
class Program
{
    static string GetEnv(string key)
    {
        string? value = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException(
                $"La variable de entorno {key} no está configurada en el archivo '/.env'."
            );
        }
        return value;
    }

    static void Main()
    {
        try
        {
            Random random = new();
            Env.Load();
            Console.WriteLine("Pruebas con Stripe");
            StripeMgr.Connect(GetEnv("STRIPE_SECRET_KEY"));

            Console.WriteLine("\nCreando cliente...");
            string name = $"customer_{random.NextDouble()}";
            string customerId = StripeMgr.CreateCustomer(
                email: $"{name}@exp.com",
                name: name,
                description: "Ejemplo de creación de cliente",
                phone: "555-555-5555"
            );

            Console.WriteLine("\nCreando Producto...");
            string productId = StripeMgr.CreateProduct(
                $"Producto x{random.NextDouble()}", "Producto de prueba", 1050
            ); // 10.50

            Console.WriteLine("\nCreando metodo de pago...");
            string paymentMethodId = StripeMgr.CreatePaymentMethod("tok_visa");

            Console.WriteLine("\nAsociando metodo de pago al cliente...");
            StripeMgr.AttachPaymentMethod(customerId, paymentMethodId);

            Console.WriteLine("\nObteniendo detalles de Producto...");
            var productDetails = StripeMgr.GetProduct(productId);
            Console.WriteLine($"Nombre: {productDetails.Name}");
            Console.WriteLine($"Descripción: {productDetails.Description}");
            Console.WriteLine($"Precio: {productDetails.Price / 100.0m} {productDetails.Currency}");
            
            Console.WriteLine("\nRealizando compra...");
            StripeMgr.BuyProduct(customerId, productId, 2);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
