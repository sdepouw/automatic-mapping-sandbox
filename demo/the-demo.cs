using Riok.Mapperly.Abstractions;
using Shouldly;

namespace demo;

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

// Riok.Mapperly NuGet package
// "Warnings as Errors" gives the best, safest experience!

// Basic mapper, built as extension methods
// Default strategy for required mappings is 'Both'
// Can create as few or as many Mapper classes as one sees fit
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class BasicMapper
{
  public static partial UserDTO ToDTOBasic(this User user);
  public static partial AddressDTO MakeAnAddressDTOBasic(this Address address);
}

// Compile-time safety!
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class MapperWithError
{
  /*
    RMG013: demo.UserDTOWithExtraData has no accessible constructor with mappable arguments
      - Appears when there are other warnings/errors
    RMG012: The member ExtraData on the mapping target type demo.UserDTOWithExtraData was not found on the mapping source type demo.User
      - This helpful extra will appear when "Warnings as Errors" is enabled!
  */
  //public static partial UserDTOWithExtraData ToDTO(this User user);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class MappingChildrenObjectsImplicitly
{
  public static partial UserDTO ToDTOImplicitChildren(this User user);
  // Don't have to specify child object mappings
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class MappingExtraData
{
  // "string ExtraData" needs a value! We'll get a warning if we don't provide it.
  // public static partial UserDTOWithExtraData ToDTO(this User user);

  // One can simply pass the missing properties as extra params and Mapperly figures it out!
  public static partial UserDTOWithExtraData ToDTOWithExtraData(this User user, string extraData);

}

// Can use as a dependency, injecting ICustomMapper, instead of extension methods
// Avoided since it makes unit testing harder, even though that might not be obvious!
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MapperThatCanBeInjected : ICustomMapper
{
  public partial UserDTO ToDTO(User user);
}

public interface ICustomMapper
{
  public UserDTO ToDTO(User user);
}

public class TestingMappers
{
  [Fact]
  public void ToDTOWithExtraData_MapsExtraData()
  {
    const string expectedExtraData = "some data";
    User user = new();

    UserDTOWithExtraData dtoWithExtraData = user.ToDTOWithExtraData(expectedExtraData);

    // Using "Shouldly". Between this and "Mapperly", I'm using all the -ly NuGets
    dtoWithExtraData.ExtraData.ShouldBe(expectedExtraData);
  }
}
