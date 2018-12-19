<%@ Page Language="C#" %>
<%@ Register TagPrefix="ed" Namespace="OboutInc.Editor" Assembly="obout_Editor" %>
<script runat="server">

OboutInc.Editor.FieldsFiller filler;

void Page_Load(object o, EventArgs e)
{
  filler = new OboutInc.Editor.FieldsFiller(Page,"textarea",Page.Request["localization_path"],Page.Request["language"]);
}
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>obout ASP.NET HTML Editor examples</title>
      <style type="text/css">
			
			.tdText {
                font:11px Verdana;
                color:#333333;
            }
      </style>
<!-- Here we use the default Editor's popup windows styles-->

<link rel="stylesheet" href="<%= Page.Request["css"] %>" media="all" />
</head>
<body style="overflow: hidden; margin: 0px; padding: 5px;">
    <form id="Form1" runat="server">
        
    <table border="0" cellspacing="0" cellpadding="0" style="margin: 0px; padding: 0px; width: 100%;">
    <tr>
    <td style="width:50%">
    <table border="0" cellspacing="2" cellpadding="0" style="margin: 0px; padding: 0px; width: 100%;">
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("id","ID") %>:
    </td>
    <td style="width:100%">
    <input type="text" id="idField" style="width:100%" />
    </td>
    </tr>
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("columns","Columns") %>:
    </td>
    <td style="width:100%">
    <input type="text" id="colsField" style="width:100%" />
    </td>
    </tr>
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("disabled","Disabled") %>:
    </td>
    <td style="width:100%">
    <input type="checkbox" id="disabledField" />
    </td>
    </tr>
    </table>
    </td>
    <td style="width:50%">
    <table border="0" cellspacing="2" cellpadding="0" style="margin: 0px; padding: 0px; width: 100%;">
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("name","Name") %>:
    </td>
    <td style="width:100%">
    <input type="text" id="nameField" style="width:100%" />
    </td>
    </tr>
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("rows","Rows") %>:
    </td>
    <td style="width:100%">
    <input type="text" id="rowsField" style="width:100%" />
    </td>
    </tr>
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("readonly","Readonly") %>:
    </td>
    <td style="width:100%">
    <input type="checkbox" id="readonlyField" />
    </td>
    </tr>
    </table>
    </td>
    </tr>
    <tr>
    <td colspan="2" style="width:100%; white-space:nowrap">
    <table border="0" cellspacing="2" cellpadding="0" style="margin: 0px; padding: 0px; width: 100%;">
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("value","Value") %>:
    </td>
    <td style="width:100%">
    <textarea rows="10" cols="10" style="width:100%; height:100px; overflow:auto;" id="valueField"></textarea>
    </td>
    </tr>
    </table>
    </td>
    </tr>
    </table>
   </form>
</body>
</html>
