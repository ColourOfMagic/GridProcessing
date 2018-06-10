using GridProcessing.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace GridProcessing.ViewModel
{

    class MainViewModel : ModelVievBase
    {
        int currentGrid;
        int currentConverter;
        int interval;
        FileWorker fileWorker;

        public LifePanel Panel { get; set; }
        public DispatcherTimer Timer { get; set; }
        public  TimeSpan TestSpan { get; set; }
        public int Interval
        {
            get => interval;
            set
            {
                interval = value;
                OnPropertyChanged("Interval");
                Timer.Interval = new TimeSpan(value * 10000);
            }
        }
        public int CurrentGrid
        {
            get => currentGrid;
            set
            {
                currentGrid = value;
                if (value == 0) Panel.Grid = GetCollection(400);
                if (value == 1) Panel.Grid = GetCollection(900);
                if (value == 2) Panel.Grid = GetCollection(1600);
                if (value == 3) Panel.Grid = GetCollection(3600);
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
                if (value == 2) Panel.Converter = new WallToWallConverter();
                if (value == 3) Panel.Converter = new LifeConverter();
                OnPropertyChanged("CurrentConverter");
            }
        }
        

        ObservableCollection<ValueItem> GetCollection(int amount)
        {
            ObservableCollection<ValueItem> values = new ObservableCollection<ValueItem>();
            for (int i = 0; i < amount; i++)
            {
                values.Add(new ValueItem(0));
            }
            return values;
        }

        public MainViewModel()
        {
            Panel = new LifePanel(new ObservableCollection<ValueItem>(), new ReverseConverter()); //Временная мера
            fileWorker = new FileWorker(new TxtHangler());
            CurrentGrid = 0;
            CurrentConverter = 3;

            Timer = new DispatcherTimer();
            Timer.Tick +=(sender,e) => Panel.NextStep();
            Interval = 1000;
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
                    },(obj)=>!Timer.IsEnabled));
            }
        }


        private RelayCommand resetGrid;

        /// <summary>
        /// Gets the Reset.
        /// </summary>
        public RelayCommand Reset
        {
            get
            {
                return resetGrid
                    ?? (resetGrid = new RelayCommand(
                    (obj) =>
                    {
                        for (int i = 0; i < Panel.Grid.Count; Panel.Grid[i].Value = 0, i++) ;
                    }));
            }
        }

        private RelayCommand startTimer;

        /// <summary>
        /// Gets the Start.
        /// </summary>
        public RelayCommand Start
        {
            get
            {
                return startTimer
                    ?? (startTimer = new RelayCommand(
                    (obj) =>
                    {
                        Timer.Start();
                    },(a)=>!Timer.IsEnabled));
            }
        }

        private RelayCommand stopTimer;

        /// <summary>
        /// Gets the Stop.
        /// </summary>
        public RelayCommand Stop
        {
            get
            {
                return stopTimer
                    ?? (stopTimer = new RelayCommand(
                    (obj) =>
                    {
                        Timer.Stop();
                    },(obj)=>Timer.IsEnabled));
            }
        }

        private RelayCommand saveCommand;

        /// <summary>
        /// Gets the Save.
        /// </summary>
        public RelayCommand Save
        {
            get
            {
                return saveCommand
                    ?? (saveCommand = new RelayCommand(
                    (obj) =>
                    {
                        fileWorker.Save(Panel.Grid);
                    }));
            }
        }

        private RelayCommand loadGrid;

        /// <summary>
        /// Gets the Load.
        /// </summary>
        public RelayCommand Load
        {
            get
            {
                return loadGrid
                    ?? (loadGrid = new RelayCommand(
                    (obj) =>
                    {
                       Panel.Grid= fileWorker.Load();
                    }));
            }
        }
        #endregion
    }
}
