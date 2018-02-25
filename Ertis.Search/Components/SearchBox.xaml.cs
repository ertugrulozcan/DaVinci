using Ertis.Infrastructure.Utilities;
using Ertis.Search.Build;
using Ertis.Shared.Helpers;
using Ertis.Shared.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Ertis.Search.Components
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl, INotifyPropertyChanged
    {
        #region Constants

        #endregion

        #region Fields

        private ObservableRangeCollection<KeyValuePair<string, object>> searchResults;
        private DataTemplate selectedItemTemplate;
        private ControlTemplate selectedItemControlTemplate;

        #endregion

        #region Properties

        public ObservableRangeCollection<KeyValuePair<string, object>> SearchResults
        {
            get
            {
                return searchResults;
            }

            set
            {
                searchResults = value;
                this.RaisePropertyChanged("SearchResults");


            }
        }

        public DataTemplate SelectedItemTemplate
        {
            get
            {
                if (selectedItemTemplate == null)
                    return this.FindResource("ResultListItemTemplate") as DataTemplate;

                return selectedItemTemplate;
            }

            private set
            {
                this.selectedItemTemplate = value;
                this.RaisePropertyChanged("SelectedItemTemplate");
            }
        }

        public ControlTemplate SelectedItemControlTemplate
        {
            get
            {
                if (selectedItemControlTemplate == null)
                    return this.FindResource("ResultListItemControlTemplate") as ControlTemplate;

                return selectedItemControlTemplate;
            }

            private set
            {
                this.selectedItemControlTemplate = value;
                this.RaisePropertyChanged("SelectedItemControlTemplate");
            }
        }

        public string BindedCaption
        {
            get
            {
                if (this.IsEnabled)
                    return this.Caption;
                else
                    return Localization.LocalizationUtility.Convert("PleaseWait3D");
            }
        }

        public bool HasSearchKeyPath
        {
            get; private set;
        }

        public bool HasResultKeyPath
        {
            get; private set;
        }

        #endregion

        #region Dependency Properties

        public SearchEngine Engine
        {
            get { return (SearchEngine)GetValue(EngineProperty); }
            set { SetValue(EngineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Engine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineProperty =
            DependencyProperty.Register("Engine", typeof(SearchEngine), typeof(SearchBox), new PropertyMetadata(null, SearchEngineChangedCallback));

        public ICommand SelectionCommand
        {
            get { return (ICommand)GetValue(SelectionCommandProperty); }
            set { SetValue(SelectionCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionCommandProperty =
            DependencyProperty.Register("SelectionCommand", typeof(ICommand), typeof(SearchBox), new PropertyMetadata(null));

        public ICommand ClearCommand
        {
            get { return (ICommand)GetValue(ClearCommandProperty); }
            set { SetValue(ClearCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClearCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearCommandProperty =
            DependencyProperty.Register("ClearCommand", typeof(ICommand), typeof(SearchBox), new PropertyMetadata(null));

        /// <summary>
        /// ItemsSource
        /// </summary>
        public IEnumerable<ISearchable> DataSource
        {
            get { return (IEnumerable<ISearchable>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(IEnumerable<ISearchable>), typeof(SearchBox), new PropertyMetadata(new ObservableCollection<ISearchable>(), DataSourceChangedCallback));

        public string SearchKeyPath
        {
            get { return (string)GetValue(SearchKeyPathProperty); }
            set { SetValue(SearchKeyPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchKeyPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchKeyPathProperty =
            DependencyProperty.Register("SearchKeyPath", typeof(string), typeof(SearchBox), new PropertyMetadata(null, SearchKeyPathChangedCallback));

        public string ResultKeyPath
        {
            get { return (string)GetValue(ResultKeyPathProperty); }
            set { SetValue(ResultKeyPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ResultKeyPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultKeyPathProperty =
            DependencyProperty.Register("ResultKeyPath", typeof(string), typeof(SearchBox), new PropertyMetadata(null, ResultKeyPathChangedCallback));

        public bool StaysOpenResults
        {
            get { return (bool)GetValue(StaysOpenResultsProperty); }
            set { SetValue(StaysOpenResultsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StaysOpenResults.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StaysOpenResultsProperty =
            DependencyProperty.Register("StaysOpenResults", typeof(bool), typeof(SearchBox), new PropertyMetadata(false));

        public bool IsResultsCombined
        {
            get { return (bool)GetValue(IsResultsCombinedProperty); }
            set { SetValue(IsResultsCombinedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsResultsCombined.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsResultsCombinedProperty =
            DependencyProperty.Register("IsResultsCombined", typeof(bool), typeof(SearchBox), new PropertyMetadata(true));

        public SearchEngine.CombineMode CombineMode
        {
            get { return (SearchEngine.CombineMode)GetValue(CombineModeProperty); }
            set { SetValue(CombineModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CombineMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CombineModeProperty =
            DependencyProperty.Register("CombineMode", typeof(SearchEngine.CombineMode), typeof(SearchBox), new PropertyMetadata(SearchEngine.CombineMode.Intersection));


        public bool IsSpellCheckerActive
        {
            get { return (bool)GetValue(IsSpellCheckerActiveProperty); }
            set { SetValue(IsSpellCheckerActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSpellCheckerActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSpellCheckerActiveProperty =
            DependencyProperty.Register("IsSpellCheckerActive", typeof(bool), typeof(SearchBox), new PropertyMetadata(true));


        public Brush SearchBoxBackground
        {
            get { return (Brush)GetValue(SearchBoxBackgroundProperty); }
            set { SetValue(SearchBoxBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchBoxBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchBoxBackgroundProperty =
            DependencyProperty.Register("SearchBoxBackground", typeof(Brush), typeof(SearchBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE))));


        public Brush PopupBackgroundBrush
        {
            get { return (Brush)GetValue(PopupBackgroundBrushProperty); }
            set { SetValue(PopupBackgroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupBackgroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupBackgroundBrushProperty =
            DependencyProperty.Register("PopupBackgroundBrush", typeof(Brush), typeof(SearchBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xFE, 0xFE, 0xFE))));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SearchBox), new PropertyMetadata(new CornerRadius(2)));


        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(SearchBox), new PropertyMetadata("Search", OnCaptionChangedCallback));

        public bool StaysFocus
        {
            get { return (bool)GetValue(StaysFocusProperty); }
            set { SetValue(StaysFocusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StaysFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StaysFocusProperty =
            DependencyProperty.Register("StaysFocus", typeof(bool), typeof(SearchBox), new PropertyMetadata(false));


        public bool ShowBadges
        {
            get { return (bool)GetValue(ShowBadgesProperty); }
            set { SetValue(ShowBadgesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowBadges.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowBadgesProperty =
            DependencyProperty.Register("ShowBadges", typeof(bool), typeof(SearchBox), new PropertyMetadata(false, OnShowBadgesChanged));




        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Filter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(SearchBox), new PropertyMetadata("ALL"));



        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchBox()
        {
            this.DataContext = this;

            this.SearchResults = new ObservableRangeCollection<KeyValuePair<string, object>>();

            InitializeComponent();
            this.Loaded += SearchBox_Loaded;

            //EventManager.RegisterClassHandler((typeof(FrameworkElement), FrameworkElement.MouseLeftButtonDownEvent, new RoutedEventHandler(this.SearchResultsListBox_MouseLeftButtonDown));

            this.SearchTextBox.PreviewGotKeyboardFocus += SearchTextBox_PreviewGotKeyboardFocus;
            this.SearchTextBox.PreviewLostKeyboardFocus += SearchTextBox_PreviewLostKeyboardFocus;
            this.SearchResultsListBox.PreviewGotKeyboardFocus += SearchResultsListBox_PreviewGotKeyboardFocus;
            this.SearchResultsListBox.PreviewLostKeyboardFocus += SearchResultsListBox_PreviewLostKeyboardFocus;
        }

        #endregion

        #region Methods

        private void Search(string text)
        {
            UIHelper.UiInvoke(DispatcherPriority.Background, true, (Action)(() =>
            {
                if (this.Engine == null || this.Engine.IsBusy)
                    return;

                var resultDictionary = new ObservableRangeCollection<KeyValuePair<string, object>>();
                HashSet<object> results;

                if (text.Length > 0)
                {
                    SearchEngine.ResultCompiling resultCompiling = this.IsResultsCombined ? SearchEngine.ResultCompiling.Combined : SearchEngine.ResultCompiling.Basic;
                    results = this.Engine.Search(text, resultCompiling, this.CombineMode);
                }
                else
                {
                    if (this.StaysOpenResults)
                        results = this.Engine.Tree.Root.Objects;
                    else
                        results = new HashSet<object>();
                }

                foreach (var obj in results)
                {
                    if (obj is ISearchable && !this.HasResultKeyPath)
                    {
                        resultDictionary.Add(new KeyValuePair<string, object>((obj as ISearchable).SearchKey, obj));
                        continue;
                    }

                    if (this.HasResultKeyPath)
                    {
                        var prop = SearchEngine.GetPropValue(obj, this.ResultKeyPath);
                        if (prop != null)
                        {
                            resultDictionary.Add(new KeyValuePair<string, object>(prop.ToString(), obj));
                        }
                        else
                        {
                            resultDictionary.Add(new KeyValuePair<string, object>(obj.ToString(), obj));
                        }
                    }
                    else if (!this.HasResultKeyPath && this.HasSearchKeyPath)
                    {
                        var prop = SearchEngine.GetPropValue(obj, this.SearchKeyPath);
                        if (prop != null)
                        {
                            resultDictionary.Add(new KeyValuePair<string, object>(prop.ToString(), obj));
                        }
                        else
                        {
                            resultDictionary.Add(new KeyValuePair<string, object>(obj.ToString(), obj));
                        }
                    }
                    else
                    {
                        resultDictionary.Add(new KeyValuePair<string, object>(obj.ToString(), obj));
                    }
                }
                var isearchableResults = resultDictionary.Where(x => x.Value is ISearchable).ToList();

                if (isearchableResults.Count == 0)
                    this.SearchResults = new ObservableRangeCollection<KeyValuePair<string, object>>(resultDictionary);
                else
                {
                    var orderedResultDict = resultDictionary.OrderBy(x => x.Key.Length).OrderBy(x => (x.Value as ISearchable).CategoryBadge.CurrentBadge);
                    if (Filter == "SMI")
                    {
                        var filtered = orderedResultDict.Where(x => (x.Value as ISearchable).CategoryBadge.CurrentBadge == SearchResultBadge.Badges.Symbol);
                        this.SearchResults = new ObservableRangeCollection<KeyValuePair<string, object>>(filtered);
                    }
                    else if (Filter == "VMI")
                    {
                        var filtered = orderedResultDict.Where(x =>
                        {
                            var curBadge = (x.Value as ISearchable).CategoryBadge.CurrentBadge;
                            return curBadge == SearchResultBadge.Badges.Chart ||
                                    curBadge == SearchResultBadge.Badges.Grid ||
                                    curBadge == SearchResultBadge.Badges.View;

                        });

                        this.SearchResults = new ObservableRangeCollection<KeyValuePair<string, object>>(filtered);
                    }
                    else
                    {
                        this.SearchResults = new ObservableRangeCollection<KeyValuePair<string, object>>(orderedResultDict);
                    }
                }

                this.RaisePropertyChanged("SearchResultsListBox");

                /*

                var smartCommandResults = CommandEngine.Current.DiscoverCommands(text, this.Engine);
                if (smartCommandResults != null && smartCommandResults.Count > 0)
                {
                    var smartCommandResultDictionary = new ObservableRangeCollection<KeyValuePair<string, object>>();
                    foreach (var item in smartCommandResults)
                    {
                        smartCommandResultDictionary.Add(new KeyValuePair<string, object>(item.SearchKey, item));
                    }

                    smartCommandResultDictionary.AddRange(orderedResultDict);
                    this.SearchResults = new ObservableRangeCollection<KeyValuePair<string, object>>(smartCommandResultDictionary);
                }
                else
                {
                    this.SearchResults = new ObservableRangeCollection<KeyValuePair<string, object>>(orderedResultDict);
                }
                */
            }));
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetCaptionVisibility();

            /*
#if DEBUG
            if (this.SearchTextBox.Text == "debug")
                this.OpenSearchBoxDebugWindow();
#endif
            */

            try
            {
                this.Search(this.SearchTextBox.Text);
            }
            catch (Exception)
            {

            }
        }

        /*
        private void OpenSearchBoxDebugWindow()
        {
            Window searchBoxDebugWindow = new Window();
            searchBoxDebugWindow.Title = "SearchBox Visualization";
            var searchTreeVisualizationView = new Views.SearchTreeVisualizationView();
            searchTreeVisualizationView.Engine = this.Engine;
            searchBoxDebugWindow.Content = searchTreeVisualizationView;
            searchBoxDebugWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            searchBoxDebugWindow.ResizeMode = ResizeMode.CanResize;
            searchBoxDebugWindow.Width = 600;
            searchBoxDebugWindow.Height = 800;
            searchBoxDebugWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            
            //var shell = ServiceLocator.Current.GetInstance<IShell>();
            //if (shell != null && (shell as Window).IsInitialized)
            //{
            //    searchBoxDebugWindow.Owner = Application.Current.MainWindow;
            //}

            searchBoxDebugWindow.Show();
        }
        */

        public void ForceFocus()
        {
            this.SearchTextBox.Focus();
            Keyboard.Focus(this.SearchTextBox);
        }

        public void SelectItem(KeyValuePair<string, object> selectedResult)
        {
            if (selectedResult.Value is SearchEngine.SpellFix)
            {
                var spellfix = (selectedResult.Value as SearchEngine.SpellFix);
                if (spellfix.SuggestionList.Count > 0)
                    this.SearchTextBox.Text = spellfix.SuggestionList.First();

                this.RaisePropertyChanged("Text");
                Keyboard.Focus(this.SearchTextBox);
                this.SearchTextBox.CaretIndex = this.SearchTextBox.Text.Length;
                return;
            }

            if (this.SelectionCommand != null && this.SelectionCommand.CanExecute(selectedResult.Value))
            {
                this.SelectionCommand.Execute(selectedResult.Value);
                this.RaisePropertyChanged("Text");

                if (this.StaysFocus)
                    this.ForceFocus();
            }

            this.SearchResults.Clear();

            this.SearchTextBox.Text = string.Empty;
        }

        private void SetCaptionVisibility()
        {
            if (this.CaptionTextBlock.IsFocused)
            {
                this.CaptionTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (this.SearchTextBox.Text.Length == 0)
                    this.CaptionTextBlock.Visibility = Visibility.Visible;
                else
                    this.CaptionTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void Enable()
        {
            try
            {
                this.IsEnabled = true;
                this.Opacity = 1.0;
                this.RaisePropertyChanged("BindedCaption");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                Shared.Helpers.UIHelper.UiInvoke(DispatcherPriority.Normal, false, delegate
                {
                    this.IsEnabled = true;
                    this.Opacity = 1.0;
                    this.RaisePropertyChanged("BindedCaption");
                });
            }
        }

        private void Disable()
        {
            try
            {
                this.IsEnabled = false;
                this.SearchTextBox.Text = string.Empty;
                this.Opacity = 0.6;
                this.RaisePropertyChanged("BindedCaption");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                Shared.Helpers.UIHelper.UiInvoke(DispatcherPriority.Normal, false, delegate
                {
                    this.IsEnabled = false;
                    this.SearchTextBox.Text = string.Empty;
                    this.Opacity = 0.6;
                    this.RaisePropertyChanged("BindedCaption");
                });
            }
        }

        #endregion

        #region Callback Methods

        private static void SearchEngineChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as SearchBox;

            if (self.Engine == null || self.Engine.IsBusy)
                self.Disable();

            if (!self.IsEnabled && !self.Engine.IsBusy)
                self.Enable();

            // OnBusy&OnReqady events;
            // Unsubscribe from previous engine
            if (e.OldValue != null && e.OldValue is SearchEngine)
            {
                var oldEngine = e.OldValue as SearchEngine;
                oldEngine.OnBusy -= self.Engine_OnBusy;
                oldEngine.OnReady -= self.Engine_OnReady;
            }

            // Subscribe to new engine
            if (self.Engine != null)
            {
                self.Engine.OnBusy += self.Engine_OnBusy;
                self.Engine.OnReady += self.Engine_OnReady;
            }

            /*
            if (e.NewValue != e.OldValue)
            {
                Binding binding = new Binding();
                binding.Source = self.Engine.IsReady;
                self.SetBinding(SearchBox.IsEnabledProperty, binding);
            }
            */
        }

        private static void DataSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as SearchBox;

            if (self.Engine == null)
            {
                if (string.IsNullOrEmpty(self.SearchKeyPath))
                    self.Engine = new SearchEngine(self.DataSource, self.IsSpellCheckerActive);
                else
                    self.Engine = new SearchEngine(self.DataSource, self.SearchKeyPath, self.IsSpellCheckerActive);
            }
            else
            {
                if (string.IsNullOrEmpty(self.SearchKeyPath))
                    self.Engine.Regenerate(self.DataSource, self.IsSpellCheckerActive);
                else
                    self.Engine.Regenerate(self.DataSource, self.SearchKeyPath, self.IsSpellCheckerActive);
            }
        }

        private static void OnCaptionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as SearchBox;
            self.RaisePropertyChanged("BindedCaption");
        }

        private static void OnShowBadgesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as SearchBox;

            if (self.ShowBadges)
            {
                self.SelectedItemTemplate = self.FindResource("ResultListItemTemplateWithBagdes") as DataTemplate;
                self.SelectedItemControlTemplate = self.FindResource("ResultListItemControlTemplateWithBagdes") as ControlTemplate;
            }
            else
            {
                self.SelectedItemTemplate = self.FindResource("ResultListItemTemplate") as DataTemplate;
                self.SelectedItemControlTemplate = self.FindResource("ResultListControlItemTemplate") as ControlTemplate;
            }
        }

        private static void SearchKeyPathChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as SearchBox;

            self.HasSearchKeyPath = !string.IsNullOrEmpty(self.SearchKeyPath);
        }

        private static void ResultKeyPathChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as SearchBox;

            self.HasResultKeyPath = !string.IsNullOrEmpty(self.ResultKeyPath);
        }

        #endregion

        #region Event Handlers

        private void SearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Engine == null)
                this.Disable();
        }

        private void Engine_OnReady(object sender, EventArgs e)
        {
            this.Enable();
        }

        private void Engine_OnBusy(object sender, EventArgs e)
        {
            this.Disable();
        }

        private void SearchTextBox_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.SearchTextBox.PreviewKeyDown += SearchTextBox_PreviewKeyDown;
        }

        private void SearchResultsListBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.SearchTextBox.PreviewKeyDown -= SearchTextBox_PreviewKeyDown;
        }

        private void SearchResultsListBox_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.SearchResultsListBox.PreviewKeyDown += SearchResultsListBox_PreviewKeyDown;
        }

        private void SearchTextBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.SearchResultsListBox.PreviewKeyDown -= SearchResultsListBox_PreviewKeyDown;
        }

        private void SearchResultsListBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (this.SearchResultsListBox == null)
                return;

            char c;
            char.TryParse(e.Key.ToString(), out c);

            if (e.Key == Key.Up && this.SearchResultsListBox.SelectedIndex == 0)
            {
                Keyboard.Focus(this.SearchTextBox);
                this.SearchResultsListBox.SelectedItem = null;
            }
            else if (e.Key == Key.Left)
            {
                e.Handled = true;

                // 5 geri
                int i = this.SearchResultsListBox.SelectedIndex;
                if (i >= 5)
                    this.SearchResultsListBox.SelectedIndex = i - 5;
                else
                    this.SearchResultsListBox.SelectedIndex = 0;

                var element = (this.SearchResultsListBox.ItemContainerGenerator.ContainerFromItem(this.SearchResultsListBox.SelectedItem) as FrameworkElement);
                if (element != null)
                {
                    element.Focus();
                    this.SearchResultsListBox.ScrollIntoView(this.SearchResultsListBox.SelectedItem);
                }
                else
                {
                    this.SearchResultsListBox.ScrollIntoView(this.SearchResultsListBox.SelectedItem);
                    element = (this.SearchResultsListBox.ItemContainerGenerator.ContainerFromItem(this.SearchResultsListBox.SelectedItem) as FrameworkElement);
                    if (element != null)
                        element.Focus();
                }
            }
            else if (e.Key == Key.Right)
            {
                e.Handled = true;

                // 5 ileri
                int i = this.SearchResultsListBox.SelectedIndex;
                if (i + 5 < this.SearchResultsListBox.Items.Count)
                    this.SearchResultsListBox.SelectedIndex = i + 5;
                else
                    this.SearchResultsListBox.SelectedIndex = this.SearchResultsListBox.Items.Count - 1;

                var element = (this.SearchResultsListBox.ItemContainerGenerator.ContainerFromItem(this.SearchResultsListBox.SelectedItem) as FrameworkElement);
                if (element != null)
                {
                    element.Focus();
                    this.SearchResultsListBox.ScrollIntoView(this.SearchResultsListBox.SelectedItem);
                }
                else
                {
                    this.SearchResultsListBox.ScrollIntoView(this.SearchResultsListBox.SelectedItem);
                    element = (this.SearchResultsListBox.ItemContainerGenerator.ContainerFromItem(this.SearchResultsListBox.SelectedItem) as FrameworkElement);
                    if (element != null)
                        element.Focus();
                }
            }
            else if ((e.Key != Key.Up && e.Key != Key.Down) && (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || e.Key == Key.Back))
            {
                Keyboard.Focus(this.SearchTextBox);
                this.SearchResultsListBox.SelectedItem = null;
            }
        }

        private void SearchTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Down || (e.Key == Key.Right && this.SearchTextBox.CaretIndex == this.SearchTextBox.Text.Length)) && this.SearchResultsListBox.HasItems)
            {
                e.Handled = true;
                this.SearchResultsListBox.SelectedIndex = 0;
                var listItem = this.SearchResultsListBox.ItemContainerGenerator.ContainerFromItem(this.SearchResultsListBox.SelectedItem) as FrameworkElement;
                if (listItem != null)
                    listItem.Focus();
            }
        }

        private void SearchResultsListBox_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext;

            if (dataContext is KeyValuePair<string, object>)
            {
                KeyValuePair<string, object> selectedResult = (KeyValuePair<string, object>)dataContext;
                this.SelectItem(selectedResult);
            }
        }

        private void SearchResultsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key & Key.Enter) == Key.Enter && (e.Key == Key.Enter))
            {
                if (this.SearchResultsListBox.SelectedValue != null && this.SearchResultsListBox.SelectedValue is KeyValuePair<string, object>)
                {
                    SelectItem((KeyValuePair<string, object>)this.SearchResultsListBox.SelectedValue);
                    e.Handled = true;
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.SearchTextBox.Text = string.Empty;
            if (this.ClearCommand != null && this.ClearCommand.CanExecute(null))
                this.ClearCommand.Execute(null);
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.SetCaptionVisibility();
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.SetCaptionVisibility();
        }

        #endregion

        #region RaisePropertyChanged

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Dispose

        ~SearchBox()
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                this.Dispose();
            }));
        }

        public void Dispose()
        {
            try
            {
                this.SearchTextBox.PreviewGotKeyboardFocus -= SearchTextBox_PreviewGotKeyboardFocus;
                this.SearchTextBox.PreviewLostKeyboardFocus -= SearchTextBox_PreviewLostKeyboardFocus;
                this.SearchResultsListBox.PreviewGotKeyboardFocus -= SearchResultsListBox_PreviewGotKeyboardFocus;
                this.SearchResultsListBox.PreviewLostKeyboardFocus -= SearchResultsListBox_PreviewLostKeyboardFocus;
                this.SearchTextBox.PreviewKeyDown -= SearchTextBox_PreviewKeyDown;
                this.SearchResultsListBox.PreviewKeyDown -= SearchResultsListBox_PreviewKeyDown;

                /*
                // SearchService yasadigi surece engine dispose edilmemeli
                if (this.Engine != null)
                    this.Engine.Dispose();
                */
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
