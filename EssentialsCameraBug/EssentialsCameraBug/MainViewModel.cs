using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EssentialsCameraBug
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand TakePictureCommand => new Command(async () => await TakePicture());

        public ImageSource CapturedPicture { get; set; } = ImageSource.FromFile("");

        public string FilePath { get; set; }

        public MainViewModel()
        {

        }


        private async Task TakePicture()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if(status != PermissionStatus.Granted)
            {
                return;
            }

            var photo = await MediaPicker.CapturePhotoAsync();

            CapturedPicture = ImageSource.FromFile(photo.FullPath);
            FilePath = photo.FullPath;
            RaisePropertyChanged(nameof(CapturedPicture));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged(params string[] properties)
        {
            foreach (var propertyName in properties)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
