<%@ Page Language="C#" %>
<%@ Register TagPrefix="ed" Namespace="OboutInc.Editor" Assembly="obout_Editor" %>
<script runat="server">

OboutInc.Editor.FieldsFiller image_filler;

void Page_Load(object o, EventArgs e)
{
  image_filler = new OboutInc.Editor.FieldsFiller(Page,"image-upload",Page.Request["localization_path"],Page.Request["language"]);
}
</script>
<script type="text/JavaScript">
function onLoad()
{
    document.getElementById("frmFile").name = "frmFile";
    document.getElementById("fraExecute").contentWindow.name = "fraExecute";
    document.getElementById("fraExecute").name = "fraExecute";
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
<link rel="stylesheet" href="<%= Page.Request["css"] %>" media="all" />
</head>
<body style="overflow: hidden; margin: 0px; padding: 5px;" onload="onLoad()">
<form target="fraExecute" id="frmFile" action="myImageUpload" style="margin: 0px;" enctype="multipart/form-data" method="post">
<table border="0" cellspacing="2" cellpadding="0" style="margin: 0px; padding: 0px; width: 100%;">
<tr>
<td style="width:1%; white-space:nowrap;">
<%= image_filler.Get("url","Image URL") %>:
</td>
<td style="width:100%">
<input type="file" id="path" name="path" style="width:100%" onkeydown="return false;" />
</td>
</tr>
<tr>
<td style="width:1%; white-space:nowrap;">
<%= image_filler.Get("description","Description") %>:
</td>
<td style="width:100%">
<input type="text" id="title" name="title" style="width:100%" />
</td>
</tr>
</table>
<input type="hidden" name="saved" value="<%= image_filler.Get("saved","Image saved successfully") %>" />
<input type="hidden" name="notsaved" value="<%= image_filler.Get("not-saved","No Image saved") %>" />
</form>

<iframe id="fraExecute" width="0" height="0" style="display: none">
</iframe>
</body>
</html>
