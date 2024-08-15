using NerdySoftTestTask.DTO;
using NerdySoftTestTask.Models;

namespace NerdySoftTestTask.Services
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<AnnouncementDTO>> GetAllAnnouncementsAsync();
        Task<AnnouncementDetailsDTO> GetAnnouncementByIdAsync(Guid id);
        Task<Announcement> AddAnnouncementAsync(Announcement announcement);
        Task<Announcement> UpdateAnnouncementAsync(Announcement announcement);
        Task<bool> DeleteAnnouncementAsync(Guid id);
        Task<IEnumerable<AnnouncementDTO>> GetSimilarAnnouncementsAsync(Guid id);
    }
}
