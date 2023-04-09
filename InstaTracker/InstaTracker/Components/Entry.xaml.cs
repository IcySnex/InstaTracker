using System.Windows.Input;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InstaTracker.Components;

public partial class Entry : Grid
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(Entry), default(string), BindingMode.TwoWay, null,
        (bindable, oldValue, newValue) =>
        {
            Entry view = (Entry)bindable;

            if (string.IsNullOrEmpty(view.PlaceholderText) || view.placeholderContainer.TranslationY == -24)
                return;

            MainThread.BeginInvokeOnMainThread(async () =>
                await view.placeholderContainer.TranslateTo(0, -24, 150, Easing.Linear));
            view.placeholderText.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        });

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(
        nameof(PlaceholderText), typeof(string), typeof(Entry), default(string), BindingMode.OneWay);

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }


    public static readonly BindableProperty HelperTextProperty = BindableProperty.Create(
        nameof(HelperText), typeof(string), typeof(Entry), default(string), BindingMode.OneWay);

    public string HelperText
    {
        get => (string)GetValue(HelperTextProperty);
        set => SetValue(HelperTextProperty, value);
    }


    public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
        nameof(ErrorText), typeof(string), typeof(Entry), default(string), BindingMode.OneWay);

    public string ErrorText
    {
        get => (string)GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }


    public static readonly BindableProperty LeadingIconProperty = BindableProperty.Create(
        nameof(LeadingIcon), typeof(ImageSource), typeof(Entry), default(ImageSource), BindingMode.OneWay);

    public ImageSource LeadingIcon
    {
        get => (ImageSource)GetValue(LeadingIconProperty);
        set => SetValue(LeadingIconProperty, value);
    }


    public static readonly BindableProperty TrailingIconProperty = BindableProperty.Create(
        nameof(TrailingIcon), typeof(ImageSource), typeof(Entry), default(ImageSource), BindingMode.OneWay);

    public ImageSource TrailingIcon
    {
        get => (ImageSource)GetValue(TrailingIconProperty);
        set => SetValue(TrailingIconProperty, value);
    }


    public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(
        nameof(HasError), typeof(bool), typeof(Entry), false, BindingMode.OneWay, null,
        (bindable, oldValue, newValue) =>
        {
            Entry view = (Entry)bindable;

            if (view.TrailingIcon is not null && !view.TrailingIcon.IsEmpty)
                view.tempIcon = view.TrailingIcon;
            view.TrailingIcon = (bool)newValue ? ImageSource.FromFile("error.png") : view.tempIcon;

            IconTintColorEffect.SetTintColor(view.TrailingIcon, (bool)newValue ? view.ErrorTextColor : view.TextColor);
            view.containerFrame.BorderColor = (bool)newValue ? view.ErrorTextColor : view.BorderColor;
            view.placeholderText.TextColor = (bool)newValue ? view.ErrorTextColor : view.BorderColor;
        });

    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }


    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword), typeof(bool), typeof(Entry), default(bool), BindingMode.OneWay);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }


    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
        nameof(MaxLength), typeof(int), typeof(Entry), int.MaxValue, BindingMode.OneWay);

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }


    public static readonly BindableProperty ParentBackgroundColorProperty = BindableProperty.Create(
        nameof(ParentBackgroundColor), typeof(Color), typeof(Entry), Color.White, BindingMode.OneWay);

    public Color ParentBackgroundColor
    {
        get => (Color)GetValue(ParentBackgroundColorProperty);
        set => SetValue(ParentBackgroundColorProperty, value);
    }


    public static readonly BindableProperty EntryBackgroundColorProperty = BindableProperty.Create(
        nameof(EntryBackgroundColor), typeof(Color), typeof(Entry), Color.White, BindingMode.OneWay);

    public Color EntryBackgroundColor
    {
        get => (Color)GetValue(EntryBackgroundColorProperty);
        set => SetValue(EntryBackgroundColorProperty, value);
    }


    public static readonly BindableProperty BorderColorActiveProperty = BindableProperty.Create(
        nameof(BorderColorActive), typeof(Color), typeof(Entry), Color.Blue, BindingMode.OneWay);

    public Color BorderColorActive
    {
        get => (Color)GetValue(BorderColorActiveProperty);
        set => SetValue(BorderColorActiveProperty, value);
    }


    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
        nameof(BorderColor), typeof(Color), typeof(Entry), Color.Gray, BindingMode.OneWay);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }


    public static readonly BindableProperty TextColorActiveProperty = BindableProperty.Create(
        nameof(TextColorActive), typeof(Color), typeof(Entry), Color.Blue, BindingMode.OneWay);

    public Color TextColorActive
    {
        get => (Color)GetValue(TextColorActiveProperty);
        set => SetValue(TextColorActiveProperty, value);
    }


    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor), typeof(Color), typeof(Entry), Color.White, BindingMode.OneWay);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }


    public static readonly BindableProperty HelperTextColorProperty = BindableProperty.Create(
        nameof(HelperTextColor), typeof(Color), typeof(Entry), Color.Gray, BindingMode.OneWay);

    public Color HelperTextColor
    {
        get => (Color)GetValue(HelperTextColorProperty);
        set => SetValue(HelperTextColorProperty, value);
    }


    public static readonly BindableProperty ErrorTextColorProperty = BindableProperty.Create(
        nameof(ErrorTextColor), typeof(Color), typeof(Entry), Color.Red, BindingMode.OneWay);

    public Color ErrorTextColor
    {
        get => (Color)GetValue(ErrorTextColorProperty);
        set => SetValue(ErrorTextColorProperty, value);
    }


    ImageSource tempIcon = default!;

    public Keyboard Keyboard
    {
        set => customEntry.Keyboard = value;
        get => customEntry.Keyboard;
    }

    public ReturnType ReturnType
    {
        set => customEntry.ReturnType = value;
        get => customEntry.ReturnType;
    }



    public Entry()
    {
        InitializeComponent();
    }


    private async Task ControlFocused()
    {
        customEntry.Focus();

        customEntry.TextColor = HasError ? ErrorTextColor : TextColorActive;
        containerFrame.BorderColor = HasError ? ErrorTextColor : BorderColorActive;
        placeholderText.TextColor = HasError ? ErrorTextColor : BorderColorActive;

        if (string.IsNullOrEmpty(PlaceholderText))
            return;

        await placeholderContainer.TranslateTo(0, -24, 150, Easing.Linear);
        placeholderText.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
    }

    private async Task ControlUnfocused()
    {
        customEntry.Unfocus();

        customEntry.TextColor = HasError ? ErrorTextColor : TextColor;
        containerFrame.BorderColor = HasError ? ErrorTextColor : BorderColor;
        placeholderText.TextColor = HasError ? ErrorTextColor : BorderColor;

        if (!string.IsNullOrEmpty(customEntry.Text) && customEntry.MaxLength > 0)
            return;

        await placeholderContainer.TranslateTo(0, 0, 150, Easing.Linear);
        placeholderText.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
    }


    private void CustomEntryFocused(object sender, FocusEventArgs e)
    {
        if (e.IsFocused)
            MainThread.BeginInvokeOnMainThread(async () => await ControlFocused());
    }

    private void CustomEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (!e.IsFocused)
            MainThread.BeginInvokeOnMainThread(async () => await ControlUnfocused());
    }

    private void EntryTapped(object sender, EventArgs e) =>
        MainThread.BeginInvokeOnMainThread(async () => await ControlFocused());


    private void PasswordEyeTapped(object sender, EventArgs e) =>
        customEntry.IsPassword = !customEntry.IsPassword;
}