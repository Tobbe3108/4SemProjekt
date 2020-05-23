using System;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using Syncfusion.SfPullToRefresh.XForms;
using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels.Resource;
using SelectionMode = Syncfusion.ListView.XForms.SelectionMode;

namespace XamarinApp.Views.Resource
{
    public class ResourceListView : ContentPage
    {
        private readonly SfListView _listView;

        public ResourceListView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
            var searchBar = new SearchBar
            {
                Placeholder = "Search to filter"
            };
            searchBar.TextChanged += ((sender, args) => ((ResourceListViewModel)BindingContext).OnFilterTextChanged(sender, args, _listView));
            
            _listView = new SfListView
            {
                ItemsSource = ((ResourceListViewModel)BindingContext).Resources,
                SelectionMode = SelectionMode.Single,
                SelectionGesture = TouchGesture.Tap,
                SelectionBackgroundColor = Color.LightGray,
                AutoFitMode = AutoFitMode.Height,
                ItemTemplate = new DataTemplate(() =>
                {
                    var name = new Label
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 21,
                    };
                    name.SetBinding(Label.TextProperty, "Name");

                    var description = new Label
                    {
                        FontSize = 15
                    };
                    description.SetBinding(Label.TextProperty, "Description");
                    
                    var stackLayout = new StackLayout
                    {
                        Children = { name, description }
                    };

                    return stackLayout;
                })
            };
            _listView.Loaded += ((ResourceListViewModel)BindingContext).GenerateResourcesEvent;
            _listView.DataSource.SortDescriptors.Add(new SortDescriptor
            {
                PropertyName = "Name",
                Direction = ListSortDirection.Ascending,
            });
            _listView.ItemTapped += ((ResourceListViewModel)BindingContext).ItemTappedEvent;

            var pullToRefresh = new SfPullToRefresh
            {
                PullableContent = _listView, 
                //TransitionMode = TransitionType.SlideOnTop
            };
            // pullToRefresh.Refreshing += ((sender, args) =>
            // {
            //     pullToRefresh.IsRefreshing = true;
            //     ((ResourceListViewModel) BindingContext).GenerateResourcesEvent(sender, new ListViewLoadedEventArgs());
            //     pullToRefresh.IsRefreshing = false;
            // });
            
            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 30,
                Spacing = 10,
                Children = {searchBar, pullToRefresh, _listView}
            };
        }
    }
}