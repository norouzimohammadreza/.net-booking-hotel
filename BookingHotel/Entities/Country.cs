namespace BookingHotel.Entities;

public class Country
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string ShortName { get; private set; }

    public List<Hotel> Hotels { get; private set; } = [];
    
    private Country(){}
    
    public Country(string name, string shortName)
    {
        Name = name;
        ShortName = shortName;
    }
    
    public void Update(string name, string shortName)
    {
        Name = name;
        ShortName = shortName;
    }
}