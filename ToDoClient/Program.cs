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
        private void PrintToDo(ToDo toDo)
        {
            Console.WriteLine("\nID\tDescription\tName\tCreated Date\t\tDeadline\t\tEstimation Time\tFinished");
            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\t{6}",
                toDo.Id, toDo.Description, toDo.Name, toDo.CreatedDate, toDo.DeadLine, toDo.EstimationTime, toDo.Finnished);
            Console.ReadLine();
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
            Console.WriteLine("\nPress any key to Continue!!!");
            Console.ReadLine();
        }

        private void GetToDoFromUser()
        {
            Console.Write("Description : ");
            string desc = Console.ReadLine().ToString();

            Console.Write("Name : ");
            string name = Console.ReadLine().ToString();

            Console.Write("Deadline : ");
            string deadlineInput = Console.ReadLine();
            DateTime deadline;
            if(!DateTime.TryParse(deadlineInput, out deadline))
            {
                Console.WriteLine("Not a valid \"Deadline\" input, setting default deadline to current time!");
                deadline = DateTime.Now;
            }

            Console.Write("Estimation Time : ");
            string estimationInput = Console.ReadLine();
            int estimation;
            if(!int.TryParse(estimationInput, out estimation))
            {
                Console.WriteLine("Not a valid \"Estimation Time\", setting default estimation time to 1 hr");
                estimation = 1;
            }

            Console.Write("Finished : ");
            string finishedInput = Console.ReadLine();
            bool finished;
            if(!bool.TryParse(finishedInput, out finished))
            {
                Console.WriteLine("Not a valid \"Finished\" status, setting default finished satus to false");
                finished = false;
            }

            AddToDo(desc, name, deadline, estimation, finished);
        }

        private void AddToDo(string desc, string name, DateTime deadline, int estimation, bool finished)
        {
            ToDo toDo = new ToDo();
            toDo.Description = desc;
            toDo.Name = name;
            toDo.CreatedDate = DateTime.Now;
            toDo.DeadLine = deadline;
            toDo.EstimationTime = estimation;
            toDo.Finnished = finished;

            proxy.AddToDo(toDo);

            Console.WriteLine("Following item added to ToDo list for {0}", toDo.Name);
            PrintToDo(toDo);
        }

        private void UpdateToDoStatus()
        {
            Console.Write("ID : ");
            string idInput = Console.ReadLine();
            int id = 0;
            if(int.TryParse(idInput, out id))
            {
                ToDo toDo = proxy.GetToDoById(id);
                if (toDo != null)
                {
                    Console.Write("Status : ");
                    bool status = Convert.ToInt32(Console.ReadLine()) == 0 ? false : true;
                    toDo.Finnished = status;
                    proxy.UpdateToDo(toDo);
                    Console.WriteLine("Status for ToDo with ID:{0} updated to {1}", id, status ? "Finished" : "Not finished");
                    PrintToDo(toDo);
                }
                else
                {
                    Console.WriteLine("ToDo with ID:{0} not found!", idInput);
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Not a valid ID:{0}!", idInput);
                Console.ReadLine();
            }
        }

        private void DeletToDoItem()
        {
            Console.Write("ID : ");
            string idInput = Console.ReadLine();
            int id;
            if (!int.TryParse(idInput, out id))
            {
                Console.WriteLine("Not a valid ID:{0}!", idInput);
                Console.ReadLine();
                return;
            }
            ToDo toDo = proxy.GetToDoById(id);
            if(toDo == null)
            {
                Console.WriteLine("ToDo with ID:{0} not found!", id);
                Console.ReadLine();
                return;
            }
            proxy.DeleteToDo(id);
            Console.WriteLine("ToDo with ID:{0} deleted!", id);
            PrintToDo(toDo);
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
