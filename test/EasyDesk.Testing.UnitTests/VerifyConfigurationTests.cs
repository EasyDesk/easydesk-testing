using EasyDesk.Testing.VerifyConfiguration;
using EasyDesk.Tools;
using System.Runtime.CompilerServices;
using static EasyDesk.Tools.StaticImports;

namespace EasyDesk.Testing.UnitTests;

public static class Setup
{
    [ModuleInitializer]
    public static void SetupTests()
    {
        VerifySettingsInitializer.Initialize();
    }
}

[UsesVerify]
public class VerifyConfigurationTests
{
    [Fact]
    public Task VerifyOptionConversionTest()
    {
        return Verify(new
        {
            Some = Some(123),
            None,
            Nested = Some(Some(123)),
            NoneInt = (Option<int>)None,
        });
    }
}
