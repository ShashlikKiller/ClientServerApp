using Client.Core.Entities;
using Client.Core.Methods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Client.Core.Methods.ClientConnection;

namespace Client.ViewModel.WindowViewModel
{
    internal class MainWindowViewModel : BasicViewModel
    {
    
        public MainWindowViewModel()
        {
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
