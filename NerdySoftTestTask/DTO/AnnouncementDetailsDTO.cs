namespace NerdySoftTestTask.DTO
{
    public class AnnouncementDetailsDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public List<AnnouncementDTO> SimilarAnnouncements { get; set; }
    }
}
