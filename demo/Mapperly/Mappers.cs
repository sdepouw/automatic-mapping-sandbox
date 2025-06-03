using Riok.Mapperly.Abstractions;

namespace demo.Mapperly;

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

// Manually map members
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class MapperWithManualMapping
{
  [MapProperty(nameof(User.Name), nameof(UserDTOWithExtraData.ExtraData))]
  public static partial UserDTOWithExtraData ToUserDTOWithExtraData(this User user);
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

// goto Tests.cs
