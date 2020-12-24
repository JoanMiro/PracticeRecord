namespace PracticeRecord.ViewModels
{
    using System.Linq;
    using Services;
    using System.Windows.Input;
    using Models;
    using Xamarin.Forms;

    public class RepertoireRandomiserViewModel : BaseViewModel
    {
        private string selectedLearnPiece;
        private string selectedPracticePiece;

        public RepertoireRandomiserViewModel()
        {
            var pieceRandomiser = new PieceRandomiser();
            this.SelectedLearnPiece = pieceRandomiser.RandomLearnSelection();
            this.SelectedPracticePiece = pieceRandomiser.RandomPracticeSelection();
            this.Title = "Repertoire Randomiser";

            this.RefreshLearnPieceCommand =
            new Command(() => this.SelectedLearnPiece = pieceRandomiser.RandomLearnSelection());

            this.RefreshPracticePieceCommand = new Command(() =>
            this.SelectedPracticePiece = pieceRandomiser.RandomPracticeSelection());

            this.RefreshAllCommand = new Command(
                () =>
                {
                    this.SelectedLearnPiece = pieceRandomiser.RandomLearnSelection();
                    this.SelectedPracticePiece = pieceRandomiser.RandomPracticeSelection();
                });
        }

        public string SelectedPracticePiece
        {
            get => this.selectedPracticePiece;
            private set => this.SetProperty(ref this.selectedPracticePiece, value);
        }

        public string SelectedLearnPiece
        {
            get => this.selectedLearnPiece;
            private set => this.SetProperty(ref this.selectedLearnPiece, value);
        }
        
        public PracticeItem CurrentPeriodRecord => this.PracticeDataViewModel.PracticeItems.OrderByDescending(i => i.CycleStartDate).First();

        public PracticeDataViewModel PracticeDataViewModel => Application.Current.MainPage.BindingContext as PracticeDataViewModel;

        public string WeeklyPracticePieces => this.CurrentPeriodRecord.SerializedPracticeSchedule.Replace(",", "\r\n");
        

        public ICommand RefreshPracticePieceCommand { get; }

        public ICommand RefreshLearnPieceCommand { get; }

        public ICommand RefreshAllCommand { get; }
    }
}