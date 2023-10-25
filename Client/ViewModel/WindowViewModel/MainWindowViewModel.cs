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
using static Client.Core.Methods.ClientConnection;

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


        const string ip = "127.0.0.1"; // this is client's ip and port
        const int port = 8082;
        EndPoint udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // client's endpoint
        EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081); // server's endpoint
        Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


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
            while (true)
            {

            }
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
