using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.Data;
using VerbTrainer.Models;
using HebrewVerbs;
using VerbTrainer.ViewModels;
using Newtonsoft.Json;

namespace VerbTrainer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly VerbTrainerDbContext _dbContext;

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
                Id = verb.Id,
                Name = verb.Name,
                Binyan = binyan.Name,
                Root = verb.Root,
                Meaning = verb.Meaning
            })
    .ToList();

        List<Conjugation> conjugations = _dbContext.Conjugations
            .Join(_dbContext.Tenses,
            conjugation => conjugation.TenseId,
            tense => tense.Id,
            (conjugation, tense) => new Conjugation
            {
                Meaning = conjugation.Meaning,
                Tense = tense.Name,
                Text = conjugation.Text,
                VerbId = conjugation.VerbId,
                Transcription = conjugation.Transcription
            }).ToList();

        string VerbsConjugations = SerializeConjugations(conjugations);

        var VerbConjViewModel = new IndexViewModel
        {
            Verbs = verbs,
            Conjugations = VerbsConjugations
        };

        return View(VerbConjViewModel);
    }

    [HttpGet]
    public IActionResult GetVerbConjugations(int id)
    {
        var conjugations = _dbContext.Conjugations
            .Join(_dbContext.Tenses,
            conjugation => conjugation.TenseId,
            tense => tense.Id,
            (conjugation, tense) => new Conjugation
            {
                Meaning = conjugation.Meaning,
                Tense = tense.Name,
                Text = conjugation.Text,
                VerbId = conjugation.VerbId,
                Transcription = conjugation.Transcription
            })
            .Where(c => c.VerbId == id).ToList();
        return Json(conjugations);
    }


    private string SerializeConjugations(List<Conjugation> conjugations)
    {
        return JsonConvert.SerializeObject(conjugations.Select(c => new
        {
            VerbId = c.VerbId,
            Tense = c.Tense,
            Text = c.Text,
            Meaning = c.Meaning,
            Transcription = c.Transcription
        }));
    }

  

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

