using System;
using System.Collections.Generic;
using PracticeRecord.ViewModels;
using PracticeRecord.Views;
using Xamarin.Forms;

namespace PracticeRecord
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            this.InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
