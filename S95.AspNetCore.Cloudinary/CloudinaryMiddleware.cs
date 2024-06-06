using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using S95.AspNetCore.Cloudinary.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace S95.AspNetCore.Cloudinary;

public class CloudinaryMiddleware
{
	private readonly RequestDelegate _next;
	private readonly Configuration _cloudinarySettings;

	private readonly string _cloudinaryUrl;
	private readonly string _imageFormats = @"\/media\/(,w_\d+)?(,h_\d+)?(,x_\d+)?(,y_\d+)?(,c_[a-zA-Z]+)?\/?(.[^ ]*\.(?:avif|gif|jpeg|jpg|png|svg|webp))";
	private readonly string _videoFormats = @"\/media\/(,w_\d+)?(,h_\d+)?(,x_\d+)?(,y_\d+)?(,c_[a-zA-Z]+)?\/?(.[^ ]*\.(?:3gp|avi|mp4|mpeg|ogg|webm|wmv))";

	public CloudinaryMiddleware(RequestDelegate next, IOptions<Configuration> cloudinarySettings)
	{
		_next = next;
		_cloudinarySettings = cloudinarySettings.Value;
		_cloudinaryUrl = _cloudinarySettings.Url;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Developments";
		var isUmbracoPath = _cloudinarySettings.ExcludePaths.Split(',').Any(context.Request.Path.Value.Contains);

		if (isUmbracoPath || isDevelopment)
		{
			await _next(context);
			return;
		}

		var originalBodyStream = context.Response.Body;

		using var newBodyStream = new MemoryStream();
		context.Response.Body = newBodyStream;

		await _next(context);

		newBodyStream.Seek(0, SeekOrigin.Begin);

		var newBodyText = await new StreamReader(newBodyStream).ReadToEndAsync();

		var isHtml = context.Response.ContentType is not null && context.Response.ContentType.Contains("text/html");

		if (isHtml)
			newBodyText = TransformString(newBodyText);

		var modifiedBodyBytes = Encoding.UTF8.GetBytes(newBodyText);
		await originalBodyStream.WriteAsync(modifiedBodyBytes, 0, modifiedBodyBytes.Length);

		context.Response.Body = originalBodyStream;
	}

	private string TransformString(string html)
	{
		var responseHtml = Regex.Replace(html, _imageFormats, _cloudinaryUrl, RegexOptions.Multiline | RegexOptions.IgnoreCase);
		return Regex.Replace(responseHtml, _videoFormats, _cloudinaryUrl.Replace("image", "video"), RegexOptions.Multiline | RegexOptions.IgnoreCase);
	}
}