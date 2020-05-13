using System;
using System.Runtime.CompilerServices;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using Syncfusion.SfBusyIndicator.XForms;
using Xamarin.Forms;
using XamarinApp.Data;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    public class ResourceListPageCS : ContentPage
    {
        private readonly SfListView _listView;
        private SearchBar _searchBar;
        private readonly ResourceViewModel _resourceViewModel;
        
        public ResourceListPageCS()
        { 
            BindingContext = _resourceViewModel
                = new ResourceViewModel();
            
            _searchBar = new SearchBar
            {
                Placeholder = "Search to filter"
            };
            _searchBar.TextChanged += OnFilterTextChanged;

            _listView = new SfListView
            {
                ItemsSource = _resourceViewModel.Resources,
                ItemTemplate = new DataTemplate(() =>
                {
                  var grid = new Grid();
                  var name = new Label
                  {
                      FontAttributes = FontAttributes.Bold,
                      FontSize = 21
                  };
                  name.SetBinding(Label.TextProperty, "Name");
                  
                  var description = new Label
                  {
                      FontSize = 15
                  };
                  description.SetBinding(Label.TextProperty, "Description");
                  
                  grid.Children.Add(name);
                  grid.Children.Add(description, 1,0);
                  return grid;
                })
            };
            _listView.DataSource.SortDescriptors.Add(new SortDescriptor
            {
                PropertyName = "Name",
                Direction = ListSortDirection.Ascending
            });

            var busyIndicator = new SfBusyIndicator
            {
                AnimationType = AnimationTypes.Cupertino,
                ViewBoxHeight = 150,
                ViewBoxWidth = 150,
                Title = "Loading...",
                IsBusy = false
            };

            // busyIndicator.IsBusy = true;
            // await _resourceViewModel.GenerateResources();
            // busyIndicator.IsBusy = false;
            
            
            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 30,
                Spacing = 10,
                Children = {_searchBar, busyIndicator, _listView}
            };
        }
        
        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            _searchBar = (sender as SearchBar);
            if (_listView.DataSource == null) return;
            _listView.DataSource.Filter = FilterContacts;
            _listView.DataSource.RefreshFilter();
        }
        
        private bool FilterContacts(object obj)
        {
            if (_searchBar?.Text == null)
                return true;

            return obj is Resource resource && (resource.Name.ToLower().Contains(_searchBar.Text.ToLower())
                                                || resource.Description.ToLower().Contains(_searchBar.Text.ToLower()));
        }
    }
}