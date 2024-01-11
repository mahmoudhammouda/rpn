using AutoMapper;
using Rpnw.CrossCutting;
using Rpnw.Domain.Enum;
using Rpnw.Domain.Model;
using Rpnw.Infrastructure.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Infrastructure.Impl.Repository.Mapper
{
    public class MeasureMapperProfile : Profile
    {

        public MeasureMapperProfile()
        {
            // using contructor to map Db entity to domain model as domain.setters are private
            //CreateMap<MeasureEntity, Measure>()
            //    .ConstructUsing(db => new Measure(
            //        db.Id,
            //        new Indicator(
            //            db.IndicatorId,
            //            db.IndicatorName,
            //            db.IndicatorDescription,
            //            db.IndicatorCategory,
            //            EnumConverter<ValueTypeEnum>.ConvertFromString(db.IndicatorValueType)
            //            ),
            //        new Source(
            //            db.SourceId,
            //            db.SourceName,
            //            EnumConverter<SourceTypeEnum>.ConvertFromString(db.SourceType)
            //            ),
            //        new MeasureValue(db.Value,db.Unity),
            //        DateTimeConverter.ConvertToDateTimeUtc(db.ObservationTime)

            //        )
            //    ); ;

            //CreateMap<Measure, MeasureEntity>()
            //    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ValueType.ToString()));


        }
    }
}

