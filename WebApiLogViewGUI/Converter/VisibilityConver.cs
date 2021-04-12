
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WebApiLogViewGUI.Converter
{
    public class VisibilityConver : IValueConverter
    {
        //有内容则显示 无内容则不显示
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
                
            string date = (string)value;

            if (date == null || string.IsNullOrWhiteSpace(date))
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }


    public class VisibilityConverReverse : IValueConverter
    {
        //有内容则显示 无内容则不显示
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }

            string date = (string)value;

            if (date == null || string.IsNullOrWhiteSpace(date))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }


    //public class VisibilityConverWithConfigType : IMultiValueConverter
    //{
    //    //有内容则显示 无内容则不显示
    //    public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value.Length == 2)
    //        {
    //            if (value[0].GetType() == typeof(ConfigTypeEnum))
    //            {
    //                if (value[1].GetType() == typeof(TextBox) && (ConfigTypeEnum)value[0] == ConfigTypeEnum.Int)
    //                {
    //                    return Visibility.Visible;
    //                }

    //                if (value[1].GetType() == typeof(TextBox) && (ConfigTypeEnum)value[0] == ConfigTypeEnum.Text)
    //                {
    //                    return Visibility.Visible;
    //                }

    //                if (value[1].GetType() == typeof(TextBox) && (ConfigTypeEnum)value[0] == ConfigTypeEnum.Array)
    //                {
    //                    return Visibility.Visible;
    //                }

    //                if (value[1].GetType() == typeof(TextBox) && (ConfigTypeEnum)value[0] == ConfigTypeEnum.Unknown)
    //                {
    //                    return Visibility.Visible;
    //                }

    //                if (value[1].GetType() == typeof(ComboBox) && (ConfigTypeEnum)value[0] == ConfigTypeEnum.CustomSelect)
    //                {
    //                    return Visibility.Visible;
    //                }

    //                if (value[1].GetType() == typeof(MahApps.Metro.Controls.ToggleSwitch) && (ConfigTypeEnum)value[0] == ConfigTypeEnum.Bool)
    //                {
    //                    return Visibility.Visible;
    //                }
    //            }
    //            return Visibility.Collapsed;
    //        }
    //        else
    //        {
    //            return Visibility.Collapsed;
    //        }
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
