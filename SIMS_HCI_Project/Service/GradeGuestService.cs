using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;

namespace BookingApp.Service
{
    public class GradeGuestService
    {
        private IGradeGuestRepository _gradeGuestRepository;

        public GradeGuestService()
        {
            _gradeGuestRepository = Injector.CreateInstance<IGradeGuestRepository>();
        }

        public List<GradeGuest> GetAllByUser(int userId)
        {
            return _gradeGuestRepository.GetAllByUser(userId);
        }

        public List<GradeGuest> GetAll()
        {
            return _gradeGuestRepository.GetAll();
        }
    }
}
