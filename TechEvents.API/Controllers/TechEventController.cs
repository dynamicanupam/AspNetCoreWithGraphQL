using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TechEvents.API.Domain;
using TechEvents.API.Infrastructure.DBContext;
using TechEvents.API.Infrastructure.Repositories;

namespace TechEvents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TechEventController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITechEventRepository _repository;
        private readonly IMemoryCache _cache;
        public TechEventController(ApplicationDbContext context, ITechEventRepository repository, IMemoryCache cache)
        {
            _context = context;
            _repository = repository;
            _cache = cache;
            LoadIntialData();
        }

        /// <summary>
        /// Retrieve all tech events.
        /// </summary>
        /// <returns>returns tech event details.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(TechEventsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetTechEventsAsync()
        {
            var userId = await GetSubjectIDFromTokenAsync();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "User is not authorized",
                    Status = (int)HttpStatusCode.Unauthorized
                });
            }

            if (!_cache.TryGetValue("GetTechEvents", out TechEventInfo[] events))
            {
                events = await _repository.GetTechEvents();
                _cache.Set("GetTechEvents", events, TimeSpan.FromSeconds(45));
            }

            return Ok(
                new TechEventsResponse
                {
                    TotalNoOfEvents = events.Count(),
                    EventInfo = events
                });
        }

        /// <summary>
        /// Retrieve a tech event by Id.
        /// </summary>
        /// <returns>returns tech event details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TechEventResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetTechEventAsync(int id)
        {
            var identifiedEvent = await _repository.GetTechEventInfoById(id);
            if (identifiedEvent == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Event not exists",
                    Status = (int)HttpStatusCode.NotFound
                });

            return Ok(identifiedEvent);
        }

        /// <summary>
        /// Create new tech event.
        /// </summary>
        /// <returns>returns tech event details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TechEventInfo), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ExceptionProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddTechEventAsync(NewTechEventRequest techEvent)
        {
            if (techEvent == null) return BadRequest();
            var result = await _repository.AddTechEvent(techEvent);

            return new CreatedResult("Success", result);
        }


        /// <summary>
        /// Update a tech event.
        /// </summary>
        /// <returns>returns tech event details.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(TechEventInfo), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateEventbyIdAsync([FromBody]TechEventInfo techEventToUpdate)
        {

            if (techEventToUpdate == null)
                return BadRequest();

            var identifiedEvent = await _repository.GetTechEventById(techEventToUpdate.EventId);
            if (identifiedEvent == null)
                return NotFound();

            identifiedEvent.EventName = techEventToUpdate.EventName;
            identifiedEvent.Speaker = techEventToUpdate.Speaker;
            identifiedEvent.EventDate = techEventToUpdate.EventDate;
            var result = await _repository.UpdateTechEvent(identifiedEvent);

            return Created("Success", result);
        }


        /// <summary>
        /// Delete a tech event by id.
        /// </summary>
        /// <returns>returns tech event details.</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeleteTechEventByIDAsync([FromHeader]int id)
        {
            var identifiedEvent = await _repository.GetTechEventById(id);
            if (identifiedEvent == null)
                return NotFound();

            var result = await _repository.DeleteTechEventById(identifiedEvent);

            return Ok(result);
        }
        protected async Task<string> GetSubjectIDFromTokenAsync()
        {
            string userPid = string.Empty;
            var handler = new JwtSecurityTokenHandler();

            if (handler.ReadToken(await this.HttpContext.GetTokenAsync("access_token")) is JwtSecurityToken jsonToken &&
                (jsonToken.Claims.FirstOrDefault(claim => claim.Type == "sub") != null))
            {
                userPid = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "sub").Value;
            }

            return userPid;
        }

        private void LoadIntialData()
        {
            if (!_context.TechEventInfo.Any())
            {
                var e1 = _context.TechEventInfo.Add(
                    new TechEventInfo
                    {
                        EventName = "ASP .NET Core Web API",
                        Speaker = "Anupam",
                        EventDate = DateTime.Today,   
                    }
                    );

                var e2 = _context.TechEventInfo.Add(
                   new TechEventInfo
                   {
                       EventName = "Angular",
                       Speaker = "Pooja",
                       EventDate = DateTime.Today,
                   }
                   );
                var e3 = _context.TechEventInfo.Add(
                  new TechEventInfo
                  {
                      EventName = "GraphQL",
                      Speaker = "Anupam",
                      EventDate = DateTime.Now.AddDays(7),
                  }
                  );
                var p1 = _context.Participant.Add(
                    new Participant
                    {
                       ParticipantName ="Balaji G",
                       Email= "test@test.com",
                       Phone= 1234567890,
                    }
                    );

                var p2 = _context.Participant.Add(
                    new Participant
                    {
                        ParticipantName = "John G",
                        Email = "test2@test.com",
                        Phone = 1234567890,
                    }
                    );

                _context.EventParticipants.AddRange(
                    new EventParticipants
                    {
                        EventId = e1.Entity.EventId,
                        ParticipantId = p1.Entity.ParticipantId,
                    },
                     new EventParticipants
                     {
                         EventId = e1.Entity.EventId,
                         ParticipantId = p2.Entity.ParticipantId,
                     },
                      new EventParticipants
                      {
                          EventId = e2.Entity.EventId,
                          ParticipantId = p2.Entity.ParticipantId,
                      }
                    );

                _context.SaveChanges();
            }
        }
    }
}
