# 01 PASARELA DE PAGO CON STRIPE
- **Dificultad: Fácil**
- **Lenguaje: C#**

## Ejercicio
Procesa pagos por internet con tarjeta de crédito en más de 135 divisas. [stripe web](https://stripe.com/es)

- Cómo recoger información del usuario.
- Cómo realizar un cargo asociado a un importe.
- Gestión de productos y precios.
- Manejo de errores.

## Información sobre la solución
> **Nota:** El objetivo de esta solución es demostrar el uso básico de la biblioteca de *Stripe* para *.NET*.

### Acerca de Stripe
- **Documentación de Stripe**: [Ver](https://docs.stripe.com/get-started/development-environment?lang=dotnet)
- **Documentación de la API de Stripe**: [Ver](https://docs.stripe.com/api?lang=dotnet)
- **Gestión de API Key**: Usaré DotNetEnv para la gestión básica y controlada de la API Key. Para producción, existen alternativas más robustas. [Documentación de DotNetEnv.](https://www.nuget.org/packages/DotNetEnv/)
- **Hacer pruebas**: [Info.](https://docs.stripe.com/testing)

### Acerca de los archivos del proyecto:
- **Creación del archivo .env**: [Ver el ejemplo.](https://github.com/Kenysdev/logica-aplicada-proyectos/blob/main/Proyectos/01%20-%20Pasarela%20de%20pago%20con%20Stripe/.env.example)
- **Llamadas a la API**: [StripeMgr.cs](https://github.com/Kenysdev/logica-aplicada-proyectos/blob/main/Proyectos/01%20-%20Pasarela%20de%20pago%20con%20Stripe/StripeMgr.cs)
- **Realizar pruebas sobre el uso de StripeMgr.cs**: [Program.cs](https://github.com/Kenysdev/logica-aplicada-proyectos/blob/main/Proyectos/01%20-%20Pasarela%20de%20pago%20con%20Stripe/Program.cs)
- **Dependencias incorporadas**: [Archivo de configuración.csproj](https://github.com/Kenysdev/logica-aplicada-proyectos/blob/main/Proyectos/01%20-%20Pasarela%20de%20pago%20con%20Stripe/01%20-%20Pasarela%20de%20pago%20con%20Stripe.csproj)
- **Archivo de solución de Visual Studio**: [Archivo de solución de Visual Studio.sln](https://github.com/Kenysdev/logica-aplicada-proyectos/blob/main/Proyectos/01%20-%20Pasarela%20de%20pago%20con%20Stripe/01%20-%20Pasarela%20de%20pago%20con%20Stripe.sln)
