using Articles.Models.Feature.Login;
using System.Linq;

namespace Articles.Api.Test.Features.Login;
public class LoginServiceBadTypeTest
{

    [Fact]
    public void LoginSuccess()
    {
         Should.Throw<Exception>(() => new LoginServiceServiceBadTypeCollectionFixture())
            .Message.ShouldBe("ImplemantationType must extend interfaceType");
    }
}