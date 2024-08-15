using AutoMapper;
using NerdySoftTestTask.DTO;
using NerdySoftTestTask.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NerdySoftTestTask.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Announcement, AnnouncementDTO>().ReverseMap();
            CreateMap<Announcement, AnnouncementDetailsDTO>().ReverseMap();
        }
    }
}
