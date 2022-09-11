using Microsoft.AspNetCore.Mvc;

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
        Category = new Subcategory(0, category);
        Ean = ean;
        Producer = producer;
    }    
    
    public Product(string id, string name, int rateSum, int rateCount, bool isFollow, bool isRated, int myRate, string image, string ean, string producer, int placeInRanging, Subcategory subcategory, Subcategory category)
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
        PlaceInRanging = placeInRanging;
        Subcategory = subcategory;
    }

    public Product()
    {
        
    }

    public string Id { get; }
    public string Name { get; }
    public int RateSum { get; }
    public int RateCount { get; }
    public bool IsFollow { get; }
    public bool IsRated { get; }
    public int MyRate { get; }  
    public string Image { get; }
    public Subcategory Category { get; }
    public Subcategory Subcategory { get; }
    public string Ean { get; }
    public string Producer { get; }
    public int PlaceInRanging { get; }

}