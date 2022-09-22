using CommunityToolkit.Mvvm.ComponentModel;

namespace Trungnt2910.Maui.Controls;

/// <summary>
/// A concrete implementation of <see cref="ITreeNode"/>
/// </summary>
public class TreeNode : ObservableObject, ITreeNode
{
    private object? _data;
    /// <summary>
    /// The data associated with the node.
    /// </summary>
    public object? Data 
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }

    private bool _isLeaf;
    /// <summary>
    /// Whether this node has children.
    /// </summary>
    public bool IsLeaf
    {
        get => _isLeaf;
        set => SetProperty(ref _isLeaf, value);
    }

    private bool _isExpanded;
    /// <summary>
    /// Whether this node is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }

    private ITreeNode? _parent;
    /// <summary>
    /// The parent of this node. 
    /// This may be <see langword="null"/> if the node is a root node.
    /// </summary>
    public ITreeNode? Parent
    {
        get => _parent;
        set
        {
            SetProperty(ref _parent, value);
            SetProperty(ref _level, (value?.Level ?? - 1) + 1);
        }
    }

    private int _level = 0;
    /// <summary>
    /// The level of this node. The higher the level, the deeper the node is.
    /// </summary>
    public int Level => _level;

    private IList<ITreeNode>? _children;
    /// <summary>
    /// Gets or sets the children collection of this node.
    /// This value should be <see langword="null"/> if the <see cref="IsLeaf"/> is <see langword="true"/>.
    /// </summary>
    public IList<ITreeNode>? Children 
    { 
        get => _children;
        set => SetProperty(ref _children, value);
    }

    private Func<ITreeNode, IEnumerable<ITreeNode>>? _childResolver;
    /// <summary>
    /// Gets or sets the children resolver function of this node.
    /// The function will be called if <see cref="IsLeaf"/> is set to <see langword="true"/> but
    /// <see cref="Children"/> is set to <see langword="null"/>.
    /// </summary>
    public Func<ITreeNode, IEnumerable<ITreeNode>>? ChildrenResolver 
    { 
        get => _childResolver; 
        set => SetProperty(ref _childResolver, value);
    }
}
