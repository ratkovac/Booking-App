using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DependencyInjection
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IGradeTourRepository), new GradeTourRepository() },
            { typeof(ITourInstanceRepository), new TourInstanceRepository() },
            { typeof(IVoucherRepository), new VoucherRepository() },
            { typeof(ITouristRepository), new TouristRepository() },
            { typeof(ITourReservationRepository), new TourReservationRepository() },
            { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
            { typeof(ITourRepository), new TourRepository() },
            { typeof(ICheckPointRepository), new CheckPointRepository() },
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(ILanguageRepository), new LanguageRepository() },
            { typeof(IAddressRepository), new AddressRepository() },
            { typeof(ITourGuestRepository), new TourGuestRepository() },
            { typeof(IGradeAccommodationRepository), new GradeAccommodationRepository() },
            { typeof(ICanceledReservationRepository), new CanceledReservationRepository() },
            { typeof(IFastDriveRepository), new FastDriveRepository() },
            { typeof(IDelayReservationRepository), new DelayReservationRepository() },
            { typeof(IImageRepository), new ImageRepository() },
            { typeof(IDriverStatsRepository), new DriverStatsRepository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(IReservedDriveRepository), new ReservedDriveRepository() },
            { typeof(ITourImageRepository), new TourImageRepository() },
            { typeof(ITourRequestRepository), new TourRequestRepository() },
            { typeof(ITourRequestSegmentRepository), new TourRequestSegmentRepository() },
            { typeof(ITourRequestGuestRepository), new TourRequestGuestRepository() },
            { typeof(IDriverUnreliableReportRepository), new DriverUnreliableReportRepository() },
            { typeof(IGroupDriveRepository), new GroupDriveRepository() }
        };
        public static T CreateInstance<T>()
        {
            Type type = typeof(T);
            if (Injector._implementations.ContainsKey(type))
            {
                return (T)Injector._implementations[type];
            }
            throw new ArgumentException($"No implementation found for type {type}");
            return default(T);
        }
    }
}