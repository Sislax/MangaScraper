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
    public class ImagePositionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ImagePositionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ImagePositions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImagePosition>>> GetImagePositions()
        {
            return await _context.ImagePositions.ToListAsync();
        }

        // GET: api/ImagePositions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImagePosition>> GetImagePosition(int id)
        {
            var imagePosition = await _context.ImagePositions.FindAsync(id);

            if (imagePosition == null)
            {
                return NotFound();
            }

            return imagePosition;
        }

        // PUT: api/ImagePositions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagePosition(int id, ImagePosition imagePosition)
        {
            if (id != imagePosition.Id)
            {
                return BadRequest();
            }

            _context.Entry(imagePosition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagePositionExists(id))
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

        // POST: api/ImagePositions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImagePosition>> PostImagePosition(ImagePosition imagePosition)
        {
            _context.ImagePositions.Add(imagePosition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImagePosition", new { id = imagePosition.Id }, imagePosition);
        }

        // DELETE: api/ImagePositions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagePosition(int id)
        {
            var imagePosition = await _context.ImagePositions.FindAsync(id);
            if (imagePosition == null)
            {
                return NotFound();
            }

            _context.ImagePositions.Remove(imagePosition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImagePositionExists(int id)
        {
            return _context.ImagePositions.Any(e => e.Id == id);
        }
    }
}
