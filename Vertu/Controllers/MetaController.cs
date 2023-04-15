using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;


namespace Meta
{
    /// <summary>
    /// A base class for an MVC controller without view support.
    /// </summary>
    [AcitonMeta]
    [Controller]
    public abstract class MetaController
    {
#pragma warning disable CS8618
        private ControllerContext _controllerContext;

        private IModelMetadataProvider _metadataProvider;

        private IModelBinderFactory _modelBinderFactory;

        private IObjectModelValidator _objectValidator;

        private IUrlHelper _url;
#pragma warning restore CS8618

        /// <summary>
        /// Gets the Microsoft.AspNetCore.Http.HttpContext for the executing action.
        /// </summary>
        public HttpContext HttpContext => ControllerContext.HttpContext;

        /// <summary>
        /// Gets the Microsoft.AspNetCore.Http.HttpRequest for the executing action.
        /// </summary>
        public HttpRequest Request => HttpContext?.Request!;

        /// <summary>
        /// Gets the Microsoft.AspNetCore.Http.HttpResponse for the executing action.
        /// </summary>
        public HttpResponse Response => HttpContext?.Response!;

        /// <summary>
        /// Gets the Microsoft.AspNetCore.Routing.RouteData for the executing action.
        /// </summary>
        public RouteData RouteData => ControllerContext.RouteData;

        /// <summary>
        /// Gets the Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary that contains the state of the model and of model-binding validation.
        /// </summary>
        public ModelStateDictionary ModelState => ControllerContext.ModelState;

        /// <summary>
        /// Gets or sets the Microsoft.AspNetCore.Mvc.ControllerContext.
        /// </summary>
        /// <remarks>
        /// Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator activates this property while activating controllers. If user code directly instantiates a controller, the getter returns an empty Microsoft.AspNetCore.Mvc.ControllerContext.
        /// </remarks>
        [ControllerContext]
        public ControllerContext ControllerContext
        {
            get
            {
                if (_controllerContext == null)
                {
                    _controllerContext = new ControllerContext();
                }

                return _controllerContext;
            }
            set => _controllerContext = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.IModelMetadataProvider.
        /// </summary>
        public IModelMetadataProvider MetadataProvider
        {
            get
            {
                if (_metadataProvider == null)
                    _metadataProvider = HttpContext?.RequestServices?.GetRequiredService<IModelMetadataProvider>()!;

                return _metadataProvider;
            }
            set
            {
                _metadataProvider = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderFactory.
        /// </summary>
        public IModelBinderFactory ModelBinderFactory
        {
            get
            {
                if (_modelBinderFactory == null)
                    _modelBinderFactory = HttpContext?.RequestServices?.GetRequiredService<IModelBinderFactory>()!;

                return _modelBinderFactory;
            }
            set => _modelBinderFactory = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the Microsoft.AspNetCore.Mvc.IUrlHelper.
        /// </summary>
        public IUrlHelper Url
        {
            get
            {
                if (_url == null)
                    _url = (HttpContext?.RequestServices?.GetRequiredService<IUrlHelperFactory>())?.GetUrlHelper(ControllerContext)!;

                return _url;
            }
            set => _url = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator.
        /// </summary>
        public IObjectModelValidator ObjectValidator
        {
            get
            {
                if (_objectValidator == null)
                    _objectValidator = HttpContext?.RequestServices?.GetRequiredService<IObjectModelValidator>()!;

                return _objectValidator;
            }
            set
            {
                _objectValidator = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets the System.Security.Claims.ClaimsPrincipal for user associated with the executing action.
        /// </summary>
        public ClaimsPrincipal User => HttpContext?.User!;

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.StatusCodeResult object by specifying a statusCode.
        /// </summary>
        /// <param name="statusCode">The status code to set on the response.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.StatusCodeResult object for the response.</returns>
        [NonAction]
        public virtual StatusCodeResult StatusCode([ActionResultStatusCode] int statusCode)
        {
            return new StatusCodeResult(statusCode);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ObjectResult object by specifying a statusCode and value
        /// </summary>
        /// <param name="statusCode">The status code to set on the response.</param>
        /// <param name="value">The value to set on the Microsoft.AspNetCore.Mvc.ObjectResult.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ObjectResult object for the response.</returns>
        [NonAction]
        public virtual ObjectResult StatusCode([ActionResultStatusCode] int statusCode, [ActionResultObjectValue] object value)
        {
            return new ObjectResult(value)
            {
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ContentResult object with Microsoft.AspNetCore.Http.StatusCodes.Status200OK by specifying a content string.
        /// </summary>
        /// <param name="content">The content to write to the response.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ContentResult object for the response.</returns>
        [NonAction]
        public virtual ContentResult Content(string content)
        {
            return Content(content, (MediaTypeHeaderValue)null!);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ContentResult object with Microsoft.AspNetCore.Http.StatusCodes.Status200OK by specifying a content string and a content type.
        /// </summary>
        /// <param name="content">The content to write to the response.</param>
        /// <param name="contentType">The content type (MIME type).</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ContentResult object for the response.</returns>
        [NonAction]
        public virtual ContentResult Content(string content, string contentType)
        {
            return Content(content, MediaTypeHeaderValue.Parse(contentType));
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ContentResult object with Microsoft.AspNetCore.Http.StatusCodes.Status200OK by specifying a content string, a contentType, and contentEncoding.
        /// </summary>
        /// <param name="content">The content to write to the response.</param>
        /// <param name="contentType">The content type (MIME type).</param>
        /// <param name="contentEncoding">The content encoding.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ContentResult object for the response.</returns>
        /// <remarks>
        /// If encoding is provided by both the 'charset' and the contentEncoding parameters, then the contentEncoding parameter is chosen as the final encoding.
        /// </remarks>
        [NonAction]
        public virtual ContentResult Content(string content, string contentType, Encoding contentEncoding)
        {
            MediaTypeHeaderValue mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(contentType);
            mediaTypeHeaderValue.Encoding = contentEncoding ?? mediaTypeHeaderValue.Encoding;
            return Content(content, mediaTypeHeaderValue);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ContentResult object with Microsoft.AspNetCore.Http.StatusCodes.Status200OK by specifying a content string and a contentType.
        /// </summary>
        /// <param name="content">The content to write to the response.</param>
        /// <param name="contentType">The content type (MIME type).</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ContentResult object for the response.</returns>
        [NonAction]
        public virtual ContentResult Content(string content, MediaTypeHeaderValue contentType)
        {
            return new ContentResult
            {
                Content = content,
                ContentType = contentType?.ToString()
            };
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.NoContentResult object that produces an empty Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent response.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.NoContentResult object for the response.</returns>
        [NonAction]
        public virtual NoContentResult NoContent()
        {
            return new NoContentResult();
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.OkResult object that produces an empty Microsoft.AspNetCore.Http.StatusCodes.Status200OK response.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.OkResult for the response.</returns>
        [NonAction]
        public virtual OkResult Ok()
        {
            return new OkResult();
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.OkObjectResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status200OK response.
        /// </summary>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.OkObjectResult for the response.</returns>
        [NonAction]
        public virtual OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            return new OkObjectResult(value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.RedirectResult object that redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified url.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.</returns>
        /// <exception cref="ArgumentException"></exception>
        [NonAction]
        public virtual RedirectResult Redirect(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(url));

            return new RedirectResult(url);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.RedirectResult object with Microsoft.AspNetCore.Mvc.RedirectResult.Permanent set to true (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) using the specified url.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.</returns>
        /// <exception cref="ArgumentException"></exception>
        [NonAction]
        public virtual RedirectResult RedirectPermanent(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(url));

            return new RedirectResult(url, permanent: true);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.RedirectResult object with Microsoft.AspNetCore.Mvc.RedirectResult.Permanent set to false and Microsoft.AspNetCore.Mvc.RedirectResult.PreserveMethod set to true (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect) using the specified url.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.</returns>
        /// <exception cref="ArgumentException"></exception>
        [NonAction]
        public virtual RedirectResult RedirectPreserveMethod(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(url));

            return new RedirectResult(url, permanent: false, preserveMethod: true);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.RedirectResult object with Microsoft.AspNetCore.Mvc.RedirectResult.Permanent set to true and Microsoft.AspNetCore.Mvc.RedirectResult.PreserveMethod set to true (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect) using the specified url.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectResult for the response.</returns>
        /// <exception cref="ArgumentException"></exception>
        [NonAction]
        public virtual RedirectResult RedirectPermanentPreserveMethod(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(url));

            return new RedirectResult(url, permanent: true, preserveMethod: true);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object that redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified local localUrl.
        /// </summary>
        /// <param name="localUrl">The local URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.</returns>
        /// <exception cref="ArgumentException"></exception>
        [NonAction]
        public virtual LocalRedirectResult LocalRedirect(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(localUrl));

            return new LocalRedirectResult(localUrl);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object with Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent set to true (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) using the specified localUrl.
        /// </summary>
        /// <param name="localUrl">The local URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.</returns>
        /// <exception cref="ArgumentException"></exception>
        [NonAction]
        public virtual LocalRedirectResult LocalRedirectPermanent(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(localUrl));

            return new LocalRedirectResult(localUrl, permanent: true);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object with Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent
        /// set to false and Microsoft.AspNetCore.Mvc.LocalRedirectResult.PreserveMethod
        /// set to true (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect)
        /// using the specified localUrl.
        /// </summary>
        /// <param name="localUrl">The local URL to redirect to.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.</returns>
        /// <exception cref="ArgumentException"></exception>
        [NonAction]
        public virtual LocalRedirectResult LocalRedirectPreserveMethod(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(localUrl));

            return new LocalRedirectResult(localUrl, permanent: false, preserveMethod: true);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.LocalRedirectResult object with Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent
        /// set to true and Microsoft.AspNetCore.Mvc.LocalRedirectResult.PreserveMethod set
        /// to true (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect) using
        /// the specified localUrl.
        /// </summary>
        /// <param name="localUrl">The local URL to redirect to.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The created Microsoft.AspNetCore.Mvc.LocalRedirectResult for the response.</exception>
        [NonAction]
        public virtual LocalRedirectResult LocalRedirectPermanentPreserveMethod(string localUrl)
        {
            if (string.IsNullOrEmpty(localUrl))
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(localUrl));

            return new LocalRedirectResult(localUrl, permanent: true, preserveMethod: true);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to an action
        /// with the same name as current one. The 'controller' and 'action' names are retrieved
        /// from the ambient values of the current request.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction()
        {
            return RedirectToAction(null);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// action using the actionName.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName)
        {
            return RedirectToAction(actionName, (object)null!);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// action using the actionName and routeValues.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, object routeValues)
        {
            return RedirectToAction(actionName, null!, routeValues);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// action using the actionName and the controllerName.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName)
        {
            return RedirectToAction(actionName, controllerName, (object)null!);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// action using the specified actionName, controllerName, and routeValues.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues)
        {
            return RedirectToAction(actionName, controllerName, routeValues, null);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// action using the specified actionName, controllerName, and fragment.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName, string fragment)
        {
            return RedirectToAction(actionName, controllerName, null!, fragment);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// action using the specified actionName, controllerName, routeValues, and fragment.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues, string fragment)
        {
            return new RedirectToActionResult(actionName, controllerName, routeValues, fragment)
            {
                UrlHelper = Url
            };
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect)
        /// to the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        /// set to false and Microsoft.AspNetCore.Mvc.RedirectToActionResult.PreserveMethod
        /// set to true, using the specified actionName, controllerName, routeValues, and
        /// fragment.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPreserveMethod(string actionName = null!, string controllerName = null!, object routeValues = null!, string fragment = null!)
        {
            return new RedirectToActionResult(actionName, controllerName, routeValues, permanent: false, preserveMethod: true, fragment)
            {
                UrlHelper = Url
            };
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        /// set to true using the specified actionName.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName)
        {
            return RedirectToActionPermanent(actionName, (object)null!);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        /// set to true using the specified actionName and routeValues.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, object routeValues)
        {
            return RedirectToActionPermanent(actionName, null!, routeValues);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        /// set to true using the specified actionName and controllerName.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName)
        {
            return RedirectToActionPermanent(actionName, controllerName, (object)null!);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        /// set to true using the specified actionName, controllerName, and fragment.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName, string fragment)
        {
            return RedirectToActionPermanent(actionName, controllerName, null!, fragment);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        /// set to true using the specified actionName, controllerName, and routeValues.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName, object routeValues)
        {
            return RedirectToActionPermanent(actionName, controllerName, routeValues, null);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        /// set to true using the specified actionName, controllerName, routeValues, and
        /// fragment.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanent(string actionName, string controllerName, object routeValues, string fragment)
        {
            return new RedirectToActionResult(actionName, controllerName, routeValues, permanent: true, fragment)
            {
                UrlHelper = Url
            };
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect)
        /// to the specified action with Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent
        /// set to true and Microsoft.AspNetCore.Mvc.RedirectToActionResult.PreserveMethod
        /// set to true, using the specified actionName, controllerName, routeValues, and
        /// fragment.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToActionResult for the response.</returns>
        [NonAction]
        public virtual RedirectToActionResult RedirectToActionPermanentPreserveMethod(string actionName = null!, string controllerName = null!, object routeValues = null!, string fragment = null!)
        {
            return new RedirectToActionResult(actionName, controllerName, routeValues, permanent: true, preserveMethod: true, fragment)
            {
                UrlHelper = Url
            };
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// route using the specified routeName.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(string routeName)
        {
            return RedirectToRoute(routeName, (object)null!);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// route using the specified routeValues.
        /// </summary>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(object routeValues)
        {
            return RedirectToRoute(null!, routeValues);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// route using the specified routeName and routeValues.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(string routeName, object routeValues)
        {
            return RedirectToRoute(routeName, routeValues, null);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// route using the specified routeName and fragment.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(string routeName, string fragment)
        {
            return RedirectToRoute(routeName, null!, fragment);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// route using the specified routeName, routeValues, and fragment.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoute(string routeName, object routeValues, string fragment)
        {
            return new RedirectToRouteResult(routeName, routeValues, fragment)
            {
                UrlHelper = Url
            };
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect)
        /// to the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        /// set to false and Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod
        /// set to true, using the specified routeName, routeValues, and fragment.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePreserveMethod(string routeName = null!, object routeValues = null!, string fragment = null!)
        {
            return new RedirectToRouteResult(routeName, routeValues, permanent: false, preserveMethod: true, fragment)
            {
                UrlHelper = Url
            };
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        /// set to true using the specified routeName.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName)
        {
            return RedirectToRoutePermanent(routeName, (object)null!);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        /// set to true using the specified routeValues.
        /// </summary>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(object routeValues)
        {
            return RedirectToRoutePermanent(null!, routeValues);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        /// set to true using the specified routeName and routeValues.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName, object routeValues)
        {
            return RedirectToRoutePermanent(routeName, routeValues, null);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        /// set to true using the specified routeName and fragment.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName, string fragment)
        {
            return RedirectToRoutePermanent(routeName, null!, fragment);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        /// set to true using the specified routeName, routeValues, and fragment.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanent(string routeName, object routeValues, string fragment)
        {
            return new RedirectToRouteResult(routeName, routeValues, permanent: true, fragment)
            {
                UrlHelper = Url
            };
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect)
        /// to the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        /// set to true and Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod
        /// set to true, using the specified routeName, routeValues, and fragment.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        [NonAction]
        public virtual RedirectToRouteResult RedirectToRoutePermanentPreserveMethod(string routeName = null!, object routeValues = null!, string fragment = null!)
        {
            return new RedirectToRouteResult(routeName, routeValues, permanent: true, preserveMethod: true, fragment)
            {
                UrlHelper = Url
            };
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified pageName.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName)
        {
            return RedirectToPage(pageName, (object)null!);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// pageName using the specified routeValues.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, object routeValues)
        {
            return RedirectToPage(pageName, null!, routeValues, null);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// pageName using the specified pageHandler.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageHandler">The page handler to redirect to.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, string pageHandler)
        {
            return RedirectToPage(pageName, pageHandler, (object)null!);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified pageName.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageHandler">The page handler to redirect to.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, string pageHandler, object routeValues)
        {
            return RedirectToPage(pageName, pageHandler, routeValues, null);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// pageName using the specified fragment.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageHandler">The page handler to redirect to.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, string pageHandler, string fragment)
        {
            return RedirectToPage(pageName, pageHandler, null!, fragment);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status302Found) to the specified
        /// pageName using the specified routeValues and fragment.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageHandler">The page handler to redirect to.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPage(string pageName, string pageHandler, object routeValues, string fragment)
        {
            return new RedirectToPageResult(pageName, pageHandler, routeValues, fragment);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified pageName.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent set.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName)
        {
            return RedirectToPagePermanent(pageName, (object)null!);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified pageName using the specified routeValues.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent set.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, object routeValues)
        {
            return RedirectToPagePermanent(pageName, null!, routeValues, null);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified pageName using the specified pageHandler.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageHandler">The page handler to redirect to.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent set.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, string pageHandler)
        {
            return RedirectToPagePermanent(pageName, pageHandler, null!, null);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified pageName using the specified fragment.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageHandler">The page handler to redirect to.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent set.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, string pageHandler, string fragment)
        {
            return RedirectToPagePermanent(pageName, pageHandler, null!, fragment);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently) to
        /// the specified pageName using the specified routeValues and fragment.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageHandler">The page handler to redirect to.</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The Microsoft.AspNetCore.Mvc.RedirectToPageResult with Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent set.</returns>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, string pageHandler, object routeValues, string fragment)
        {
            return new RedirectToPageResult(pageName, pageHandler, routeValues, permanent: true, fragment);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect)
        /// to the specified page with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        /// set to false and Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod
        /// set to true, using the specified pageName, routeValues, and fragment.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageHandler">The page handler to redirect to.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePreserveMethod(string pageName, string pageHandler = null!, object routeValues = null!, string fragment = null!)
        {
            if (pageName == null)
                throw new ArgumentNullException(nameof(pageName));

            return new RedirectToPageResult(pageName, pageHandler, routeValues, permanent: false, preserveMethod: true, fragment);
        }

        /// <summary>
        /// Redirects (Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect)
        /// to the specified route with Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent
        /// set to true and Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod
        /// set to true, using the specified pageName, routeValues, and fragment.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageHandler">The page handler to redirect to.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="fragment">The fragment to add to the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.RedirectToRouteResult for the response.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual RedirectToPageResult RedirectToPagePermanentPreserveMethod(string pageName, string pageHandler = null!, object routeValues = null!, string fragment = null!)
        {
            if (pageName == null)
                throw new ArgumentNullException(nameof(pageName));

            return new RedirectToPageResult(pageName, pageHandler, routeValues, permanent: true, preserveMethod: true, fragment);
        }

        /// <summary>
        /// Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.</returns>
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType)
        {
            return File(fileContents, contentType, null);
        }

        /// <summary>
        /// Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.</returns>
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, bool enableRangeProcessing)
        {
            return File(fileContents, contentType, null!, enableRangeProcessing);
        }

        /// <summary>
        /// Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.</returns>
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName)
        {
            return new FileContentResult(fileContents, contentType)
            {
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.</returns>
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new FileContentResult(fileContents, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.</returns>
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag
            };
        }

        /// <summary>
        /// Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.</returns>
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type, and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.</returns>
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// Returns a file with the specified fileContents as content (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type, and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileContentResult for the response.</returns>
        [NonAction]
        public virtual FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new FileContentResult(fileContents, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// with the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The System.IO.Stream with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.</returns>
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType)
        {
            return File(fileStream, contentType, null);
        }

        /// <summary>
        /// Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// with the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The System.IO.Stream with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.</returns>
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, bool enableRangeProcessing)
        {
            return File(fileStream, contentType, null!, enableRangeProcessing);
        }

        /// <summary>
        /// Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The System.IO.Stream with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.</returns>
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName)
        {
            return new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The System.IO.Stream with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.</returns>
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The System.IO.Stream with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.</returns>
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag
            };
        }

        /// <summary>
        /// Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The System.IO.Stream with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.</returns>
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type, and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The System.IO.Stream with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.</returns>
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// Returns a file in the specified fileStream (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type, and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="fileStream">The System.IO.Stream with the contents of the file.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.FileStreamResult for the response.</returns>
        [NonAction]
        public virtual FileStreamResult File(Stream fileStream, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new FileStreamResult(fileStream, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="virtualPath">The virtual path of the file to be returned.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.</returns>
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType)
        {
            return File(virtualPath, contentType, null);
        }

        /// <summary>
        /// Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="virtualPath">The virtual path of the file to be returned.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.</returns>
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, bool enableRangeProcessing)
        {
            return File(virtualPath, contentType, null!, enableRangeProcessing);
        }

        /// <summary>
        /// Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="virtualPath">The virtual path of the file to be returned.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.</returns>
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName)
        {
            return new VirtualFileResult(virtualPath, contentType)
            {
                FileDownloadName = fileDownloadName
            };
        }
        
        /// <summary>
        /// Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="virtualPath">The virtual path of the file to be returned.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.</returns>
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new VirtualFileResult(virtualPath, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="virtualPath">The virtual path of the file to be returned.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.</returns>
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag
            };
        }

        /// <summary>
        /// Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="virtualPath">The virtual path of the file to be returned.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.</returns>
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type, and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="virtualPath">The virtual path of the file to be returned.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.</returns>
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// Returns the file specified by virtualPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type, and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="virtualPath">The virtual path of the file to be returned.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.VirtualFileResult for the response.</returns>
        [NonAction]
        public virtual VirtualFileResult File(string virtualPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new VirtualFileResult(virtualPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.</returns>
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType)
        {
            return PhysicalFile(physicalPath, contentType, null);
        }

        /// <summary>
        /// Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.</returns>
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, bool enableRangeProcessing)
        {
            return PhysicalFile(physicalPath, contentType, null!, enableRangeProcessing);
        }

        /// <summary>
        /// Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.</returns>
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName)
        {
            return new PhysicalFileResult(physicalPath, contentType)
            {
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK)
        /// with the specified contentType as the Content-Type and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.</returns>
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName, bool enableRangeProcessing)
        {
            return new PhysicalFileResult(physicalPath, contentType)
            {
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.</returns>
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag
            };
        }

        /// <summary>
        /// Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// and the specified contentType as the Content-Type. This supports range requests
        /// (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable
        /// if the range is not satisfiable).
        /// </summary>
        /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.</returns>
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type, and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.</returns>
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag)
        {
            return new PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// Returns the file specified by physicalPath (Microsoft.AspNetCore.Http.StatusCodes.Status200OK),
        /// the specified contentType as the Content-Type, and the specified fileDownloadName
        /// as the suggested file name. This supports range requests (Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent
        /// or Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable if the
        /// range is not satisfiable).
        /// </summary>
        /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
        /// <param name="contentType">The Content-Type of the file.</param>
        /// <param name="fileDownloadName">The suggested file name.</param>
        /// <param name="lastModified">The System.DateTimeOffset of when the file was last modified.</param>
        /// <param name="entityTag">The Microsoft.Net.Http.Headers.EntityTagHeaderValue associated with the file.</param>
        /// <param name="enableRangeProcessing">Set to true to enable range requests processing.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.PhysicalFileResult for the response.</returns>
        [NonAction]
        public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType, string fileDownloadName, DateTimeOffset? lastModified, EntityTagHeaderValue entityTag, bool enableRangeProcessing)
        {
            return new PhysicalFileResult(physicalPath, contentType)
            {
                LastModified = lastModified,
                EntityTag = entityTag,
                FileDownloadName = fileDownloadName,
                EnableRangeProcessing = enableRangeProcessing
            };
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.UnauthorizedResult that produces an Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized response.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.UnauthorizedResult for the response.</returns>
        [NonAction]
        public virtual UnauthorizedResult Unauthorized()
        {
            return new UnauthorizedResult();
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult that produces a
        /// Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized response.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult for the response.</returns>
        [NonAction]
        public virtual UnauthorizedObjectResult Unauthorized([ActionResultObjectValue] object value)
        {
            return new UnauthorizedObjectResult(value);
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.NotFoundResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound response.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.NotFoundResult for the response.</returns>
        [NonAction]
        public virtual NotFoundResult NotFound()
        {
            return new NotFoundResult();
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.NotFoundObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound response.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.NotFoundObjectResult for the response.</returns>
        [NonAction]
        public virtual NotFoundObjectResult NotFound([ActionResultObjectValue] object value)
        {
            return new NotFoundObjectResult(value);
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.BadRequestResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest response.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.BadRequestResult for the response.</returns>
        [NonAction]
        public virtual BadRequestResult BadRequest()
        {
            return new BadRequestResult();
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest response.
        /// </summary>
        /// <param name="error">An error object to be returned to the client.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.</returns>
        [NonAction]
        public virtual BadRequestObjectResult BadRequest([ActionResultObjectValue] object error)
        {
            return new BadRequestObjectResult(error);
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest response.
        /// </summary>
        /// <param name="modelState">The Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary containing errors to be returned to the client.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual BadRequestObjectResult BadRequest([ActionResultObjectValue] ModelStateDictionary modelState)
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));

            return new BadRequestObjectResult(modelState);
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityResult that produces a
        /// Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity response.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.UnprocessableEntityResult for the response.</returns>
        [NonAction]
        public virtual UnprocessableEntityResult UnprocessableEntity()
        {
            return new UnprocessableEntityResult();
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult that produces
        /// a Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity response.
        /// </summary>
        /// <param name="error">An error object to be returned to the client.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult for the response.</returns>
        [NonAction]
        public virtual UnprocessableEntityObjectResult UnprocessableEntity([ActionResultObjectValue] object error)
        {
            return new UnprocessableEntityObjectResult(error);
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult that produces
        /// a Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity response.
        /// </summary>
        /// <param name="modelState">The Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary containing errors to be returned to the client.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult for the response.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual UnprocessableEntityObjectResult UnprocessableEntity([ActionResultObjectValue] ModelStateDictionary modelState)
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));

            return new UnprocessableEntityObjectResult(modelState);
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.ConflictResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict response.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ConflictResult for the response.</returns>
        [NonAction]
        public virtual ConflictResult Conflict()
        {
            return new ConflictResult();
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.ConflictObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict response.
        /// </summary>
        /// <param name="error">Contains errors to be returned to the client.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ConflictObjectResult for the response.</returns>
        [NonAction]
        public virtual ConflictObjectResult Conflict([ActionResultObjectValue] object error)
        {
            return new ConflictObjectResult(error);
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.ConflictObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict response.
        /// </summary>
        /// <param name="modelState">The Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary containing errors to be returned to the client.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ConflictObjectResult for the response.</returns>
        [NonAction]
        public virtual ConflictObjectResult Conflict([ActionResultObjectValue] ModelStateDictionary modelState)
        {
            return new ConflictObjectResult(modelState);
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest response.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual ActionResult ValidationProblem([ActionResultObjectValue] ValidationProblemDetails descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            return new BadRequestObjectResult(descriptor);
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest response.
        /// </summary>
        /// <param name="modelStateDictionary"></param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary == null)
                throw new ArgumentNullException(nameof(modelStateDictionary));

            return new BadRequestObjectResult(new ValidationProblemDetails(modelStateDictionary));
        }

        /// <summary>
        /// Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        /// response with validation errors from Microsoft.AspNetCore.Mvc.ControllerBase.ModelState.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response.</returns>
        [NonAction]
        public virtual ActionResult ValidationProblem()
        {
            return new BadRequestObjectResult(new ValidationProblemDetails(ModelState));
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.CreatedResult object that produces a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        /// </summary>
        /// <param name="uri">The URI at which the content has been created.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.CreatedResult for the response.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual CreatedResult Created(string uri, [ActionResultObjectValue] object value)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return new CreatedResult(uri, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.CreatedResult object that produces a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        /// </summary>
        /// <param name="uri">The URI at which the content has been created.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The created Microsoft.AspNetCore.Mvc.CreatedResult for the response.</exception>
        [NonAction]
        public virtual CreatedResult Created(Uri uri, [ActionResultObjectValue] object value)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return new CreatedResult(uri, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.CreatedAtActionResult object that produces
        /// a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        /// </summary>
        /// <param name="actionName">The name of the action to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.CreatedAtActionResult for the response.</returns>
        [NonAction]
        public virtual CreatedAtActionResult CreatedAtAction(string actionName, [ActionResultObjectValue] object value)
        {
            return CreatedAtAction(actionName, null!, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.CreatedAtActionResult object that produces
        /// a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        /// </summary>
        /// <param name="actionName">The name of the action to use for generating the URL.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.CreatedAtActionResult for the response.</returns>
        [NonAction]
        public virtual CreatedAtActionResult CreatedAtAction(string actionName, object routeValues, [ActionResultObjectValue] object value)
        {
            return CreatedAtAction(actionName, null!, routeValues, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.CreatedAtActionResult object that produces
        /// a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        /// </summary>
        /// <param name="actionName">The name of the action to use for generating the URL.</param>
        /// <param name="controllerName">The name of the controller to use for generating the URL.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.CreatedAtActionResult for the response.</returns>
        [NonAction]
        public virtual CreatedAtActionResult CreatedAtAction(string actionName, string controllerName, object routeValues, [ActionResultObjectValue] object value)
        {
            return new CreatedAtActionResult(actionName, controllerName, routeValues, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.CreatedAtRouteResult object that produces
        /// a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        /// </summary>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.CreatedAtRouteResult for the response.</returns>
        [NonAction]
        public virtual CreatedAtRouteResult CreatedAtRoute(string routeName, [ActionResultObjectValue] object value)
        {
            return CreatedAtRoute(routeName, null!, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.CreatedAtRouteResult object that produces
        /// a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        /// </summary>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.CreatedAtRouteResult for the response.</returns>
        [NonAction]
        public virtual CreatedAtRouteResult CreatedAtRoute(object routeValues, [ActionResultObjectValue] object value)
        {
            return CreatedAtRoute(null!, routeValues, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.CreatedAtRouteResult object that produces
        /// a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
        /// </summary>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="value">The content value to format in the entity body.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.CreatedAtRouteResult for the response.</returns>
        [NonAction]
        public virtual CreatedAtRouteResult CreatedAtRoute(string routeName, object routeValues, [ActionResultObjectValue] object value)
        {
            return new CreatedAtRouteResult(routeName, routeValues, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.</returns>
        [NonAction]
        public virtual AcceptedResult Accepted()
        {
            return new AcceptedResult();
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="value">The optional content value to format in the entity body; may be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.</returns>
        [NonAction]
        public virtual AcceptedResult Accepted([ActionResultObjectValue] object value)
        {
            return new AcceptedResult((string)null!, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="uri">The optional URI with the location at which the status of requested content can be monitored. May be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual AcceptedResult Accepted(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return new AcceptedResult(uri, null);
        }

        //
        // 摘要:
        //     Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted
        //     response.
        //
        // 参数:
        //   uri:
        //     The optional URI with the location at which the status of requested content can
        //     be monitored. May be null.
        //
        // 返回结果:
        //     The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.
        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="uri">The optional URI with the location at which the status of requested content can be monitored. May be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.</returns>
        [NonAction]
        public virtual AcceptedResult Accepted(string uri)
        {
            return new AcceptedResult(uri, null);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="uri">The URI with the location at which the status of requested content can be monitored.</param>
        /// <param name="value">The optional content value to format in the entity body; may be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.</returns>
        [NonAction]
        public virtual AcceptedResult Accepted(string uri, [ActionResultObjectValue] object value)
        {
            return new AcceptedResult(uri, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="uri">The URI with the location at which the status of requested content can be monitored.</param>
        /// <param name="value">The optional content value to format in the entity body; may be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedResult for the response.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual AcceptedResult Accepted(Uri uri, [ActionResultObjectValue] object value)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return new AcceptedResult(uri, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="actionName">The name of the action to use for generating the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName)
        {
            return AcceptedAtAction(actionName, (object)null!, (object)null!);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="actionName">The name of the action to use for generating the URL.</param>
        /// <param name="controllerName">The name of the controller to use for generating the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, string controllerName)
        {
            return AcceptedAtAction(actionName, controllerName, null!, null);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="actionName">The name of the action to use for generating the URL.</param>
        /// <param name="value">The optional content value to format in the entity body; may be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, [ActionResultObjectValue] object value)
        {
            return AcceptedAtAction(actionName, (object)null!, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="actionName">The name of the action to use for generating the URL.</param>
        /// <param name="controllerName">The name of the controller to use for generating the URL.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, string controllerName, [ActionResultObjectValue] object routeValues)
        {
            return AcceptedAtAction(actionName, controllerName, routeValues, null);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="actionName">The name of the action to use for generating the URL.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="value">The optional content value to format in the entity body; may be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, object routeValues, [ActionResultObjectValue] object value)
        {
            return AcceptedAtAction(actionName, null!, routeValues, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtActionResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="actionName">The name of the action to use for generating the URL.</param>
        /// <param name="controllerName">The name of the controller to use for generating the URL.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="value">The optional content value to format in the entity body; may be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtActionResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtActionResult AcceptedAtAction(string actionName, string controllerName, object routeValues, [ActionResultObjectValue] object value)
        {
            return new AcceptedAtActionResult(actionName, controllerName, routeValues, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute([ActionResultObjectValue] object routeValues)
        {
            return AcceptedAtRoute(null!, routeValues, null);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute(string routeName)
        {
            return AcceptedAtRoute(routeName, null!, null);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute(string routeName, object routeValues)
        {
            return AcceptedAtRoute(routeName, routeValues, null);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="value">The optional content value to format in the entity body; may be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute(object routeValues, [ActionResultObjectValue] object value)
        {
            return AcceptedAtRoute(null!, routeValues, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult object that produces
        /// an Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted response.
        /// </summary>
        /// <param name="routeName">The name of the route to use for generating the URL.</param>
        /// <param name="routeValues">The route data to use for generating the URL.</param>
        /// <param name="value">The optional content value to format in the entity body; may be null.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult for the response.</returns>
        [NonAction]
        public virtual AcceptedAtRouteResult AcceptedAtRoute(string routeName, object routeValues, [ActionResultObjectValue] object value)
        {
            return new AcceptedAtRouteResult(routeName, routeValues, value);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ChallengeResult.
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ChallengeResult for the response.</returns>
        /// <remarks>
        /// The behavior of this method depends on the Microsoft.AspNetCore.Authentication.IAuthenticationService
        /// in use. Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized and Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// are among likely status results.
        /// </remarks>
        [NonAction]
        public virtual ChallengeResult Challenge()
        {
            return new ChallengeResult();
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ChallengeResult with the specified authentication schemes.
        /// </summary>
        /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ChallengeResult for the response.</returns>
        /// <remarks>
        /// The behavior of this method depends on the Microsoft.AspNetCore.Authentication.IAuthenticationService
        /// in use. Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized and Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// are among likely status results.
        /// </remarks>
        [NonAction]
        public virtual ChallengeResult Challenge(params string[] authenticationSchemes)
        {
            return new ChallengeResult(authenticationSchemes);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ChallengeResult with the specified properties.
        /// </summary>
        /// <param name="properties">Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform the authentication challenge.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ChallengeResult for the response.</returns>
        /// <remarks>
        /// The behavior of this method depends on the Microsoft.AspNetCore.Authentication.IAuthenticationService
        /// in use. Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized and Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// are among likely status results.
        /// </remarks>
        [NonAction]
        public virtual ChallengeResult Challenge(AuthenticationProperties properties)
        {
            return new ChallengeResult(properties);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ChallengeResult with the specified authentication
        /// schemes and properties.
        /// </summary>
        /// <param name="properties">Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform the authentication challenge.</param>
        /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ChallengeResult for the response.</returns>
        /// <remarks>
        /// The behavior of this method depends on the Microsoft.AspNetCore.Authentication.IAuthenticationService
        /// in use. Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized and Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// are among likely status results.</remarks>
        [NonAction]
        public virtual ChallengeResult Challenge(AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return new ChallengeResult(authenticationSchemes, properties);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ForbidResult (Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden by default).
        /// </summary>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ForbidResult for the response.</returns>
        /// <remarks>
        /// Some authentication schemes, such as cookies, will convert Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// to a redirect to show a login page.
        /// </remarks>
        [NonAction]
        public virtual ForbidResult Forbid()
        {
            return new ForbidResult();
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ForbidResult (Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// by default) with the specified authentication schemes.
        /// </summary>
        /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ForbidResult for the response.</returns>
        /// <remarks>
        /// Some authentication schemes, such as cookies, will convert Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// to a redirect to show a login page.
        /// </remarks>
        [NonAction]
        public virtual ForbidResult Forbid(params string[] authenticationSchemes)
        {
            return new ForbidResult(authenticationSchemes);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ForbidResult (Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// by default) with the specified properties.
        /// </summary>
        /// <param name="properties">Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform the authentication challenge.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ForbidResult for the response.</returns>
        /// <remarks>
        /// Some authentication schemes, such as cookies, will convert Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// to a redirect to show a login page.
        /// </remarks>
        [NonAction]
        public virtual ForbidResult Forbid(AuthenticationProperties properties)
        {
            return new ForbidResult(properties);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.ForbidResult (Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// by default) with the specified authentication schemes and properties.
        /// </summary>
        /// <param name="properties">Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform the authentication challenge.</param>
        /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.ForbidResult for the response.</returns>
        /// <remarks>
        /// Some authentication schemes, such as cookies, will convert Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        /// to a redirect to show a login page.
        /// </remarks>
        [NonAction]
        public virtual ForbidResult Forbid(AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return new ForbidResult(authenticationSchemes, properties);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.SignInResult with the specified authentication scheme.
        /// </summary>
        /// <param name="principal">The System.Security.Claims.ClaimsPrincipal containing the user claims.</param>
        /// <param name="authenticationScheme">The authentication scheme to use for the sign-in operation.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.SignInResult for the response.</returns>
        [NonAction]
        public virtual SignInResult SignIn(ClaimsPrincipal principal, string authenticationScheme)
        {
            return new SignInResult(authenticationScheme, principal);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.SignInResult with the specified authentication
        /// scheme and properties.
        /// </summary>
        /// <param name="principal">The System.Security.Claims.ClaimsPrincipal containing the user claims.</param>
        /// <param name="properties">Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform the sign-in operation.</param>
        /// <param name="authenticationScheme">The authentication scheme to use for the sign-in operation.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.SignInResult for the response.</returns>
        [NonAction]
        public virtual SignInResult SignIn(ClaimsPrincipal principal, AuthenticationProperties properties, string authenticationScheme)
        {
            return new SignInResult(authenticationScheme, principal, properties);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.SignOutResult with the specified authentication schemes.
        /// </summary>
        /// <param name="authenticationSchemes">The authentication schemes to use for the sign-out operation.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.SignOutResult for the response.</returns>
        [NonAction]
        public virtual SignOutResult SignOut(params string[] authenticationSchemes)
        {
            return new SignOutResult(authenticationSchemes);
        }

        /// <summary>
        /// Creates a Microsoft.AspNetCore.Mvc.SignOutResult with the specified authentication
        /// schemes and properties.
        /// </summary>
        /// <param name="properties">Microsoft.AspNetCore.Authentication.AuthenticationProperties used to perform the sign-out operation.</param>
        /// <param name="authenticationSchemes">The authentication scheme to use for the sign-out operation.</param>
        /// <returns>The created Microsoft.AspNetCore.Mvc.SignOutResult for the response.</returns>
        [NonAction]
        public virtual SignOutResult SignOut(AuthenticationProperties properties, params string[] authenticationSchemes)
        {
            return new SignOutResult(authenticationSchemes, properties);
        }

        /// <summary>
        /// Updates the specified model instance using values from the controller's current
        /// Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="model">The model instance to update.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion returns true if the update is successful.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual Task<bool> TryUpdateModelAsync<TModel>(TModel model) where TModel : class
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return TryUpdateModelAsync(model, string.Empty);
        }

        /// <summary>
        /// Updates the specified model instance using values from the controller's current
        /// Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider and a prefix.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="model">The model instance to update.</param>
        /// <param name="prefix">The prefix to use when looking up values in the current Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion returns true if the update is successful.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual async Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix) where TModel : class
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            return await TryUpdateModelAsync(model, prefix, await CompositeValueProvider.CreateAsync(ControllerContext));
        }

        /// <summary>
        /// Updates the specified model instance using the valueProvider and a prefix.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="model">The model instance to update.</param>
        /// <param name="prefix">The prefix to use when looking up values in the valueProvider.</param>
        /// <param name="valueProvider">The Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider used for looking up values.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion returns true if the update is successful.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, IValueProvider valueProvider) where TModel : class
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            if (valueProvider == null)
                throw new ArgumentNullException(nameof(valueProvider));

            return ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator);
        }

        /// <summary>
        /// Updates the specified model instance using values from the controller's current
        /// Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider and a prefix.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="model">The model instance to update.</param>
        /// <param name="prefix">The prefix to use when looking up values in the current Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.</param>
        /// <param name="includeExpressions">System.Linq.Expressions.Expression(s) which represent top-level properties which need to be included for the current model.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion returns true if the update is successful.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public async Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, params Expression<Func<TModel, object>>[] includeExpressions) where TModel : class
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (includeExpressions == null)
                throw new ArgumentNullException(nameof(includeExpressions));

            CompositeValueProvider valueProvider = await CompositeValueProvider.CreateAsync(ControllerContext);
            return await ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, includeExpressions);
        }

        /// <summary>
        /// Updates the specified model instance using values from the controller's current
        /// Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider and a prefix.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="model">The model instance to update.</param>
        /// <param name="prefix">The prefix to use when looking up values in the current Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.</param>
        /// <param name="propertyFilter">A predicate which can be used to filter properties at runtime.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion returns true if the update is successful.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public async Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, Func<ModelMetadata, bool> propertyFilter) where TModel : class
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (propertyFilter == null)
                throw new ArgumentNullException(nameof(propertyFilter));

            CompositeValueProvider valueProvider = await CompositeValueProvider.CreateAsync(ControllerContext);
            return await ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, propertyFilter);
        }

        /// <summary>
        /// Updates the specified model instance using the valueProvider and a prefix.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="model">The model instance to update.</param>
        /// <param name="prefix">The prefix to use when looking up values in the valueProvider.</param>
        /// <param name="valueProvider">The Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider used for looking up values.</param>
        /// <param name="includeExpressions">System.Linq.Expressions.Expression(s) which represent top-level properties which need to be included for the current model.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion returns true if the update is successful.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, IValueProvider valueProvider, params Expression<Func<TModel, object>>[] includeExpressions) where TModel : class
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (valueProvider == null)
                throw new ArgumentNullException(nameof(valueProvider));

            if (includeExpressions == null)
                throw new ArgumentNullException(nameof(includeExpressions));

            return ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, includeExpressions);
        }

        /// <summary>
        /// Updates the specified model instance using the valueProvider and a prefix.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="model">The model instance to update.</param>
        /// <param name="prefix">The prefix to use when looking up values in the valueProvider.</param>
        /// <param name="valueProvider">The Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider used for looking up values.</param>
        /// <param name="propertyFilter">A predicate which can be used to filter properties at runtime.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion returns true if the update is successful.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, IValueProvider valueProvider, Func<ModelMetadata, bool> propertyFilter) where TModel : class
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (valueProvider == null)
                throw new ArgumentNullException(nameof(valueProvider));

            if (propertyFilter == null)
                throw new ArgumentNullException(nameof(propertyFilter));

            return ModelBindingHelper.TryUpdateModelAsync(model, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, propertyFilter);
        }

        /// <summary>
        /// Updates the specified model instance using values from the controller's current
        /// Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider and a prefix.
        /// </summary>
        /// <param name="model">The model instance to update.</param>
        /// <param name="modelType">The type of model instance to update.</param>
        /// <param name="prefix">The prefix to use when looking up values in the current Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion returns true if the update is successful.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual async Task<bool> TryUpdateModelAsync(object model, Type modelType, string prefix)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (modelType == null)
                throw new ArgumentNullException(nameof(modelType));

            CompositeValueProvider valueProvider = await CompositeValueProvider.CreateAsync(ControllerContext);
            return await ModelBindingHelper.TryUpdateModelAsync(model, modelType, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator);
        }

        /// <summary>
        /// Updates the specified model instance using the valueProvider and a prefix.
        /// </summary>
        /// <param name="model">The model instance to update.</param>
        /// <param name="modelType">The type of model instance to update.</param>
        /// <param name="prefix">The prefix to use when looking up values in the valueProvider.</param>
        /// <param name="valueProvider">The Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider used for looking up values.</param>
        /// <param name="propertyFilter">A predicate which can be used to filter properties at runtime.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion returns true if the update is successful.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public Task<bool> TryUpdateModelAsync(object model, Type modelType, string prefix, IValueProvider valueProvider, Func<ModelMetadata, bool> propertyFilter)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (modelType == null)
                throw new ArgumentNullException(nameof(modelType));

            if (valueProvider == null)
                throw new ArgumentNullException(nameof(valueProvider));

            if (propertyFilter == null)
                throw new ArgumentNullException(nameof(propertyFilter));

            return ModelBindingHelper.TryUpdateModelAsync(model, modelType, prefix, ControllerContext, MetadataProvider, ModelBinderFactory, valueProvider, ObjectValidator, propertyFilter);
        }

        /// <summary>
        /// Validates the specified model instance.
        /// </summary>
        /// <param name="model">The model to validate.</param>
        /// <returns>true if the Microsoft.AspNetCore.Mvc.ControllerBase.ModelState is valid; false otherwise.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual bool TryValidateModel(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return TryValidateModel(model, null);
        }

        /// <summary>
        /// Validates the specified model instance.
        /// </summary>
        /// <param name="model">The model to validate.</param>
        /// <param name="prefix">The key to use when looking up information in Microsoft.AspNetCore.Mvc.ControllerBase.ModelState.</param>
        /// <returns>true if the Microsoft.AspNetCore.Mvc.ControllerBase.ModelState is valid;false otherwise.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NonAction]
        public virtual bool TryValidateModel(object model, string prefix)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            ObjectValidator.Validate(ControllerContext, null!, prefix ?? string.Empty, model);
            return ModelState.IsValid;
        }
    }
}
