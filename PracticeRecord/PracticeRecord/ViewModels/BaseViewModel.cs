using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using PracticeRecord.Models;
using PracticeRecord.Services;

namespace PracticeRecord.ViewModels
{
    using System.Diagnostics;
    using System.Linq;
    using Xamarin.Essentials;


    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<PracticeItem> DataStore => DependencyService.Get<IDataStore<PracticeItem>>();

        private string title = string.Empty;
        private Color done;
        private Color notDone;
        private Color primary;

        public Color Done
        {
            get => this.done;
            set => this.SetProperty(ref this.done, value);
        }
        public Color NotDone
        {
            get => this.notDone;
            set => this.SetProperty(ref this.notDone, value);
        }
        public Color Primary
        {
            get => this.primary;
            set => this.SetProperty(ref this.primary, value);
        }

        public string Version => VersionTracking.CurrentVersion;

        public BaseViewModel()
        {
            VersionTracking.Track();
            if (Application.Current is App app)
            {
                var doneResourcePair = app?.Resources.First(res => res.Key == "Done");
                {
                    var resourcePair = doneResourcePair.Value;
                    this.Done = (Color)resourcePair.Value;
                }
                var notDoneResourcePair = app?.Resources.First(res => res.Key == "NotDone");
                {
                    var resourcePair = notDoneResourcePair.Value;
                    this.NotDone = (Color)resourcePair.Value;
                }
                var primaryResourcePair = app?.Resources.First(res => res.Key == "Primary");
                {
                    var resourcePair = primaryResourcePair.Value;
                    this.Primary = (Color)resourcePair.Value;
                }
            }
        }

        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
