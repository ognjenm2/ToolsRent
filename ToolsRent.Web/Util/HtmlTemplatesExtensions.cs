
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ToolsRent.Web.Util
{
    /// <summary>
    /// HtmlTemplatesExtensions contains extension methods for EditorFor templates.
    /// </summary>
    public static class HtmlTemplatesExtensions {
        /// <summary>
        /// Default start year for year custom template.
        /// </summary>
        const int START_YEAR = 1970;
        /// <summary>
        /// Default end year for year custom template.
        /// </summary>
        const int END_YEAR = 2070;

        /// <summary>
        /// True postavlja tooltip na sve input kontrole, ali jedino ako kontrola nije dovoljno široka da prikaže cijeli tekst.
        /// Ovo vrijedi jedino ako je input kontrola u read modu
        /// Client side inicijalizacija je u _Scripts.html, $(".handleTooltip")...
        /// </summary>
        const bool activateTooltip = false;

        public static MvcHtmlString EditorForNumericTextBox<TModel, TValue>( this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isReadonly, object htmlAttributes = null ) {
            return html.EditorFor( expression, "CustomNumericTextBoxTemplate", new { disabled = isReadonly, htmlAttributes, handleTooltip = activateTooltip } );
        }

        public static MvcHtmlString EditorForDecimalTextBox<TModel, TValue>( this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isReadonly, object htmlAttributes = null ) {
            return html.EditorFor( expression, "CustomDecimalTextBoxTemplate", new { disabled = isReadonly, htmlAttributes, handleTooltip = activateTooltip } );
        }

        public static MvcHtmlString EditorForGeoCoordinates<TModel, TValue>( this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isReadonly, object htmlAttributes = null ) {
            return html.EditorFor( expression, "CustomGeoCoordinatesTemplate", new { disabled = isReadonly, htmlAttributes, handleTooltip = activateTooltip } );
        }

        public static MvcHtmlString EditorForTextBox<TModel, TValue>( this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isReadonly, object htmlAttributes = null ) {
            return html.EditorFor( expression, "CustomTextBoxTemplate", new { disabled = isReadonly, htmlAttributes, handleTooltip = activateTooltip } );
        }

        //public static MvcHtmlString EditorForDropDown<TModel, TValue>( this HtmlHelper<TModel> html,
        //    Expression<Func<TModel, TValue>> expression, IEnumerable<CodeListValue> codeList, bool isReadonly, object htmlAttributes = null, bool searchable = false ) {
        //    return html.EditorFor( expression, "CustomDropDownTemplate", new { codeList, disabled = isReadonly, htmlAttributes, searchable, handleTooltip = activateTooltip } );
        //}

        public static MvcHtmlString EditorForDropDown<TModel, TValue>( this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> codeList, bool isReadonly, object htmlAttributes = null, bool searchable = false ) {
            return html.EditorFor( expression, "CustomDropDownTemplateSelectList", new { codeList, disabled = isReadonly, htmlAttributes, searchable, handleTooltip = activateTooltip } );
        }

        public static MvcHtmlString EditorForDropDownRead<TModel, TValue>( this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> codeList, bool isReadonly, object htmlAttributes = null, bool searchable = false ) {
            return html.EditorFor( expression, "CustomDropDownTemplateSelectListRead", new { codeList, disabled = isReadonly, htmlAttributes, searchable, handleTooltip = activateTooltip } );
        }

        public static MvcHtmlString EditorForDate<TModel, TValue>( this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isReadonly, object htmlAttributes = null ) {
            return html.EditorFor( expression, "CustomDateTemplate", new { disabled = isReadonly, htmlAttributes, handleTooltip = activateTooltip } );
        }

        public static MvcHtmlString EditorForTime<TModel, TValue>( this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isReadonly, object htmlAttributes = null, bool showSeconds = false ) {
            return html.EditorFor( expression, "CustomClockTemplate", new { disabled = isReadonly, htmlAttributes, handleTooltip = activateTooltip, showSeconds } );
        }

        public static MvcHtmlString EditorForTextArea<TModel, TValue>( this HtmlHelper<TModel> html,
           Expression<Func<TModel, TValue>> expression, bool isReadonly, object htmlAttributes = null ) {
            return html.EditorFor( expression, "CustomTextAreaTemplate", new { disabled = isReadonly, htmlAttributes, handleTooltip = activateTooltip } );
        }

        //public static MvcHtmlString EditorForDropDown<TModel, TValue>( this HtmlHelper<TModel> helper,
        //   Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> codeList, bool isReadonly,
        //   CodeListTypeEnum dynamicDataType, string dynamicDataTarget, object htmlAttributes = null, string callBackFunction = null ) {
        //    return HtmlTemplatesExtensions.EditorForChildDropDown<TModel, TValue>( helper, expression, codeList, isReadonly, dynamicDataType, dynamicDataTarget, htmlAttributes, callBackFunction );
        //}

        //public static MvcHtmlString EditorForChildDropDown<TModel, TValue>( this HtmlHelper<TModel> helper,
        //   Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> codeList, bool isReadonly,
        //   CodeListTypeEnum dynamicDataType, string dynamicDataTarget, object htmlAttributes = null, string callBackFunction = null ) {
        //    string actionName = null;
        //    switch( dynamicDataType ) {
        //        case CodeListTypeEnum.Activities:
        //            actionName = nameof( CodeListController.Activities );
        //            break;
        //        case CodeListTypeEnum.Employees:
        //            actionName = nameof( CodeListController.Employees );
        //            break;
        //    }

        //    RouteValueDictionary htmlAttributesDictionary = new RouteValueDictionary( htmlAttributes );
        //    htmlAttributesDictionary = new RouteValueDictionary( htmlAttributesDictionary.ToDictionary( x => x.Key.Replace( "data_", "data-" ), x => x.Value ) ) {
        //        { "data-dddynamic", true },
        //        { "data-result", dynamicDataTarget },
        //        {
        //            "data-source",
        //            UrlHelper.GenerateUrl( RouteConfig.MainRouteName, actionName, "CodeList",
        //                null, null, null, null, helper.RouteCollection, helper.ViewContext.RequestContext, true )
        //        },
        //        { "data-callback", callBackFunction }
        //    };

        //    return helper.EditorForDropDown( expression, codeList, isReadonly, htmlAttributesDictionary, true );
        //}
    }
}