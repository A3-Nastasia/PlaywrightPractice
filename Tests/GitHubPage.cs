using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightPractice

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class GitHubTest
{
    private static IPlaywright _playwright;
    private static IBrowser _browser;

    private BrowserNewContextOptions _options = new BrowserNewContextOptions
    {
        ColorScheme = ColorScheme.Light,
        ViewPortSize = new()
        {
            Width = 1920,
            Height = 1080
        },
        BaseURL = "https://github.com";
    };

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
    }

    [OneTimeTearDown]
    public async Task OneTimeTeardown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Test]
    public async Task TestWithCustomContextOptions()
    {
        await using var context = await _browser.NewContextAsync(_options);
        var page = await context.NewPageAsync();

        await page.GotoAsync("/login");
    }
}