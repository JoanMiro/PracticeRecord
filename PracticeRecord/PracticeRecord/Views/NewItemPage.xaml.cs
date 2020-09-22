using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PracticeRecord.Models;
using PracticeRecord.ViewModels;

namespace PracticeRecord.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            this.InitializeComponent();
            this.BindingContext = new NewItemViewModel();
        }
    }
}