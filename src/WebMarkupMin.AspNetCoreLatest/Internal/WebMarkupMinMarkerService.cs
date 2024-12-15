﻿#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1.Internal
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2.Internal
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3.Internal
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6.Internal
#elif ASPNETCORE_LATEST
namespace WebMarkupMin.AspNetCoreLatest.Internal
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// This is a Marker class which is used to determine if all the services were added
	/// to when WebMarkupMin is loaded.
	/// </summary>
	public class WebMarkupMinMarkerService
	{ }
}