using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BookingApp.ViewModel.Driver
{
    public class DrivesViewModel : INotifyPropertyChanged
    {
        private readonly DriveRepository _driveRepository;
        private readonly UserRepository _userRepository;
        private readonly UnsuccessfulDrivesRepository _unsuccessfulDriveRepository;
        private readonly DriverStatsUpdateRepository _driverStatsUpdateRepository;
        private readonly DriverStatsRepository _driverStatsRepository;

        public ObservableCollection<DriveDTO> ListDrive { get; set; } = new ObservableCollection<DriveDTO>();
        public User LoggedInUser { get; }

        private bool _isOverlayVisible;
        public bool IsOverlayVisible
        {
            get { return _isOverlayVisible; }
            set
            {
                if (_isOverlayVisible != value)
                {
                    _isOverlayVisible = value;
                    OnPropertyChanged(nameof(IsOverlayVisible));
                }
            }
        }
        private string _colorOne = "PaleTurquoise";
        public string ColorOne
        {
            get { return _colorOne; }
            set
            {
                if (_colorOne != value)
                {
                    _colorOne = value;
                    OnPropertyChanged(nameof(ColorOne));
                }
            }
        }

        private string _colorTwo = "LightGray";

        public event PropertyChangedEventHandler PropertyChanged;

        public string ColorTwo
        {
            get { return _colorTwo; }
            set
            {
                if (_colorTwo != value)
                {
                    _colorTwo = value;
                    OnPropertyChanged(nameof(ColorTwo));
                }
            }
        }

        public DrivesViewModel(User user)
        {
            LoggedInUser = user;
            _userRepository = new UserRepository();
            _driveRepository = new DriveRepository();
            _unsuccessfulDriveRepository = new UnsuccessfulDrivesRepository();
            _driverStatsRepository = new DriverStatsRepository();
            _driverStatsUpdateRepository = new DriverStatsUpdateRepository();
            //LoadDrives();
        }

        /*private void LoadDrives()
        {
            ListDrive.Clear();
            var drives = _driveRepository.GetDrivesByDriver(LoggedInUser);
            foreach (var drive in drives)
            {
                DriveDTO driveDTO = new DriveDTO(drive);
                if (driveDTO.Date.Date == DateTime.Today) {
                    ListDrive.Add(driveDTO);
                }
            }
        }*/

        public void CancelDrive(DriveDTO selectedDrive)
        {
            if (selectedDrive != null)
            {
                _unsuccessfulDriveRepository.Save(selectedDrive.ToDrive());
                _driveRepository.Delete(selectedDrive.ToDrive());
                var update = new DriverStatsUpdate(LoggedInUser.Id);
                update.CancelledDrivesUpdate = 1;
                _driverStatsUpdateRepository.Create(update);
                var stats = _driverStatsRepository.GetById(LoggedInUser.Id);
                stats.CancelledDrives += 1;
                _driverStatsRepository.Update(stats);
                //LoadDrives();

            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void RefreshDriveList()
        {
            //LoadDrives();
            IsOverlayVisible = false;
        }
    }
}
