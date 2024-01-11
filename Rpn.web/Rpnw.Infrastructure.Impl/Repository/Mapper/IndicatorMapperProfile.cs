//using AutoMapper;
//using Rpnw.Domain.Enum;
//using Rpnw.Domain.Model;
//using Rpnw.Infrastructure.Repository.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rpnw.Infrastructure.Impl.Repository.Mapper
//{
//    public class IndicatorMapperProfile : Profile
//    {

//        public IndicatorMapperProfile()
//        {
//            // using contructor to map Db entity to domain model as domain.setters are private
//            CreateMap<IndicatorEntity, Indicator>()
//                .ConstructUsing(db => new Indicator(
//                    db.Id.Value,
//                    db.Name,
//                    db.Description,
//                    db.Category,
//                    EnumConverter<ValueTypeEnum>.ConvertFromString(db.Type)
//                    )
//                );

//            CreateMap<Indicator, IndicatorEntity>()
//                .ForMember(dest => dest.Type,opt => opt.MapFrom(src => src.ValueType.ToString()));

//            CreateMap<IndicatorSearchCriteria, IndicatorEntity>()
//               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ValueType.HasValue?src.ValueType.ToString():null));
//        }



//    }
//}
