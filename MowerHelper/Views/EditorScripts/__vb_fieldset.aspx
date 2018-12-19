<%@ Page Language="VB" %>
<%@ Register TagPrefix="ed" Namespace="OboutInc.Editor" Assembly="obout_Editor" %>
<script runat="server">

Protected filler As OboutInc.Editor.FieldsFiller

Public Sub Page_Load(sender As Object, e As EventArgs)
  filler = New OboutInc.Editor.FieldsFiller(Page,"fieldset",Page.Request("localization_path"),Page.Request("language"))
End Sub
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

<link rel="stylesheet" href="<%= Page.Request("css") %>" media="all" />
</head>
<body style="overflow: hidden; margin: 0px; padding: 5px;">
    <form id="Form1" runat="server">
       
    <table border="0" cellspacing="0" cellpadding="0" style="margin: 0px; padding: 0px; width: 100%;">
    <tr>
    <td style="width:50%">
    <table border="0" cellspacing="2" cellpadding="0" style="margin: 0px; padding: 0px; width: 100%;">
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("width","Width") %>:
    </td>
    <td style="width:100%">
    <input type="text" id="widthField" style="width:100%" />
    </td>
    </tr>
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("height","Height") %>:
    </td>
    <td style="width:100%">
    <input type="text" id="heightField" style="width:100%" />
    </td>
    </tr>
    </table>
    </td>
    <td style="width:50%">
    <table border="0" cellspacing="2" cellpadding="0" style="margin: 0px; padding: 0px; width: 100%;">
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("padding","Padding") %>:
    </td>
    <td style="width:100%">
    <input type="text" id="paddingField" style="width:100%" />
    </td>
    </tr>
    <tr>
    <td style="width:1%; white-space:nowrap;">
    <%= filler.Get("margin","Margin") %>:
    </td>
    <td style="width:100%">
    <input type="text" id="marginField" style="width:100%" />
    </td>
    </tr>
    </table>
    </td>
    </tr>
    </table>
    </form>
</body>
</html>
