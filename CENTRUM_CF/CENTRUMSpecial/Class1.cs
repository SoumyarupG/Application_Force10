using System.Collections;
using System.ComponentModel;
using System.Web.UI;


namespace CENTRUMSpecial.Web.WebControls
{
    ///

    /// Represents a Control that allows the users to select a Single Value from the dropdown.
    /// This control also stores an extra field (may be primary Key) for each item besides Key value pair.
    ///
    [DefaultProperty("Text"),
     ToolboxData("<{0}:DropDownStoresPK runat=server>")]
    public class DropDownStoresPK : System.Web.UI.WebControls.DropDownList
    {
        private SortedList _extraField;
        private string text;

        ///
        /// Stores the Extra Field
        ///
        [Bindable(true),
         Category("Misc"),
            Browsable(false),
         DefaultValue(""),
            Description("Stores the extra field as SortedList to DataKeyField")]
        public SortedList ExtraField
        {
            get
            {
                return (base.EnableViewState) ? (SortedList)ViewState["ExtraField"] : _extraField;
            }
            set
            {
                _extraField = value;
                if (base.EnableViewState)
                    ViewState["ExtraField"] = _extraField;
            }
        }

        public object GetExtraField(string selectedValue)
        {
            return (_extraField.ContainsKey(selectedValue)) ? _extraField[selectedValue] : null;
            //return _extraField.Length > 0? _extraField[selectedValue].ToString():null;
        }

        ///

        /// Gets the Key for the Value passed that is stored in Extra Fields Sorted List
        ///
        /// The Value for which the key is required
        /// The Key that corresponds with the value passed
        public object GetExtraKey(string Value)
        {
            return _extraField.GetKey(_extraField.IndexOfValue(Value));
        }


        ///

        /// Render this control to the output parameter specified.
        ///
        /// The HTML writer to write out to
        protected override void Render(HtmlTextWriter output)
        {
            base.Render(output);
        }
    }
}