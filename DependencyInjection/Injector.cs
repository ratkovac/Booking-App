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
            { typeof(ITourGuestRepository), new TourGuestRepository() },

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