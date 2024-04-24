using Amazon.Runtime.Internal.Transform;
using EventService.Server.Core.Entities;
using EventService.Server.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EventService.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class EventController(IEventRepository eventRepository) : ControllerBase
    {
        private readonly IEventRepository _eventRepository = eventRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> Get()
        {
            await CreateRealm("test");
            return Ok(await _eventRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> Get(Guid id)
        {
            var res = await _eventRepository.GetEntityById(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Post([FromBody] Event ev)
        {
            if (await _eventRepository.GetByName(ev.Name) != null) BadRequest("Eventname existiert bereits!");
            try 
            {
                await _eventRepository.Add(ev);
                await CreateRealm(ev.Name);
            }
            catch (Exception) 
            {
                if((await _eventRepository.GetEntityById(ev.Id)) != null) await _eventRepository.Delete(ev.Id);
                return Problem();
            }

            return Ok(ev.Id);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Event ev)
        {
            await _eventRepository.Update(ev);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _eventRepository.Delete(id);
            return NoContent();
        }

        private static async Task CreateRealm(string name)
        {
            var url = @"http://localhost:8080/admin/realms";

            using var Http = new HttpClient();

            var httpContent = new StringContent(Realm.Replace("_newrealmname_", name), Encoding.UTF8, "application/json");
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await LoginAdmin());

            var response = await Http.PostAsync(url, httpContent);

            var responseObject = await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> LoginAdmin()
        {
            var postData = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", "admin-cli" },
                { "username", "admin" },
                { "password", "admin" }
            };

            var url = @"http://localhost:8080/realms/master/protocol/openid-connect/token";

            using var Http = new HttpClient();
            using var content = new FormUrlEncodedContent(postData);

            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            var response = await Http.PostAsync(url, content);

            var responseObject = await response.Content.ReadAsStringAsync();

            using var jsonDoc = JsonDocument.Parse(responseObject);
            return jsonDoc.RootElement.GetProperty("access_token").ToString();
        }

        public static string Realm => System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/realm-export-template.json");
    }
}
