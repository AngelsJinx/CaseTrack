
using Microsoft.AspNetCore.Mvc.Testing;

namespace CaseTrack.Tests;

public class BaseTest
{
    private readonly CaseTrackWebAppFactory _factory;

    public BaseTest(CaseTrackWebAppFactory? factory = null)
    {
        _factory = factory ?? new CaseTrackWebAppFactory();
    }
    
    protected HttpClient Client => _factory.CreateClient();
}