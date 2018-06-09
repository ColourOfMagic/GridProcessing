using GridProcessing.Model;
using System.Collections.ObjectModel;

namespace GridProcessing.ViewModel
{
    class MainViewModel : ModelVievBase
    {
        int currentGrid;
        int currentConverter;

        public LifePanel Panel { get; set; }
        public int CurrentGrid
        {
            get => currentGrid;
            set
            {
                currentGrid = value;
                if (value == 0) Panel.Grid = GetCollection(400);
                if (value == 1) Panel.Grid = GetCollection(900);
                if (value == 2) Panel.Grid = GetCollection(1600);
                OnPropertyChanged("CurrentGrid"); 
                OnPropertyChanged("Panel");

            }
        }
        public int CurrentConverter
        {
            get => currentConverter;
            set
            {
                currentConverter = value;
                if (value == 0) Panel.Converter = new StepByStepConverter();
                if (value == 1) Panel.Converter = new ReverseConverter();
                if (value==2) Panel.Converter = new WallToWallConverter();
                OnPropertyChanged("CurrentConverter");
            }
        }

        ObservableCollection<ValueItem> GetCollection(int amount)
        {
            ObservableCollection<ValueItem> values = new ObservableCollection<ValueItem>();
            for (int i=0; i<amount; i++)
            {
                values.Add(new ValueItem(0));
            }
            return values;
        }

        public MainViewModel()
        {
            Panel = new LifePanel(new ObservableCollection<ValueItem>(), new ReverseConverter()); //Временная мера
            CurrentGrid = 0;
            CurrentConverter = 0;
        }

        #region Command

        private RelayCommand nextStep;

        /// <summary>
        /// NextStep
        /// </summary>
        public RelayCommand NextStep
        {
            get
            {
                return nextStep
                    ?? (nextStep = new RelayCommand(
                    (obj) =>
                    {
                        Panel.NextStep();
                    }));
            }
        }

        #endregion
    }
}
