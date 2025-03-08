namespace StripeProgram;

using DotNetEnv;
using StripePayment;
class Program
{
    static string GetEnv(string key)
    {
        string? value = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"La variable de entorno {key} no está configurada.");
        }
        return value;
    }

    static void Main(string[] args)
    {
        try
        {
            Env.Load();
            Console.WriteLine("Pruebas con Stripe");
            StripeMgr stripeMgr = new(GetEnv("STRIPE_SECRET_KEY"));

            Console.WriteLine("\nCreando cliente...");
            StripeMgr.CreateCustomer(
                email: "Ken@exp.com", 
                name: "Kenysdev", 
                description: "Ejemplo de creación de cliente", 
                phone: "555-555-5555"
            );

        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
