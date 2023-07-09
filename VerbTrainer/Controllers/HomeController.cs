using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Data;
using VerbTrainer.Models;
using HebrewVerbs;

namespace VerbTrainer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, VerbTrainerDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;

    }

    public IActionResult Index()
    {
        List<Verb> verbs = _dbContext.Verbs
        .Join(_dbContext.Binyanim,
            verb => verb.BinyanId,
            binyan => binyan.Id,
            (verb, binyan) => new Verb
            {
                Name = verb.Name,
                Binyan = binyan.Name,
                Root = verb.Root,
                Meaning = verb.Meaning
            })
    .ToList();
        return View(verbs);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    private readonly VerbTrainerDbContext _dbContext;

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

