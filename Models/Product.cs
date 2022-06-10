using Microsoft.Extensions.Primitives;

namespace rateit.Models;

public class Product
{
    public Product(string id, string name, int rateSum, int rateCount, bool isFollow, bool isRated, int myRate)
    {
        Id = id;
        Name = name;
        RateSum = rateSum;
        RateCount = rateCount;
        IsFollow = isFollow;
        IsRated = isRated;
        MyRate = myRate;
    }

    public string Id { get; }
    public string Name { get; }
    public int RateSum { get; }
    public int RateCount { get; }
    public bool IsFollow { get; }
    public bool IsRated { get; }
    public int MyRate { get; }
}