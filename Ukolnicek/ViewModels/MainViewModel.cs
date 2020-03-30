using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ukolnicek.Model;
using Utf8Json;
using Windows.Storage;
using Windows.UI.Popups;

namespace Ukolnicek.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Homework> _homeworks = new ObservableCollection<Homework>();
        private int _selectedHomeworkIndex;
        private Homework _selectedHomework;
        private StorageFile _file;

        public RelayCommand Add { get; set; }
        public RelayCommand Remove { get; set; }
        public RelayCommand Load { get; set; }
        public RelayCommand Save { get; set; }

        public MainViewModel()
        {
            Homeworks.Add(new Homework { Name = "černá svině", Date = new DateTime(2020, 3, 27) , Subject = Subject.PRG, IsDone = false });
            Homeworks.Add(new Homework { Name = "bílá svině", Date = new DateTime(2020,3,28), Subject = Subject.CJL, IsDone = false });

            Add = new RelayCommand(
                () => { Homeworks.Add(new Homework { Name = "Nový úkol" }); },
                () => true
            );
            Remove = new RelayCommand(
                () => { Homeworks.Remove(SelectedHomework); },
                () => { return SelectedHomework != null; }
            );
            Load = new RelayCommand(
                async () => {
                    try
                    {
                        string serializedSourceText = await Windows.Storage.FileIO.ReadTextAsync(File);
                        Homeworks = JsonSerializer.Deserialize<ObservableCollection<Homework>>(serializedSourceText);
                    }
                    catch (Exception ex)
                    {
                        var messageDialog = new MessageDialog("Error: " + ex.Message); // vyhození dialogu odsud není ideální, ale funguje
                        await messageDialog.ShowAsync();
                    }
                },
                () => true
            );
            Save = new RelayCommand(
                async () => {
                    try
                    {
                        string serializedData = JsonSerializer.ToJsonString(Homeworks);
                        await Windows.Storage.FileIO.WriteTextAsync(File, serializedData);
                        /* pokusně přes stream
                        var stream = await File.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                        using (var outputStream = stream.GetOutputStreamAt(0))
                        {
                            using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
                            {
                                dataWriter.WriteString(JsonSerializer.ToJsonString(_students));
                                await dataWriter.StoreAsync();
                                await outputStream.FlushAsync();
                            }                      
                        }
                        stream.Dispose();
                    */
                    }
                    catch (Exception ex)
                    {
                        var messageDialog = new MessageDialog("Error: " + ex.Message);
                        await messageDialog.ShowAsync();
                    }
                },
                () => true
            );
        }

        public List<Subject> Subjects
        {
            get
            {
                return Enum.GetValues(typeof(Subject)).Cast<Subject>().ToList();
            }
        }
        public ObservableCollection<Homework> Homeworks { get { return _homeworks; } set { _homeworks = value; NotifyPropertyChanged(); } }

        public StorageFile File { get { return _file; } set { _file = value; NotifyPropertyChanged(); } }
        public int SelectedHomeworkIndex { get { return _selectedHomeworkIndex + 1; } set { _selectedHomeworkIndex = value; NotifyPropertyChanged(); Remove.RaiseCanExecuteChanged(); } }
        public Homework SelectedHomework { get { return _selectedHomework; } set { _selectedHomework = value; NotifyPropertyChanged(); } }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
