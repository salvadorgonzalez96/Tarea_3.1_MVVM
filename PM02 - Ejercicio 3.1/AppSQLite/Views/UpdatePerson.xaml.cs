using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSQLite.Views;
using AppSQLite.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.Media;

namespace AppSQLite.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdatePerson : ContentPage
    {
        byte[] imgbytess;
        public UpdatePerson()
        {
            InitializeComponent();
        }

        private async void btnConfirmUpdate_Clicked(object sender, EventArgs e)
        {
            var personForUpdating = new Personas()
            {
                code = Convert.ToInt32(txtCode.Text),
                firstNames = txtFirstnames.Text,
                lastNames = txtLastnames.Text,
                age = Convert.ToInt32(txtAge.Text),
                address = txtAddress.Text,
                puesto = txtPuesto.Text,
                img = imgbytess
            };

            var result = await App.BaseDatos.savePerson(personForUpdating);
            if(result == 1)
            {
                await DisplayAlert("Actualizar", "Actualizado correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Actualizar", "No se pudo actualizar la información", "OK");
            }
            
        }

        private async void btnCancelUpdate_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private byte[] GetImgageBytes(Stream stream)
        {
            byte[] ImgBytes;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                ImgBytes = memoryStream.ToArray();
            }
            return ImgBytes;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var fototap = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Mispics",
                Name = DateTime.Now.ToString() + "IM0011.jpg",
                SaveToAlbum = true
            });

            if (fototap != null)
            {
                imgbytess = null;
                MemoryStream memoryStream = new MemoryStream();
                fototap.GetStream().CopyTo(memoryStream);
                imgbytess = memoryStream.ToArray();
                imgg.Source = ImageSource.FromStream(() => { return fototap.GetStream(); });
            }
        }
    }
}