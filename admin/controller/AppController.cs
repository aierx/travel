using Microsoft.AspNetCore.Mvc;

namespace webapi.controller;

public class AppController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}