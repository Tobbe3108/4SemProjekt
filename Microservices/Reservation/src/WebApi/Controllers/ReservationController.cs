using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservation.Application.Reservation.Commands.CreateReservation;
using Reservation.Application.Reservation.Commands.DeleteReservation;
using Reservation.Application.Reservation.Commands.UpdateReservation;
using Reservation.Application.Reservation.Queries.GetReservation;
using ToolBox.Contracts;
using ToolBox.Contracts.Reservation;
using SubmitReservationCommandValidator = Reservation.Application.Reservation.Commands.CreateReservation.SubmitReservationCommandValidator;

namespace Reservation.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IRequestClient<GetReservation> _getReservationRequestClient;
        private readonly IRequestClient<GetReservationFromUser> _getReservationFromUserRequestClient;
        private readonly IRequestClient<SubmitReservation> _submitReservationRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public ReservationController(IRequestClient<GetReservation> getReservationRequestClient, IRequestClient<GetReservationFromUser> getReservationFromUserRequestClient, IRequestClient<SubmitReservation> submitReservationRequestClient, ISendEndpointProvider sendEndpointProvider)
        {
            _getReservationRequestClient = getReservationRequestClient;
            _getReservationFromUserRequestClient = getReservationFromUserRequestClient;
            _submitReservationRequestClient = submitReservationRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(SubmitReservationCommand command)
        {
            var validator = new SubmitReservationCommandValidator();
            var result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);
            
            var response = await _submitReservationRequestClient.GetResponse<SubmitReservationAccepted>(new
            {
                Id = Guid.NewGuid(),
                command.UserId,
                command.ResourceId,
                command.From,
                command.To
,
            });
            
            return Ok(response.Message.Id);
        }
        
        [HttpGet("UserId/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            var (reservationListVm, notFound) = await _getReservationFromUserRequestClient.GetResponse<ReservationListVm, NotFound>(new
            {
                UserId = id
            });
            return reservationListVm.IsCompletedSuccessfully ? Ok(reservationListVm.Result.Message.Reservations) : Problem(notFound.Result.Message.Message);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var (reservationVm, notFound) = await _getReservationRequestClient.GetResponse<ReservationVm, NotFound>(new
            {
                Id = id
            });
            return reservationVm.IsCompletedSuccessfully ? Ok(reservationVm.Result.Message.Reservation) : Problem(notFound.Result.Message.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SubmitUpdateReservationCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            var validator = new SubmitUpdateReservationCommandValidator();
            var result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);
        
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-update-reservation"));
            await endpoint.Send<SubmitUpdateReservation>(new
            {
                command.Id,
                command.From,
                command.To
            });
        
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new SubmitDeleteReservationCommand{ Id = id };
            var validator = new SubmitDeleteReservationCommandValidator();
            var result = await validator.ValidateAsync(request);
            if (!result.IsValid) return BadRequest(result.Errors);
        
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-delete-reservation"));
            await endpoint.Send<SubmitDeleteReservation>(new
            {
                Id = request.Id
            });
        
            return NoContent();
        }
    }
}