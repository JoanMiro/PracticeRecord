using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PracticeRecord.Views
{
    using ViewModels;

    public partial class RepertoireRandomiserPage : ContentPage
    {
        public RepertoireRandomiserPage()
        {
            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.ViewModel.RefreshAllCommand.Execute(null);
        }

        private RepertoireRandomiserViewModel ViewModel => (RepertoireRandomiserViewModel)this.BindingContext;

    }
}