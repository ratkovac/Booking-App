using BookingApp.Model;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class TourRequestDescription : Page
    {
        public TourRequestSegment SelectedSegment { get; set; }
        public TourRequestGuestService tourRequestGuestService;
        public BookingApp.Model.Tourist Tourist { get; set; }
        public List<TourRequestGuest> TourRequestGuestList { get; set; }

        public TourRequestDescription(TourRequestSegment selectedSegment, BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            SelectedSegment = selectedSegment;
            Tourist = tourist;
            tourRequestGuestService = new TourRequestGuestService();
            TourRequestGuestList = tourRequestGuestService.GetAllForSegmentId(SelectedSegment.Id);

            CityTextBox.Text = SelectedSegment.Location.City;
            CountryTextBox.Text = SelectedSegment.Location.Country;
            DescriptionTextBox.Text = SelectedSegment.TourDescription;
            LanguageTextBox.Text = SelectedSegment.Language.Name;
            GuestsTextBox.Text = SelectedSegment.Capacity.ToString();
            StatusTextBox.Text = SelectedSegment.IsAccepted.ToString();
            DurationSpanTextBox.Text = $"{SelectedSegment.StartDate.ToString("dd.MM.yyyy")} - {SelectedSegment.EndDate.ToString("dd.MM.yyyy")}";
            TourGuestsTextBox.Text = string.Join(Environment.NewLine, TourRequestGuestList.Select((guest, index) => $"{index + 1}. {guest.Name}, {guest.Age}"));
        }
    }
}
