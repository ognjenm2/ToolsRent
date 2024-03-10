using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ToolsRent.Web.Util
{
    /// <summary>
    /// HtmlUtilityExtensions contains extension methods for different utility methods
    /// </summary>
    public static class HtmlUtilityExtensions
    {


        /// <summary>
        /// Merge objects with html attributes. Used in EditorTemplates
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="htmlAttributesObject"></param>
        /// <param name="defaultHtmlAttributesObject"></param>
        /// <returns></returns>
        public static IDictionary<string, object> MergeHtmlAttributes(this HtmlHelper helper, object htmlAttributesObject, object defaultHtmlAttributesObject)
        {
            var concatKeys = new string[] { "class" };

            var htmlAttributesDict = htmlAttributesObject as IDictionary<string, object>;
            var defaultHtmlAttributesDict = defaultHtmlAttributesObject as IDictionary<string, object>;

            RouteValueDictionary htmlAttributes = (htmlAttributesDict != null)
                ? new RouteValueDictionary(htmlAttributesDict)
                : HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesObject);
            RouteValueDictionary defaultHtmlAttributes = (defaultHtmlAttributesDict != null)
                ? new RouteValueDictionary(defaultHtmlAttributesDict)
                : HtmlHelper.AnonymousObjectToHtmlAttributes(defaultHtmlAttributesObject);

            foreach (var item in htmlAttributes)
            {
                if (item.Value != null)
                {
                    if (concatKeys.Contains(item.Key))
                    {
                        defaultHtmlAttributes[item.Key] = (defaultHtmlAttributes[item.Key] != null)
                            ? string.Format("{0} {1}", defaultHtmlAttributes[item.Key], item.Value)
                            : item.Value;
                    }
                    else
                    {
                        defaultHtmlAttributes[item.Key] = item.Value;
                    }
                }
            }

            return defaultHtmlAttributes;
        }

        /// <summary>
        /// Begin script block. Used in partial views. 
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IDisposable BeginScripts(this HtmlHelper helper)
        {
            return new ScriptBlock((WebViewPage)helper.ViewDataContainer);
        }

        /// <summary>
        /// Render ScriptBlock scripts. Used in _Scripts.cshtml to render specific partial view scripts.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString PageScripts(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.pageScripts.Select(s => s.ToString())));
        }

        /// <summary>
        /// Serializes the property names of the specified object to a JSON string as a markup that is not HTML encoded.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="obj">The object used for property extraction.</param>
        /// <param name="camelCase">True for camelCase property names, otherwise false.</param>
        /// <returns></returns>
        public static IHtmlString GetPropertyNamesJson<T>(this HtmlHelper helper, T obj, bool camelCase = true) where T : new()
        {
            JObject jObj = null;
            if (obj == null)
            {
                obj = new T();
            }

            if (camelCase)
            {
                jObj = JObject.FromObject(obj, new JsonSerializer()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }
            else
            {
                jObj = JObject.FromObject(obj);
            }

            IEnumerable<string> propNames = jObj.Properties().Select(p => p.Name);
            return helper.ToJson(propNames);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string as a markup that is not HTML encoded.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="camelCase">True for camelCase property names, otherwise false.</param>
        /// <returns></returns>
        public static IHtmlString ToJson(this HtmlHelper helper, object value, bool camelCase = true)
        {
            string json;

            if (camelCase)
            {
                json = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(value);
            }

            return helper.Raw(json);
        }

        /// <summary>
        /// ScriptBlock contains page scripts
        /// </summary>
        private class ScriptBlock : IDisposable
        {
            private const string scriptsKey = "scripts";

            public static List<string> pageScripts
            {
                get
                {
                    if (HttpContext.Current.Items[scriptsKey] == null)
                        HttpContext.Current.Items[scriptsKey] = new List<string>();
                    return (List<string>)HttpContext.Current.Items[scriptsKey];
                }
            }

            private readonly WebViewPage webPageBase;

            public ScriptBlock(WebViewPage webPageBase)
            {
                this.webPageBase = webPageBase;
                this.webPageBase.OutputStack.Push(new StringWriter());
            }

            public void Dispose()
            {
                pageScripts.Add(((StringWriter)this.webPageBase.OutputStack.Pop()).ToString());
            }
        }

        /// <summary>
        /// Postavlja ispravan prefix na polja partial viewa kako bi se polja pravilo bindala.
        /// Koristimo kada se ViewModel sastoji od više manjih ViewModela, npr:
        /// class BigViewModel 
        /// {
        ///     public SmallViewModel1 ViewModel1 { get; set; }
        ///     public SmallViewModel2 ViewModel2 { get; set; }
        /// }
        /// 
        /// U viewu što renderira BigViewModel, renderiramo dva partial viewa koji renderiraju SmallViewModel1 i SmallViewModel2 objekte
        /// 
        /// @model BigViewModel
        /// 
        /// @Html.PartialFor( m => m.ViewModel1, "~/Views/Shared/SmallViewModel1.cshtml" )
        /// @Html.PartialFor( m => m.ViewModel2, "~/Views/Shared/SmallViewModel2.cshtml" )
        /// 
        /// Sada će polja imati prefix ViewModel1 odosno ViewModel2, npr:
        /// 
        /// input id="ViewModel1_TestField1" name="ViewModel1.TestField1" type="text" value=""
        /// input id="ViewModel2_TestField2" name="ViewModel2.TestField2" type="text" value=""
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="partialViewName"></param>
        /// <returns></returns>
        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> helper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression, string partialViewName)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            object model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
            var viewData = new ViewDataDictionary(((HtmlHelper)helper).ViewData)
            {
                TemplateInfo = new TemplateInfo
                {
                    HtmlFieldPrefix = name
                }
            };

            return helper.Partial(partialViewName, model, viewData);

        }
    }
}