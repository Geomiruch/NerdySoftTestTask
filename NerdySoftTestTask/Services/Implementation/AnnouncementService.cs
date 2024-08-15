using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NerdySoftTestTask.Data;
using NerdySoftTestTask.DTO;
using NerdySoftTestTask.Models;

namespace NerdySoftTestTask.Services.Implementation
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AnnouncementService> _logger;

        public AnnouncementService(ApplicationDbContext context, IMapper mapper, ILogger<AnnouncementService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AnnouncementDTO>> GetAllAnnouncementsAsync()
        {
            try
            {
                var announcements = await _context.Announcements.ToListAsync();
                return announcements.Select(_mapper.Map<AnnouncementDTO>);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all announcements.");
                throw;
            }
        }

        public async Task<AnnouncementDetailsDTO> GetAnnouncementByIdAsync(Guid id)
        {
            try
            {
                var announcement = await _context.Announcements.FindAsync(id);
                if (announcement == null)
                    return null;

                var announcementDTO = _mapper.Map<AnnouncementDetailsDTO>(announcement);
                announcementDTO.SimilarAnnouncements = (await GetSimilarAnnouncementsAsync(id)).ToList();
                return announcementDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the announcement with ID {Id}.", id);
                throw;
            }
        }

        public async Task<Announcement> AddAnnouncementAsync(Announcement announcement)
        {
            try
            {
                announcement.Id = Guid.NewGuid();
                _context.Announcements.Add(announcement);
                await _context.SaveChangesAsync();
                return announcement;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new announcement.");
                throw;
            }
        }

        public async Task<Announcement> UpdateAnnouncementAsync(Announcement announcement)
        {
            try
            {
                _context.Entry(announcement).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return announcement;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the announcement with ID {Id}.", announcement.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAnnouncementAsync(Guid id)
        {
            try
            {
                var announcement = await _context.Announcements.FindAsync(id);
                if (announcement == null)
                    return false;

                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the announcement with ID {Id}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<AnnouncementDTO>> GetSimilarAnnouncementsAsync(Guid id)
        {
            try
            {
                var target = await _context.Announcements.FindAsync(id);
                if (target == null)
                    return Enumerable.Empty<AnnouncementDTO>();

                var targetTitleWords = GetWords(target.Title);
                var targetDescriptionWords = GetWords(target.Description);

                var similarAnnouncements = await _context.Announcements
                    .Where(a => a.Id != id)
                    .ToListAsync();

                var result = similarAnnouncements
                    .Where(a => WordsIntersect(GetWords(a.Title), targetTitleWords) && WordsIntersect(GetWords(a.Description), targetDescriptionWords))
                    .OrderByDescending(a => a.DateAdded)
                    .Take(3)
                    .Select(_mapper.Map<AnnouncementDTO>);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving similar announcements for the announcement with ID {Id}.", id);
                throw;
            }
        }

        private List<string> GetWords(string text)
        {
            return text
                .ToLower()  
                .Split(new char[] { ' ', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();
        }

        private bool WordsIntersect(List<string> words1, List<string> words2)
        {
            return words1.Intersect(words2).Any();
        }
    }
}
