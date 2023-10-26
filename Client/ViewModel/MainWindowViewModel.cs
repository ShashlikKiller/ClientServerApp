using Client.Core.Entities;
using Client.Core.Methods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Client.Core.Methods.ClientController;
using System.Text.Json;
using System.Timers;

namespace Client.ViewModel.WindowViewModel
{
    internal class MainWindowViewModel : BasicViewModel
    {
        private ObservableCollection<Student> students;
        private static ObservableCollection<Group> groups;
        private static ObservableCollection<LearningStatus> learningStatuses;

        public ObservableCollection<Student> Students { get => students; set => students = value; }
        public static ObservableCollection<Group> Groups { get => groups; set => groups = value; }
        public static ObservableCollection<LearningStatus> Statuses { get => learningStatuses; set => learningStatuses = value; }

        public Student SelectedStudent { get; set; }


        static IPAddress clientIP = IPAddress.Parse("127.0.0.1");
        const int clientPort = 8082;
        EndPoint udpEndPoint = new IPEndPoint(clientIP, clientPort); // client's endpoint

        static IPAddress serverIP = IPAddress.Parse("127.0.0.1");
        const int serverPort = 8081;
        EndPoint serverEndPoint = new IPEndPoint(serverIP, serverPort); // server's endpoint

        Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        private Command deleteStudent;
        public Command DeleteStudentCommand
        {
            get
            {
                return deleteStudent ?? (deleteStudent = new Command(obj =>
                {
                    DeleteStudent(udpSocket, serverEndPoint);
                }));
            }
        }
        private Command addStudent;
        public Command AddStudentCommand
        {
            get
            {
                return addStudent ?? (addStudent = new Command(obj =>
                {
                    AddStudent(udpSocket, serverEndPoint);
                }));
            }
        }

        public async void DeleteStudent(Socket udpSocket, EndPoint serverEndPoint)
        {
            if (!(SelectedStudent is null))
            {
                if (!(String.IsNullOrEmpty(SelectedStudent.name) || String.IsNullOrEmpty(SelectedStudent.surname) || SelectedStudent.Group is null || SelectedStudent.LearningStatus is null))
                {
                    SendData(udpSocket, serverEndPoint, "delete");
                    await SendDataAsync(udpSocket, serverEndPoint, SelectedStudent.id.ToString());
                    Thread.Sleep(2000);
                    Students.Remove(SelectedStudent);
                }
            }
        }
        public async void AddStudent(Socket udpSocket, EndPoint serverEndPoint)
        {
            if (!(SelectedStudent is null))
            {
                if (!(String.IsNullOrEmpty(SelectedStudent.name) || String.IsNullOrEmpty(SelectedStudent.surname) || SelectedStudent.group_id == 0 || SelectedStudent.learningstatus_id == 0))
                {
                    SendData(udpSocket, serverEndPoint, "add");
                    string _message = JsonSerializer.Serialize(SelectedStudent);
                    await SendDataAsync(udpSocket, serverEndPoint, _message);
                    Thread.Sleep(2000);
                    Students = GetList<Student>("students", udpSocket, serverEndPoint);
                }
            }
        }


        public MainWindowViewModel()
        {
            udpSocket.Bind(udpEndPoint);
            Thread.Sleep(5000);
            Groups = GetList<Group>("groups", udpSocket, serverEndPoint);
            Students = GetList<Student>("students", udpSocket, serverEndPoint);
            Statuses = GetList<LearningStatus>("learningstatuses", udpSocket, serverEndPoint);
            welcome = new View.Pages.WelcomePage();
            status = new View.Pages.StatusPage();
            StatusPage = status;
            CurrentPage = welcome;
            FrameOpacity = 1;
        }

        #region Work with pages

        private Page currentPage; // Текущая страница (реализует смену страниц)
        private Page status; // Страница статуса
        private Page welcome; // Приветственная страница

        public Page CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public Page StatusPage
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("StatusPage");
            }
        }

        public Page WelcomePage
        {
            get
            {
                return welcome;
            }
            set
            {
                welcome = value;
            }
        }

        private Command openPage;
        public Command OpenPage // Обобщение метода открытия страниц
        {
            get
            {
                return openPage ?? (openPage = new Command(obj =>
                {
                    Page selectedpage = obj as Page;
                    SlowOpacity(selectedpage);
                }
                ));
            }
        }

        private double frameOpacity;
        public double FrameOpacity // Переменная, отвечающая за прозрачность страницы
        {
            get
            {
                return frameOpacity;
            }
            set
            {
                frameOpacity = value;
                OnPropertyChanged("FrameOpacity");
            }
        }

        /// <summary>
        /// Метод медленного изменения прозрачности страницы при переходе на новую
        /// </summary>
        /// <param name="page">Новая страница, на которую переходит пользователь</param>
        private protected async void SlowOpacity(Page page)
        {
            await Task.Factory.StartNew(() =>
            {
                for (double i = 1.0; i > 0.0; i -= 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(10);
                }
                CurrentPage = page;
                for (double i = 0.0; i < 1.1; i += 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(10);
                }
            });
        }
        #endregion
    }
}
