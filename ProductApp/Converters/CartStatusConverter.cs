using Newtonsoft.Json;
using ProductApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ProductApp.Converters
{
    public class CartStatusConverter : IValueConverter
    {
        List<ProductModel> cartProducts = new List<ProductModel>();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

                cartProducts = JsonConvert.DeserializeObject<List<ProductModel>>(Settings.AppCartList);
                if (value != null && value is int && cartProducts != null && cartProducts.Count > 0) 
                {
                    if (cartProducts.Any(x => x.ID == (int)value))
                        return "Remove from Cart";
                }

            return "Add to Cart";
                    
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
