using Microsoft.Playwright;

public class PlaywrightPage
{
    private readonly IPage _page;
    private readonly ILocator _searchButton;
    private readonly ILocator _searchField;
    private readonly ILocator _searchListItem;
    private readonly ILocator _titleLocators;

    public PlaywrightPage(IPage page)
    {
        _page = page;

        _searchButton = page.Locator("DocSearch-Button");
        _searchField = page.Locator("#docsearch-input");
        _searchListItem = page.Locator("//*[@id=\"docsearch-hits0-item-0\"]/a/div", new PageLocatorOptions
        {
            HasTextString = "Locators"
        });

        _titleLocators = page.Locator("//*[@id=\"__docusaurus_skipToContent_fallback\"]/div/div/main/div/div/div[1]/div/article/div[2]/header/h1");
    }

    public async Task GotoAsync()
    {
        await _page.GotoAsync("/", new PageGotoOptions
        {
            Timeout = 60000
        });
    }
    
    public string GetCurrentUrl() => _page.Url;

    public async Task SearchFieldEnterLocator(string text)
    {
        await _searchButton.ClickAsync();
        await _searchField.TypeAsync(text, new()
        {
            Delay = 100    
        });

        await _searchListItem.WaitForAsync();
        await _searchListItem.ClickAsync();

    }

    public async Task<string> LocatorPageGetTitle()
    {
        await _titleLocators.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible
        });

        return await _titleLocators.TextContentAsync();
    }
}