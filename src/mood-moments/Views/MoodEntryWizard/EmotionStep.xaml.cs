using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class EmotionStep : ContentView
    {
        public EmotionStep()
        {
            InitializeComponent();
        }

        public void OnCoreEmotionCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value && sender is RadioButton rb && rb.Content is string emotion && BindingContext is mood_moments.ViewModels.NewEntryWizardViewModel vm)
            {
                if (vm.SelectCoreEmotionCommand.CanExecute(emotion))
                    vm.SelectCoreEmotionCommand.Execute(emotion);
            }
        }
    }
}
