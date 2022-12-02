using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.Helpers
{
	public static class LabelExtensions
	{
		public static MvcHtmlString CustomLabel(this HtmlHelper htmlHelper, string name, string labelText,
			bool isRequired, bool withColon, object htmlAttributes)
		{
			return htmlHelper.CustomLabel(name, labelText, isRequired, withColon,
				HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString CustomLabel(this HtmlHelper htmlHelper, string name, string labelText,
			bool isRequired, bool withColon, IDictionary<string, object> htmlAttributes)
		{
			string id = TagBuilder.CreateSanitizedId(name);

			var label = new TagBuilder("label");
			label.MergeAttribute("for", id);
			if (htmlAttributes != null)
			{
				label.MergeAttributes(htmlAttributes);
			}
			label.InnerHtml = labelText + ((withColon) ? ":" : String.Empty);
			if (isRequired)
			{
				var requiredFieldMarker = new TagBuilder("span");
				requiredFieldMarker.SetInnerText("*");
				requiredFieldMarker.AddCssClass(htmlHelper.Constants().RequiredFieldMarkerCssClassName);

				label.InnerHtml += requiredFieldMarker.ToString();
			}

			return MvcHtmlString.Create(label.ToString());
		}


		public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression)
		{
			return htmlHelper.CustomLabelFor(expression, false, false, null);
		}

		public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			return htmlHelper.CustomLabelFor(expression, false, false,
				HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.CustomLabelFor(expression, false, false, htmlAttributes);
		}

		public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression, bool withColon)
		{
			return htmlHelper.CustomLabelFor(expression, withColon, false, null);
		}

		public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression, bool withColon, object htmlAttributes)
		{
			return htmlHelper.CustomLabelFor(expression, withColon, false,
				HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression, bool withColon, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.CustomLabelFor(expression, withColon, false, htmlAttributes);
		}

		public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression, bool withColon, bool hideRequiredFieldMarker)
		{
			return htmlHelper.CustomLabelFor(expression, withColon, hideRequiredFieldMarker, null);
		}

		public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression, bool withColon, bool hideRequiredFieldMarker,
			object htmlAttributes)
		{
			return htmlHelper.CustomLabelFor(expression, withColon, hideRequiredFieldMarker,
				HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression, bool withColon, bool hideRequiredFieldMarker,
			IDictionary<string, object> htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			string name = ExpressionHelper.GetExpressionText(expression);
			string labelText = metadata.DisplayName;
			bool isRequired = false;
			if (!hideRequiredFieldMarker && !metadata.IsReadOnly && metadata.ShowForEdit)
			{
				foreach (ModelValidator validator in metadata.GetValidators(htmlHelper.ViewContext.Controller.ControllerContext))
				{
					if (validator.IsRequired)
					{
						isRequired = true;
						break;
					}
				}
			}

			return htmlHelper.CustomLabel(name, labelText, isRequired, withColon, htmlAttributes);
		}
	}
}