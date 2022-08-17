using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading;

namespace AppSQLite.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonPages : ContentPage
    {
        byte[] imgbytess;
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PM02.db3");
        public PersonPages()
        {
            InitializeComponent();
            imgg.Source = null;
        }

        private async void btnSavePerson_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);

            var person = new Models.Personas
            {
                img = imgbytess,
                firstNames = txtFirstnames.Text,
                lastNames = txtLastnames.Text,
                age = Convert.ToInt32(txtAge.Text),
                address = txtAddress.Text,
                puesto = txtPuesto.Text
            };

            var result = await App.BaseDatos.savePerson(person);
            if(result == 1)
            {
                await DisplayAlert("Agregar", "Ingresado correctamente", "OK");
                var ListPersons = await App.BaseDatos.getListPersons();
            } 
            else
            {
                await DisplayAlert("Agregar", "No se pudo guardar la información", "OK");
            }
            clearFormPerson();
        }


        private void clearFormPerson()
        {
            this.imgg.Source = null;
            this.txtFirstnames.Text = String.Empty;
            this.txtLastnames.Text = String.Empty;
            this.txtAge.Text = String.Empty;
            this.txtAddress.Text = String.Empty;
            this.txtPuesto.Text = String.Empty;
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