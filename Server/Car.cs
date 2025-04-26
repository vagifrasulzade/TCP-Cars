
public enum Color
{
    Black = 1,
    White,
    Blue,
    Red,
    Grey,
    Yellow



}
public enum Currency
{
    USD = 1,
    EUR ,
    AZN ,
    GBP ,
    TRY 
}

public class Car
{
    public int Id { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public Currency Currency { get; set; }
    public Color Colors { get; set; }
    public decimal Price { get; set; }



}
