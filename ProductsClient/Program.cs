using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ProductInterfaces;

namespace ProductsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ChannelFactory<IWCFPoducttService> chF = new ChannelFactory<IWCFPoducttService>("ProductServiceEndpoint");
                IWCFPoducttService proxy = chF.CreateChannel();

                // just an out put from client side
                Console.WriteLine("Client has start asking request !");
                Console.WriteLine("Enter go to get data from server");
                string input = Console.ReadLine();

                if (input == "go")
                { 
                    // calls the server
                    List<string> toDos = proxy.ListProducts();

                    foreach (var p in toDos)
                    {
                        Console.WriteLine(p);
                    }
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
