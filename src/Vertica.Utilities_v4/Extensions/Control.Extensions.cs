using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vertica.Utilities_v4.Extensions.StringExt;
using Vertica.Utilities_v4.Resources;

namespace Vertica.Utilities_v4.Extensions.ControlExt
{
  public static class ControlExtensions
  {
	  public static T FindControl<T>(this Control parentControl, string targetControlID)
			where T : Control
	  {
		  Guard.AgainstNullArgument("parentControl", parentControl);
		  Guard.AgainstArgument("targetControlID", targetControlID.IsEmpty());

		  Control targetControlBase = parentControl.FindControl(targetControlID);

		  if (targetControlBase == null)
		  {
			  ExceptionHelper.Throw<ArgumentOutOfRangeException>(Exceptions.ControlExtensions_NotFoundTemplate,
				  targetControlID,
				  parentControl.ID);
		  }

		  return (T)targetControlBase;
	  }

	  public static bool TryFindControl<T>(this Control parentControl, string targetControlID, out T control)
			where T : Control
	  {
		  control = default(T);

		  try
		  {
			  control = FindControl<T>(parentControl, targetControlID);

			  return control != null;
		  }
		  catch (ArgumentException)
		  { }
		  catch (InvalidCastException)
		  { }

		  return false;
	  }

	  public static bool SelectValue(this ListControl list, string value)
	  {
		  if (list == null) throw new ArgumentNullException("list");

		  if (list.Items.Count > 0)
		  {
			  ListItem listItem = list.Items.FindByValue(value);

			  if (listItem != null)
			  {
				  list.ClearSelection();
				  listItem.Selected = true;
				  return true;
			  }
		  }

		  return false;
	  }

	  private static string userControlFileName<T>() where T : UserControl
	  {
		  return typeof(T).Name + ".ascx";
	  }

	  public static T LoadControl<T>(this TemplateControl entry) where T : UserControl
	  {
		  return (T)entry.LoadControl(userControlFileName<T>());
	  }

	  public static T LoadControl<T>(this TemplateControl entry, Uri userControlsPath) where T : UserControl
	  {
		  return (T)entry.LoadControl(userControlsPath.ToString().AppendIfNotThere("/") + userControlFileName<T>());
	  }

  }
}