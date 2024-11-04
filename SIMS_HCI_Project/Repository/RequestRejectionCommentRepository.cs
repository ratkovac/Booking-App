using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class RequestRejectionCommentRepository : IRequestRejectionCommentRepository
    {
        private const string FilePath = "../../../Resources/Data/requestRejectionComment.csv";

        private readonly Serializer<RequestRejectionComment> _serializer;

        private List<RequestRejectionComment> _requestRejectionComments;

        public Subject RequestRejectionCommentSubject;

        public RequestRejectionCommentRepository()
        {
            _serializer = new Serializer<RequestRejectionComment>();
            _requestRejectionComments = _serializer.FromCSV(FilePath);
            RequestRejectionCommentSubject = new Subject();
        }
        public int NextId()
        {
            _requestRejectionComments = _serializer.FromCSV(FilePath);
            if (_requestRejectionComments.Count < 1)
            {
                return 1;
            }
            return _requestRejectionComments.Max(c => c.Id) + 1;
        }

        public List<RequestRejectionComment> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Create(RequestRejectionComment rejectionComment)
        {
            rejectionComment.Id = NextId();
            _requestRejectionComments = _serializer.FromCSV(FilePath);
            _requestRejectionComments.Add(rejectionComment);
            RequestRejectionCommentSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _requestRejectionComments);
        }

        public void Update(RequestRejectionComment entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(RequestRejectionComment entity)
        {
            throw new NotImplementedException();
        }

        public RequestRejectionComment GetById(int key)
        {
            throw new NotImplementedException();
        }
    }
}
