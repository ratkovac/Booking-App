using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Domain.RepositoryInterface;
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
            { typeof(BookingApp.Domain.RepositoryInterface.IGradeTourRepository), new GradeTourRepository() },
            { typeof(BookingApp.Domain.RepositoryInterface.ITourInstanceRepository), new TourInstanceRepository() },
            { typeof(BookingApp.Domain.RepositoryInterface.IVoucherRepository), new VoucherRepository() },
            { typeof(BookingApp.Domain.RepositoryInterface.ITouristRepository), new TouristRepository() },
            { typeof(BookingApp.Domain.RepositoryInterface.ITourReservationRepository), new TourReservationRepository() },
            { typeof(BookingApp.Repository.RepositoryInterface.IAccommodationReservationRepository), new AccommodationReservationRepository() },
            { typeof(BookingApp.Domain.RepositoryInterface.ITourGuestRepository), new TourGuestRepository() },
            { typeof(BookingApp.Repository.RepositoryInterface.IGradeAccommodationRepository), new GradeAccommodationRepository() },
            { typeof(BookingApp.Domain.RepositoryInterface.IFastDriveRepository), new FastDriveRepository() }, 
            { typeof(BookingApp.Repository.RepositoryInterface.IDelayReservationRepository), new DelayReservationRepository() }
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