using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace To_Do_List
{
    
    public class ToDo
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string head { get; set; }
        public bool done { get; set; }
        public int priority { get; set; }
    }

}
