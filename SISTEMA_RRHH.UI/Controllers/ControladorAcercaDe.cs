using Microsoft.AspNetCore.Mvc;

public class ControladorAcercaDe : Controller
{
    public IActionResult Index()
    {
        try
        {
            string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "textos", "AcercaDe.txt");

            if (!System.IO.File.Exists(rutaArchivo))
            {
                ViewBag.Contenido = "⚠ El archivo 'acerca.txt' no fue encontrado.";
            }
            else
            {
                ViewBag.Contenido = System.IO.File.ReadAllText(rutaArchivo);
            }
        }
        catch (Exception ex)
        {
            ViewBag.Contenido = "❌ Error al leer el archivo: " + ex.Message;
        }

        return View(); // ← buscará Index.cshtml
    }
}
