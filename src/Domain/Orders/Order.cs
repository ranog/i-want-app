namespace IWantApp.Domain.Orders;

public class Order : Entity
{
    public string ClientId { get; private set; }
    public List<Product> Products { get; private set; }
    public decimal Total { get; private set; }
    public string DeliveryAddres { get; private set; }

    private Order()
    {
    }

    public Order(string clientId, string clientName, List<Product> products, string deliveryAddres)
    {
        ClientId = clientId;
        Products = products;
        DeliveryAddres = deliveryAddres;
        CreatedBy = clientName;
        EditedBy = clientName;
        CreatedOn = DateTime.UtcNow;
        EditedOn = DateTime.UtcNow;

        Total = 0;
        foreach (var item in Products)
        {
            Total += item.Price;
        }

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Order>().IsNotNull(ClientId, "Client").IsNotNull(Products, "Products");
        AddNotifications(contract);
    }
}
