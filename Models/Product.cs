namespace rateit.Models;

[Serializable]
public class Product
{
    public Product(string id, string name, int rateSum, int rateCount, bool isFollow, bool isRated, int myRate, string image, string category, string ean, string producer)
    {
        Id = id;
        Name = name;
        RateSum = rateSum;
        RateCount = rateCount;
        IsFollow = isFollow;
        IsRated = isRated;
        MyRate = myRate;
        Image = image;
        Category = category;
        Ean = ean;
        Producer = producer;
    }

    public string Id { get; }
    public string Name { get; }
    public int RateSum { get; }
    public int RateCount { get; }
    public bool IsFollow { get; }
    public bool IsRated { get; }
    public int MyRate { get; }  
    public string Image { get; }
    public string Category { get; }
    public string Ean { get; }
    public string Producer { get; }
}