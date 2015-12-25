using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ToDoInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IToDoService" in both code and config file together.
    [ServiceContract]
    public interface IToDoService
    {
        [OperationContract]
        List<ToDo> GetToDoList();

        [OperationContract]
        List<ToDo> GetDoDoListByName(string name);

        [OperationContract]
        void AddToDo(ToDo toDo);

        [OperationContract]
        void UpdateToDo(ToDo toDo);

        [OperationContract]
        void DeleteToDo(int id);

        [OperationContract]
        ToDo GetToDoById(int id);
    }
}
