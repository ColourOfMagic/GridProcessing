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

    public class LifePanel :ModelVievBase
    {
        Collection<ValueItem> coll;
        public IConverter<ValueItem> Converter { get; set; }
        public Collection<ValueItem> Grid
        {
            get => coll;
            set
            {
                coll = value;
                OnPropertyChanged("Grid");
            }
        }
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
                    PaintColumn(list, side, side - i - 1);
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

    public class LifeConverter : IConverter<ValueItem>
    {
        class LifeMatrix   //Если понадобится в другом конвертере - вынести из этого класса
        {
            Collection<ValueItem> Values;
            public int Side;

            public int this[int x, int y]
            {
                get
                {
                    int height = y;
                    int width = x;
                    if (x == -1) width = Side - 1;
                    if (y == -1) height = Side - 1;
                    if (x == Side) width = 0;
                    if (y == Side) height = 0;
                    return Values[height * Side + width].Value;
                }
                set
                {
                    int height = y;
                    int width = x;
                    if (x == -1) width = Side - 1;
                    if (y == -1) height = Side - 1;
                    if (x == Side) width = 0;
                    if (y == Side) height = 0;
                    Values[height * Side + width].Value = value;
                }
            }

            public LifeMatrix(Collection<ValueItem> values)
            {
                Values = values;
                Side = (int)Math.Sqrt(values.Count);
            }

            public int count_live_neighbors(int y, int x)
            {
                int count = 0;
                if (this[y - 1, x - 1] == 1) count++;
                if (this[y, x - 1] == 1) count++;
                if (this[y + 1, x - 1] == 1) count++;
                if (this[y - 1, x] == 1) count++;
                if (this[y + 1, x] == 1) count++;
                if (this[y - 1, x + 1] == 1) count++;
                if (this[y, x + 1] == 1) count++;
                if (this[y + 1, x + 1] == 1) count++;

                return count;
            }

            public static LifeMatrix CopyMatrix(Collection<ValueItem> list) //Потом может переделать поинтересней
            {
                var items = new Collection<ValueItem>();
                foreach (var item in list)
                {
                    items.Add(new ValueItem(item.Value));
                }
                return new LifeMatrix(items);
            }
        }
        public void NextStep(Collection<ValueItem> list)
        {

            var old_generation = LifeMatrix.CopyMatrix(list);
            var new_generation = new LifeMatrix(list);

            for (int i = 0; i < new_generation.Side; i++)
            {
                for (int j = 0; j < new_generation.Side; j++)
                {
                    if (old_generation[i, j] == 1 && (old_generation.count_live_neighbors(i, j) == 2 || old_generation.count_live_neighbors(i, j) == 3)) { }  //Подправить бы
                    else new_generation[i, j] = 0;
                    if (old_generation[i, j] == 0 && old_generation.count_live_neighbors(i, j) == 3)
                        new_generation[i, j] = 1;
                }
            }
        }
    }
}
