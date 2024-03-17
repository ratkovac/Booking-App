﻿using BookingApp.Model;
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
using System.Windows.Shapes;

namespace BookingApp.View.Driver
{
    /// <summary>
    /// Interaction logic for DriveCreationWindow.xaml
    /// </summary>
    public partial class DriveCreationWindow : Window
    {
        public User LoggedInUser { get; }

        public DriveCreationWindow(User user)
        {
            LoggedInUser = user;
            InitializeComponent();
        }

    }
}
