// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉
// <copyright company="brightman software studios" file="IModel.cs">
//   Copyright © brightman software studios 2008-2012. All rights reserved.
// </copyright>
// <author>Peter Brightman</author>
// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉

using System.Collections.Generic;

namespace VirtualModeTreeListView.Model
{
    /// <summary>
    /// The model interface
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// The interface to the model of the application.
        /// </summary>
        void ImportDataModel();
        /// <summary>
        /// The pool of data nodes.
        /// </summary>
        List<IDataNode> DataPool { get; }
    }
}
