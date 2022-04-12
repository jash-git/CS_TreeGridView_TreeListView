// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉
// <copyright company="brightman software studios" file="DataNode.cs">
//   Copyright © brightman software studios 2008-2012. All rights reserved.
// </copyright>
// <author>Peter Brightman</author>
// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉

using System.Collections.Generic;

namespace VirtualModeTreeListView.Model
{
    /// <summary>
    /// Data node interface
    /// </summary>
    public interface IDataNode
    {
        /// <summary>
        /// Adds a child node.
        /// </summary>
        /// <param name="child"></param>
        void AddChild(IDataNode child);
        /// <summary>
        /// Counts the number of kids.
        /// </summary>
        int CountChildren { get; }
        /// <summary>
        /// The kid collection.
        /// </summary>
        List<IDataNode> Children { get; }
        /// <summary>
        /// The name of the data node.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// The status expanded/collapsed.
        /// </summary>
        bool Expanded { get; set; }
        /// <summary>
        /// The indentation level of the data node.
        /// </summary>
        int Level { get; set; }
        /// <summary>
        /// The comment of the data node.
        /// </summary>
        string Comment { get; set; }
    }

    /// <summary>
    /// Data node class
    /// </summary>
    public class DataNode : IDataNode
    {
        private string m_Name;
        private string m_Comment;
        private bool m_Expanded;
        private int m_Level;
        internal List<IDataNode> m_Children;
        
        /// <summary>
        /// The data node class.
        /// </summary>
        /// <param name="nam">name of the node</param>
        /// <param name="cmt">comment of the node</param>
        /// <param name="lvl">indentation level of the node</param>
        /// <param name="stat">collapsed/expanded state</param>
        public DataNode(string nam, string cmt, int lvl, bool stat)
        {
            m_Name = nam;
            m_Comment = cmt;
            m_Level = lvl;
            m_Expanded = stat;
            m_Children = new List<IDataNode>(16);
        }

        /// <summary>
        /// Adds a child node.
        /// </summary>
        /// <param name="child">The child to add.</param>
        public void AddChild(IDataNode child)
        {
            m_Children.Add(child);
            
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// Gets the children count.
        /// </summary>
        /// <value>The children count.</value>
        public int CountChildren
        {
            get { return m_Children.Count; }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<IDataNode> Children
        {
            get { return m_Children; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataNode"/> is expanded.
        /// </summary>
        /// <value><c>true</c> if expanded; otherwise, <c>false</c>.</value>
        public bool Expanded
        {
            get { return m_Expanded; }
            set { m_Expanded = value; }
        }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment
        {
            get { return m_Comment; }
            set { m_Comment = value; }
        }
    }
}