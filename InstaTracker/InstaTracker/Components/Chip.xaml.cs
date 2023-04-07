using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using InstaTracker.Helpers;

namespace InstaTracker.Components;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Chip : Grid
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(Entry), default(string), BindingMode.OneWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor), typeof(Color), typeof(Entry), Color.White, BindingMode.OneWay);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }


    public static readonly BindableProperty TextColorCheckedProperty = BindableProperty.Create(
        nameof(TextColorChecked), typeof(Color), typeof(Entry), Color.Black, BindingMode.OneWay);

    public Color TextColorChecked
    {
        get => (Color)GetValue(TextColorCheckedProperty);
        set => SetValue(TextColorCheckedProperty, value);
    }


    public static readonly BindableProperty ChipBackgroundColorProperty = BindableProperty.Create(
        nameof(ChipBackgroundColor), typeof(Color), typeof(Entry), Color.Gray, BindingMode.OneWay);

    public Color ChipBackgroundColor
    {
        get => (Color)GetValue(ChipBackgroundColorProperty);
        set => SetValue(ChipBackgroundColorProperty, value);
    }


    public static readonly BindableProperty ChipBackgroundColorCheckedProperty = BindableProperty.Create(
        nameof(ChipBackgroundColorChecked), typeof(Color), typeof(Entry), Color.White, BindingMode.OneWay);

    public Color ChipBackgroundColorChecked
    {
        get => (Color)GetValue(ChipBackgroundColorCheckedProperty);
        set => SetValue(ChipBackgroundColorCheckedProperty, value);
    }


    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
        nameof(BorderColor), typeof(Color), typeof(Entry), Color.Black, BindingMode.OneWay);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }


    public static readonly BindableProperty BorderColorCheckedProperty = BindableProperty.Create(
        nameof(BorderColorChecked), typeof(Color), typeof(Entry), Color.Gray, BindingMode.OneWay);

    public Color BorderColorChecked
    {
        get => (Color)GetValue(BorderColorCheckedProperty);
        set => SetValue(BorderColorCheckedProperty, value);
    }


    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked), typeof(bool), typeof(Entry), false, BindingMode.OneWay, null,
        async (bindable, oldValue, newValue) =>
        {
            Chip view = (Chip)bindable;
            view.textLabel.TextColor = (bool)newValue ? view.TextColorChecked : view.TextColor;
            IconTintColorEffect.SetTintColor(view.iconImage, (bool)newValue ? view.TextColorChecked : view.TextColor);

            if (view.ShowCheckmark)
            {
                if ((bool)newValue)
                    view.checkIcon.IsVisible = true;
                await Task.WhenAll(
                    view.container.ColorBackgroundTo((bool)newValue ? view.ChipBackgroundColorChecked : view.ChipBackgroundColor, 150, Easing.Linear),
                    view.container.ColorBorderTo((bool)newValue ? view.BorderColorChecked : view.BorderColor, 150, Easing.Linear),
                    view.checkIcon.WidthTo((bool)newValue ? 20 : 0, 100, Easing.Linear, (f, b) => view.checkIcon.IsVisible = (bool)newValue));
                return;
            }

            await Task.WhenAll(
                view.container.ColorBackgroundTo((bool)newValue ? view.ChipBackgroundColorChecked : view.ChipBackgroundColor, 150, Easing.Linear),
                view.container.ColorBorderTo((bool)newValue ? view.BorderColorChecked : view.BorderColor, 150, Easing.Linear));
        });

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }


    public static readonly BindableProperty ShowCheckmarkProperty = BindableProperty.Create(
        nameof(ShowCheckmark), typeof(bool), typeof(Entry), true, BindingMode.OneWay);

    public bool ShowCheckmark
    {
        get => (bool)GetValue(ShowCheckmarkProperty);
        set => SetValue(ShowCheckmarkProperty, value);
    }


    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(ImageSource), typeof(Chip), default(ImageSource), BindingMode.OneWay, null,
        (bindable, oldValue, newValue) =>
        {
            Chip view = (Chip)bindable;
            view.iconImage.Source = (ImageSource)newValue;
            view.iconImage.IsVisible = !view.iconImage.Source.IsEmpty;
        });

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }




    public Chip()
    {
        InitializeComponent();

        BindingContext = this;
    }


    private void OnTapped(object sender, EventArgs e)
    {
        IsChecked = !IsChecked;
    }
}