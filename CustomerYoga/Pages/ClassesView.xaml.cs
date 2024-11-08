using Firebase.Database;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CustomerYoga.Pages;

public partial class ClassesView : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private ObservableCollection<YogaClass> _allClasses;

    public string SearchText { get; set; }
    public ICommand SearchCommand { get; }

    public ICommand OrderCommand { get; }

    public ClassesView()
    {
        _firebaseClient = new FirebaseClient("https://exercise-2-e02e9-default-rtdb.asia-southeast1.firebasedatabase.app/");
        _allClasses = new ObservableCollection<YogaClass>();

        LoadClasses();

        // Initialize Commands
        OrderCommand = new Command<YogaClass>(PerformOrder);
        BindingContext = this;
        InitializeComponent();
    }

    private async void LoadClasses()
    {
        var classes = await GetClassesAsync();
        _allClasses = new ObservableCollection<YogaClass>(classes);
        ClassesListView.ItemsSource = _allClasses;
    }

    private async Task<List<YogaClass>> GetClassesAsync()
    {
        var classes = await _firebaseClient
            .Child("Class")
            .OnceAsync<YogaClass>();

        return classes.Select(yogaClass => new YogaClass
        {
            Id = yogaClass.Key,
            Name = yogaClass.Object.Name,
            Date = yogaClass.Object.Date,
            CourseId = yogaClass.Object.CourseId,
            Comment = yogaClass.Object.Comment,
            Teacher = yogaClass.Object.Teacher
        }).ToList();
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue;
        if (string.IsNullOrEmpty(searchText))
        {
            ClassesListView.ItemsSource = _allClasses;
        }
        else
        {
            var filteredClasses = _allClasses.Where(c => c.Date.Contains(searchText)).ToList();
            ClassesListView.ItemsSource = filteredClasses;
        }
    }

    private void PerformOrder(YogaClass yogaClass)
    {
        // Perform the ordering action (e.g., navigate to a new page, show a confirmation message, etc.)
        DisplayAlert("Order", $"You have ordered the class: {yogaClass.Name}", "OK");
        Navigation.PushModalAsync(new ShoppingCart(yogaClass), true);
    }
}