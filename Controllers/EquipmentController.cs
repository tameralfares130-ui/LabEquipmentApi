using LabEquipmentApi.Data;
using FluentValidation;
using LabEquipmentApi.Contracts.Equipment;
using Mapster;
using LabEquipmentApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabEquipmentApi.Controllers;

[Route("api/equipment")]
[ApiController]
public class EquipmentController(LabDbContext context, IValidator<CreateEquipmentRequest> createValidator) : ControllerBase
{
    private readonly LabDbContext _context = context;
    private readonly IValidator<CreateEquipmentRequest> _createValidator = createValidator;

    // GET: api/equipment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipmentItems()
    {
        return await _context.EquipmentItems.AsNoTracking().ToListAsync();
    }

    // GET: api/equipment/available
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<EquipmentListItemResponse>>> GetAvailableEquipment()
    {
        var available = await _context.EquipmentItems
            .AsNoTracking()
            .Where(e => e.IsAvailable)
            .OrderBy(e => e.Name)
            .ProjectToType<EquipmentListItemResponse>()
            .ToListAsync();
        return Ok(available);
    }

    // GET: api/equipment/room-summary (optional item 6)
    [HttpGet("room-summary")]
    public async Task<ActionResult<IEnumerable<RoomSummaryResponse>>> GetRoomSummary()
    {
        var summary = await _context.EquipmentItems
            .AsNoTracking()
            .GroupBy(e => e.Room)
            .Select(group => new RoomSummaryResponse
            {
                Room = group.Key,
                TotalCount = group.Count(),
                AvailableCount = group.Count(e => e.IsAvailable)
            })
            .OrderBy(item => item.Room)
            .ToListAsync();

        return Ok(summary);
    }

    // GET: api/equipment/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Equipment>> GetEquipment(int id)
    {
        var equipment = await _context.EquipmentItems.FindAsync(id);
        if (equipment is null) return NotFound();
        return equipment;
    }

    // PUT: api/equipment/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutEquipment(int id, Equipment equipment)
    {
        if (id != equipment.Id) return BadRequest();
        _context.Entry(equipment).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.EquipmentItems.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    // POST: api/equipment
    [HttpPost]
    public async Task<ActionResult<Equipment>> PostEquipment(CreateEquipmentRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return ValidationProblem(ModelState);
        }

        var equipment = new Equipment
        {
            Name = request.Name,
            Room = request.Room,
            IsAvailable = true
        };

        _context.EquipmentItems.Add(equipment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEquipment), new { id = equipment.Id }, equipment);
    }

    // DELETE: api/equipment/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEquipment(int id)
    {
        var equipment = await _context.EquipmentItems.FindAsync(id);
        if (equipment is null) return NotFound();
        _context.EquipmentItems.Remove(equipment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
