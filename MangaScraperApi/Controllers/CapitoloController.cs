using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MangaScraperApi.Data;
using MangaScraperApi.Models.Domain;

namespace MangaScraperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapitoloController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CapitoloController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Capitolo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Capitolo>>> GetCapitolos()
        {
            return await _context.Capitolos.ToListAsync();
        }

        // GET: api/Capitolo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Capitolo>> GetCapitolo(int id)
        {
            var capitolo = await _context.Capitolos.FindAsync(id);

            if (capitolo == null)
            {
                return NotFound();
            }

            return capitolo;
        }

        // PUT: api/Capitolo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCapitolo(int id, Capitolo capitolo)
        {
            if (id != capitolo.Id)
            {
                return BadRequest();
            }

            _context.Entry(capitolo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CapitoloExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Capitolo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Capitolo>> PostCapitolo(Capitolo capitolo)
        {
            _context.Capitolos.Add(capitolo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCapitolo", new { id = capitolo.Id }, capitolo);
        }

        // DELETE: api/Capitolo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCapitolo(int id)
        {
            var capitolo = await _context.Capitolos.FindAsync(id);
            if (capitolo == null)
            {
                return NotFound();
            }

            _context.Capitolos.Remove(capitolo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CapitoloExists(int id)
        {
            return _context.Capitolos.Any(e => e.Id == id);
        }
    }
}
