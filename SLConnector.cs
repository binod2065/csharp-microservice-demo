
using System;
using System.Threading.Tasks;
using B1SLayer;

public class SlConnetor : ISLConnector
{

    private  B1SLayer.SLConnection serviceLayer;
    public void Connect(SLLogin login)
    {
    serviceLayer =  new SLConnection(login.url, login.CompanyDB, login.UserName, login.Password); 
    serviceLayer.LoginAsync();
                        
    }

    public async Task UpdateItems(Items items)
    {
       await serviceLayer.Request("Items", items.ItemCode).PatchAsync(items);
    }
}