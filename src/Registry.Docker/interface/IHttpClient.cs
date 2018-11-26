namespace Registry.Docker
{
    using System.Net;

    public interface IHttpClient
    {
        HttpWebResponse Execute(HttpWebRequest webRequest);
    }
}
