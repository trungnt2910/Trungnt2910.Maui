//using System.ComponentModel;
//using System.Runtime.CompilerServices;

//namespace Trungnt2910.Maui.Controls;

//internal class TreeNodeView : ContentView
//{
//    /// <summary>
//    /// Backing store for the <see cref="ItemSource"/> property.
//    /// </summary>
//    public static readonly BindableProperty ItemSourceProperty
//        = BindableProperty.Create(nameof(ItemSource), typeof(object), typeof(TreeNodeView),
//            propertyChanged: (bindable, oldValue, newValue) =>
//            {
//                if (bindable is TreeNodeView treeNodeView)
//                if (oldValue != newValue)
//                {
//                    if (oldValue is INotifyPropertyChanged oldNotify)
//                    {
//                        oldNotify.PropertyChanged -= treeNodeView.ItemSource_PropertyChanged;
//                    }
//                    if (newValue is INotifyPropertyChanged newNotify)
//                    {
//                        newNotify.PropertyChanged += treeNodeView.ItemSource_PropertyChanged;
//                    }
//                }
//            });

//    /// <summary>
//    /// Gets or sets the items source of the <see cref="TreeView"/>.
//    /// </summary>
//    public object? ItemSource
//    {
//        get { return (object?)GetValue(ItemSourceProperty); }
//        set { SetValue(ItemSourceProperty, value); }
//    }

//    public static readonly BindableProperty ParentNodeProperty
//        = BindableProperty.Create(nameof(ParentNode), typeof(TreeNodeView), typeof(TreeNodeView), null);

//    public TreeNodeView? ParentNode
//    {
//        get { return (TreeNodeView?)GetValue(ParentNodeProperty); }
//        private set { SetValue(ParentNodeProperty, value); }
//    }
  
//    public static readonly BindableProperty OwnerTreeProperty
//        = BindableProperty.Create(nameof(OwnerTree), typeof(TreeView), typeof(TreeNodeView), null);

//    public TreeView OwnerTree
//    {
//        get { return (TreeView)GetValue(OwnerTreeProperty); }
//        // Should be internal but this breaks the XAML parser.
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        init { SetValue(OwnerTreeProperty, value); }
//    }

//    public static readonly BindableProperty IsExpandedProperty
//        = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(TreeNodeView), true);

//    public bool IsExpanded
//    {
//        get { return (bool)GetValue(IsExpandedProperty); }
//        set { SetValue(IsExpandedProperty, value); }
//    }

//    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
//    {
//        base.OnPropertyChanged(propertyName);

//        switch (propertyName)
//        {
//            case nameof(ItemSource):
//            case nameof(OwnerTree):
//                Render();
//            break;
//        }
//    }

//    protected virtual void ItemSource_PropertyChanged(object? sender, PropertyChangedEventArgs e)
//    {
//        Render();
//    }

//    public TreeNodeView()
//    {
//        Render();
//    }

//    private void Render()
//    {
//        if (ItemSource is ITreeNode treeNode)
//        {
//            var contentLayout = new VerticalStackLayout();

//            contentLayout.SetBinding(VerticalOptionsProperty, new Binding()
//            {
//                Source = this,
//                Path = nameof(VerticalOptions)
//            });
//            contentLayout.SetBinding(HorizontalOptionsProperty, new Binding()
//            {
//                Source = this,
//                Path = nameof(HorizontalOptions)
//            });
//            var contentView = OwnerTree?.ItemTemplate?.CreateContent() as View;
//            if (contentView != null)
//            {
//                contentView.BindingContext = treeNode.Data;
//                contentLayout.Add(contentView);
//                if (!treeNode.IsLeaf && IsExpanded)
//                {
//                    if (treeNode.Children == null && treeNode.ChildResolver != null)
//                    {
//                        treeNode.Children = treeNode.ChildResolver(treeNode);
//                    }
//                    var childView = new CollectionView();
//                    childView.SetBinding(VerticalOptionsProperty, new Binding()
//                    {
//                        Source = this,
//                        Path = nameof(VerticalOptions)
//                    });
//                    childView.SetBinding(HorizontalOptionsProperty, new Binding()
//                    {
//                        Source = this,
//                        Path = nameof(HorizontalOptions)
//                    });
//                    childView.ItemsSource = treeNode.Children;
//                    childView.ItemTemplate = new DataTemplate(() =>
//                    {
//                        var tnv = new TreeNodeView()
//                        {
//                            OwnerTree = OwnerTree!,
//                            ParentNode = this
//                        };
//                        tnv.SetBinding(VerticalOptionsProperty, new Binding()
//                        {
//                            Source = this,
//                            Path = nameof(VerticalOptions)
//                        });
//                        tnv.SetBinding(HorizontalOptionsProperty, new Binding()
//                        {
//                            Source = this,
//                            Path = nameof(HorizontalOptions)
//                        });
//                        tnv.BindingContext = new Binding();
//                        tnv.ItemSource = new Binding();
//                        return tnv;
//                    });
//                    contentLayout.Add(childView);
//                }
//                Content = contentLayout;
//            }
//        }
//        else
//        {
//            Content = OwnerTree?.ItemTemplate?.CreateContent() as View;
//        }
//    }
//}
