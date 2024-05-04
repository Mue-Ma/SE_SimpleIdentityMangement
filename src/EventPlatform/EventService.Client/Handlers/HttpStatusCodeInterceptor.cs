using BlazorBootstrap;
using EventService.Client.Services.Contracts;
using System.Net;
using Toolbelt.Blazor;

namespace EventService.Client.Handlers
{
    public class HttpStatusCodeInterceptor(IMessageService messageService, HttpClientInterceptor httpClientInterceptor)
    {
        private readonly IMessageService _messageService = messageService;
        private readonly HttpClientInterceptor _interceptor = httpClientInterceptor;

        public void RegisterEvent() => _interceptor.AfterSend += InterceptResponse!;

        private void InterceptResponse(object sender, HttpClientInterceptorEventArgs e)
        {
            var response = e.Response;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    _messageService.ShowMessage(ToastType.Success, $"Successfuly created item!");
                    break;

                case HttpStatusCode.BadRequest:
                    _messageService.ShowMessage(ToastType.Danger, "Something was not executed right!");
                    break;

                case HttpStatusCode.NotFound:
                    _messageService.ShowMessage(ToastType.Warning, "Something was not found!");
                    break;

                case HttpStatusCode.Unauthorized:
                    _messageService.ShowMessage(ToastType.Info, "You need to login to access all ressources!");
                    break;

                case HttpStatusCode.InternalServerError:
                    _messageService.ShowMessage(ToastType.Danger, "Something unexpected happend!");
                    break;

                case HttpStatusCode.Forbidden:
                    _messageService.ShowMessage(ToastType.Info, "You need another role to access all ressources!");
                    break;

                case HttpStatusCode.NoContent:
                    _messageService.ShowMessage(ToastType.Success, "Action was successful!");
                    break;

                default:
                    break;
            }
        }
        public void DisposeEvent() => _interceptor.AfterSend -= InterceptResponse!;
    }
}
