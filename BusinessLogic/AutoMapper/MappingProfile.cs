using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Entities.DataTransferObjects;

namespace BusinessLogic.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameDto>()
                .ForMember(g => g.Genres,
                opt => opt.MapFrom(x => string.Join(", ", x.Genres.Select(genre => genre.Genre.Name))));
            CreateMap<Achievement, AchievementDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<GameForCreationDto, Game>();
            CreateMap<AchievementForCreationDto, Achievement>();
            CreateMap<GenreForCreationDto, Genre>();
            CreateMap<GameForUpdateDto, Game>().ReverseMap();
            CreateMap<GenreForUpdateDto, Genre>().ReverseMap();
            CreateMap<AchievementForUpdateDto, Achievement>().ReverseMap();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
