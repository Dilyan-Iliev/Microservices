using BuildingBlocks.Pagination;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Application.Orders.Commands.DeleteOrder;
using Ordering.Application.Orders.Commands.UpdateOrder;
using Ordering.Application.Orders.Queries.GetOrderByName;
using Ordering.Application.Orders.Queries.GetOrders;
using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ISender _sender;

        public OrdersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<ActionResult<GetOrdersResponse>> GetOrders([FromQuery] PaginationRequest request)
        {
            var query = new GetOrdersQuery(request);
            var result = await _sender.Send(query);
            var response = result.Adapt<GetOrdersResponse>();

            return Ok(response);
        }

        [HttpGet("byName/{orderName}")]
        public async Task<ActionResult<GetOrdersByNameResponse>> GetOrdersByName(string orderName)
        {
            var result = await _sender.Send(new GetOrdersByNameQuery(orderName));

            var response = result.Adapt<GetOrdersByNameResponse>();

            return Ok(response);
        }

        [HttpGet("byCustomer/{customerId}")]
        public async Task<ActionResult<GetOrdersByCustomerResponse>> GetOrdersByCustomer(Guid customerId)
        {
            var result = await _sender.Send(new GetOrdersByCustomerQuery(customerId));

            var response = result.Adapt<GetOrdersByCustomerResponse>();

            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<DeleteOrderResponse>> DeleteOrder(Guid id)
        {
            var result = await _sender.Send(new DeleteOrderCommand(id));
            var response = result.Adapt<DeleteOrderResponse>();

            return Ok(response);
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<CreateOrderReponse>> CreateOrder(CreateOrderRequest request)
        {
            var command = request.Adapt<CreateOrderCommand>();
            var result = await _sender.Send(command);

            var response = result.Adapt<CreateOrderReponse>();
            return Ok(response);
        }

        [HttpPut("UpdateOrder")]
        public async Task<ActionResult<UpdateOrderResponse>> UpdateOrder(UpdateOrderRequest request)
        {
            var command = request.Adapt<UpdateOrderCommand>();
            var result = await _sender.Send(command);
            var response = result.Adapt<UpdateOrderResponse>();

            return Ok(response);
        }
    }
}
