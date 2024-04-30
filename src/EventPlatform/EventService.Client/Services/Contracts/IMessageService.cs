using BlazorBootstrap;

namespace EventService.Client.Services.Contracts
{
    public interface IMessageService
    {
        void ShowMessage(ToastType toastType, string text);
    }
}