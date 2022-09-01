namespace Footballers
{
    using AutoMapper;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ExportDto;

    public class FootballersProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public FootballersProfile()
        {
/*            CreateMap<Footballer, ExportCoachFootballerDTO>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.PositionType.ToString()));
            CreateMap<Coach, ExportCoachDTO>()
                .ForMember(dest => dest.FootballersCount, opt => opt.MapFrom(src => src.Footballers.Count))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Footballers, opt => opt.MapFrom(src => src.Footballers));*/
        }
    }
}
