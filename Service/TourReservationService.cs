﻿using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.View;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourReservationService
    {
        private ITourReservationRepository tourReservationRepository;
        private ITouristRepository touristRepository;
        private ITourInstanceRepository tourInstanceRepository;

        public TourReservationService()
        {
            tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            touristRepository = Injector.CreateInstance<ITouristRepository>();
            tourInstanceRepository = Injector.CreateInstance<ITourInstanceRepository>();
            InitializeGuest();
            InitializeTourInstance();
        }
        private void InitializeGuest()
        {
            foreach (var item in tourReservationRepository.GetAll())
            {
                item.Tourist = touristRepository.GetById(item.TouristId);
            }
        }
        private void InitializeTourInstance()
        {
            foreach (var item in tourReservationRepository.GetAll())
            {
                item.TourInstance = tourInstanceRepository.GetById(item.TourInstanceId);
            }
        }
        public List<TourReservation> GetAllReservations()
        {
            return tourReservationRepository.GetAll();
        }
        public List<TourReservation> GetReservationsByTourInstance(TourInstance tourInstance)
        {
            return tourReservationRepository.GetReservationsByTourInstance(tourInstance);
        }
        public List<TourReservation> GetReservationsByTourInstanceAndState(TourInstance tourInstance, TouristState state)
        {
            return tourReservationRepository.GetReservationsByTourInstanceAndState(tourInstance, state);
        }
        public List<int> GetGuestIdsByTourInstanceAndState(TourInstance tourInstance, TouristState state)
        {
            return GetReservationsByTourInstanceAndState(tourInstance, state).Select(r => r.TouristId).ToList();
        }
        public TourReservation GetReservationByTouristAndTourInstance(TourInstance tourInstance, Tourist tourist)
        {
            return tourReservationRepository.GetReservationByTouristAndTourInstance(tourInstance, tourist);
        }
        public void Create(TourReservation reservation)
        {
            tourReservationRepository.Create(reservation);
        }
        public void Remove(TourReservation reservation)
        {
            tourReservationRepository.Delete(reservation);
        }
        public void UpdateTouristsState(TourInstance tourInstance, TouristState state)
        {
            List<TourReservation> tourReservations = GetReservationsByTourInstance(tourInstance);
            tourReservations.ForEach(r => r.State = state);
            tourReservations.ForEach(r => Update(r));
        }
        public List<TourReservation> GetToursWhichFinished()
        {
            List<TourReservation> toursFinished = new List<TourReservation>();
            List<TourReservation> allTourInstances = GetAllReservations();

            foreach (TourReservation tourReservation in allTourInstances)
            {
                if (tourReservation.TourInstance.State == TourInstanceState.Finished && tourReservation.RatedTour == false)
                {
                    toursFinished.Add(tourReservation);
                }
            }

            return toursFinished;
        }
        public void UpdateTouristsState(int touristId, TourInstance tourInstance, TouristState state)
        {
            TourReservation reservation = GetReservationsByTourInstance(tourInstance).Find(r => r.TouristId == touristId);
            reservation.State = state;
            /*if (state == TouristState.Present)
                reservation.KeyPointWhereGuestArrivedId = tour.ActiveKeyPointId;*/
            Update(reservation);
        }
        public TourReservation GetTourInstanceIdWhereTouristIsWaiting(Tourist tourist)
        {
            return tourReservationRepository.GetTourInstanceIdWhereTouristIsWaiting(tourist);
        }
        public List<int> FindTourInstanceIdsWhereTouristPresent(int touristId)
        {
            List<int> tourIds = new List<int>();
            foreach (TourReservation reservation in GetAllReservations())
            {
                if (reservation.TouristId == touristId && reservation.State == TouristState.Present && reservation.RatedTour != true)

                {
                    tourIds.Add(reservation.TourInstanceId);
                }
            }
            return tourIds;
        }
        public void Update(TourReservation reserevation)
        {
            tourReservationRepository.Update(reserevation);
        }
        public void Subscribe(IObserver observer)
        {
            tourReservationRepository.Subscribe(observer);
        }
        public List<TourReservation> GetReservationsForGuest(int touristId)
        {
            return GetAllReservations().Where(r => r.TouristId == touristId).ToList();
        }
    }
}