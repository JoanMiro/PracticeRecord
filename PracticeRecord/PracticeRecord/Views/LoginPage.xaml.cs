using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticeRecord.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PracticeRecord.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
    }
}