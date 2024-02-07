namespace ShareSpace.Server.Extensions;

public static class PostFileCheck
{
    public static string GetFileExtension(this string type)
    {
        return type.ToLower() switch
        {
            string ext when ext.Contains("png") => "png",
            string ext when ext.Contains("jpeg") => "jpeg",
            string ext when ext.Contains("webp") => "webp",
            _ => throw new Exception("Invalid file format!")
        };
    }
}
