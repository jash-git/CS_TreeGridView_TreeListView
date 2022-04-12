using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeGridViewTestOnly
{
    public partial class Form1 : Form
    {
        public List<Person> personList = new List<Person>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //首先生成数据
            this.GenerateData();
            //然后生成列
            this.GenerateClo();

        }
        /// <summary>
        /// 生成数据
        /// </summary>
        private void GenerateData() {
            for (int i = 0; i < 100; i++)
            {
                Person p1 = new Person() { Id = i, Password = "密码" + i, Level = 1 };
                for (int j = 0; j < 10; j++)
                {
                    Person p2 = new Person() { Id = j, Password = "密码" + j, Level = 2 };
                    for (int k = 0; k < 10; k++)
                    {
                        Person p3 = new Person() { Id = k, Password = "密码" + k, Level = 3 };
                        if (p2.ChildList == null)
                        {
                            p2.ChildList = new List<Person>();
                        }
                        p2.ChildList.Add(p3);
                    }
                    if (p1.ChildList == null)
                    {
                        p1.ChildList = new List<Person>();
                    }
                    p1.ChildList.Add(p2);
                }
                personList.Add(p1);
            }
        }

        /// <summary>
        /// 生成列
        /// </summary>
        private void GenerateClo()
        {
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;

            #region 影响性能

            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            #endregion

            this.dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            //this.dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dataGridView1.VirtualMode = true;

            this.dataGridView1.CellValueNeeded -= dataGridView1_CellValueNeeded;
            this.dataGridView1.CellValueNeeded += dataGridView1_CellValueNeeded;


            this.dataGridView1.Columns.Clear();

            int colWidth = 100;

            DataGridViewImageColumn colImg = new DataGridViewImageColumn();
            colImg.HeaderText = String.Empty;
            colImg.Width = 20;

            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "colId";
            colId.HeaderText = "序号";
            colId.MinimumWidth = 160;



            DataGridViewTextBoxColumn colA = new DataGridViewTextBoxColumn();
            colA.HeaderText = "ID";
            colA.Width = colWidth;

            DataGridViewTextBoxColumn colB = new DataGridViewTextBoxColumn();
            colB.HeaderText = "密码";
            colB.Width = colWidth;




            this.dataGridView1.Columns.Add(colId);
            this.dataGridView1.Columns.Add(colA);
            this.dataGridView1.Columns.Add(colB);


            foreach (DataGridViewColumn col in this.dataGridView1.Columns)
            {
                col.ReadOnly = true;
            }

            this.dataGridView1.CellMouseClick += dataGridView1_CellMouseClick;
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= this.dataGridView1.RowCount || e.RowIndex > this.personList.Count)
            {
                return;
            }
            Random r = new Random();
            r.NextDouble();
            Person record = this.personList[e.RowIndex];
            object[] values = new object[] { record.Id, record.id, record.password };
            e.Value = values[e.ColumnIndex];

            if (e.ColumnIndex == 0)
            {
                Person currRecord = this.personList[e.RowIndex];
                if (currRecord.ChildList != null && currRecord.ChildList.Count > 0)
                {
                    if (currRecord.IsExpanded)
                    {
                        e.Value = GenerateSpace(currRecord.Level) + "  [-]  " + values[e.ColumnIndex];
                    }
                    else
                    {
                        e.Value = GenerateSpace(currRecord.Level) + "  [+]  " + values[e.ColumnIndex];
                    }
                }
                else
                {
                    e.Value = GenerateSpace(currRecord.Level + 1) + values[e.ColumnIndex];
                }
            }
        }

        /// <summary>
        /// 根据节点级别生成要显示的空格数
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public string GenerateSpace(int level)
        {
            int step = 5;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < step * (level - 1); i++)
            {
                sb.Append(" ");
            }
            return sb.ToString();
        }
        void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "colId")
            {

                Console.WriteLine("MouseClick...");
                if (this.personList[e.RowIndex].ChildList != null && this.personList[e.RowIndex].ChildList.Count > 0)
                {
                    if (this.personList[e.RowIndex].IsExpanded)
                    {
                        //this.lst2.RemoveRange(e.RowIndex + 1, this.lst2[e.RowIndex].ChildList.Count);
                        this.RecursiveRemove(this.personList, e.RowIndex);
                        this.personList[e.RowIndex].IsExpanded = false;
                        this.ShowData(this.personList);
                    }
                    else
                    {
                        this.personList.InsertRange(e.RowIndex + 1, this.personList[e.RowIndex].ChildList);
                        this.personList[e.RowIndex].IsExpanded = true;
                        this.ShowData(this.personList);
                    }

                    this.dataGridView1.Rows[0].Selected = false;
                    this.dataGridView1.Rows[e.RowIndex].Selected = true;
                }
            }
        }

        /// <summary>
        /// 递归移除
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public void RecursiveRemove(List<Person> lst, int index)
        {
            int count = GetAllExpandChildCount(lst[index]);
            lst.RemoveRange(index + 1, count);
        }

        public int GetAllExpandChildCount(Person r)
        {
            if (r.ChildList != null && r.ChildList.Count > 0)
            {
                int cnt = 0;
                if (r.IsExpanded)
                {
                    cnt = r.ChildList.Count;
                    for (int i = 0; i < r.ChildList.Count; i++)
                    {
                        cnt += GetAllExpandChildCount(r.ChildList[i]);
                    }
                }

                r.IsExpanded = false;
                return cnt;
            }
            else
            {
                return 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ShowData(this.personList);
        }

        public void ShowData(List<Person> lst)
        {
            int recordCount = lst.Count;
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.RowCount = recordCount;
        }
    }
}
