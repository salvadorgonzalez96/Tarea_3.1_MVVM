using System;
using AppSQLite.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppSQLite.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class ShowPersons : ContentPage
    {
        private Personas temporalPerson = new Personas();

        public ShowPersons()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            chargeListView();
        }
        private async void chargeListView()
        {

            var ListViewPersons = await App.BaseDatos.getListPersons();
            if(ListViewPersons!=null)
            {
                listPersons.ItemsSource = ListViewPersons;
            }
        }

        private async void listPersons_ItemSelected(object sender, SelectedItemChangedEventArgs eventListener)
        {
            var itemSelected = (Personas) eventListener.SelectedItem;

            btnUpdatePerson.IsVisible = true;
            btnDeletePerson.IsVisible = true;

            var idPersonSelected = itemSelected.code;

            if (idPersonSelected != 0) 
            {
                var personObtained = await App.BaseDatos.getPersonByCode(idPersonSelected);
                if(personObtained != null)
                {
                    temporalPerson.img = personObtained.img;
                    temporalPerson.code = personObtained.code;
                    temporalPerson.firstNames = personObtained.firstNames;
                    temporalPerson.lastNames = personObtained.lastNames;
                    temporalPerson.age = personObtained.age;
                    temporalPerson.address = personObtained.address;
                    temporalPerson.puesto = personObtained.puesto;
            
                }
            }

        }

        private async void btnUpdatePerson_Clicked(object sender, EventArgs e)
        {
            // Crear una instancia de la ventana de Actualizar persona
            var secondPage = new UpdatePerson();

            // Enlanzando los datos del objeto Personas temporal para enviarlo a la ventana de Actualizacion
            secondPage.BindingContext = temporalPerson;

            // Antes de pasar a la ventana de Actualizacion volvemos a ocultar los botones de actualizacion y eliminacion
            btnUpdatePerson.IsVisible = false;
            btnDeletePerson.IsVisible = false;
            await Navigation.PushAsync(secondPage);
            
            // await Navigation.PushAsync(new UpdatePerson());
        }

        private async void btnDeletePerson_Clicked(object sender, EventArgs e)
        {
            // Obtenemos el objeto personas de la BD para usarlo en el metodo de eliminar
            var personObtained = await App.BaseDatos.getPersonByCode(temporalPerson.code);

            if(personObtained != null)
            {
                // Borrando el registro de la BD
                await App.BaseDatos.deletePerson(personObtained);

                await DisplayAlert("Eliminar", "El registro se elimino correctamente", "OK");

                // Una vez borrado el registro y cerrado el DisplayAlert volvemos a ocultar los botones de Actualizacion y Elminar
                btnUpdatePerson.IsVisible = false;
                btnDeletePerson.IsVisible = false;
                chargeListView();
            }
            
        }
    }
}