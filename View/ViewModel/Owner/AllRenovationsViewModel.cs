using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using CLI.Observer;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.View.ViewModel.Owner
{
    public class AllRenovationsViewModel : IObserver
    {
        public ObservableCollection<RenovationDTO> renovations { get; set; }
        private User user { get; set; }
        private RenovationService renovationService;
        private ImageService imageService;
        public ICommand SelectRenovationCommand { get; private set; }
        public ICommand CancelRenovationCommand { get; private set; }
        public RenovationDTO selectedRenovation { get; set; }
        public AllRenovationsViewModel(User user)
        {
            this.user = user;
            renovationService = new RenovationService();
            imageService = new ImageService();
            renovationService.Subscribe(this);
            renovations = new ObservableCollection<RenovationDTO>();
            Update();
            SelectRenovationCommand = new RelayCommand<RenovationDTO>(SelectRenovation);
            CancelRenovationCommand = new RelayCommand(CancelRenovation);
        }
        public void allRenovations()
        {
            renovations.Clear();
            foreach(var renovation in renovationService.GetAll())
            {
                if(user.Id == renovation.Accommodation.User.Id)
                {
                    string imagePath;
                    Image frontImage = imageService.GetByAccommodationId(renovation.Accommodation.Id);
                    if (frontImage != null)
                    {
                        imagePath = frontImage.Path;
                    }
                    else
                    {
                        imagePath = "/View/Owner/noimage.png";
                    }
                    renovations.Add(new RenovationDTO
                    {
                        Id = renovation.Id,
                        AccommodationName = renovation.Accommodation.Name,
                        StartRenovationDate = renovation.StartDate,
                        EndRenovationDate = renovation.EndDate,
                        Description = renovation.Description,
                        ImageFrontPath = imagePath,
                        Accommodation = renovation.Accommodation,
                        Warning = HowManyDaysToCancel(renovation)
                    });
                }
            }
        }
        private string HowManyDaysToCancel(Renovations renovation)
        {
            int daysToCancel = renovationService.DaysToCancel(renovation.StartDate);
            if (daysToCancel >= 5)
            {
                return (daysToCancel - 5).ToString() + " more days to cancel this renovation!"; 
            }
            else
            {
                return "You can't cancel this renovation!";
            }
        }
        private void SelectRenovation(RenovationDTO renovationDTO)
        {
            if (renovationDTO != null)
            {
                int daysToCancel = renovationService.DaysToCancel(renovationDTO.StartRenovationDate);

                if (daysToCancel < 5)
                {
                    MessageBox.Show("The cancellation period for the renovation has passed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedRenovation != null && selectedRenovation == renovationDTO)
                {
                    selectedRenovation.IsSelected = false;
                    selectedRenovation = null;
                }
                else
                {
                    if (selectedRenovation != null)
                    {
                        selectedRenovation.IsSelected = false;
                    }
                    renovationDTO.IsSelected = true;
                    selectedRenovation = renovationDTO;
                }
            }
        }
        private void CancelRenovation()
        {
            if(selectedRenovation != null)
            {
                Renovations renovation = selectedRenovation.ToRenovation();
                renovation.Id = selectedRenovation.Id;
                renovation.Accommodation = selectedRenovation.Accommodation;
                renovationService.Delete(renovation);
                MessageBox.Show("You have successfully cancelled the selected renovation", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Update();
            }
            else
            {
                MessageBox.Show("Select the renovation you want to cancel!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }
        public void Update()
        {
            allRenovations();
        }
    }
}
