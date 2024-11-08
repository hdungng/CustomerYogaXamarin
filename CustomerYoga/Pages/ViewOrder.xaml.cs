using Firebase.Database;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CustomerYoga.Pages;

public partial class ViewOrder : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private ObservableCollection<Order> _allOrdered;

    public string SearchText { get; set; }
    public ICommand SearchCommand { get; }

    public ViewOrder()
	{
        _firebaseClient = new FirebaseClient("https://exercise-2-e02e9-default-rtdb.asia-southeast1.firebasedatabase.app/");
        _allOrdered = new ObservableCollection<Order>();

        LoadOrders();

        // Initialize Commands
        InitializeComponent();
        BindingContext = this;
	}

    private async void LoadOrders()
    {
        var allOrdered = await GetOrdersAsync();
        OrderListView.ItemsSource = _allOrdered;
    }

    private async Task<List<Order>> GetOrdersAsync()
    {
        var classes = await _firebaseClient
            .Child("orders")
            .OnceAsync<Order>();

        return classes.Select(order => new Order
        {
            ClassOdered = order.Object.ClassOdered,
            Email = order.Object.Email,
        }).ToList();
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue;
        if (string.IsNullOrEmpty(searchText))
        {
            OrderListView.ItemsSource = _allOrdered;
        }
        else
        {
            var filteredClasses = _allOrdered.Where(c => c.Email.Equals(searchText)).ToList();
            OrderListView.ItemsSource = filteredClasses;
        }
    }


}