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
                case HttpStatusCode.OK:
                    _messageService.ShowMessage(ToastType.Success, "Ok");
                    break;

                case HttpStatusCode.Created:
                    _messageService.ShowMessage(ToastType.Success, "Event created");
                    break;

                case HttpStatusCode.BadRequest:
                    _messageService.ShowMessage(ToastType.Danger, "Eventname already existing!");
                    break;

                case HttpStatusCode.NotFound:
                    _messageService.ShowMessage(ToastType.Warning, "Event not found");
                    break;

                case HttpStatusCode.Unauthorized:
                    _messageService.ShowMessage(ToastType.Info, "Unauthorized access");
                    break;

                case HttpStatusCode.InternalServerError:
                    _messageService.ShowMessage(ToastType.Info, "Internal server error");
                    break;

                case HttpStatusCode.NoContent:
                    _messageService.ShowMessage(ToastType.Success, "Event deleted");
                    break;

                default:
                    break;
            }
        }
        public void DisposeEvent() => _interceptor.AfterSend -= InterceptResponse!;
    }
}
