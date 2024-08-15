using Microsoft.AspNetCore.Mvc;
using NerdySoftTestTask.Models;
using NerdySoftTestTask.Services;

namespace NerdySoftTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _service;
        private readonly ILogger<AnnouncementsController> _logger;

        public AnnouncementsController(IAnnouncementService service, ILogger<AnnouncementsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnnouncements()
        {
            try
            {
                var announcements = await _service.GetAllAnnouncementsAsync();
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting announcements.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnnouncement(Guid id)
        {
            try
            {
                var announcement = await _service.GetAnnouncementByIdAsync(id);
                if (announcement == null)
                    return NotFound($"Announcement with ID {id} not found.");

                return Ok(announcement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the announcement with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAnnouncement([FromBody] Announcement announcement)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var addedAnnouncement = await _service.AddAnnouncementAsync(announcement);
                return CreatedAtAction(nameof(GetAnnouncement), new { id = addedAnnouncement.Id }, addedAnnouncement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the announcement.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnnouncement(Guid id, [FromBody] Announcement announcement)
        {
            if (id != announcement.Id)
                return BadRequest("Announcement ID mismatch.");

            try
            {
                await _service.UpdateAnnouncementAsync(announcement);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the announcement with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid id)
        {
            try
            {
                var success = await _service.DeleteAnnouncementAsync(id);
                if (!success)
                    return NotFound($"Announcement with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the announcement with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
