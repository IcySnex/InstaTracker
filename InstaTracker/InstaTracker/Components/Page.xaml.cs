using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Components;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Page : ContentPage
{
    public Page()
    {
        InitializeComponent();
    }


    public bool IsSnackBarVisible { get; set; }

    public Frame SnackBar { get; private set; } = default!;
    public Label SnackBarLabel { get; private set; } = default!;
    public Button SnackBarButton { get; private set; } = default!;

    public Action<Page>? OnSnackBarButtonClicked { get; set; }


    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        SnackBar = (Frame)GetTemplateChild("snackBar");
        SnackBarLabel = (Label)GetTemplateChild("snackBarLabel");
        SnackBarButton = (Button)GetTemplateChild("snackBarButton");
    }


    private void OnSnackbarButtonClicked(object sender, EventArgs e) =>
        OnSnackBarButtonClicked?.Invoke(this);
}