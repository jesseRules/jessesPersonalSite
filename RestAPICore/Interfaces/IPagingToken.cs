namespace RestAPICore.Interfaces
{
    public interface IPagingToken
    {
        /// <summary>
        /// A continuation token to get the next page of the results.
        /// </summary>
#nullable enable
        string? nextPageToken { get; set; }
    }
}