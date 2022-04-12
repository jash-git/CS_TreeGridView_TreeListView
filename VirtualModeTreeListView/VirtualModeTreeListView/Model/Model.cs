// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉
// <copyright company="brightman software studios" file="Model.cs">
//   Copyright © brightman software studios 2008-2012. All rights reserved.
// </copyright>
// <author>Peter Brightman</author>
// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉

using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace VirtualModeTreeListView.Model
{
    /// <summary>
    /// The model class
    /// </summary>
    public class Model : IModel
    {
        private OpenFileDialog openFileDialog;
        private List<IDataNode> m_DataPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        public Model()
        {
            openFileDialog = new OpenFileDialog();
            m_DataPool = new List<IDataNode>(64);
        }

        /// <summary>
        /// Imports the data model.
        /// </summary>
        public void ImportDataModel()
        {
            FormMain fm = (FormMain)Application.OpenForms[0].FindForm();
            openFileDialog.Filter = "Xml files (*.xml)|*.xml";
            DialogResult dr = openFileDialog.ShowDialog(fm);
            if (DialogResult.OK == dr)
            {
                //m_SelectedPlc.Clear();
                string mXmlFile = openFileDialog.FileName;
                try
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(mXmlFile);
                    ObtainModel(xdoc);
                }
                catch (XmlException xex)
                {
                    MessageBox.Show(fm, xex.Message, "Error when loading xml file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Obtains the model.
        /// </summary>
        /// <param name="mXmlDoc">The m XML doc.</param>
        private void ObtainModel(XmlDocument mXmlDoc)
        {
            int level = 0;
            
            // check for data model
            string xPath = "TREELISTVIEW-MODEL";
            XmlNode xnlProg = mXmlDoc.SelectSingleNode(xPath);

            if (xnlProg != null)
            {
                DataPool.Clear();

                // check for nodes
                xPath = "./Node";
                XmlNodeList xnl = xnlProg.SelectNodes(xPath);

                if (xnl != null)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        string nam = ObtainNodeName(xn);
                        string cmt = ObtainNodeComment(xn);
                        bool stat = ObtainNodeState(xn);
                        IDataNode m = new DataNode(nam, cmt, level, stat);
                        CollectNodes(m, xn, ref level);
                        DataPool.Add(m);
                    }
                }
            }
        }

        /// <summary>
        /// Collects the nodes.
        /// </summary>
        /// <param name="mbr">The data node.</param>
        /// <param name="mnl">The xml node.</param>
        /// <param name="level">The level.</param>
        private void CollectNodes(IDataNode mbr, XmlNode mnl, ref int level)
        {
            // check for nodes
            string xPath = "./Node";
            XmlNodeList xnl = mnl.SelectNodes(xPath);

            level++;

            foreach (XmlNode xn in xnl)
            {
                string nam = ObtainNodeName(xn);
                string cmt = ObtainNodeComment(xn);
                bool stat = ObtainNodeState(xn);
                IDataNode m = new DataNode(nam, cmt, level, stat);
                mbr.AddChild(m);
                CollectNodes(m, xn, ref level);
            }

            level--;
        }

        /// <summary>
        /// Obtains the node comment.
        /// </summary>
        /// <param name="xn">The xml node.</param>
        /// <returns></returns>
        private string ObtainNodeComment(XmlNode xn)
        {
            string xPath = "./@comment";
            string comment = string.Empty;

            XmlNode n = xn.SelectSingleNode(xPath);
            if (n != null) comment = n.Value;

            return comment;
        }

        /// <summary>
        /// Obtains the name of the node.
        /// </summary>
        /// <param name="xn">The xml node.</param>
        /// <returns></returns>
        private string ObtainNodeName(XmlNode xn)
        {
            string xPath = "./@name";
            string blockName = string.Empty;

            XmlNode n = xn.SelectSingleNode(xPath);
            if (n != null) blockName = n.Value;

            return blockName;
        }

        /// <summary>
        /// Obtains the state of the node.
        /// </summary>
        /// <param name="xn">The xml node.</param>
        /// <returns></returns>
        private bool ObtainNodeState(XmlNode xn)
        {
            string xPath = "./@expanded";
            string nodeState = string.Empty;

            XmlNode n = xn.SelectSingleNode(xPath);
            if (n != null) nodeState = n.Value;

            if (nodeState.Equals("y") || nodeState.Equals("yes") || nodeState.Equals("true"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the data pool.
        /// </summary>
        /// <value>The data pool.</value>
        public List<IDataNode> DataPool
        {
            get { return m_DataPool; }
        }
    }
    
}