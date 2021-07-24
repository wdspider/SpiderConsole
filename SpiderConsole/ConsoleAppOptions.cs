namespace SpiderConsole
{
    public sealed record ConsoleAppOptions
    {
        public string AppTitle { get; init; } = "Spider Console App";
        public bool UseBreadcrumbHeader { get; init; } = true;
    }
}
