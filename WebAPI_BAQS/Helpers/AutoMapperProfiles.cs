using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.DTOs.Area;
using WebAPI_BAQS.DTOs.BAQ;
using WebAPI_BAQS.Entities;

namespace WebAPI_BAQS.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Area, AreaDTO>().ReverseMap();
            CreateMap<AreaCreacionDTO, Area>();

            CreateMap<BaqCabecera, BAQDTO>();
        }
    }
}
