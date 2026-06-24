using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevWiki.Client.Services
{
    public class ToastMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "success"; // success, error, info
    }

    public interface IToastService
    {
        event Action? OnChanged;
        List<ToastMessage> Messages { get; }
        void ShowSuccess(string message, string title = "Успешно");
        void ShowError(string message, string title = "Ошибка");
        void Remove(Guid id);
    }

    public class ToastService : IToastService
    {
        public event Action? OnChanged;
        public List<ToastMessage> Messages { get; private set; } = new();

        public void ShowSuccess(string message, string title = "Успешно")
        {
            AddToast(new ToastMessage { Message = message, Title = title, Type = "success" });
        }

        public void ShowError(string message, string title = "Ошибка")
        {
            AddToast(new ToastMessage { Message = message, Title = title, Type = "error" });
        }

        private async void AddToast(ToastMessage toast)
        {
            Messages.Add(toast);
            OnChanged?.Invoke();

            await Task.Delay(5000); // авто-удаление через 5 сек
            Remove(toast.Id);
        }

        public void Remove(Guid id)
        {
            var toast = Messages.Find(x => x.Id == id);
            if (toast != null)
            {
                Messages.Remove(toast);
                OnChanged?.Invoke();
            }
        }
    }
}
