using Shouldly;

namespace demo.Mapperly;

public class Tests
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
