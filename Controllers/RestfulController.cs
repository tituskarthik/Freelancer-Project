using freelancers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class FreelancersController : ControllerBase
{
    private readonly AppDbContext _context;

    public FreelancersController(AppDbContext context)
    {
        _context = context;
    }

    // @POST - Register a new freelancer
    [HttpPost]
    public async Task<IActionResult> CreateFreelancer([FromBody] Freelancer freelancer)
    {
        _context.Freelancers.Add(freelancer);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetFreelancerById), new { id = freelancer.Id }, freelancer);
    }

    // @GET - Get all freelancers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Freelancer>>> GetAllFreelancers()
    {
        return await _context.Freelancers
                             .Include(f => f.Skillsets)
                             .Include(f => f.Hobbies)
                             .Where(f => !f.IsArchived)
                             .ToListAsync();
    }

    // @GET - Wildcard search by username or email
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Freelancer>>> SearchFreelancers(string query)
    {
        return await _context.Freelancers
                             .Include(f => f.Skillsets)
                             .Include(f => f.Hobbies)
                             .Where(f => !f.IsArchived &&
                                        (f.Username.Contains(query) || f.Email.Contains(query)))
                             .ToListAsync();
    }

    // @GET - Get freelancer by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Freelancer>> GetFreelancerById(int id)
    {
        var freelancer = await _context.Freelancers
                                       .Include(f => f.Skillsets)
                                       .Include(f => f.Hobbies)
                                       .FirstOrDefaultAsync(f => f.Id == id);

        if (freelancer == null)
            return NotFound();

        return freelancer;
    }

    // @PUT - Update freelancer details
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFreelancer(int id, [FromBody] Freelancer freelancer)
    {
        if (id != freelancer.Id)
            return BadRequest();

        _context.Entry(freelancer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Freelancers.Any(f => f.Id == id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    // @DELETE - Delete a freelancer
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFreelancer(int id)
    {
        var freelancer = await _context.Freelancers.FindAsync(id);

        if (freelancer == null)
            return NotFound();

        _context.Freelancers.Remove(freelancer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Archive a freelancer
    [HttpPost("{id}/archive")]
    public async Task<IActionResult> ArchiveFreelancer(int id)
    {
        var freelancer = await _context.Freelancers.FindAsync(id);

        if (freelancer == null)
            return NotFound();

        freelancer.IsArchived = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Unarchive a freelancer
    [HttpPost("{id}/unarchive")]
    public async Task<IActionResult> UnarchiveFreelancer(int id)
    {
        var freelancer = await _context.Freelancers.FindAsync(id);

        if (freelancer == null)
            return NotFound();

        freelancer.IsArchived = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

