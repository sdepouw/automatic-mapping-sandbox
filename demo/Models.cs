namespace demo;

// Example Models/Entities/DTOs

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public Address HomeAddress { get; set; } = Address.Empty;
}

public record Address(int Id, string Street, string City, string State, string Zip)
{
    public static readonly Address Empty = new(0, "", "", "", "");
};

public record UserDTO(string Name, AddressDTO HomeAddress);

public record AddressDTO(string Street, string City, string State, string Zip);

public record UserDTOWithExtraData(string Name, AddressDTO HomeAddress, string ExtraData);
