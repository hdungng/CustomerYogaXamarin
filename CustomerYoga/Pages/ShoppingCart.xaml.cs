using Firebase.Database;
using System.Collections.ObjectModel;

namespace CustomerYoga.Pages;

public partial class ShoppingCart : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private YogaClass yogaClass;

    public ShoppingCart()
    {
        InitializeComponent();
    }

    public ShoppingCart(YogaClass yogaClass)
    {
        _firebaseClient = new FirebaseClient("https://exercise-2-e02e9-default-rtdb.asia-southeast1.firebasedatabase.app/");

        InitializeComponent();

        this.yogaClass = yogaClass;

        BindingContext = yogaClass;
    }


    private async void OnCheckoutClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
        {
            await DisplayAlert("Invalid Email", "Please enter a valid email address.", "OK");
            return;
        }

        // Process the checkout (e.g., save the order, send a confirmation email, etc.)
        await DisplayAlert("Checkout", "Thank you for your purchase " + yogaClass.Name, "OK");


        var order = new Order
        {
            Email = email,
            ClassOdered = yogaClass,
        };

        await _firebaseClient
            .Child("orders")
            .PostAsync(order);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}