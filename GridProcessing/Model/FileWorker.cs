using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace GridProcessing.Model
{
    public class FileWorker
    {
        IFileHandler fileHandler;

        public FileWorker(IFileHandler handler)
        {
            fileHandler = handler;
        }

        public void Save(Collection<ValueItem> list)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                fileHandler.Save(list, fileDialog.FileName);
            }
        }

        public Collection<ValueItem> Load()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                return fileHandler.Load(fileDialog.FileName);
            }
            return new Collection<ValueItem>();
        }
    }

    public interface IFileHandler
    {
        void Save(Collection<ValueItem> list, string path);
        ObservableCollection<ValueItem> Load(string path);
    }

    public class TxtHangler : IFileHandler
    {
        public ObservableCollection<ValueItem> Load(string path)
        {
            ObservableCollection<ValueItem> values = new ObservableCollection<ValueItem>();
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string[] mass = reader.ReadLine().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in mass)
                    {
                        values.Add(new ValueItem(Int32.Parse(item)));
                    }
                }
            }
            return values;
        }

        public void Save(Collection<ValueItem> list, string path)
        {
            using (StreamWriter writer = new StreamWriter(path,false))
            {
                int side = (int)Math.Sqrt(list.Count);
                for (int i = 0; i < side; i++)
                {
                    string line = "";
                    for (int j = 0; j < side; j++)
                    {
                        line += list[i * side + j].Value + ",";
                    }
                    writer.WriteLine(line);
                }
            }
        }
    }
}
