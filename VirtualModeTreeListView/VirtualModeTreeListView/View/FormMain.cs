// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉
// <copyright company="brightman software studios" file="FormMain.cs">
//   Copyright © brightman software studios 2008-2012. All rights reserved.
// </copyright>
// <author>Peter Brightman</author>
// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using System.Reflection;
using VirtualModeTreeListView.Model;

namespace VirtualModeTreeListView
{
    /// <summary>
    /// The main form.
    /// </summary>
    public partial class FormMain : Form
    {
        private readonly IModel m_Model;
        private readonly List<IDataNode> m_Mapper = new List<IDataNode>(64);
        private const TextFormatFlags s_MTff = TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis | TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine;
        private Rectangle m_Rect;
        private readonly Color m_ColorFrom = SystemColors.HotTrack;
        private readonly Color m_ColorTo = SystemColors.ControlLight;
        private readonly Blend m_Blend = new Blend();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain"/> class.
        /// </summary>
        /// <param name="mdl">The model.</param>
        public FormMain(IModel mdl)
        {
            m_Model = mdl;
            InitializeComponent();
            float[] myFactors = { .2f, .4f, .6f, .6f, .4f, .2f };
            float[] myPositions = { 0.0f, .2f, .4f, .6f, .8f, 1.0f };
            m_Blend.Factors = myFactors;
            m_Blend.Positions = myPositions;
        }

        /// <summary>
        /// Inits the virtual list view nodes.
        /// </summary>
        private void InitVirtualListViewNodes()
        {
            m_Mapper.Clear();

            ObtainAllNodes(m_Model.DataPool); // obtain top level nodes and expanded child nodes

            listView1.VirtualListSize = m_Mapper.Count;
            listView1.VirtualMode = true;
            listView1.Invalidate();
        }

        /// <summary>
        /// Obtains all nodes.
        /// </summary>
        /// <param name="nds">The node list.</param>
        private void ObtainAllNodes(IEnumerable<IDataNode> nds)
        {
            foreach (IDataNode dn in nds)
            {
                m_Mapper.Add(dn);
                if (dn.Expanded)
                {
                    ObtainAllNodes(dn.Children);
                }
            }
        }

        /// <summary>
        /// Makes the list view item.
        /// </summary>
        /// <param name="dn">The data node.</param>
        /// <returns></returns>
        private static ListViewItem MakeListViewItem(IDataNode dn)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = dn.Name;
            ListViewItem.ListViewSubItem lvsi1 = new ListViewItem.ListViewSubItem();
            lvsi1.Text = dn.CountChildren.ToString(CultureInfo.InvariantCulture);
            ListViewItem.ListViewSubItem lvsi2 = new ListViewItem.ListViewSubItem();
            lvsi2.Text = dn.Level.ToString(CultureInfo.InvariantCulture);
            ListViewItem.ListViewSubItem lvsi3 = new ListViewItem.ListViewSubItem();
            lvsi3.Text = dn.Comment;

            lvi.IndentCount = dn.Level;
            
            lvi.SubItems.Add(lvsi1);
            lvi.SubItems.Add(lvsi2);
            lvi.SubItems.Add(lvsi3);
            
            if (dn.Expanded)
                lvi.StateImageIndex = 1;
            else if (dn.CountChildren > 0)
                lvi.StateImageIndex = 0;

            return lvi;
        }

        /// <summary>
        /// Exits the app.
        /// </summary>
        private void ExitApp()
        {
            Close();
        }

        /// <summary>
        /// Shows the about box.
        /// </summary>
        private void ShowAboutBox()
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog(this);
        }

        /// <summary>
        /// Retrieve virtual item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.RetrieveVirtualItemEventArgs"/> instance containing the event data.</param>
        private void ListView1RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = MakeListViewItem(m_Mapper[e.ItemIndex]);
        }

        /// <summary>
        /// the main form load event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FormMainLoad(object sender, EventArgs e)
        {
            PropertyInfo aProp = typeof(ListView).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            aProp.SetValue(listView1, true, null);
        }

        /// <summary>
        /// Mouse click event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void ListView1MouseClick(object sender, MouseEventArgs e)
        {
            ExpandCollapseNodes(sender, e);
        }

        /// <summary>
        /// Mouse double click event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExpandCollapseNodes(sender, e);
        }

        /// <summary>
        /// Expands or collapse a node.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void ExpandCollapseNodes(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListView lv = (ListView)sender;
                ListViewItem lvi = lv.GetItemAt(e.X, e.Y);
                if (lvi != null)
                {
                    // hack to draw first column correctly
                    lv.RedrawItems(lvi.Index, lvi.Index, true);

                    IDataNode mbr = m_Mapper[lvi.Index];

                    int xfrom = lvi.IndentCount * 16;
                    int xto = xfrom + 16;

                    if ((e.X >= xfrom && e.X <= xto) || e.Clicks > 1)
                    {
                        if (mbr.CountChildren > 0)
                        {
                            mbr.Expanded = !mbr.Expanded;
                            lvi.Checked = !lvi.Checked;

                            PrepareNodes(lvi.Index, mbr.Expanded);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Prepares the nodes.
        /// </summary>
        /// <param name="pos">The pos.</param>
        /// <param name="add">if set to <c>true</c> [add].</param>
        private void PrepareNodes(int pos, bool add)
        {
            IDataNode mbr = m_Mapper[pos];
            pos++;

            if (add)
            {
                PopulateDescendantMembers(ref pos, mbr);
            }
            else
            {
                int kids = ObtainExpandedChildrenCount(pos-1);
                m_Mapper.RemoveRange(pos, kids);
            }
           
            listView1.VirtualListSize = m_Mapper.Count;
        }

        /// <summary>
        /// Populates the descendant members.
        /// </summary>
        /// <param name="pos">The pos.</param>
        /// <param name="mbr">The data node.</param>
        private void PopulateDescendantMembers(ref int pos, IDataNode mbr)
        {
            foreach (IDataNode m in mbr.Children)
            {
                m_Mapper.Insert(pos++, m);
                if (m.Expanded)
                {
                    PopulateDescendantMembers(ref pos, m);
                }
            }
        }

        /// <summary>
        /// Obtains the expanded children count.
        /// </summary>
        /// <param name="pos">The pos.</param>
        /// <returns></returns>
        private int ObtainExpandedChildrenCount(int pos)
        {
            int kids = 0;
            IDataNode mi = m_Mapper[pos];
            int level = mi.Level;

            for (int i = pos + 1; i < m_Mapper.Count; i++, kids++)
            {
                IDataNode mix = m_Mapper[i];
                int lvlx = mix.Level;
                if (lvlx <= level) break;
            }

            return kids;
        }

        /// <summary>
        /// Creates the brush.
        /// </summary>
        /// <returns></returns>
        private LinearGradientBrush CreateBrush()
        {
            LinearGradientBrush br = new LinearGradientBrush(m_Rect, m_ColorFrom, m_ColorTo, 90f);
            br.Blend = m_Blend;
            return br;
        }

        /// <summary>
        /// Handles the DrawItem event of the listView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawListViewItemEventArgs"/> instance containing the event data.</param>
        private void ListView1DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // not used
        }

        /// <summary>
        /// Handles the DrawSubItem event of the listView1 control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawListViewSubItemEventArgs"/> instance containing the event data.</param>
        private void ListView1DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // calculate x offset from ident-level
            int xOffset = e.Item.IndentCount * 16;

            // calculate x position
            int xPos = e.Bounds.X + xOffset + 16;

            Rectangle r = e.Bounds;

            if (e.ColumnIndex == 0)
            {
                // drawing of first column, icon as well as text

                r = e.Bounds;
                r.Y += 1; r.Height -= 1; r.Width -= 1;
                e.Graphics.FillRectangle(SystemBrushes.ControlLight, r);

                // set rectangle bounds for drawing of state-icon
                m_Rect.Height = 16;
                m_Rect.Width = 16;
                m_Rect.X = e.Bounds.X + xOffset;
                m_Rect.Y = e.Bounds.Y;
                
                if (e.Item.Checked)
                {
                    // draw expanded icon
                    e.Graphics.DrawImage(imageList1.Images[1], m_Rect);
                }
                else if (0 == e.Item.StateImageIndex)
                {
                    // draw collapsed icon
                    e.Graphics.DrawImage(imageList1.Images[0], m_Rect);
                }
                else
                {
                    // draw normal icon (for unexpandable items)
                    e.Graphics.DrawImage(imageList1.Images[2], m_Rect);
                }

                // set rectangle bounds for drawing of item/subitem text
                m_Rect.Height = e.Bounds.Height;
                m_Rect.Width = e.Bounds.Width - xPos;
                m_Rect.X = xPos;
                m_Rect.Y = e.Bounds.Y;

                // draw item/subitem text
                if ((e.ItemState & ListViewItemStates.Selected) != 0)
                {
                    LinearGradientBrush br = CreateBrush();
                    e.Graphics.FillRectangle(br, m_Rect);

                    // draw selected item's text
                    TextRenderer.DrawText(e.Graphics, e.Item.Text, e.Item.Font, m_Rect, SystemColors.HighlightText, s_MTff);
                }
                else
                {
                    // draw unselected item's text
                    TextRenderer.DrawText(e.Graphics, e.Item.Text, e.Item.Font, m_Rect, e.Item.ForeColor, s_MTff);
                }
            }
            else
            {
                r.Y += 1; r.Height -= 1; r.Width -= 1;
                e.Graphics.FillRectangle(SystemBrushes.ControlLight, r);

                // drawing of all other columns, e. g. drawing of subitems
                if ((e.ItemState & ListViewItemStates.Selected) != 0)
                {
                    LinearGradientBrush br = CreateBrush();
                    e.Graphics.FillRectangle(br, e.Bounds);
                    
                    TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.Item.Font, e.Bounds, SystemColors.HighlightText, s_MTff);
                }
                else
                {
                    TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.Item.Font, e.Bounds, e.Item.ForeColor, s_MTff);
                }
            }
        }

        /// <summary>
        /// Handles the DrawColumnHeader event of the listView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawListViewColumnHeaderEventArgs"/> instance containing the event data.</param>
        private void ListView1DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        /// <summary>
        /// Toolstrip menu item exit click handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripMenuItemExitClick(object sender, EventArgs e)
        {
            ExitApp();
        }

        /// <summary>
        /// Toolstrip menu item load click handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripMenuItemLoadClick(object sender, EventArgs e)
        {
            m_Model.ImportDataModel();
            InitVirtualListViewNodes();
        }

        /// <summary>
        /// Toolstrip menu item about click handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripMenuItemAboutClick(object sender, EventArgs e)
        {
            ShowAboutBox();
        }

    }
}