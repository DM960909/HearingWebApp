
using HearingApp.Data;
using HearingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class TinnitusController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager; //UserManager handles from Data folder - Application User

    public TinnitusController(ApplicationDbContext context, UserManager<ApplicationUser> userManager )
    {

        _context = context;
        _userManager = userManager;

    }

    [HttpGet]
    public IActionResult Input()
    {
        return View();
    }
    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);

        var scores = await _context.TinnitusScores
            .Where(s => s.UserId == user.Id)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return View(scores);
    }

    //DATE SORTING PREFERENCES START

    [Authorize]

    public async Task<IActionResult> Weekly()
    {
        var user = await _userManager.GetUserAsync(User);
        var scores = await _context.TinnitusScores
            .Where(s => s.UserId == user.Id)
            .OrderByDescending(s => s.CreatedAt) //Self Explanatory
            .ToListAsync();

        var weekly = scores
            .GroupBy(s => System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(

                s.CreatedAt,System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday))
            .Select(g => new WeeklyViewModel
            {
                WeekNumber = g.Key,
                AverageSeverity = g.Average(x => x.SeverityScore),
                AverageCoping = g.Average(x => x.CopingScore),   
                Entries = g.ToList()
            })
            .OrderByDescending(w => w.WeekNumber)
            .ToList();

            return View(weekly);
    }

    [HttpGet]

    public async Task<IActionResult> Calendar (DateTime? date)
    {

        var selectedDate = date ?? DateTime.Today;
        var user = await _userManager.GetUserAsync(User);

        var score = await _context.TinnitusScores
            .Where(s => s.UserId == user.Id && s.CreatedAt.Date == selectedDate.Date)
            .FirstOrDefaultAsync();

        ViewBag.selectedDate = selectedDate;
        return View(score);
    }

    [HttpPost]
    public async Task<IActionResult> Input(TinnitusInputViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);

        var score = new TinnitusScore
        {
            SeverityScore = model.SeverityScore,
            CopingScore = model.CopingScore,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.TinnitusScores.Add(score);
        await _context.SaveChangesAsync();

        return RedirectToAction("Dashboard", "Tinnitus");
    }


}
