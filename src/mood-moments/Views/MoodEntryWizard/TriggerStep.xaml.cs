using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class TriggerStep : ContentView
    {
        public TriggerStep()
        {
            InitializeComponent();
        }

        protected override async void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is mood_moments.ViewModels.MoodEntryWizard.TriggerStepViewModel vm)
            {
                await vm.LoadAsync();
            }
        }
    }
}
