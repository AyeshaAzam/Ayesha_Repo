using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ToDoInterfaces;

namespace ToDoClient
{
    class Program
    {
        private IToDoService proxy;

        public Program(ref IToDoService pxy)
        {
            proxy = pxy;
        }
        private void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Get ToDo list");
            Console.WriteLine("2. Get ToDo list by name");
            Console.WriteLine("3. Create new ToDo list");
            Console.WriteLine("4. Update ToDo status");
            Console.WriteLine("5. Delet an item from ToDo list");
            Console.WriteLine("6. Add an item to ToDo list");
            Console.WriteLine("7. Set ToDo status to finished");
            Console.WriteLine("8. Exit");
        }
        private void PrintToDoList(String name = "")
        {
            List<ToDo> toDos = null;
            if (name.Length == 0)
            {
                toDos = proxy.GetToDoList();
            }
            else
            {
                toDos = proxy.GetDoDoListByName(name);
            }
            Console.WriteLine("ID\tDescription\tName\tCreated Date\t\tDeadline\t\tEstimation Time\tFinished");

            foreach (var p in toDos)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\t{6}",
                    p.Id, p.Description, p.Name, p.CreatedDate, p.DeadLine, p.EstimationTime, p.Finnished);
            }
            Console.WriteLine("\nPress Enter to Continue!!!");
            Console.ReadLine();
        }

        private void GetToDoFromUser()
        {
            Console.Write("Description : ");
            string desc = Console.ReadLine().ToString();
            Console.Write("Name : ");
            string name = Console.ReadLine().ToString();
            Console.Write("Deadline : ");
            DateTime deadline = Convert.ToDateTime(Console.ReadLine().ToString());
            Console.Write("Estimation Time : ");
            int estimation = Convert.ToInt32(Console.ReadLine());
            Console.Write("Finished : ");
            bool finished = Convert.ToInt32(Console.ReadLine()) == 0 ? false : true;

            AddToDo(desc, name, deadline, estimation, finished);
        }

        private void AddToDo(/*int id, */string desc, string name, /*DateTime created,*/ DateTime deadline, int estimation, bool finished)
        {
            ToDo addToDo = new ToDo();
            //addToDo.Id = id;
            addToDo.Description = desc;
            addToDo.Name = name;
            addToDo.CreatedDate = DateTime.Now;
            addToDo.DeadLine = deadline;
            addToDo.EstimationTime = estimation;
            addToDo.Finnished = finished;
            proxy.AddToDo(addToDo);
            Console.WriteLine("Following item added to ToDo list for {0}\n", addToDo.Name);
            Console.WriteLine("ID\tDescription\tName\tCreated Date\t\tDeadline\t\tEstimation Time\tFinished");
            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\t{6}",
                addToDo.Id, addToDo.Description, addToDo.Name, addToDo.CreatedDate, addToDo.DeadLine, addToDo.EstimationTime, addToDo.Finnished);
            Console.ReadLine();
        }

        private void UpdateToDoStatus()
        {
            Console.Write("ID : ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Status : ");
            bool status = Convert.ToInt32(Console.ReadLine()) == 0 ? false : true;
            ToDo toDo = proxy.GetToDoById(id);
            toDo.Finnished = status;
            proxy.UpdateToDo(toDo);
        }

        private void DeletToDoItem()
        {
            Console.Write("ID : ");
            int id = Convert.ToInt32(Console.ReadLine());
            proxy.DeleteToDo(id);
        }

        static void Main(string[] args)
        {
            ChannelFactory<IToDoService> chF = new ChannelFactory<IToDoService>("ToDoServiceEndpoint");
            IToDoService proxy = chF.CreateChannel();

            Program prog = new Program(ref proxy);
            bool keepRunning = true;

            while (keepRunning)
            {
                prog.PrintMenu();
                // capture key press
                char key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                // ensure captured key is a valid option
                int option = (Char.IsDigit(key) ? (int)Char.GetNumericValue(key) : 0);

                switch (option)
                {
                    case 1:
                        prog.PrintToDoList();
                        break;
                    case 2:
                        Console.Write("Enter name : ");
                        prog.PrintToDoList(Console.ReadLine().ToString());
                        break;
                    case 3:
                        prog.GetToDoFromUser();
                        break;
                    case 4:
                        prog.UpdateToDoStatus();
                        break;
                    case 5:
                        prog.DeletToDoItem();
                        break;
                    case 8:
                        keepRunning = false;
                        break;
                    case 0:
                    default:
                        Console.WriteLine("Not a valid option {0} please choose again!", key);
                        break;
                }
            }
        }
    }
}
