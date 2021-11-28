using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ProductApp.ViewModels
{
    public class ProductDetailsViewModel : ViewModelBase
    {
        IPageDialogService _pageDialogService;
        public ProductModel model { get; set; }
        public List<ProductModel> CartList = new List<ProductModel>();
        public ProductDetailsViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            if (!string.IsNullOrEmpty(Settings.AppCartList))
            {
                CartList = JsonConvert.DeserializeObject<List<ProductModel>>(Settings.AppCartList);
            }
            CartStatusText = "Add to Cart";
        }


        private string _cartStatusText;

        public string CartStatusText
        {
            get { return _cartStatusText; }
            set
            {
                _cartStatusText = value;
                RaisePropertyChanged();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("model"))
                model = parameters.GetValue<ProductModel>("model");
            RaisePropertyChanged("model");

            LoadCartData();
        }

        private void LoadCartData()
        {
            if (CartList != null && CartList.Count > 0)
            {
                if (CartList.Any(x => x.ID == model.ID))
                    _cartStatusText = "Remove from Cart";
                else _cartStatusText = "Add to Cart";
               
            }
            else _cartStatusText = "Add to Cart";
            RaisePropertyChanged("CartStatusText");
        }

        private Command addtoCartCommand;
        public Command AddtoCartCommand
        {
            get
            {
                return addtoCartCommand = addtoCartCommand ?? new Command(async()=>
                {
                    try
                    {
                        var item = CartList?.Where(x => x.ID == model.ID)?.FirstOrDefault();
                        if (item == null)
                        {
                            if (await _pageDialogService.DisplayAlertAsync("Hi", "Do you wish to add this Product in cart", "Yes", "Cancel"))
                                CartList.Add(model);
                        }
                        else
                        {
                            if (await _pageDialogService.DisplayAlertAsync("Alert", "Are you sure to remove this product?", "Remove", "Cancel"))
                            {
                                CartList.Remove(item);
                            }
                        }
                        LoadCartData();
                    }
                    catch (Exception ex)
                    {

                    }

                });
            }
        }

    }
}
