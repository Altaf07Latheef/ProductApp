using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProductApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        IPageDialogService _pageDialogService;
        INavigationService _navigationService;
        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _navigationService = navigationService;
            Title = "";

        }

        public void LoadProductData()
        {
            ProductList.Clear();
            ProductList.Add(new ProductModel() { ID = 1, Title = "Flannel shirt", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 4, Price = 23.5 });
            ProductList.Add(new ProductModel() { ID = 2, Title = "Bomber Jacket", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 2, Price = 34 });
            ProductList.Add(new ProductModel() { ID = 3, Title = "Classic black", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 5, Price = 45 });
            ProductList.Add(new ProductModel() { ID = 4, Title = "Flowers Shirt", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 3, Price = 42 });
            ProductList.Add(new ProductModel() { ID = 5, Title = "Long Kurta", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 1, Price = 23.5 });
            ProductList.Add(new ProductModel() { ID = 6, Title = "Leather Jacket", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 5, Price = 34 });
            ProductList.Add(new ProductModel() { ID = 7, Title = "Classic Hoodies", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 3, Price = 45 });
            ProductList.Add(new ProductModel() { ID = 8, Title = "Linen Shirt", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 2, Price = 47 });
            ProductList.Add(new ProductModel() { ID = 9, Title = "Matte black", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 4, Price = 65 });
            ProductList.Add(new ProductModel() { ID = 10, Title = "Flowers Pants", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s", Image = "mendress.jpg", Rating = 2, Price = 48 });
            RaisePropertyChanged("ProductList");
        }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { _isRefreshing = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<ProductModel> ProductList { get; set; } = new ObservableCollection<ProductModel>();
        public ObservableCollection<ProductModel> CartList { get; set; } = new ObservableCollection<ProductModel>();

        #region Commands

        private Command<ProductModel> addtoCartCommand;
        public Command<ProductModel> AddtoCartCommand
        {
            get
            {
                return addtoCartCommand = addtoCartCommand ?? new Command<ProductModel>(async(model) =>
                {
                    try
                    {
                        var item = CartList?.Where(x => x.ID == model.ID)?.FirstOrDefault();
                        if (item == null)
                        {
                            if (await _pageDialogService.DisplayAlertAsync("Hi", "Do you wish to add this Product in cart", "Yes", "Cancel"))
                            {
                                CartList.Add(model);
                            }
                        }
                        else
                        {
                            if (await _pageDialogService.DisplayAlertAsync("Alert", "Are you sure to remove this product?", "Remove", "Cancel"))
                                CartList.Remove(model);
                        }
                        RaisePropertyChanged("CartList");
                        Settings.AppCartList = JsonConvert.SerializeObject(CartList);
                        IsRefreshing = true;
                        LoadProductData();
                        IsRefreshing = false;
                    }
                    catch(Exception ex)
                    {

                    }
                });
            }
        }

        private Command<ProductModel> removeCommand;
        public Command<ProductModel> RemoveCommand
        {
            get
            {
                return removeCommand = removeCommand ?? new Command<ProductModel>(async (model) =>
                {
                    if (await _pageDialogService.DisplayAlertAsync("Alert", "Are you sure to remove this product?", "Yes", "Cancel"))
                        CartList.Remove(model);
                    RaisePropertyChanged("CartList");
                    IsRefreshing = true;
                    LoadProductData();
                    IsRefreshing = false;
                });
            }
        }

        private Command<ProductModel> viewDetailsCommand;
        public Command<ProductModel> ViewDetailsCommand
        {
            get
            {
                return viewDetailsCommand = viewDetailsCommand ?? new Command<ProductModel>(async(model) =>
                {
                    Settings.AppCartList = JsonConvert.SerializeObject(CartList);
                    var nav = new NavigationParameters
                    {
                        {"model",model }
                    };
                    await _navigationService.NavigateAsync("ProductDetails", nav);
                });
            }
        }
        private Command refreshCommand;
        public Command RefreshCommand
        {
            get
            {
                return refreshCommand = refreshCommand ?? new Command(() =>
                {
                    IsRefreshing = true;
                    LoadProductData();
                    IsRefreshing = false;
                });
            }
        }

        #endregion

        public void ReloadData()
        {
            if (!string.IsNullOrEmpty(Settings.AppCartList))
            {
                CartList = JsonConvert.DeserializeObject<ObservableCollection<ProductModel>>(Settings.AppCartList);
                RaisePropertyChanged("CartList");
            }
        }

    }

    public class ProductModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public bool IsAddedToCart { get; set; }
    }
}
