namespace Trungnt2910.Maui.Controls;

/// <summary>
/// Represents a node of a <see cref="TreeView"/>
/// </summary>
public interface ITreeNode
{
    /// <summary>
    /// The data associated with the node.
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Whether this node has children.
    /// </summary>
    public bool IsLeaf { get; set; }

    /// <summary>
    /// Whether this node is expanded.
    /// </summary>
    public bool IsExpanded { get; set; }

    /// <summary>
    /// The parent of this node. 
    /// This may be <see langword="null"/> if the node is a root node.
    /// </summary>
    public ITreeNode? Parent { get; set; }

    /// <summary>
    /// The level of this node. The higher the level, the deeper the node is.
    /// </summary>
    public int Level { get; }

    /// <summary>
    /// Gets or sets the children collection of this node.
    /// This value should be <see langword="null"/> if the <see cref="IsLeaf"/> is <see langword="true"/>.
    /// </summary>
    public IList<ITreeNode>? Children { get; set; }

    /// <summary>
    /// Gets or sets the children resolver function of this node.
    /// The function will be called if <see cref="IsLeaf"/> is set to <see langword="true"/> but
    /// <see cref="Children"/> is set to <see langword="null"/>.
    /// </summary>
    public Func<ITreeNode, IEnumerable<ITreeNode>>? ChildrenResolver { get; set; }
}
