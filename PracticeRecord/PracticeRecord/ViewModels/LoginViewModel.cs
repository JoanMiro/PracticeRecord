using PracticeRecord.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PracticeRecord.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            this.LoginCommand = new Command(this.OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}
