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
    public class VolumeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VolumeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Volume
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Volume>>> GetVolumes()
        {
            return await _context.Volumes.ToListAsync();
        }

        // GET: api/Volume/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Volume>> GetVolume(int id)
        {
            var volume = await _context.Volumes.FindAsync(id);

            if (volume == null)
            {
                return NotFound();
            }

            return volume;
        }

        // PUT: api/Volume/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVolume(int id, Volume volume)
        {
            if (id != volume.Id)
            {
                return BadRequest();
            }

            _context.Entry(volume).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolumeExists(id))
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

        // POST: api/Volume
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Volume>> PostVolume(Volume volume)
        {
            _context.Volumes.Add(volume);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVolume", new { id = volume.Id }, volume);
        }

        // DELETE: api/Volume/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVolume(int id)
        {
            var volume = await _context.Volumes.FindAsync(id);
            if (volume == null)
            {
                return NotFound();
            }

            _context.Volumes.Remove(volume);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VolumeExists(int id)
        {
            return _context.Volumes.Any(e => e.Id == id);
        }
    }
}
