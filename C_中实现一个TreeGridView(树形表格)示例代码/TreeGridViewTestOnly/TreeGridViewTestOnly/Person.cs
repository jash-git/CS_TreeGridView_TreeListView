using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeGridViewTestOnly
{
    public class Person
    {
        public int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private int level;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        private bool isExpanded;

        public bool IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; }
        }

        private List<Person> childList;

        public List<Person> ChildList
        {
            get { return childList; }
            set { childList = value; }
        }
    }
}
