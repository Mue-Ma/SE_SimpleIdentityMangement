
using BlazorBootstrap;
using EventService.Client.Services.Contracts;
using System.Net;

namespace EventService.Client.Handlers
{
    public class HttpStatusCodeHandler(IMessageService messageService) : DelegatingHandler
    {
        private readonly IMessageService _messageService = messageService;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

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

            return response;
        }
    }
}
