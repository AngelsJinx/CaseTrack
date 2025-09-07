
using Microsoft.AspNetCore.Mvc.Testing;

namespace CaseTrack.Tests;

public class BaseTest : IClassFixture<CaseTrackWebAppFactory>
{
    private readonly CaseTrackWebAppFactory _factory;

    public BaseTest(CaseTrackWebAppFactory factory)
    {
        _factory = factory;
    }
    
    protected HttpClient Client => _factory.CreateClient();
}