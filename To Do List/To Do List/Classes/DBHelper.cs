using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace To_Do_List.Classes
{
    class DBHelper
    {
        SQLiteConnection dbConn;
        public async Task<bool> onCreate(string DB_PATH)
        {
            try
            {
                bool x = await CheckFileExists(DB_PATH);
                if (!x)
                {
                    using (dbConn = new SQLiteConnection(DB_PATH))
                    {
                        dbConn.CreateTable<ToDo>();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ObservableCollection<ToDo> ReadItems()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<ToDo> myCollection = dbConn.Table<ToDo>().ToList<ToDo>();
                ObservableCollection<ToDo> ContactsList = new ObservableCollection<ToDo>(myCollection);
                return ContactsList;
            }
        }

        public void Insert(ToDo newItem)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newItem);
                });
            }
        }

        public void Delete(int Id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingitem = dbConn.Query<ToDo>("select * from ToDo where Id =" + Id).FirstOrDefault();
                if (existingitem != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingitem);
                    });
                }
            }
        }

        public ToDo getItem(string head)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                ToDo existngitem = dbConn.Query<ToDo>("select * from ToDo where Head =" + head).FirstOrDefault();
                return existngitem;
            }
        }

        public void Update(ToDo todo)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingitem = dbConn.Query<ToDo>("select * from ToDo where Id =" + todo.Id).FirstOrDefault();
                if (existingitem != null)
                {
                    existingitem.head = todo.head;
                    existingitem.priority = todo.priority;
                    existingitem.done = todo.done;
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existingitem);
                    });
                }
            }
        }





    }
}
