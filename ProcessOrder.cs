using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using FluentEmail.Core;
using B1SLayer;

public class ProcessOrder : IInvocable
{
    private readonly ILogger<ProcessOrder> logger;
    private readonly IOrderConnector orderConnector;
    private readonly IFluentEmail email;

    private readonly ISLConnector sl;



    public ProcessOrder(ILogger<ProcessOrder> logger, IOrderConnector orderConnector, IFluentEmail email, ISLConnector sl)
    {
        this.logger = logger;
        this.orderConnector = orderConnector;
        this.email = email;
        this.sl = sl;
    }

    public async Task Invoke()
    {
        var nextOrder = await orderConnector.GetNextOrder();

        if (nextOrder != null)
        {
            logger.LogInformation("Processing order {@nextOrder}", nextOrder);
            var item = new Items {ItemCode = "9000362", ItemName = "New Items Update from SL"};
            await sl.UpdateItems(item);
            var emailTemplate = 
                @"<p>Dear @Model.CustomerName,</p> 
                <p>Your order of @Model.QuantityOrdered @Model.ItemName has been received!</p>
                <p>Sincerely,<br>Roberts Dev Talk</p>";
            
            var newEmail = email
                .To(nextOrder.CustomerEmail)
                .Subject($"Thanks for your order {nextOrder.CustomerName}")
                .UsingTemplate<OrderInfo>(emailTemplate, nextOrder);
                
            await newEmail.SendAsync();
            await orderConnector.RemoveOrder(nextOrder);

            logger.LogInformation($"Order {nextOrder.OrderId} processed and email sent");
        }
    }
}