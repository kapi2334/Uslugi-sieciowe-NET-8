using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    [Route("/api/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleError()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        if (exceptionFeature != null)
        {
            // Logowanie błędu
            Console.WriteLine(exceptionFeature.Error);

            // Zwróć odpowiedź dla klienta
            return Problem(detail: exceptionFeature.Error.Message, title: "Error occured");
        }

        return Problem(); // Zwraca odpowiedź 500 Internal Server Error bez szczegółów
    }
}