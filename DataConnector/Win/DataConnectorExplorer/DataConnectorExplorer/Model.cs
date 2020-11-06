using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConnectorExplorer
{
    public class SchemaColumn
    {
        public SchemaColumn(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class SchemaTable
    {
        public SchemaTable(string name)
        {
            Name = name;
            Columns = new BindingList<SchemaColumn>();
        }

        public string Name { get; set; }
        public BindingList<SchemaColumn> Columns { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
