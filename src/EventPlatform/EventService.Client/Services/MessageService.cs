using BlazorBootstrap;

namespace EventService.Client.Services
{
    public class MessageService(ToastService toastService)
    {
        private readonly ToastService _toastService = toastService;

        public void ShowMessage(ToastType toastType, string text) => _toastService.Notify(CreateToastMessage(toastType, text));

        private static ToastMessage CreateToastMessage(ToastType toastType, string text)
        => new()
        {
            Type = toastType,
            Message = text,
            AutoHide = true
        };

    }
}
