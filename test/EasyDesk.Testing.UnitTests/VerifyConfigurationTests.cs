using EasyDesk.Testing.VerifyConfiguration;
using EasyDesk.Tools;
using System.Collections.Immutable;
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

    private record CustomError(int Field, Error Inner) : Error;

    private record CustomErrorInside(string AnotherField) : Error;

    [Fact]
    public Task VerifyErrorConversionTest()
    {
        return Verify(new
        {
            Custom = new CustomError(42, new CustomErrorInside("hello")),
            MultiEmpty = new MultiError(new CustomError(42, new CustomErrorInside("hello")), ImmutableHashSet.Create<Error>()),
            Multi = new MultiError(
                new CustomError(42, new CustomErrorInside("hello")),
                ImmutableHashSet.Create<Error>(
                    new CustomErrorInside("asd"),
                    new CustomError(
                        123,
                        new CustomErrorInside("world")))),
        });
    }
}
