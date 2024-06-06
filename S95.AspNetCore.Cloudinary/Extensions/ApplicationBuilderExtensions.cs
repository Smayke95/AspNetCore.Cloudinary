using Microsoft.AspNetCore.Builder;

namespace S95.AspNetCore.Cloudinary.Extensions;

public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder UseCloudinary(this IApplicationBuilder builder)
		=> builder.UseMiddleware<CloudinaryMiddleware>();
}