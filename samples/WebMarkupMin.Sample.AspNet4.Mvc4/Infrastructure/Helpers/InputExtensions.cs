using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.Helpers
{
	public static class InputExtensions
	{
		#region CheckBoxWithLabel
		public static MvcHtmlString CheckBoxWithLabel(this HtmlHelper htmlHelper, string name, object value,
			string text, bool isChecked, int index = -1)
		{
			return CheckBoxWithLabel(htmlHelper, name, value, text, isChecked, (object)null, index);
		}

		public static MvcHtmlString CheckBoxWithLabel(this HtmlHelper htmlHelper, string name, object value,
			string text, bool isChecked, object htmlAttributes, int index = -1)
		{
			return CheckBoxWithLabelHelper(htmlHelper, name, value, text, isChecked,
				HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), index);
		}

		public static MvcHtmlString CheckBoxWithLabel(this HtmlHelper htmlHelper, string name, object value,
			string text, bool isChecked, Dictionary<string, object> htmlAttributes, int index = -1)
		{
			return CheckBoxWithLabelHelper(htmlHelper, name, value, text, isChecked,
				new RouteValueDictionary(htmlAttributes), index);
		}

		private static MvcHtmlString CheckBoxWithLabelHelper(this HtmlHelper htmlHelper, string name, object value,
			string text, bool isChecked, RouteValueDictionary htmlAttributes, int index = -1)
		{
			var checkBoxHtmlAttributes = new RouteValueDictionary();
			if (value != null)
			{
				checkBoxHtmlAttributes["value"] = value.ToString();
			}

			string subIndexName = htmlHelper.CreateSubIndexName(name, index);
			string id = TagBuilder.CreateSanitizedId(subIndexName);

			htmlAttributes["id"] = id;

			var label = new TagBuilder("label")
			{
				InnerHtml = htmlHelper.CheckBox(name, isChecked, checkBoxHtmlAttributes).ToHtmlString() + " " + text
			};
			label.MergeAttributes(htmlAttributes);

			return MvcHtmlString.Create(label.ToString());
		}


		public static MvcHtmlString CheckBoxWithLabelFor<TModel>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, bool>> expression)
		{
			return CheckBoxWithLabelFor(htmlHelper, expression, (object)null);
		}

		public static MvcHtmlString CheckBoxWithLabelFor<TModel>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, bool>> expression, object htmlAttributes)
		{
			return CheckBoxWithLabelForHelper(htmlHelper, expression,
				HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString CheckBoxWithLabelFor<TModel>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, bool>> expression, Dictionary<string, object> htmlAttributes)
		{
			return CheckBoxWithLabelForHelper(htmlHelper, expression, new RouteValueDictionary(htmlAttributes));
		}

		private static MvcHtmlString CheckBoxWithLabelForHelper<TModel>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, bool>> expression, RouteValueDictionary htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			string labelText = metadata.DisplayName;

			var label = new TagBuilder("label")
			{
				InnerHtml = htmlHelper.CheckBoxFor(expression) + " " + labelText
			};
			label.MergeAttributes(htmlAttributes);

			return MvcHtmlString.Create(label.ToString());
		}
		#endregion
	}
}