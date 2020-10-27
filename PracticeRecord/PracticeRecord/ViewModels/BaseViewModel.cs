//------------------------------------------------------------------
//
// Copyright (c) 2012 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

namespace PracticeRecord.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Models;
    using Services;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class BaseViewModel : INotifyPropertyChanged
    {
        private Color done;
        private bool isBusy;
        private Color notDone;
        private Color primary;

        private string title = string.Empty;

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

        public IDataStore<PracticeItem> DataStore => DependencyService.Get<IDataStore<PracticeItem>>();

        public bool IsBusy
        {
            get => this.isBusy;
            set => this.SetProperty(ref this.isBusy, value);
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = this.PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}