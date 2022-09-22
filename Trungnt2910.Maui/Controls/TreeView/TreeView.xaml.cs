using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Controls.Internals;
using Trungnt2910.Maui.Converters;

namespace Trungnt2910.Maui.Controls;

/// <summary>
/// A control that displays a hierarchical collection of labeled items.
/// </summary>
[Preserve]
public partial class TreeView : ContentView
{
    /// <summary>
    /// Backing store for the <see cref="ItemsSource"/> property.
    /// </summary>
    public static readonly BindableProperty ItemsSourceProperty
        = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TreeView), null,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is TreeView treeView && oldValue != newValue)
                {
                    treeView.OnItemsSourceChanged((IEnumerable)oldValue, (IEnumerable)newValue);
                }
            });

    /// <summary>
    /// Gets or sets the items source of the <see cref="TreeView"/>.
    /// </summary>
    public IEnumerable? ItemsSource
    {
        get { return (IEnumerable?)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// Backing store for the <see cref="ItemTemplate"/> property.
    /// </summary>
    public static readonly BindableProperty ItemTemplateProperty
        = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(TreeView), null,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is TreeView treeView && oldValue != newValue)
                {
                    treeView.OnItemTemplateChanged((DataTemplate)oldValue, (DataTemplate)newValue);
                }
            });

    /// <summary>
    /// Gets or sets the item template of the <see cref="TreeView"/>
    /// </summary>
    public DataTemplate ItemTemplate
    {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    /// <summary>
    /// Backing store for the <see cref="HeaderTemplate"/> property.
    /// </summary>
    public static readonly BindableProperty HeaderTemplateProperty
        = BindableProperty.Create(nameof(HeaderTemplate), typeof(DataTemplate), typeof(TreeView));

    /// <summary>
    /// Gets or sets a data template to use to format a data object for display at the 
    /// top of the tree view.
    /// </summary>
    public DataTemplate HeaderTemplate 
    {
        get => (DataTemplate)GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    /// <summary>
    /// Backing store for the <see cref="Header"/> property.
    /// </summary>
    public static readonly BindableProperty HeaderProperty
        = BindableProperty.Create(nameof(Header), typeof(object), typeof(TreeView));

    /// <summary>
    ///  Gets or sets the string, binding, or view that will be displayed at the top of the tree view.
    /// </summary>
    public object Header 
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Backing store for the <see cref="FooterTemplate"/> property.
    /// </summary>
    public static readonly BindableProperty FooterTemplateProperty
        = BindableProperty.Create(nameof(FooterTemplate), typeof(DataTemplate), typeof(TreeView));

    /// <summary>
    /// Gets or sets a data template to use to format a data object for display at the 
    /// bottom of the tree view.
    /// </summary>
    public DataTemplate FooterTemplate
    {
        get => (DataTemplate)GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    /// <summary>
    /// Backing store for the <see cref="Footer"/> property.
    /// </summary>
    public static readonly BindableProperty FooterProperty
        = BindableProperty.Create(nameof(Footer), typeof(object), typeof(TreeView));

    /// <summary>
    ///  Gets or sets the string, binding, or view that will be displayed at the bottom of the tree view.
    /// </summary>
    public object Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    /// <summary>
    /// Backing store for the <see cref="Indent"/> property.
    /// </summary>
    public static readonly BindableProperty IndentProperty
        = BindableProperty.Create(nameof(Indent), typeof(double), typeof(TreeView), (double)10,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is TreeView treeView && (double)oldValue != (double)newValue)
                {
                    treeView.OnItemTemplateChanged(treeView.ItemTemplate, treeView.ItemTemplate);
                }
            });

    /// <summary>
    /// Gets or sets the distance to indent each child tree node level.
    /// </summary>
    public double Indent
    {
        get { return (double)GetValue(IndentProperty); }
        set { SetValue(IndentProperty, value); }
    }

    private ObservableCollection<object?> _collection = new();
    private IList _itemsSourceList = Array.Empty<object>();

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeView"/> class.
    /// </summary>
    public TreeView()
	{
		InitializeComponent();
        ItemsView.ItemsSource = _collection;
	}

    private void OnItemTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
    {
        ItemsView.ItemTemplate = new DataTemplate(() =>
        {
            var layout = new Grid()
            {
                HorizontalOptions = LayoutOptions.Fill,
                ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition() { Width = GridLength.Auto },
                    new ColumnDefinition() { Width = GridLength.Star }
                }
            };
            var converter = new MultiConverter()
            {
                new IntToDoubleConverter(),
                new MathExpressionConverter(),
                new NumberToThicknessConverter() { ConverterOptions = NumberToThicknessOptions.Left }
            };
            layout.SetBinding(MarginProperty,
                new Binding(
                    nameof(TreeNode.Level),
                    converter: converter,
                    converterParameter: $"x*{Indent}"
                    ));
            var expandCollapseButton = new ImageButton()
            {
                Background = Colors.Transparent,
                Source = $"treeview_down_{(Application.Current?.RequestedTheme == AppTheme.Dark ? "light" : "dark")}.png"
            };
            expandCollapseButton.Clicked += ExpandCollapseButton_Clicked;
            expandCollapseButton.SetBinding(
                RotationProperty,
                new Binding(
                    nameof(ITreeNode.IsExpanded),
                    converter: new BoolToObjectConverter { TrueObject = (double)0, FalseObject = (double)-90 }));
            expandCollapseButton.SetBinding(
                OpacityProperty,
                new Binding(nameof(ITreeNode.IsLeaf),
                converter: new BoolToObjectConverter { TrueObject = (double)0, FalseObject = (double)1 }));
            layout.Add(expandCollapseButton);
            layout.SetColumn(expandCollapseButton, 0);
            var childView = newValue.CreateContent() as View;
            if (childView != null)
            {
                childView.HorizontalOptions = LayoutOptions.Fill;
                layout.Add(childView);
                layout.SetColumn(childView, 1);
            }
            return new ViewCell() { View = layout };
        });
    }

    private void ExpandCollapseButton_Clicked(object? sender, EventArgs e)
    {
        if (sender is ImageButton button)
        {
            if (button.BindingContext is ITreeNode treeNode && !treeNode.IsLeaf)
            {
                treeNode.IsExpanded = !treeNode.IsExpanded;
            }
        }
    }

    private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        if (oldValue is INotifyCollectionChanged oldNotifyCollectionChanged)
        {
            oldNotifyCollectionChanged.CollectionChanged -= OnItemsSourceCollectionChanged;
        }
        if (newValue is INotifyCollectionChanged newNotifyCollectionChanged)
        {
            newNotifyCollectionChanged.CollectionChanged += OnItemsSourceCollectionChanged;
        }
        if (newValue is IList newList)
        {
            _itemsSourceList = newList;
        }
        else
        {
            _itemsSourceList = newValue.Cast<object?>().ToList();
        }
        Render();
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        switch (args.PropertyName)
        {
            case nameof(ITreeNode.IsExpanded):
            case nameof(ITreeNode.Children):
            case nameof(ITreeNode.ChildrenResolver):
            case nameof(ITreeNode.Parent):
                Render();
            break;
        }
    }

    private void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        if (ItemsSource is IList newList)
        {
            _itemsSourceList = newList;
        }
        else
        {
            _itemsSourceList = (IList?)ItemsSource?.Cast<object?>()?.ToList() ?? Array.Empty<object>();
        }
        Render();
    }

    void Render()
    {
        foreach (var item in _collection)
        {
            if (item is INotifyPropertyChanged notifyPropertyChangedItem)
            {
                notifyPropertyChangedItem.PropertyChanged -= OnItemPropertyChanged;
            }
        }
        var oldSelected = ItemsView.SelectedItem;
        var newCollection = new ObservableCollection<object?>();
        if (ItemsSource != null)
        {
            foreach (var obj in _itemsSourceList)
            {
                if (obj is ITreeNode currentNode)
                {
                    void InsertNodeAndChildren(ITreeNode node)
                    {
                        newCollection.Add(node);
                        if (!node.IsLeaf && node.IsExpanded)
                        {
                            if (node.Children == null && node.ChildrenResolver != null)
                            {
                                node.Children = node.ChildrenResolver(node)?.ToList();
                            }
                            if (node.Children != null)
                            {
                                foreach (var childNode in node.Children)
                                {
                                    InsertNodeAndChildren(childNode);
                                }
                            }
                        }
                        if (node is INotifyPropertyChanged notifyPropertyChangedNode)
                        {
                            notifyPropertyChangedNode.PropertyChanged += OnItemPropertyChanged;
                        }
                    }
                    InsertNodeAndChildren(currentNode);
                }
                else
                {
                    newCollection.Add(obj);
                }
            }
        }
        _collection = newCollection;
        ItemsView.ItemsSource = newCollection;
        if (newCollection.Contains(oldSelected))
        {
            ItemsView.SelectedItem = oldSelected;
        }
    }
}