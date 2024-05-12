using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class RequestRejectionCommentService
    {
        private IRequestRejectionCommentRepository _repository;
        public RequestRejectionCommentService()
        {
            _repository = Injector.CreateInstance<IRequestRejectionCommentRepository>();
        }
        public void Create(RequestRejectionComment requestRejectionComment)
        {
            _repository.Create(requestRejectionComment);
        }
    }
}
