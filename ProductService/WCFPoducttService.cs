using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.ServiceModel;
using System.Text;
using TodoDatabase;

namespace ProductInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WCFPoducttService" in both code and config file together.
    public class WCFPoducttService : IWCFPoducttService
    {
        // server name: AYESHA-VAIO(SQL Server 12.0.4213.0-AYESHA-VAIO\AYESHA)
        static string strConnString = @"Data Source = AYESHA-VAIO; Initial Catalog = DB_ToDoList; User ID = RestFullUser; Password = RestFull123";

        DAL dal = new DAL(strConnString);
        

        //ToDo toDo;
        public List<string> ListProducts()
        {
            Console.WriteLine("ListProducts has been called by a client!");
            List<string> productList = new List<string>();
            try
            {

                List<ToDo> toDo = dal.GetToDoListByName("Hamid");
                Console.WriteLine(dal.GetErrorMessage());
                foreach(var p in toDo)
                {
                   // Console.WriteLine("ID: {0}\t Description: {1}\t Name {2}", p.Id, p.Description, p.Name);
                    Console.WriteLine("ID:{0}\n Description: {1}\n Name: {2}\n CreatedDate: {3}\n", p.Id, p.Description, p.Name,p.CreatedDate);
                    Console.WriteLine("DeadLine: {0}\n Estimation time: {1}\n ", p.DeadLine, p.EstimationTime);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return productList;
        }
    }
}
