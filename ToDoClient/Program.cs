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
            Console.WriteLine("/***********************************/");
            Console.WriteLine("/*         TODO Main Menu          */");
            Console.WriteLine("/***********************************/");
            Console.WriteLine("1. Get ToDo list");
            Console.WriteLine("2. Get ToDo list by name");
            Console.WriteLine("3. Create new ToDo list");
            Console.WriteLine("4. Update ToDo status");
            Console.WriteLine("5. Delet an item from ToDo list");
            Console.WriteLine("6. Add an item to ToDo list");
            Console.WriteLine("7. Set ToDo status to finished");
            Console.WriteLine("8. Exit");

            Console.Write("\nSelect option: ");
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
            List<ToDo> toDoList = null;
            if (name.Length == 0)
            {
                toDoList = proxy.GetToDoList();
            }
            else
            {
                toDoList = proxy.GetDoDoListByName(name);
            }

            if(toDoList.Count > 0)
            {
                Console.WriteLine("ID\tDescription\tName\tCreated Date\t\tDeadline\t\tEstimation Time\tFinished");
                foreach (var toDo in toDoList)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\t{6}",
                        toDo.Id, toDo.Description, toDo.Name, toDo.CreatedDate, toDo.DeadLine, toDo.EstimationTime, toDo.Finnished);
                }
            }

            if(toDoList.Count == 0)
            {
                if(name.Length > 0)
                {
                    Console.WriteLine("No ToDo with name \"{0}\" exists!", name);
                }
                else
                {
                    // ToDo list empty
                    Console.WriteLine("ID\tDescription\tName\tCreated Date\t\tDeadline\t\tEstimation Time\tFinished");
                }
            }

            Console.WriteLine("\nPress any key to Continue!!!");
            Console.ReadLine();
        }

        private void GetToDoFromUser()
        {
            string desc;
            bool error = false;
            do
            {
                if(error) //initially no error, is used for retries
                {
                    Console.WriteLine("Description must be at least 6 characters long, please try again!");
                }
                Console.Write("Description : ");
                desc = Console.ReadLine();
                error = (desc.Length < 6) ? true : false; 
            } while (error);

            Console.Write("Name : ");
            string name = Console.ReadLine();

            Console.Write("Deadline [in 1hr] : ");
            string deadlineInput = Console.ReadLine();
            DateTime deadline;
            if(!DateTime.TryParse(deadlineInput, out deadline))
            {
                Console.WriteLine("Setting default deadline to current time plus 1hr!");
                deadline = DateTime.Now.AddHours(1);
            }

            Console.Write("Estimation Time [1hr] : ");
            string estimationInput = Console.ReadLine();
            int estimation;
            if(!int.TryParse(estimationInput, out estimation))
            {
                Console.WriteLine("Setting default estimation time to 1 hr");
                estimation = 1;
            }

            Console.Write("Finished [0]: ");
            string finishedInput = Console.ReadLine();
            bool finished = finishedInput == "0" ? false : (finishedInput == "1" ? true : false);

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

        private void UpdateToDoFinishStatus()
        {
            Console.Write("ID : ");
            string idInput = Console.ReadLine();
            int id = 0;
            if(int.TryParse(idInput, out id))
            {
                ToDo toDo = proxy.GetToDoById(id);
                if (toDo != null)
                {
                    Console.Write("Finished [0] : ");
                    string finishedInput = Console.ReadLine();
                    bool finished = finishedInput == "0" ? false : (finishedInput == "1" ? true : false);
                    toDo.Finnished = finished;
                    proxy.UpdateToDo(toDo);
                    Console.WriteLine("Status for ToDo with ID:{0} updated to {1}", id, finished ? "Finished" : "Not finished");
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
            IToDoService srvProxy = chF.CreateChannel();

            Program prog = new Program(ref srvProxy);
            bool keepRunning = true;

            while (keepRunning)
            {
                prog.PrintMenu();
                string optionInput = Console.ReadLine();
                int option;
                if (!int.TryParse(optionInput, out option))
                {
                    Console.WriteLine("Not a valid option {0} please select again!", optionInput);
                    Console.ReadLine();
                    continue;
                }

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
                        prog.UpdateToDoFinishStatus();
                        break;
                    case 5:
                        prog.DeletToDoItem();
                        break;
                    case 8:
                        keepRunning = false;
                        break;
                    case 0:
                    default:
                        Console.WriteLine("Not a valid option {0} please select again!", optionInput);
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
