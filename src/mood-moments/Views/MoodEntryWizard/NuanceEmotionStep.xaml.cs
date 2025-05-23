using Microsoft.Maui.Controls;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class NuanceEmotionStep : ContentView
    {
        public NuanceEmotionStep()
        {
            InitializeComponent();
        }

        public void OnNuancedEmotionCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e != null
                && e.Value
                && sender is RadioButton rb
                && rb.Content is string emotion
                && BindingContext is mood_moments.ViewModels.NewEntryWizardViewModel vm
                && vm.SelectNuancedEmotionCommand != null
                && vm.SelectNuancedEmotionCommand.CanExecute(emotion))
            {
                vm.SelectNuancedEmotionCommand?.Execute(emotion);
            }
        }
    }
}
