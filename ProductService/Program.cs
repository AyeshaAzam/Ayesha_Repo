using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductInterfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (ServiceHost host = new ServiceHost(typeof(WCFPoducttService)))
            {
                    host.Open();
                    Console.WriteLine("Server is open");
                    Console.WriteLine("Press enter to close connection");
                    Console.ReadLine();
            }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                Console.ReadLine();
                }
        }
    }
}
