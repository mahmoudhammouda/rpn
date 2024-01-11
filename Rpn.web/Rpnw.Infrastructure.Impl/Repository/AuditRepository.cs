//using AutoMapper;
//using Rpnw.Domain.Model;
//using Rpnw.Infrastructure.Repository;
//using Rpnw.Infrastructure.Repository.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rpnw.Infrastructure.Impl.Repository
//{
//    public class IndicatorRepository : IIndicatorRepository
//    {
//        private readonly IGenericRepository<IndicatorEntity> _indicatorEntityRepository;
//        private readonly IMapper _mapper;
//        public IndicatorRepository(
//            IGenericRepository<IndicatorEntity> genericRepository,
//            IMapper mapper
//            )
//        {
//            _indicatorEntityRepository = genericRepository??throw new ArgumentNullException(nameof(genericRepository)); 
//            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
//        }

//        public int Add(Indicator indicator)
//        {
//            var dbIndicator = _mapper.Map<IndicatorEntity>(indicator);
//            var newId =_indicatorEntityRepository.Add(dbIndicator);
//            indicator.SetAndValidateId(newId);
//            return newId;
//        }

//        public IEnumerable<Indicator> GetAll() 
//        {
//            var indicatorDbLst = _indicatorEntityRepository.GetAll();
//            var indicatorLst = _mapper.Map<IEnumerable<IndicatorEntity>, IEnumerable<Indicator>>(indicatorDbLst);

//            return indicatorLst;
//        }

//        public Indicator GetById(int id)
//        {
//            var indicatorDb =_indicatorEntityRepository.GetById(id);
//            var indicator = _mapper.Map<Indicator>(indicatorDb);
//            return indicator;
//        }

//        public bool Update(Indicator indicator)
//        {
//            var indicatorDb = _mapper.Map<IndicatorEntity>(indicator);
//            return _indicatorEntityRepository.Update(indicatorDb);
//        }

//        public void Delete(Indicator indicator)
//        {
//            var indicatorDb = _mapper.Map<IndicatorEntity>(indicator);
//            var isDeleted = _indicatorEntityRepository.Delete(indicatorDb);
//        }

//        public IEnumerable<Indicator> FindByCriteria(IndicatorSearchCriteria searchCriteria)
//        {
//            var indicatorDb = _mapper.Map<IndicatorEntity>(searchCriteria);
//            var indicatorDbLst= _indicatorEntityRepository.FindByCriteria(indicatorDb);
//            var indicatorLst = _mapper.Map<IEnumerable<IndicatorEntity>, IEnumerable<Indicator>>(indicatorDbLst);

//            return indicatorLst;
//        }
//    }
//}
