﻿using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using InstaTracker.Helpers;
using System.Windows.Input;
using System;
using System.Threading.Tasks;

namespace InstaTracker.Components;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Chip : Grid
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(Chip), default(string), BindingMode.OneWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public static readonly BindableProperty FormattedTextProperty = BindableProperty.Create(
        nameof(FormattedText), typeof(FormattedString), typeof(Chip), default(FormattedString), BindingMode.OneWay);

    public FormattedString FormattedText
    {
        get => (FormattedString)GetValue(FormattedTextProperty);
        set => SetValue(FormattedTextProperty, value);
    }
    
    
    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize), typeof(double), typeof(Chip), default(double), BindingMode.OneWay);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }


    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor), typeof(Color), typeof(Chip), Color.White, BindingMode.OneWay);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }


    public static readonly BindableProperty TextColorCheckedProperty = BindableProperty.Create(
        nameof(TextColorChecked), typeof(Color), typeof(Chip), Color.Black, BindingMode.OneWay);

    public Color TextColorChecked
    {
        get => (Color)GetValue(TextColorCheckedProperty);
        set => SetValue(TextColorCheckedProperty, value);
    }


    public static readonly BindableProperty ChipBackgroundColorProperty = BindableProperty.Create(
        nameof(ChipBackgroundColor), typeof(Color), typeof(Chip), Color.Gray, BindingMode.OneWay);

    public Color ChipBackgroundColor
    {
        get => (Color)GetValue(ChipBackgroundColorProperty);
        set => SetValue(ChipBackgroundColorProperty, value);
    }


    public static readonly BindableProperty ChipBackgroundColorCheckedProperty = BindableProperty.Create(
        nameof(ChipBackgroundColorChecked), typeof(Color), typeof(Chip), Color.White, BindingMode.OneWay);

    public Color ChipBackgroundColorChecked
    {
        get => (Color)GetValue(ChipBackgroundColorCheckedProperty);
        set => SetValue(ChipBackgroundColorCheckedProperty, value);
    }


    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
        nameof(BorderColor), typeof(Color), typeof(Chip), Color.Black, BindingMode.OneWay);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }


    public static readonly BindableProperty BorderColorCheckedProperty = BindableProperty.Create(
        nameof(BorderColorChecked), typeof(Color), typeof(Chip), Color.Gray, BindingMode.OneWay);

    public Color BorderColorChecked
    {
        get => (Color)GetValue(BorderColorCheckedProperty);
        set => SetValue(BorderColorCheckedProperty, value);
    }


    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked), typeof(bool), typeof(Chip), false, BindingMode.OneWay, null,
        async (bindable, oldValue, newValue) =>
        {
            Chip view = (Chip)bindable;
            IconTintColorEffect.SetTintColor(view.iconImage, (bool)newValue ? view.TextColorChecked : view.TextColor);

            if (view.ShowCheckmark)
            {
                if ((bool)newValue)
                    view.checkIcon.IsVisible = true;
                await Task.WhenAll(
                    view.container.ColorBackgroundTo((bool)newValue ? view.ChipBackgroundColorChecked : view.ChipBackgroundColor, 150, Easing.Linear),
                    view.borderContainer.ColorBackgroundTo((bool)newValue ? view.BorderColorChecked : view.BorderColor, 150, Easing.Linear),
                    view.textLabel.ColorTextTo((bool)newValue ? view.TextColorChecked : view.TextColor, 150, Easing.Linear),
                    view.checkIcon.WidthTo((bool)newValue ? 20 : 0, 100, Easing.Linear, (f, b) => view.checkIcon.IsVisible = (bool)newValue));
                return;
            }

            await Task.WhenAll(
                view.container.ColorBackgroundTo((bool)newValue ? view.ChipBackgroundColorChecked : view.ChipBackgroundColor, 150, Easing.Linear),
                view.borderContainer.ColorBackgroundTo((bool)newValue ? view.BorderColorChecked : view.BorderColor, 150, Easing.Linear),
                    view.textLabel.ColorTextTo((bool)newValue ? view.TextColorChecked : view.TextColor, 150, Easing.Linear));
        });

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }


    public static readonly BindableProperty ShowCheckmarkProperty = BindableProperty.Create(
        nameof(ShowCheckmark), typeof(bool), typeof(Chip), false, BindingMode.OneWay);

    public bool ShowCheckmark
    {
        get => (bool)GetValue(ShowCheckmarkProperty);
        set => SetValue(ShowCheckmarkProperty, value);
    }

    public static readonly BindableProperty InteractiveProperty = BindableProperty.Create(
        nameof(Interactive), typeof(bool), typeof(Chip), true, BindingMode.OneWay);

    public bool Interactive
    {
        get => (bool)GetValue(InteractiveProperty);
        set => SetValue(InteractiveProperty, value);
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


    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command), typeof(ICommand), typeof(Chip), default(ICommand), BindingMode.OneWay);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter), typeof(object), typeof(Chip), default, BindingMode.OneWay);

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    

    public static readonly BindableProperty TagProperty = BindableProperty.Create(
        nameof(Tag), typeof(object), typeof(Chip), default, BindingMode.OneWay);

    public object Tag
    {
        get => GetValue(TagProperty);
        set => SetValue(TagProperty, value);
    }


    public Chip()
    {
        InitializeComponent();
    }


    private void OnTapped(object sender, EventArgs e)
    {
        if (Interactive)
            IsChecked = !IsChecked;
    }
}