using System.Windows.Input;
using Xamarin.Forms;

namespace PracticeRecord.ViewModels
{
    using Services;

    public class RepertoireRandomiserViewModel : BaseViewModel
    {
        private readonly PieceRandomiser pieceRandomiser;
        private string selectedPracticePiece;
        private string selectedLearnPiece;

        public RepertoireRandomiserViewModel()
        {
            this.pieceRandomiser = new PieceRandomiser();
            this.SelectedLearnPiece = this.pieceRandomiser.RandomLearnSelection();
            this.SelectedPracticePiece = this.pieceRandomiser.RandomPracticeSelection();
            this.Title = "Repertoire Randomiser";

            this.RefreshLearnPieceCommand =
                new Command(() => this.SelectedLearnPiece = this.pieceRandomiser.RandomLearnSelection());

            this.RefreshPracticePieceCommand = new Command(() =>
                this.SelectedPracticePiece = this.pieceRandomiser.RandomPracticeSelection());

            this.RefreshAllCommand = new Command(
                () =>
                {
                    this.SelectedLearnPiece = this.pieceRandomiser.RandomLearnSelection();
                    this.SelectedPracticePiece = this.pieceRandomiser.RandomPracticeSelection();
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

        public ICommand RefreshPracticePieceCommand { get; }
        public ICommand RefreshLearnPieceCommand { get; }
        public ICommand RefreshAllCommand { get; }
    }
}