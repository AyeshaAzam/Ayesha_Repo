using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ToDoInterfaces;

namespace ToDoService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ToDoService" in both code and config file together.
    public class ToDoService : IToDoService
    {
        static string strConnString = @"Data Source = WAQAR-PC; Initial Catalog = DB_ToDoList; User ID = RestFullUser; Password = RestFull123";
        DAL dal = new DAL(strConnString);

        public void AddToDo(ToDo toDo)
        {
            try
            {
                dal.AddToDo(toDo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DeleteToDo(int id)
        {
            dal.DeleteToDo(id);
        }

        public List<ToDo> GetDoDoListByName(string name)
        {
            Console.WriteLine("GetDoDoListByName has been called by a client!");
            List<ToDo> toDo = null;
            try
            {
                toDo = dal.GetToDoListByName(name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(dal.GetErrorMessage());
            }
            return toDo;
        }

        public ToDo GetToDoById(int id)
        {
            return dal.GetToDoById(id);
        }

        public List<ToDo> GetToDoList()
        {
            return dal.GetToDoList();
        }

        public void UpdateToDo(ToDo toDo)
        {
            dal.UpdateToDo(toDo);
        }
    }
}
