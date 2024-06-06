# AspNetCore.Cloudinary
AspNetCore.Cloudinary is a middleware for ASP.NET Core applications designed to modify all hyperlinks (href) and source links (src) in the HTML responses to a specified Cloudinary link. It uses automatic uploading of assets to Cloudinary, ensuring that all your resources are efficiently managed and delivered through Cloudinary's CDN.

## Features

- **Dynamic Link Replacement:** Automatically replaces all href and src attributes in HTML responses with a specified Cloudinary link.
- **Automatic Asset Upload:** Seamlessly uploads local assets to Cloudinary, ensuring they are always available and served from Cloudinary's CDN.
- **Easy Integration:** Simple setup and integration into existing ASP.NET Core projects.
- **Efficient Processing:** Directly manipulates the response stream for efficient content modification.
- **Configuration Flexibility:** Easily configurable through middleware extension methods.

## How to use

1. Add Auto upload mapping on Cloudinary account:
```
Folder: Media
URL prefix: https://www.{yourWebsite}.com/media/
```
2. Install the AspNetCore.Cloudinary NuGet package in your AspNetCore project.
3. In Program.cs add this:
```
builder.Services.Configure<S95.AspNetCore.Cloudinary.Models.Configuration>(builder.Configuration.GetSection("Cloudinary"));

app.UseMiddleware<CloudinaryMiddleware>();
```
4. Configure the package in appsettings.json.
5. Middleware **is not active** in DEVELOPMENT profile.


### Configuration example in appsettings.json

```
"Cloudinary": {
  "Url": "https://res.cloudinary.com/{yourCloudinaryAccount}/image/upload/f_auto$1$2$3$4$5/media/$6",
  "ExcludePaths": "umbraco"
}
```

## Licence

Project is licensed under the [MIT License](https://github.com/Smayke95/AspNetCore.Cloudinary/blob/master/LICENSE)