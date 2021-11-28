
using Newtonsoft.Json;
using ProductApp.ViewModels;
using Xamarin.Forms;

namespace ProductApp.Views
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = (MainPageViewModel)BindingContext;
            vm.LoadProductData();
            vm.ReloadData();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var vm = (MainPageViewModel)BindingContext;
            Settings.AppCartList = JsonConvert.SerializeObject(vm.CartList);
        }

        private void ToolbarItem_Clicked(object sender, System.EventArgs e)
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
