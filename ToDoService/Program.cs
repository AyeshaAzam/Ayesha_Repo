using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ToDoInterfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ToDoService.ToDoService)))
            {
                host.Open();
                Console.WriteLine("Server is open");
                Console.WriteLine("Press enter to close connection");
                Console.ReadLine();
            }
        }
    }
}
