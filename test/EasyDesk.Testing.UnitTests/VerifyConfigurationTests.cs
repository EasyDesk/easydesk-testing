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
    public Task VerifyErrorConversionTestCustom()
    {
        return Verify(new CustomError(42, new CustomErrorInside("hello")));
    }

    [Fact]
    public Task VerifyErrorConversionTestMultiEmpty()
    {
        return Verify(new MultiError(new CustomError(42, new CustomErrorInside("hello")), ImmutableHashSet.Create<Error>()));
    }

    [Fact]
    public Task VerifyErrorConversionTestMulti()
    {
        return Verify(new MultiError(
                new CustomError(42, new CustomErrorInside("hello")),
                ImmutableHashSet.Create<Error>(
                    new CustomErrorInside("asd"),
                    new CustomError(
                        123,
                        new CustomErrorInside("world")))));
    }
}
