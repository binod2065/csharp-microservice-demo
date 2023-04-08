using System.Threading.Tasks;

public interface ISLConnector
{
    Task<OrderInfo> UpdateItems(Items item);
  
    void Connect(SLLogin login);
}