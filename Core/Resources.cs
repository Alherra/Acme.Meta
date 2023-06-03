using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.Core
{
    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager = new("Microsoft.AspNetCore.Mvc.Core.Resources", typeof(Resources).GetTypeInfo().Assembly);

        //
        // 摘要:
        //     The argument '{0}' is invalid. Media types which match all types or match all
        //     subtypes are not supported.
        internal static string MatchAllContentTypeIsNotAllowed => GetString("MatchAllContentTypeIsNotAllowed");

        //
        // 摘要:
        //     The content-type '{0}' added in the '{1}' property is invalid. Media types which
        //     match all types or match all subtypes are not supported.
        internal static string ObjectResult_MatchAllContentType => GetString("ObjectResult_MatchAllContentType");

        //
        // 摘要:
        //     The method '{0}' on type '{1}' returned an instance of '{2}'. Make sure to call
        //     Unwrap on the returned value to avoid unobserved faulted Task.
        internal static string ActionExecutor_WrappedTaskInstance => GetString("ActionExecutor_WrappedTaskInstance");

        //
        // 摘要:
        //     The method '{0}' on type '{1}' returned a Task instance even though it is not
        //     an asynchronous method.
        internal static string ActionExecutor_UnexpectedTaskInstance => GetString("ActionExecutor_UnexpectedTaskInstance");

        //
        // 摘要:
        //     An action invoker could not be created for action '{0}'.
        internal static string ActionInvokerFactory_CouldNotCreateInvoker => GetString("ActionInvokerFactory_CouldNotCreateInvoker");

        //
        // 摘要:
        //     The action descriptor must be of type '{0}'.
        internal static string ActionDescriptorMustBeBasedOnControllerAction => GetString("ActionDescriptorMustBeBasedOnControllerAction");

        //
        // 摘要:
        //     Value cannot be null or empty.
        internal static string ArgumentCannotBeNullOrEmpty => GetString("ArgumentCannotBeNullOrEmpty");

        //
        // 摘要:
        //     The '{0}' property of '{1}' must not be null.
        internal static string PropertyOfTypeCannotBeNull => GetString("PropertyOfTypeCannotBeNull");

        //
        // 摘要:
        //     The '{0}' method of type '{1}' cannot return a null value.
        internal static string TypeMethodMustReturnNotNullValue => GetString("TypeMethodMustReturnNotNullValue");

        //
        // 摘要:
        //     The value '{0}' is invalid.
        internal static string ModelBinding_NullValueNotValid => GetString("ModelBinding_NullValueNotValid");

        //
        // 摘要:
        //     The passed expression of expression node type '{0}' is invalid. Only simple member
        //     access expressions for model properties are supported.
        internal static string Invalid_IncludePropertyExpression => GetString("Invalid_IncludePropertyExpression");

        //
        // 摘要:
        //     No route matches the supplied values.
        internal static string NoRoutesMatched => GetString("NoRoutesMatched");

        //
        // 摘要:
        //     If an {0} provides a result value by setting the {1} property of {2} to a non-null
        //     value, then it cannot call the next filter by invoking {3}.
        internal static string AsyncActionFilter_InvalidShortCircuit => GetString("AsyncActionFilter_InvalidShortCircuit");

        //
        // 摘要:
        //     If an {0} cancels execution by setting the {1} property of {2} to 'true', then
        //     it cannot call the next filter by invoking {3}.
        internal static string AsyncResultFilter_InvalidShortCircuit => GetString("AsyncResultFilter_InvalidShortCircuit");

        //
        // 摘要:
        //     The type provided to '{0}' must implement '{1}'.
        internal static string FilterFactoryAttribute_TypeMustImplementIFilter => GetString("FilterFactoryAttribute_TypeMustImplementIFilter");

        //
        // 摘要:
        //     Cannot return null from an action method with a return type of '{0}'.
        internal static string ActionResult_ActionReturnValueCannotBeNull => GetString("ActionResult_ActionReturnValueCannotBeNull");

        //
        // 摘要:
        //     The type '{0}' must derive from '{1}'.
        internal static string TypeMustDeriveFromType => GetString("TypeMustDeriveFromType");

        //
        // 摘要:
        //     No encoding found for input formatter '{0}'. There must be at least one supported
        //     encoding registered in order for the formatter to read content.
        internal static string InputFormatterNoEncoding => GetString("InputFormatterNoEncoding");

        //
        // 摘要:
        //     Unsupported content type '{0}'.
        internal static string UnsupportedContentType => GetString("UnsupportedContentType");

        //
        // 摘要:
        //     No supported media type registered for output formatter '{0}'. There must be
        //     at least one supported media type registered in order for the output formatter
        //     to write content.
        internal static string OutputFormatterNoMediaType => GetString("OutputFormatterNoMediaType");

        //
        // 摘要:
        //     The following errors occurred with attribute routing information:{0}{0}{1}
        internal static string AttributeRoute_AggregateErrorMessage => GetString("AttributeRoute_AggregateErrorMessage");

        //
        // 摘要:
        //     The attribute route '{0}' cannot contain a parameter named '{{{1}}}'. Use '[{1}]'
        //     in the route template to insert the value '{2}'.
        internal static string AttributeRoute_CannotContainParameter => GetString("AttributeRoute_CannotContainParameter");

        //
        // 摘要:
        //     For action: '{0}'{1}Error: {2}
        internal static string AttributeRoute_IndividualErrorMessage => GetString("AttributeRoute_IndividualErrorMessage");

        //
        // 摘要:
        //     An empty replacement token ('[]') is not allowed.
        internal static string AttributeRoute_TokenReplacement_EmptyTokenNotAllowed => GetString("AttributeRoute_TokenReplacement_EmptyTokenNotAllowed");

        //
        // 摘要:
        //     Token delimiters ('[', ']') are imbalanced.
        internal static string AttributeRoute_TokenReplacement_ImbalancedSquareBrackets => GetString("AttributeRoute_TokenReplacement_ImbalancedSquareBrackets");

        //
        // 摘要:
        //     The route template '{0}' has invalid syntax. {1}
        internal static string AttributeRoute_TokenReplacement_InvalidSyntax => GetString("AttributeRoute_TokenReplacement_InvalidSyntax");

        //
        // 摘要:
        //     While processing template '{0}', a replacement value for the token '{1}' could
        //     not be found. Available tokens: '{2}'. To use a '[' or ']' as a literal string
        //     in a route or within a constraint, use '[[' or ']]' instead.
        internal static string AttributeRoute_TokenReplacement_ReplacementValueNotFound => GetString("AttributeRoute_TokenReplacement_ReplacementValueNotFound");

        //
        // 摘要:
        //     A replacement token is not closed.
        internal static string AttributeRoute_TokenReplacement_UnclosedToken => GetString("AttributeRoute_TokenReplacement_UnclosedToken");

        //
        // 摘要:
        //     An unescaped '[' token is not allowed inside of a replacement token. Use '[['
        //     to escape.
        internal static string AttributeRoute_TokenReplacement_UnescapedBraceInToken => GetString("AttributeRoute_TokenReplacement_UnescapedBraceInToken");

        //
        // 摘要:
        //     Unable to find the required services. Please add all the required services by
        //     calling '{0}.{1}' inside the call to '{2}' in the application startup code.
        internal static string UnableToFindServices => GetString("UnableToFindServices");

        //
        // 摘要:
        //     Action: '{0}' - Template: '{1}'
        internal static string AttributeRoute_DuplicateNames_Item => GetString("AttributeRoute_DuplicateNames_Item");

        //
        // 摘要:
        //     Attribute routes with the same name '{0}' must have the same template:{1}{2}
        internal static string AttributeRoute_DuplicateNames => GetString("AttributeRoute_DuplicateNames");

        //
        // 摘要:
        //     Error {0}:{1}{2}
        internal static string AttributeRoute_AggregateErrorMessage_ErrorNumber => GetString("AttributeRoute_AggregateErrorMessage_ErrorNumber");

        //
        // 摘要:
        //     A method '{0}' must not define attribute routed actions and non attribute routed
        //     actions at the same time:{1}{2}{1}{1}Use 'AcceptVerbsAttribute' to create a single
        //     route that allows multiple HTTP verbs and defines a route, or set a route template
        //     in all attributes that constrain HTTP verbs.
        internal static string AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod => GetString("AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod");

        //
        // 摘要:
        //     Action: '{0}' - Route Template: '{1}' - HTTP Verbs: '{2}'
        internal static string AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod_Item => GetString("AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod_Item");

        //
        // 摘要:
        //     (none)
        internal static string AttributeRoute_NullTemplateRepresentation => GetString("AttributeRoute_NullTemplateRepresentation");

        //
        // 摘要:
        //     Multiple actions matched. The following actions matched route data and had all
        //     constraints satisfied:{0}{0}{1}
        internal static string DefaultActionSelector_AmbiguousActions => GetString("DefaultActionSelector_AmbiguousActions");

        //
        // 摘要:
        //     Could not find file: {0}
        internal static string FileResult_InvalidPath => GetString("FileResult_InvalidPath");

        //
        // 摘要:
        //     The input was not valid.
        internal static string SerializableError_DefaultError => GetString("SerializableError_DefaultError");

        //
        // 摘要:
        //     If an {0} provides a result value by setting the {1} property of {2} to a non-null
        //     value, then it cannot call the next filter by invoking {3}.
        internal static string AsyncResourceFilter_InvalidShortCircuit => GetString("AsyncResourceFilter_InvalidShortCircuit");

        //
        // 摘要:
        //     If the '{0}' property is not set to true, '{1}' property must be specified.
        internal static string ResponseCache_SpecifyDuration => GetString("ResponseCache_SpecifyDuration");

        //
        // 摘要:
        //     The action '{0}' has ApiExplorer enabled, but is using conventional routing.
        //     Only actions which use attribute routing support ApiExplorer.
        internal static string ApiExplorer_UnsupportedAction => GetString("ApiExplorer_UnsupportedAction");

        //
        // 摘要:
        //     The media type "{0}" is not valid. MediaTypes containing wildcards (*) are not
        //     allowed in formatter mappings.
        internal static string FormatterMappings_NotValidMediaType => GetString("FormatterMappings_NotValidMediaType");

        //
        // 摘要:
        //     The format provided is invalid '{0}'. A format must be a non-empty file-extension,
        //     optionally prefixed with a '.' character.
        internal static string Format_NotValid => GetString("Format_NotValid");

        //
        // 摘要:
        //     The '{0}' cache profile is not defined.
        internal static string CacheProfileNotFound => GetString("CacheProfileNotFound");

        //
        // 摘要:
        //     The model's runtime type '{0}' is not assignable to the type '{1}'.
        internal static string ModelType_WrongType => GetString("ModelType_WrongType");

        //
        // 摘要:
        //     The type '{0}' cannot be activated by '{1}' because it is either a value type,
        //     an interface, an abstract class or an open generic type.
        internal static string ValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated => GetString("ValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated");

        //
        // 摘要:
        //     The type '{0}' must implement '{1}' to be used as a model binder.
        internal static string BinderType_MustBeIModelBinder => GetString("BinderType_MustBeIModelBinder");

        //
        // 摘要:
        //     The provided binding source '{0}' is a composite. '{1}' requires that the source
        //     must represent a single type of input.
        internal static string BindingSource_CannotBeComposite => GetString("BindingSource_CannotBeComposite");

        //
        // 摘要:
        //     The provided binding source '{0}' is a greedy data source. '{1}' does not support
        //     greedy data sources.
        internal static string BindingSource_CannotBeGreedy => GetString("BindingSource_CannotBeGreedy");

        //
        // 摘要:
        //     The property {0}.{1} could not be found.
        internal static string Common_PropertyNotFound => GetString("Common_PropertyNotFound");

        //
        // 摘要:
        //     The key '{0}' is invalid JQuery syntax because it is missing a closing bracket.
        internal static string JQueryFormValueProviderFactory_MissingClosingBracket => GetString("JQueryFormValueProviderFactory_MissingClosingBracket");

        //
        // 摘要:
        //     A value is required.
        internal static string KeyValuePair_BothKeyAndValueMustBePresent => GetString("KeyValuePair_BothKeyAndValueMustBePresent");

        //
        // 摘要:
        //     The binding context has a null Model, but this binder requires a non-null model
        //     of type '{0}'.
        internal static string ModelBinderUtil_ModelCannotBeNull => GetString("ModelBinderUtil_ModelCannotBeNull");

        //
        // 摘要:
        //     The binding context has a Model of type '{0}', but this binder can only operate
        //     on models of type '{1}'.
        internal static string ModelBinderUtil_ModelInstanceIsWrong => GetString("ModelBinderUtil_ModelInstanceIsWrong");

        //
        // 摘要:
        //     The binding context cannot have a null ModelMetadata.
        internal static string ModelBinderUtil_ModelMetadataCannotBeNull => GetString("ModelBinderUtil_ModelMetadataCannotBeNull");

        //
        // 摘要:
        //     A value for the '{0}' parameter or property was not provided.
        internal static string ModelBinding_MissingBindRequiredMember => GetString("ModelBinding_MissingBindRequiredMember");

        //
        // 摘要:
        //     A non-empty request body is required.
        internal static string ModelBinding_MissingRequestBodyRequiredMember => GetString("ModelBinding_MissingRequestBodyRequiredMember");

        //
        // 摘要:
        //     The parameter conversion from type '{0}' to type '{1}' failed because no type
        //     converter can convert between these types.
        internal static string ValueProviderResult_NoConverterExists => GetString("ValueProviderResult_NoConverterExists");

        //
        // 摘要:
        //     Path '{0}' was not rooted.
        internal static string FileResult_PathNotRooted => GetString("FileResult_PathNotRooted");

        //
        // 摘要:
        //     The supplied URL is not local. A URL with an absolute path is considered local
        //     if it does not have a host/authority part. URLs using virtual paths ('~/') are
        //     also local.
        internal static string UrlNotLocal => GetString("UrlNotLocal");

        //
        // 摘要:
        //     The argument '{0}' is invalid. Empty or null formats are not supported.
        internal static string FormatFormatterMappings_GetMediaTypeMappingForFormat_InvalidFormat => GetString("FormatFormatterMappings_GetMediaTypeMappingForFormat_InvalidFormat");

        //
        // 摘要:
        //     "Invalid values '{0}'."
        internal static string AcceptHeaderParser_ParseAcceptHeader_InvalidValues => GetString("AcceptHeaderParser_ParseAcceptHeader_InvalidValues");

        //
        // 摘要:
        //     The value '{0}' is not valid for {1}.
        internal static string ModelState_AttemptedValueIsInvalid => GetString("ModelState_AttemptedValueIsInvalid");

        //
        // 摘要:
        //     The value '{0}' is not valid.
        internal static string ModelState_NonPropertyAttemptedValueIsInvalid => GetString("ModelState_NonPropertyAttemptedValueIsInvalid");

        //
        // 摘要:
        //     The supplied value is invalid for {0}.
        internal static string ModelState_UnknownValueIsInvalid => GetString("ModelState_UnknownValueIsInvalid");

        //
        // 摘要:
        //     The supplied value is invalid.
        internal static string ModelState_NonPropertyUnknownValueIsInvalid => GetString("ModelState_NonPropertyUnknownValueIsInvalid");

        //
        // 摘要:
        //     The value '{0}' is invalid.
        internal static string HtmlGeneration_ValueIsInvalid => GetString("HtmlGeneration_ValueIsInvalid");

        //
        // 摘要:
        //     The field {0} must be a number.
        internal static string HtmlGeneration_ValueMustBeNumber => GetString("HtmlGeneration_ValueMustBeNumber");

        //
        // 摘要:
        //     The field must be a number.
        internal static string HtmlGeneration_NonPropertyValueMustBeNumber => GetString("HtmlGeneration_NonPropertyValueMustBeNumber");

        //
        // 摘要:
        //     The list of '{0}' must not be empty. Add at least one supported encoding.
        internal static string TextInputFormatter_SupportedEncodingsMustNotBeEmpty => GetString("TextInputFormatter_SupportedEncodingsMustNotBeEmpty");

        //
        // 摘要:
        //     The list of '{0}' must not be empty. Add at least one supported encoding.
        internal static string TextOutputFormatter_SupportedEncodingsMustNotBeEmpty => GetString("TextOutputFormatter_SupportedEncodingsMustNotBeEmpty");

        //
        // 摘要:
        //     '{0}' is not supported by '{1}'. Use '{2}' instead.
        internal static string TextOutputFormatter_WriteResponseBodyAsyncNotSupported => GetString("TextOutputFormatter_WriteResponseBodyAsyncNotSupported");

        //
        // 摘要:
        //     No media types found in '{0}.{1}'. Add at least one media type to the list of
        //     supported media types.
        internal static string Formatter_NoMediaTypes => GetString("Formatter_NoMediaTypes");

        //
        // 摘要:
        //     Could not create a model binder for model object of type '{0}'.
        internal static string CouldNotCreateIModelBinder => GetString("CouldNotCreateIModelBinder");

        //
        // 摘要:
        //     '{0}.{1}' must not be empty. At least one '{2}' is required to bind from the
        //     body.
        internal static string InputFormattersAreRequired => GetString("InputFormattersAreRequired");

        //
        // 摘要:
        //     '{0}.{1}' must not be empty. At least one '{2}' is required to model bind.
        internal static string ModelBinderProvidersAreRequired => GetString("ModelBinderProvidersAreRequired");

        //
        // 摘要:
        //     '{0}.{1}' must not be empty. At least one '{2}' is required to format a response.
        internal static string OutputFormattersAreRequired => GetString("OutputFormattersAreRequired");

        //
        // 摘要:
        //     Multiple overloads of method '{0}' are not supported.
        internal static string MiddewareFilter_ConfigureMethodOverload => GetString("MiddewareFilter_ConfigureMethodOverload");

        //
        // 摘要:
        //     A public method named '{0}' could not be found in the '{1}' type.
        internal static string MiddewareFilter_NoConfigureMethod => GetString("MiddewareFilter_NoConfigureMethod");

        //
        // 摘要:
        //     Could not find '{0}' in the feature list.
        internal static string MiddlewareFilterBuilder_NoMiddlewareFeature => GetString("MiddlewareFilterBuilder_NoMiddlewareFeature");

        //
        // 摘要:
        //     The '{0}' property cannot be null.
        internal static string MiddlewareFilterBuilder_NullApplicationBuilder => GetString("MiddlewareFilterBuilder_NullApplicationBuilder");

        //
        // 摘要:
        //     The '{0}' method in the type '{1}' must have a return type of '{2}'.
        internal static string MiddlewareFilter_InvalidConfigureReturnType => GetString("MiddlewareFilter_InvalidConfigureReturnType");

        //
        // 摘要:
        //     Could not resolve a service of type '{0}' for the parameter '{1}' of method '{2}'
        //     on type '{3}'.
        internal static string MiddlewareFilter_ServiceResolutionFail => GetString("MiddlewareFilter_ServiceResolutionFail");

        //
        // 摘要:
        //     An {0} cannot be created without a valid instance of {1}.
        internal static string AuthorizeFilter_AuthorizationPolicyCannotBeCreated => GetString("AuthorizeFilter_AuthorizationPolicyCannotBeCreated");

        //
        // 摘要:
        //     The '{0}' cannot bind to a model of type '{1}'. Change the model type to '{2}'
        //     instead.
        internal static string FormCollectionModelBinder_CannotBindToFormCollection => GetString("FormCollectionModelBinder_CannotBindToFormCollection");

        //
        // 摘要:
        //     '{0}' requires the response cache middleware.
        internal static string VaryByQueryKeys_Requires_ResponseCachingMiddleware => GetString("VaryByQueryKeys_Requires_ResponseCachingMiddleware");

        //
        // 摘要:
        //     A duplicate entry for library reference {0} was found. Please check that all
        //     package references in all projects use the same casing for the same package references.
        internal static string CandidateResolver_DifferentCasedReference => GetString("CandidateResolver_DifferentCasedReference");

        //
        // 摘要:
        //     Unable to create an instance of type '{0}'. The type specified in {1} must not
        //     be abstract and must have a parameterless constructor.
        internal static string MiddlewareFilterConfigurationProvider_CreateConfigureDelegate_CannotCreateType => GetString("MiddlewareFilterConfigurationProvider_CreateConfigureDelegate_CannotCreateType");

        //
        // 摘要:
        //     '{0}' and '{1}' are out of bounds for the string.
        internal static string Argument_InvalidOffsetLength => GetString("Argument_InvalidOffsetLength");

        //
        // 摘要:
        //     Could not create an instance of type '{0}'. Model bound complex types must not
        //     be abstract or value types and must have a parameterless constructor.
        internal static string ComplexTypeModelBinder_NoParameterlessConstructor_ForType => GetString("ComplexTypeModelBinder_NoParameterlessConstructor_ForType");

        //
        // 摘要:
        //     Could not create an instance of type '{0}'. Model bound complex types must not
        //     be abstract or value types and must have a parameterless constructor. Alternatively,
        //     set the '{1}' property to a non-null value in the '{2}' constructor.
        internal static string ComplexTypeModelBinder_NoParameterlessConstructor_ForProperty => GetString("ComplexTypeModelBinder_NoParameterlessConstructor_ForProperty");

        //
        // 摘要:
        //     No page named '{0}' matches the supplied values.
        internal static string NoRoutesMatchedForPage => GetString("NoRoutesMatchedForPage");

        //
        // 摘要:
        //     The relative page path '{0}' can only be used while executing a Razor Page. Specify
        //     a root relative path with a leading '/' to generate a URL outside of a Razor
        //     Page. If you are using {1} then you must provide the current {2} to use relative
        //     pages.
        internal static string UrlHelper_RelativePagePathIsNotSupported => GetString("UrlHelper_RelativePagePathIsNotSupported");

        //
        // 摘要:
        //     One or more validation errors occurred.
        internal static string ValidationProblemDescription_Title => GetString("ValidationProblemDescription_Title");

        //
        // 摘要:
        //     Action '{0}' does not have an attribute route. Action methods on controllers
        //     annotated with {1} must be attribute routed.
        internal static string ApiController_AttributeRouteRequired => GetString("ApiController_AttributeRouteRequired");

        //
        // 摘要:
        //     No file provider has been configured to process the supplied file.
        internal static string VirtualFileResultExecutor_NoFileProviderConfigured => GetString("VirtualFileResultExecutor_NoFileProviderConfigured");

        //
        // 摘要:
        //     Type {0} specified by {1} is invalid. Type specified by {1} must derive from
        //     {2}.
        internal static string ApplicationPartFactory_InvalidFactoryType => GetString("ApplicationPartFactory_InvalidFactoryType");

        //
        // 摘要:
        //     {0} specified on {1} cannot be self referential.
        internal static string RelatedAssemblyAttribute_AssemblyCannotReferenceSelf => GetString("RelatedAssemblyAttribute_AssemblyCannotReferenceSelf");

        //
        // 摘要:
        //     Related assembly '{0}' specified by assembly '{1}' could not be found in the
        //     directory {2}. Related assemblies must be co-located with the specifying assemblies.
        internal static string RelatedAssemblyAttribute_CouldNotBeFound => GetString("RelatedAssemblyAttribute_CouldNotBeFound");

        //
        // 摘要:
        //     Each related assembly must be declared by exactly one assembly. The assembly
        //     '{0}' was declared as related assembly by the following:
        internal static string ApplicationAssembliesProvider_DuplicateRelatedAssembly => GetString("ApplicationAssembliesProvider_DuplicateRelatedAssembly");

        //
        // 摘要:
        //     Assembly '{0}' declared as a related assembly by assembly '{1}' cannot define
        //     additional related assemblies.
        internal static string ApplicationAssembliesProvider_RelatedAssemblyCannotDefineAdditional => GetString("ApplicationAssembliesProvider_RelatedAssemblyCannotDefineAdditional");

        //
        // 摘要:
        //     Could not create an instance of type '{0}'. Model bound complex types must not
        //     be abstract or value types and must have a parameterless constructor. Alternatively,
        //     give the '{1}' parameter a non-null default value.
        internal static string ComplexTypeModelBinder_NoParameterlessConstructor_ForParameter => GetString("ComplexTypeModelBinder_NoParameterlessConstructor_ForParameter");

        //
        // 摘要:
        //     Action '{0}' has more than one parameter that was specified or inferred as bound
        //     from request body. Only one parameter per action may be bound from body. Inspect
        //     the following parameters, and use '{1}' to specify bound from query, '{2}' to
        //     specify bound from route, and '{3}' for parameters to be bound from body:
        internal static string ApiController_MultipleBodyParametersFound => GetString("ApiController_MultipleBodyParametersFound");

        //
        // 摘要:
        //     API convention type '{0}' must be a static type.
        internal static string ApiConventionMustBeStatic => GetString("ApiConventionMustBeStatic");

        //
        // 摘要:
        //     Invalid type parameter '{0}' specified for '{1}'.
        internal static string InvalidTypeTForActionResultOfT => GetString("InvalidTypeTForActionResultOfT");

        //
        // 摘要:
        //     Method {0} is decorated with the following attributes that are not allowed on
        //     an API convention method:{1}The following attributes are allowed on API convention
        //     methods: {2}.
        internal static string ApiConvention_UnsupportedAttributesOnConvention => GetString("ApiConvention_UnsupportedAttributesOnConvention");

        //
        // 摘要:
        //     Method name '{0}' is ambiguous for convention type '{1}'. More than one method
        //     found with the name '{0}'.
        internal static string ApiConventionMethod_AmbiguousMethodName => GetString("ApiConventionMethod_AmbiguousMethodName");

        //
        // 摘要:
        //     A method named '{0}' was not found on convention type '{1}'.
        internal static string ApiConventionMethod_NoMethodFound => GetString("ApiConventionMethod_NoMethodFound");

        //
        // 摘要:
        //     {0} exceeded the maximum configured validation depth '{1}' when validating type
        //     '{2}'.
        internal static string ValidationVisitor_ExceededMaxDepth => GetString("ValidationVisitor_ExceededMaxDepth");

        //
        // 摘要:
        //     This may indicate a very deep or infinitely recursive object graph. Consider
        //     modifying '{0}.{1}' or suppressing validation on the model type.
        internal static string ValidationVisitor_ExceededMaxDepthFix => GetString("ValidationVisitor_ExceededMaxDepthFix");

        //
        // 摘要:
        //     {0} exceeded the maximum configured validation depth '{1}' when validating property
        //     '{2}' on type '{3}'.
        internal static string ValidationVisitor_ExceededMaxPropertyDepth => GetString("ValidationVisitor_ExceededMaxPropertyDepth");

        //
        // 摘要:
        //     Bad Request
        internal static string ApiConventions_Title_400 => GetString("ApiConventions_Title_400");

        //
        // 摘要:
        //     Unauthorized
        internal static string ApiConventions_Title_401 => GetString("ApiConventions_Title_401");

        //
        // 摘要:
        //     Forbidden
        internal static string ApiConventions_Title_403 => GetString("ApiConventions_Title_403");

        //
        // 摘要:
        //     Not Found
        internal static string ApiConventions_Title_404 => GetString("ApiConventions_Title_404");

        //
        // 摘要:
        //     Not Acceptable
        internal static string ApiConventions_Title_406 => GetString("ApiConventions_Title_406");

        //
        // 摘要:
        //     Conflict
        internal static string ApiConventions_Title_409 => GetString("ApiConventions_Title_409");

        //
        // 摘要:
        //     Unsupported Media Type
        internal static string ApiConventions_Title_415 => GetString("ApiConventions_Title_415");

        //
        // 摘要:
        //     Unprocessable Entity
        internal static string ApiConventions_Title_422 => GetString("ApiConventions_Title_422");

        //
        // 摘要:
        //     The argument '{0}' is invalid. Media types which match all types or match all
        //     subtypes are not supported.
        internal static string FormatMatchAllContentTypeIsNotAllowed(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MatchAllContentTypeIsNotAllowed"), p0);
        }

        //
        // 摘要:
        //     The content-type '{0}' added in the '{1}' property is invalid. Media types which
        //     match all types or match all subtypes are not supported.
        internal static string FormatObjectResult_MatchAllContentType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ObjectResult_MatchAllContentType"), p0, p1);
        }

        //
        // 摘要:
        //     The method '{0}' on type '{1}' returned an instance of '{2}'. Make sure to call
        //     Unwrap on the returned value to avoid unobserved faulted Task.
        internal static string FormatActionExecutor_WrappedTaskInstance(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionExecutor_WrappedTaskInstance"), p0, p1, p2);
        }

        //
        // 摘要:
        //     The method '{0}' on type '{1}' returned a Task instance even though it is not
        //     an asynchronous method.
        internal static string FormatActionExecutor_UnexpectedTaskInstance(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionExecutor_UnexpectedTaskInstance"), p0, p1);
        }

        //
        // 摘要:
        //     An action invoker could not be created for action '{0}'.
        internal static string FormatActionInvokerFactory_CouldNotCreateInvoker(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionInvokerFactory_CouldNotCreateInvoker"), p0);
        }

        //
        // 摘要:
        //     The action descriptor must be of type '{0}'.
        internal static string FormatActionDescriptorMustBeBasedOnControllerAction(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionDescriptorMustBeBasedOnControllerAction"), p0);
        }

        //
        // 摘要:
        //     Value cannot be null or empty.
        internal static string FormatArgumentCannotBeNullOrEmpty()
        {
            return GetString("ArgumentCannotBeNullOrEmpty");
        }

        //
        // 摘要:
        //     The '{0}' property of '{1}' must not be null.
        internal static string FormatPropertyOfTypeCannotBeNull(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("PropertyOfTypeCannotBeNull"), p0, p1);
        }

        //
        // 摘要:
        //     The '{0}' method of type '{1}' cannot return a null value.
        internal static string FormatTypeMethodMustReturnNotNullValue(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TypeMethodMustReturnNotNullValue"), p0, p1);
        }

        //
        // 摘要:
        //     The value '{0}' is invalid.
        internal static string FormatModelBinding_NullValueNotValid(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinding_NullValueNotValid"), p0);
        }

        //
        // 摘要:
        //     The passed expression of expression node type '{0}' is invalid. Only simple member
        //     access expressions for model properties are supported.
        internal static string FormatInvalid_IncludePropertyExpression(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Invalid_IncludePropertyExpression"), p0);
        }

        //
        // 摘要:
        //     No route matches the supplied values.
        internal static string FormatNoRoutesMatched()
        {
            return GetString("NoRoutesMatched");
        }

        //
        // 摘要:
        //     If an {0} provides a result value by setting the {1} property of {2} to a non-null
        //     value, then it cannot call the next filter by invoking {3}.
        internal static string FormatAsyncActionFilter_InvalidShortCircuit(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AsyncActionFilter_InvalidShortCircuit"), p0, p1, p2, p3);
        }

        //
        // 摘要:
        //     If an {0} cancels execution by setting the {1} property of {2} to 'true', then
        //     it cannot call the next filter by invoking {3}.
        internal static string FormatAsyncResultFilter_InvalidShortCircuit(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AsyncResultFilter_InvalidShortCircuit"), p0, p1, p2, p3);
        }

        //
        // 摘要:
        //     The type provided to '{0}' must implement '{1}'.
        internal static string FormatFilterFactoryAttribute_TypeMustImplementIFilter(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FilterFactoryAttribute_TypeMustImplementIFilter"), p0, p1);
        }

        //
        // 摘要:
        //     Cannot return null from an action method with a return type of '{0}'.
        internal static string FormatActionResult_ActionReturnValueCannotBeNull(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionResult_ActionReturnValueCannotBeNull"), p0);
        }

        //
        // 摘要:
        //     The type '{0}' must derive from '{1}'.
        internal static string FormatTypeMustDeriveFromType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TypeMustDeriveFromType"), p0, p1);
        }

        //
        // 摘要:
        //     No encoding found for input formatter '{0}'. There must be at least one supported
        //     encoding registered in order for the formatter to read content.
        internal static string FormatInputFormatterNoEncoding(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InputFormatterNoEncoding"), p0);
        }

        //
        // 摘要:
        //     Unsupported content type '{0}'.
        internal static string FormatUnsupportedContentType(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UnsupportedContentType"), p0);
        }

        //
        // 摘要:
        //     No supported media type registered for output formatter '{0}'. There must be
        //     at least one supported media type registered in order for the output formatter
        //     to write content.
        internal static string FormatOutputFormatterNoMediaType(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("OutputFormatterNoMediaType"), p0);
        }

        //
        // 摘要:
        //     The following errors occurred with attribute routing information:{0}{0}{1}
        internal static string FormatAttributeRoute_AggregateErrorMessage(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_AggregateErrorMessage"), p0, p1);
        }

        //
        // 摘要:
        //     The attribute route '{0}' cannot contain a parameter named '{{{1}}}'. Use '[{1}]'
        //     in the route template to insert the value '{2}'.
        internal static string FormatAttributeRoute_CannotContainParameter(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_CannotContainParameter"), p0, p1, p2);
        }

        //
        // 摘要:
        //     For action: '{0}'{1}Error: {2}
        internal static string FormatAttributeRoute_IndividualErrorMessage(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_IndividualErrorMessage"), p0, p1, p2);
        }

        //
        // 摘要:
        //     An empty replacement token ('[]') is not allowed.
        internal static string FormatAttributeRoute_TokenReplacement_EmptyTokenNotAllowed()
        {
            return GetString("AttributeRoute_TokenReplacement_EmptyTokenNotAllowed");
        }

        //
        // 摘要:
        //     Token delimiters ('[', ']') are imbalanced.
        internal static string FormatAttributeRoute_TokenReplacement_ImbalancedSquareBrackets()
        {
            return GetString("AttributeRoute_TokenReplacement_ImbalancedSquareBrackets");
        }

        //
        // 摘要:
        //     The route template '{0}' has invalid syntax. {1}
        internal static string FormatAttributeRoute_TokenReplacement_InvalidSyntax(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_TokenReplacement_InvalidSyntax"), p0, p1);
        }

        //
        // 摘要:
        //     While processing template '{0}', a replacement value for the token '{1}' could
        //     not be found. Available tokens: '{2}'. To use a '[' or ']' as a literal string
        //     in a route or within a constraint, use '[[' or ']]' instead.
        internal static string FormatAttributeRoute_TokenReplacement_ReplacementValueNotFound(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_TokenReplacement_ReplacementValueNotFound"), p0, p1, p2);
        }

        //
        // 摘要:
        //     A replacement token is not closed.
        internal static string FormatAttributeRoute_TokenReplacement_UnclosedToken()
        {
            return GetString("AttributeRoute_TokenReplacement_UnclosedToken");
        }

        //
        // 摘要:
        //     An unescaped '[' token is not allowed inside of a replacement token. Use '[['
        //     to escape.
        internal static string FormatAttributeRoute_TokenReplacement_UnescapedBraceInToken()
        {
            return GetString("AttributeRoute_TokenReplacement_UnescapedBraceInToken");
        }

        //
        // 摘要:
        //     Unable to find the required services. Please add all the required services by
        //     calling '{0}.{1}' inside the call to '{2}' in the application startup code.
        internal static string FormatUnableToFindServices(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UnableToFindServices"), p0, p1, p2);
        }

        //
        // 摘要:
        //     Action: '{0}' - Template: '{1}'
        internal static string FormatAttributeRoute_DuplicateNames_Item(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_DuplicateNames_Item"), p0, p1);
        }

        //
        // 摘要:
        //     Attribute routes with the same name '{0}' must have the same template:{1}{2}
        internal static string FormatAttributeRoute_DuplicateNames(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_DuplicateNames"), p0, p1, p2);
        }

        //
        // 摘要:
        //     Error {0}:{1}{2}
        internal static string FormatAttributeRoute_AggregateErrorMessage_ErrorNumber(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_AggregateErrorMessage_ErrorNumber"), p0, p1, p2);
        }

        //
        // 摘要:
        //     A method '{0}' must not define attribute routed actions and non attribute routed
        //     actions at the same time:{1}{2}{1}{1}Use 'AcceptVerbsAttribute' to create a single
        //     route that allows multiple HTTP verbs and defines a route, or set a route template
        //     in all attributes that constrain HTTP verbs.
        internal static string FormatAttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod"), p0, p1, p2);
        }

        //
        // 摘要:
        //     Action: '{0}' - Route Template: '{1}' - HTTP Verbs: '{2}'
        internal static string FormatAttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod_Item(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod_Item"), p0, p1, p2);
        }

        //
        // 摘要:
        //     (none)
        internal static string FormatAttributeRoute_NullTemplateRepresentation()
        {
            return GetString("AttributeRoute_NullTemplateRepresentation");
        }

        //
        // 摘要:
        //     Multiple actions matched. The following actions matched route data and had all
        //     constraints satisfied:{0}{0}{1}
        internal static string FormatDefaultActionSelector_AmbiguousActions(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DefaultActionSelector_AmbiguousActions"), p0, p1);
        }

        //
        // 摘要:
        //     Could not find file: {0}
        internal static string FormatFileResult_InvalidPath(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FileResult_InvalidPath"), p0);
        }

        //
        // 摘要:
        //     The input was not valid.
        internal static string FormatSerializableError_DefaultError()
        {
            return GetString("SerializableError_DefaultError");
        }

        //
        // 摘要:
        //     If an {0} provides a result value by setting the {1} property of {2} to a non-null
        //     value, then it cannot call the next filter by invoking {3}.
        internal static string FormatAsyncResourceFilter_InvalidShortCircuit(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AsyncResourceFilter_InvalidShortCircuit"), p0, p1, p2, p3);
        }

        //
        // 摘要:
        //     If the '{0}' property is not set to true, '{1}' property must be specified.
        internal static string FormatResponseCache_SpecifyDuration(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ResponseCache_SpecifyDuration"), p0, p1);
        }

        //
        // 摘要:
        //     The action '{0}' has ApiExplorer enabled, but is using conventional routing.
        //     Only actions which use attribute routing support ApiExplorer.
        internal static string FormatApiExplorer_UnsupportedAction(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApiExplorer_UnsupportedAction"), p0);
        }

        //
        // 摘要:
        //     The media type "{0}" is not valid. MediaTypes containing wildcards (*) are not
        //     allowed in formatter mappings.
        internal static string FormatFormatterMappings_NotValidMediaType(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FormatterMappings_NotValidMediaType"), p0);
        }

        //
        // 摘要:
        //     The format provided is invalid '{0}'. A format must be a non-empty file-extension,
        //     optionally prefixed with a '.' character.
        internal static string FormatFormat_NotValid(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Format_NotValid"), p0);
        }

        //
        // 摘要:
        //     The '{0}' cache profile is not defined.
        internal static string FormatCacheProfileNotFound(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CacheProfileNotFound"), p0);
        }

        //
        // 摘要:
        //     The model's runtime type '{0}' is not assignable to the type '{1}'.
        internal static string FormatModelType_WrongType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelType_WrongType"), p0, p1);
        }

        //
        // 摘要:
        //     The type '{0}' cannot be activated by '{1}' because it is either a value type,
        //     an interface, an abstract class or an open generic type.
        internal static string FormatValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated"), p0, p1);
        }

        //
        // 摘要:
        //     The type '{0}' must implement '{1}' to be used as a model binder.
        internal static string FormatBinderType_MustBeIModelBinder(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("BinderType_MustBeIModelBinder"), p0, p1);
        }

        //
        // 摘要:
        //     The provided binding source '{0}' is a composite. '{1}' requires that the source
        //     must represent a single type of input.
        internal static string FormatBindingSource_CannotBeComposite(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("BindingSource_CannotBeComposite"), p0, p1);
        }

        //
        // 摘要:
        //     The provided binding source '{0}' is a greedy data source. '{1}' does not support
        //     greedy data sources.
        internal static string FormatBindingSource_CannotBeGreedy(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("BindingSource_CannotBeGreedy"), p0, p1);
        }

        //
        // 摘要:
        //     The property {0}.{1} could not be found.
        internal static string FormatCommon_PropertyNotFound(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Common_PropertyNotFound"), p0, p1);
        }

        //
        // 摘要:
        //     The key '{0}' is invalid JQuery syntax because it is missing a closing bracket.
        internal static string FormatJQueryFormValueProviderFactory_MissingClosingBracket(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("JQueryFormValueProviderFactory_MissingClosingBracket"), p0);
        }

        //
        // 摘要:
        //     A value is required.
        internal static string FormatKeyValuePair_BothKeyAndValueMustBePresent()
        {
            return GetString("KeyValuePair_BothKeyAndValueMustBePresent");
        }

        //
        // 摘要:
        //     The binding context has a null Model, but this binder requires a non-null model
        //     of type '{0}'.
        internal static string FormatModelBinderUtil_ModelCannotBeNull(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinderUtil_ModelCannotBeNull"), p0);
        }

        //
        // 摘要:
        //     The binding context has a Model of type '{0}', but this binder can only operate
        //     on models of type '{1}'.
        internal static string FormatModelBinderUtil_ModelInstanceIsWrong(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinderUtil_ModelInstanceIsWrong"), p0, p1);
        }

        //
        // 摘要:
        //     The binding context cannot have a null ModelMetadata.
        internal static string FormatModelBinderUtil_ModelMetadataCannotBeNull()
        {
            return GetString("ModelBinderUtil_ModelMetadataCannotBeNull");
        }

        //
        // 摘要:
        //     A value for the '{0}' parameter or property was not provided.
        internal static string FormatModelBinding_MissingBindRequiredMember(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinding_MissingBindRequiredMember"), p0);
        }

        //
        // 摘要:
        //     A non-empty request body is required.
        internal static string FormatModelBinding_MissingRequestBodyRequiredMember()
        {
            return GetString("ModelBinding_MissingRequestBodyRequiredMember");
        }

        //
        // 摘要:
        //     The parameter conversion from type '{0}' to type '{1}' failed because no type
        //     converter can convert between these types.
        internal static string FormatValueProviderResult_NoConverterExists(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ValueProviderResult_NoConverterExists"), p0, p1);
        }

        //
        // 摘要:
        //     Path '{0}' was not rooted.
        internal static string FormatFileResult_PathNotRooted(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FileResult_PathNotRooted"), p0);
        }

        //
        // 摘要:
        //     The supplied URL is not local. A URL with an absolute path is considered local
        //     if it does not have a host/authority part. URLs using virtual paths ('~/') are
        //     also local.
        internal static string FormatUrlNotLocal()
        {
            return GetString("UrlNotLocal");
        }

        //
        // 摘要:
        //     The argument '{0}' is invalid. Empty or null formats are not supported.
        internal static string FormatFormatFormatterMappings_GetMediaTypeMappingForFormat_InvalidFormat(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FormatFormatterMappings_GetMediaTypeMappingForFormat_InvalidFormat"), p0);
        }

        //
        // 摘要:
        //     "Invalid values '{0}'."
        internal static string FormatAcceptHeaderParser_ParseAcceptHeader_InvalidValues(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AcceptHeaderParser_ParseAcceptHeader_InvalidValues"), p0);
        }

        //
        // 摘要:
        //     The value '{0}' is not valid for {1}.
        internal static string FormatModelState_AttemptedValueIsInvalid(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelState_AttemptedValueIsInvalid"), p0, p1);
        }

        //
        // 摘要:
        //     The value '{0}' is not valid.
        internal static string FormatModelState_NonPropertyAttemptedValueIsInvalid(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelState_NonPropertyAttemptedValueIsInvalid"), p0);
        }

        //
        // 摘要:
        //     The supplied value is invalid for {0}.
        internal static string FormatModelState_UnknownValueIsInvalid(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelState_UnknownValueIsInvalid"), p0);
        }

        //
        // 摘要:
        //     The supplied value is invalid.
        internal static string FormatModelState_NonPropertyUnknownValueIsInvalid()
        {
            return GetString("ModelState_NonPropertyUnknownValueIsInvalid");
        }

        //
        // 摘要:
        //     The value '{0}' is invalid.
        internal static string FormatHtmlGeneration_ValueIsInvalid(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HtmlGeneration_ValueIsInvalid"), p0);
        }

        //
        // 摘要:
        //     The field {0} must be a number.
        internal static string FormatHtmlGeneration_ValueMustBeNumber(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HtmlGeneration_ValueMustBeNumber"), p0);
        }

        //
        // 摘要:
        //     The field must be a number.
        internal static string FormatHtmlGeneration_NonPropertyValueMustBeNumber()
        {
            return GetString("HtmlGeneration_NonPropertyValueMustBeNumber");
        }

        //
        // 摘要:
        //     The list of '{0}' must not be empty. Add at least one supported encoding.
        internal static string FormatTextInputFormatter_SupportedEncodingsMustNotBeEmpty(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TextInputFormatter_SupportedEncodingsMustNotBeEmpty"), p0);
        }

        //
        // 摘要:
        //     The list of '{0}' must not be empty. Add at least one supported encoding.
        internal static string FormatTextOutputFormatter_SupportedEncodingsMustNotBeEmpty(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TextOutputFormatter_SupportedEncodingsMustNotBeEmpty"), p0);
        }

        //
        // 摘要:
        //     '{0}' is not supported by '{1}'. Use '{2}' instead.
        internal static string FormatTextOutputFormatter_WriteResponseBodyAsyncNotSupported(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TextOutputFormatter_WriteResponseBodyAsyncNotSupported"), p0, p1, p2);
        }

        //
        // 摘要:
        //     No media types found in '{0}.{1}'. Add at least one media type to the list of
        //     supported media types.
        internal static string FormatFormatter_NoMediaTypes(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Formatter_NoMediaTypes"), p0, p1);
        }

        //
        // 摘要:
        //     Could not create a model binder for model object of type '{0}'.
        internal static string FormatCouldNotCreateIModelBinder(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CouldNotCreateIModelBinder"), p0);
        }

        //
        // 摘要:
        //     '{0}.{1}' must not be empty. At least one '{2}' is required to bind from the
        //     body.
        internal static string FormatInputFormattersAreRequired(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InputFormattersAreRequired"), p0, p1, p2);
        }

        //
        // 摘要:
        //     '{0}.{1}' must not be empty. At least one '{2}' is required to model bind.
        internal static string FormatModelBinderProvidersAreRequired(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinderProvidersAreRequired"), p0, p1, p2);
        }

        //
        // 摘要:
        //     '{0}.{1}' must not be empty. At least one '{2}' is required to format a response.
        internal static string FormatOutputFormattersAreRequired(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("OutputFormattersAreRequired"), p0, p1, p2);
        }

        //
        // 摘要:
        //     Multiple overloads of method '{0}' are not supported.
        internal static string FormatMiddewareFilter_ConfigureMethodOverload(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MiddewareFilter_ConfigureMethodOverload"), p0);
        }

        //
        // 摘要:
        //     A public method named '{0}' could not be found in the '{1}' type.
        internal static string FormatMiddewareFilter_NoConfigureMethod(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MiddewareFilter_NoConfigureMethod"), p0, p1);
        }

        //
        // 摘要:
        //     Could not find '{0}' in the feature list.
        internal static string FormatMiddlewareFilterBuilder_NoMiddlewareFeature(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MiddlewareFilterBuilder_NoMiddlewareFeature"), p0);
        }

        //
        // 摘要:
        //     The '{0}' property cannot be null.
        internal static string FormatMiddlewareFilterBuilder_NullApplicationBuilder(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MiddlewareFilterBuilder_NullApplicationBuilder"), p0);
        }

        //
        // 摘要:
        //     The '{0}' method in the type '{1}' must have a return type of '{2}'.
        internal static string FormatMiddlewareFilter_InvalidConfigureReturnType(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MiddlewareFilter_InvalidConfigureReturnType"), p0, p1, p2);
        }

        //
        // 摘要:
        //     Could not resolve a service of type '{0}' for the parameter '{1}' of method '{2}'
        //     on type '{3}'.
        internal static string FormatMiddlewareFilter_ServiceResolutionFail(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MiddlewareFilter_ServiceResolutionFail"), p0, p1, p2, p3);
        }

        //
        // 摘要:
        //     An {0} cannot be created without a valid instance of {1}.
        internal static string FormatAuthorizeFilter_AuthorizationPolicyCannotBeCreated(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AuthorizeFilter_AuthorizationPolicyCannotBeCreated"), p0, p1);
        }

        //
        // 摘要:
        //     The '{0}' cannot bind to a model of type '{1}'. Change the model type to '{2}'
        //     instead.
        internal static string FormatFormCollectionModelBinder_CannotBindToFormCollection(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FormCollectionModelBinder_CannotBindToFormCollection"), p0, p1, p2);
        }

        //
        // 摘要:
        //     '{0}' requires the response cache middleware.
        internal static string FormatVaryByQueryKeys_Requires_ResponseCachingMiddleware(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("VaryByQueryKeys_Requires_ResponseCachingMiddleware"), p0);
        }

        //
        // 摘要:
        //     A duplicate entry for library reference {0} was found. Please check that all
        //     package references in all projects use the same casing for the same package references.
        internal static string FormatCandidateResolver_DifferentCasedReference(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CandidateResolver_DifferentCasedReference"), p0);
        }

        //
        // 摘要:
        //     Unable to create an instance of type '{0}'. The type specified in {1} must not
        //     be abstract and must have a parameterless constructor.
        internal static string FormatMiddlewareFilterConfigurationProvider_CreateConfigureDelegate_CannotCreateType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MiddlewareFilterConfigurationProvider_CreateConfigureDelegate_CannotCreateType"), p0, p1);
        }

        //
        // 摘要:
        //     '{0}' and '{1}' are out of bounds for the string.
        internal static string FormatArgument_InvalidOffsetLength(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Argument_InvalidOffsetLength"), p0, p1);
        }

        //
        // 摘要:
        //     Could not create an instance of type '{0}'. Model bound complex types must not
        //     be abstract or value types and must have a parameterless constructor.
        internal static string FormatComplexTypeModelBinder_NoParameterlessConstructor_ForType(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ComplexTypeModelBinder_NoParameterlessConstructor_ForType"), p0);
        }

        //
        // 摘要:
        //     Could not create an instance of type '{0}'. Model bound complex types must not
        //     be abstract or value types and must have a parameterless constructor. Alternatively,
        //     set the '{1}' property to a non-null value in the '{2}' constructor.
        internal static string FormatComplexTypeModelBinder_NoParameterlessConstructor_ForProperty(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ComplexTypeModelBinder_NoParameterlessConstructor_ForProperty"), p0, p1, p2);
        }

        //
        // 摘要:
        //     No page named '{0}' matches the supplied values.
        internal static string FormatNoRoutesMatchedForPage(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("NoRoutesMatchedForPage"), p0);
        }

        //
        // 摘要:
        //     The relative page path '{0}' can only be used while executing a Razor Page. Specify
        //     a root relative path with a leading '/' to generate a URL outside of a Razor
        //     Page. If you are using {1} then you must provide the current {2} to use relative
        //     pages.
        internal static string FormatUrlHelper_RelativePagePathIsNotSupported(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UrlHelper_RelativePagePathIsNotSupported"), p0, p1, p2);
        }

        //
        // 摘要:
        //     One or more validation errors occurred.
        internal static string FormatValidationProblemDescription_Title()
        {
            return GetString("ValidationProblemDescription_Title");
        }

        //
        // 摘要:
        //     Action '{0}' does not have an attribute route. Action methods on controllers
        //     annotated with {1} must be attribute routed.
        internal static string FormatApiController_AttributeRouteRequired(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApiController_AttributeRouteRequired"), p0, p1);
        }

        //
        // 摘要:
        //     No file provider has been configured to process the supplied file.
        internal static string FormatVirtualFileResultExecutor_NoFileProviderConfigured()
        {
            return GetString("VirtualFileResultExecutor_NoFileProviderConfigured");
        }

        //
        // 摘要:
        //     Type {0} specified by {1} is invalid. Type specified by {1} must derive from
        //     {2}.
        internal static string FormatApplicationPartFactory_InvalidFactoryType(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApplicationPartFactory_InvalidFactoryType"), p0, p1, p2);
        }

        //
        // 摘要:
        //     {0} specified on {1} cannot be self referential.
        internal static string FormatRelatedAssemblyAttribute_AssemblyCannotReferenceSelf(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("RelatedAssemblyAttribute_AssemblyCannotReferenceSelf"), p0, p1);
        }

        //
        // 摘要:
        //     Related assembly '{0}' specified by assembly '{1}' could not be found in the
        //     directory {2}. Related assemblies must be co-located with the specifying assemblies.
        internal static string FormatRelatedAssemblyAttribute_CouldNotBeFound(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("RelatedAssemblyAttribute_CouldNotBeFound"), p0, p1, p2);
        }

        //
        // 摘要:
        //     Each related assembly must be declared by exactly one assembly. The assembly
        //     '{0}' was declared as related assembly by the following:
        internal static string FormatApplicationAssembliesProvider_DuplicateRelatedAssembly(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApplicationAssembliesProvider_DuplicateRelatedAssembly"), p0);
        }

        //
        // 摘要:
        //     Assembly '{0}' declared as a related assembly by assembly '{1}' cannot define
        //     additional related assemblies.
        internal static string FormatApplicationAssembliesProvider_RelatedAssemblyCannotDefineAdditional(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApplicationAssembliesProvider_RelatedAssemblyCannotDefineAdditional"), p0, p1);
        }

        //
        // 摘要:
        //     Could not create an instance of type '{0}'. Model bound complex types must not
        //     be abstract or value types and must have a parameterless constructor. Alternatively,
        //     give the '{1}' parameter a non-null default value.
        internal static string FormatComplexTypeModelBinder_NoParameterlessConstructor_ForParameter(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ComplexTypeModelBinder_NoParameterlessConstructor_ForParameter"), p0, p1);
        }

        //
        // 摘要:
        //     Action '{0}' has more than one parameter that was specified or inferred as bound
        //     from request body. Only one parameter per action may be bound from body. Inspect
        //     the following parameters, and use '{1}' to specify bound from query, '{2}' to
        //     specify bound from route, and '{3}' for parameters to be bound from body:
        internal static string FormatApiController_MultipleBodyParametersFound(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApiController_MultipleBodyParametersFound"), p0, p1, p2, p3);
        }

        //
        // 摘要:
        //     API convention type '{0}' must be a static type.
        internal static string FormatApiConventionMustBeStatic(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApiConventionMustBeStatic"), p0);
        }

        //
        // 摘要:
        //     Invalid type parameter '{0}' specified for '{1}'.
        internal static string FormatInvalidTypeTForActionResultOfT(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidTypeTForActionResultOfT"), p0, p1);
        }

        //
        // 摘要:
        //     Method {0} is decorated with the following attributes that are not allowed on
        //     an API convention method:{1}The following attributes are allowed on API convention
        //     methods: {2}.
        internal static string FormatApiConvention_UnsupportedAttributesOnConvention(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApiConvention_UnsupportedAttributesOnConvention"), p0, p1, p2);
        }

        //
        // 摘要:
        //     Method name '{0}' is ambiguous for convention type '{1}'. More than one method
        //     found with the name '{0}'.
        internal static string FormatApiConventionMethod_AmbiguousMethodName(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApiConventionMethod_AmbiguousMethodName"), p0, p1);
        }

        //
        // 摘要:
        //     A method named '{0}' was not found on convention type '{1}'.
        internal static string FormatApiConventionMethod_NoMethodFound(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApiConventionMethod_NoMethodFound"), p0, p1);
        }

        //
        // 摘要:
        //     {0} exceeded the maximum configured validation depth '{1}' when validating type
        //     '{2}'.
        internal static string FormatValidationVisitor_ExceededMaxDepth(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ValidationVisitor_ExceededMaxDepth"), p0, p1, p2);
        }

        //
        // 摘要:
        //     This may indicate a very deep or infinitely recursive object graph. Consider
        //     modifying '{0}.{1}' or suppressing validation on the model type.
        internal static string FormatValidationVisitor_ExceededMaxDepthFix(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ValidationVisitor_ExceededMaxDepthFix"), p0, p1);
        }

        //
        // 摘要:
        //     {0} exceeded the maximum configured validation depth '{1}' when validating property
        //     '{2}' on type '{3}'.
        internal static string FormatValidationVisitor_ExceededMaxPropertyDepth(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ValidationVisitor_ExceededMaxPropertyDepth"), p0, p1, p2, p3);
        }

        //
        // 摘要:
        //     Bad Request
        internal static string FormatApiConventions_Title_400()
        {
            return GetString("ApiConventions_Title_400");
        }

        //
        // 摘要:
        //     Unauthorized
        internal static string FormatApiConventions_Title_401()
        {
            return GetString("ApiConventions_Title_401");
        }

        //
        // 摘要:
        //     Forbidden
        internal static string FormatApiConventions_Title_403()
        {
            return GetString("ApiConventions_Title_403");
        }

        //
        // 摘要:
        //     Not Found
        internal static string FormatApiConventions_Title_404()
        {
            return GetString("ApiConventions_Title_404");
        }

        //
        // 摘要:
        //     Not Acceptable
        internal static string FormatApiConventions_Title_406()
        {
            return GetString("ApiConventions_Title_406");
        }

        //
        // 摘要:
        //     Conflict
        internal static string FormatApiConventions_Title_409()
        {
            return GetString("ApiConventions_Title_409");
        }

        //
        // 摘要:
        //     Unsupported Media Type
        internal static string FormatApiConventions_Title_415()
        {
            return GetString("ApiConventions_Title_415");
        }

        //
        // 摘要:
        //     Unprocessable Entity
        internal static string FormatApiConventions_Title_422()
        {
            return GetString("ApiConventions_Title_422");
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            string text = _resourceManager.GetString(name) ?? string.Empty;
            if (formatterNames != null)
            {
                for (int i = 0; i < formatterNames.Length; i++)
                {
                    text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return text;
        }
    }
}
