using Newtonsoft.Json;
using ProductApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProductApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetails : ContentPage
    {
        public ProductDetails()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var vm = (ProductDetailsViewModel)BindingContext;
            Settings.AppCartList = JsonConvert.SerializeObject(vm.CartList);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (this.FlowDirection == FlowDirection.RightToLeft)
            {
                (sender as ToolbarItem).Text = "RTL";
                this.FlowDirection = FlowDirection.LeftToRight;
            }
            else
            {
                (sender as ToolbarItem).Text = "LTR";
                this.FlowDirection = FlowDirection.RightToLeft;
            }
        }
    }
}