using ConfigurationManagement.DAL;
using ConfigurationManagement.DAL.Entities;
using ConfigurationManagement.Library;
using ConfigurationManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ConfigurationMvcController : Controller
{
    private readonly AppDbContext _context;
    private readonly ConfigurationReader _configurationReader;

    public ConfigurationMvcController(AppDbContext context)
    {
        _context = context;

        _configurationReader = new ConfigurationReader(
            applicationName: "",
            connectionString: "Server=host.docker.internal,1433;Database=ConfigDB;User Id=sa;Password=SIOgtBQKzXIVfq43;TrustServerCertificate=True;",
            refreshTimerIntervalInMs: 5000
        );
    }



    public async Task<IActionResult> Index(string? filter)
    {

        var allItems = await _context.ConfigurationItems
            .Where(c => c.IsActive)
            .ToListAsync();


        var items = allItems.Select(c =>
        {
            try
            {
                object value;
                switch (c.Type.ToLower())
                {
                    case "int":
                        value = _configurationReader.GetValue<int>(c.Name);
                        break;
                    case "bool":
                        value = _configurationReader.GetValue<bool>(c.Name);
                        break;
                    case "double":
                        value = _configurationReader.GetValue<double>(c.Name);
                        break;
                    default:
                        value = _configurationReader.GetValue<string>(c.Name);
                        break;
                }

                return new ConfigurationItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    Value = value?.ToString() ?? string.Empty,
                    IsActive = c.IsActive,
                    ApplicationName = c.ApplicationName
                };
            }
            catch
            {

                return new ConfigurationItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    Value = c.Value,
                    IsActive = c.IsActive,
                    ApplicationName = c.ApplicationName
                };
            }
        }).ToList();

        if (!string.IsNullOrEmpty(filter))
            items = items.Where(i => i.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();

        return View(items);
    }


    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ConfigurationItemViewModel model)
    {
        if (ModelState.IsValid)
        {
            _context.ConfigurationItems.Add(new ConfigurationItem
            {
                Name = model.Name,
                Type = model.Type,
                Value = model.Value,
                IsActive = model.IsActive,
                ApplicationName = model.ApplicationName
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }


    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _context.ConfigurationItems.FindAsync(id);
        if (entity == null) return NotFound();

        var model = new ConfigurationItemViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
            Value = entity.Value,
            IsActive = entity.IsActive,
            ApplicationName = entity.ApplicationName
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ConfigurationItemViewModel model)
    {
        if (ModelState.IsValid)
        {
            var entity = await _context.ConfigurationItems.FindAsync(model.Id);
            if (entity == null) return NotFound();

            entity.Name = model.Name;
            entity.Type = model.Type;
            entity.Value = model.Value;
            entity.IsActive = model.IsActive;
            entity.ApplicationName = model.ApplicationName;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }


    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.ConfigurationItems.FindAsync(id);
        if (entity == null) return NotFound();

        _context.ConfigurationItems.Remove(entity);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
