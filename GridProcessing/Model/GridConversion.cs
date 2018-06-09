using System;
using System.Collections.ObjectModel;

namespace GridProcessing.Model
{
    public interface IConverter<T>
    {
        void NextStep(Collection<T> list);
    }

    public class ValueItem : ModelVievBase
    {
        int value;
        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }
        public ValueItem(int i)
        {
            Value = i;
        }
    }

    public class LifePanel
    {
        public IConverter<ValueItem> Converter { get; set; }
        public Collection<ValueItem> Grid { get; set; }
        public LifePanel(Collection<ValueItem> grid, IConverter<ValueItem> converter)
        {
            Grid = grid;
            this.Converter = converter;
        }

        public void NextStep()
        {
            Converter.NextStep(Grid);
        }
    }



    public class StepByStepConverter : IConverter<ValueItem>
    {
        public void NextStep(Collection<ValueItem> list)
        {
            foreach (var item in list)
            {
                if (item.Value == 0)
                {
                    item.Value = 1;
                    return;
                }
            }
        }
    }

    public class ReverseConverter : IConverter<ValueItem>
    {
        public void NextStep(Collection<ValueItem> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Value == 0)
                {
                    list[i].Value = 1;
                }
                else list[i].Value = 0;
            }
        }
    }

    public class WallToWallConverter : IConverter<ValueItem>
    {
        public void NextStep(Collection<ValueItem> list)
        {
            int side = (int)Math.Sqrt(list.Count);
            for (int i = 0; i < side; i++)
            {
                if (list[i].Value == 0)
                {
                    PaintColumn(list, side, i);
                    PaintColumn(list, side, side - i-1);
                    return;
                }
            }

        }

        void PaintColumn(Collection<ValueItem> list, int side, int column)
        {
            for (int i = 0; i < side; i++)
            {
                list[i * side + column].Value = 1;
            }
        }
    }
}
