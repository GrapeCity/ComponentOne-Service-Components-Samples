using C1.DataCollection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace C1DataCollection101;

public class YouTubeDataCollection : C1CursorDataCollection<YouTubeVideo>
{
    private string _q;

    public YouTubeDataCollection()
    {
        PageSize = 20;
    }

    public int PageSize { get; set; }

    public override bool HasMoreItems
    {
        get { return _q != null && base.HasMoreItems; }
    }

    private SemaphoreSlim _searchSemaphore = new SemaphoreSlim(1);

    public async Task SearchAsync(string q)
    {
        //Sets the filter string and wait the Delay time.
        _q = q;
        await Task.Delay(500);
        if (_q != q)//If the text changed means there was another keystroke.
            return;
        try
        {
            await _searchSemaphore.WaitAsync();

            if (_q != q)//If the text changed means there was another keystroke.
                return;

            await RefreshAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
        finally
        {
            _searchSemaphore.Release();
        }
    }

    protected override async Task<Tuple<string, IReadOnlyList<YouTubeVideo>>> GetPageAsync(int startingIndex, string pageToken, int? count = null, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpresssion = null, CancellationToken cancellationToken = default)
    {
        return await LoadVideosAsync(_q, "date", pageToken, PageSize, cancellationToken);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The YouTubeSearchResult class was annotated so the properties are not trimmed.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The YouTubeSearchResult class was annotated so the properties are not trimmed.")]
    public static async Task<Tuple<string, IReadOnlyList<YouTubeVideo>>> LoadVideosAsync(string q, string orderBy, string pageToken, int maxResults, CancellationToken cancellationToken = default)
    {
        q = q ?? "";

            var youtubeUrl = string.Format("https://www.googleapis.com/youtube/v3/search?part=snippet&type=video&q={0}&order={1}&maxResults={2}{3}&key={4}", Uri.EscapeDataString(q), orderBy, maxResults, string.IsNullOrWhiteSpace(pageToken) ? "" : "&pageToken=" + pageToken, "AIzaSyCGIvlreIsHaOVVoR2JAPJnbHkSUv3v4y0");

        var client = new HttpClient();
        var response = await client.GetAsync(youtubeUrl, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var videos = new List<YouTubeVideo>();
            var result = await response.Content.ReadFromJsonAsync<YouTubeSearchResult>(cancellationToken);
            foreach (var item in result.Items)
            {
                var video = new YouTubeVideo()
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    Thumbnail = item.Snippet.Thumbnails.Default.Url,
                    Link = "http://www.youtube.com/watch?v=" + item.Id.VideoId,
                    ChannelTitle = item.Snippet.ChannelTitle,
                    PublishedAt = DateTime.Parse(item.Snippet.PublishedAt),
                };
                videos.Add(video);
            }
            return new Tuple<string, IReadOnlyList<YouTubeVideo>>(result.NextPageToken, videos);
        }
        else
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

    }
}

[DataContract]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.AllProperties)]
public class YouTubeSearchResult
{
    [DataMember(Name = "nextPageToken")]
    public string NextPageToken { get; set; }
    [DataMember(Name = "items")]
    public YouTubeSearchItemResult[] Items { get; set; }
}

[DataContract]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.AllProperties)]
public class YouTubeSearchItemResult
{
    [DataMember(Name = "id")]
    public YouTubeVideoId Id { get; set; }
    [DataMember(Name = "snippet")]
    public YouTubeSnippet Snippet { get; set; }
}

[DataContract]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.AllProperties)]
public class YouTubeVideoId
{
    [DataMember(Name = "videoId")]
    public string VideoId { get; set; }
}

[DataContract]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.AllProperties)]
public class YouTubeSnippet
{
    [DataMember(Name = "title")]
    public string Title { get; set; }
    [DataMember(Name = "description")]
    public string Description { get; set; }
    [DataMember(Name = "thumbnails")]
    public YouTubeThumbnails Thumbnails { get; set; }
    [DataMember(Name = "channelTitle")]
    public string ChannelTitle { get; set; }
    [DataMember(Name = "publishedAt")]
    public string PublishedAt { get; set; }
}

[DataContract]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.AllProperties)]
public class YouTubeThumbnails
{
    [DataMember(Name = "default")]
    public YouTubeThumbnail Default { get; set; }
    [DataMember(Name = "medium")]
    public YouTubeThumbnail Medium { get; set; }
    [DataMember(Name = "high")]
    public YouTubeThumbnail High { get; set; }
}

[DataContract]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.AllProperties)]
public class YouTubeThumbnail
{
    [DataMember(Name = "url")]
    public string Url { get; set; }
}

[DataContract]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.AllProperties)]
public class YouTubeVideo
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Thumbnail { get; set; }
    public string Link { get; set; }
    public string ChannelTitle { get; set; }
    public DateTime PublishedAt { get; set; }

    public string PublishedDay
    {
        get
        {
            return $"{PublishedAt.Date:D}";
        }
    }
}
