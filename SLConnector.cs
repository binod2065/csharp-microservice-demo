
using System;
using System.Threading.Tasks;
using B1SLayer;
using Microsoft.Extensions.Logging;
public class SlConnetor : ISLConnector
{
    private readonly ILogger<OrderQueueConnector> logger;
    private B1SLayer.SLConnection serviceLayer;
    public SlConnetor(ILogger<OrderQueueConnector> logger)
    {
        this.logger = logger;
        serviceLayer = new SLConnection(
                            Environment.GetEnvironmentVariable("SAP_SL_URL"),
                            Environment.GetEnvironmentVariable("SAP_SL_COMPANYDB"),
                            Environment.GetEnvironmentVariable("SAP_SL_USER"),
                            Environment.GetEnvironmentVariable("SAP_SL_PASS"));
        try
        {

            serviceLayer.LoginAsync();
        }
        catch (System.Exception ex)
        {

            logger.LogError(ex, "Error deserializing message");
        }

    }

    public async Task UpdateItems(Items items)
    {
        try
        {
            string itemCode = $"{items.ItemCode}";
            await serviceLayer.Request("Items", itemCode).PatchAsync(items);
            logger.LogInformation(items.ItemCode, "Service layer");
        }
        catch (System.Exception ex)
        {

            logger.LogError(ex, "Error deserializing message");
        }
    }
}