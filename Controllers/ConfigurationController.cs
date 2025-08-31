using ConfigurationManagement.DAL.Entities;
using ConfigurationManagement.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationController : ControllerBase
{
	private readonly AppDbContext _context;

	public ConfigurationController(AppDbContext context)
	{
		_context = context;
	}

	// GET: api/configuration?applicationName=SERVICE-A&filter=Site
	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] string applicationName, [FromQuery] string? filter = null)
	{
		var query = _context.ConfigurationItems
							.Where(c => c.ApplicationName == applicationName && c.IsActive);

		if (!string.IsNullOrEmpty(filter))
			query = query.Where(c => c.Name.Contains(filter));

		var items = await query.ToListAsync();
		return Ok(items);
	}

	// GET: api/configuration/id
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		var item = await _context.ConfigurationItems.FindAsync(id);
		if (item == null) return NotFound();
		return Ok(item);
	}

	// POST: api/configuration
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] ConfigurationItem item)
	{
		_context.ConfigurationItems.Add(item);
		await _context.SaveChangesAsync();
		return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
	}

	// PUT: api/configuration/id
	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, [FromBody] ConfigurationItem item)
	{
		var existing = await _context.ConfigurationItems.FindAsync(id);
		if (existing == null) return NotFound();

		existing.Name = item.Name;
		existing.Type = item.Type;
		existing.Value = item.Value;
		existing.IsActive = item.IsActive;
		existing.ApplicationName = item.ApplicationName;

		await _context.SaveChangesAsync();
		return NoContent();
	}

	// DELETE: api/configuration/id
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var existing = await _context.ConfigurationItems.FindAsync(id);
		if (existing == null) return NotFound();

		_context.ConfigurationItems.Remove(existing);
		await _context.SaveChangesAsync();
		return NoContent();
	}
}
