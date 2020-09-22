using System.ComponentModel;
using Xamarin.Forms;
using PracticeRecord.ViewModels;

namespace PracticeRecord.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
            this.BindingContext = new ItemDetailViewModel();
        }
    }
}