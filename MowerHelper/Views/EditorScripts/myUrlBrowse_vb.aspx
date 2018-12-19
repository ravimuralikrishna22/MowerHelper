<%@ Page Language="VB" Debug="true"%>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="obout_ASPTreeView_2_NET" %>

<script language="VB" runat="server">
Dim prefix As String = ""
Public Sub Page_Load(sender As Object, e As EventArgs)
  Dim currentProtocol As String = Page.Request("protocol")
  Dim anchors As String = Page.Request("anchors")

  If currentProtocol = "#" Then
    '' Let view all available anchors for Hrefs

     If anchors.Length > 0 Then
        Dim anchorsArray as String () = anchors.Split(VbLf)

        For i As Integer = 0  To anchorsArray.Length-1 Step 1
          Dim html As String = "<div style='margin:1px; padding:1px; border-width:1px; border-color:white; border-style:solid; color:blue;background-color:white;' onmouseover=""this.style.cursor='pointer'"" onclick=""myOnClick(event,'#" & anchorsArray(i) & "',false)"" ondblclick=""myOnClick(event,'#" & anchorsArray(i) & "',true)"">" & anchorsArray(i) & "</div>"
          Place.Text &= html
        Next
     End If
  Else
  If currentProtocol = "http://" Or currentProtocol = "https://" Then
     '' Let view all available values for Href

     Dim separator  As Char() = New [Char]() {"/"c}
                Dim fileFolder As String = "~/Content/Obout/Editor/Content/files"
     Dim protocol   As String = Page.Request.ServerVariables("SERVER_PROTOCOL").Split(separator)(0).ToLower()
     Dim port       As String = ":" & Page.Request.ServerVariables("SERVER_PORT")

     If Page.Request.ServerVariables("HTTPS") = "on" Then
        protocol = protocol & "s"
     End If

     If port=":80" OR port=":443" Then
        port = ""
     End If

     prefix = protocol & "://" & Page.Request.ServerVariables("SERVER_NAME") & port
     Dim curPath As String = prefix & Path.GetDirectoryName(Page.Request.ServerVariables("SCRIPT_NAME")).Replace("\","/")+"/"
     Dim folder  As String = System.Web.HttpContext.Current.Server.MapPath(fileFolder)

     Dim oTree As New obout_ASPTreeView_2_NET.Tree()
     
     oTree.AddRootNode(Page.Request.ServerVariables("SERVER_NAME") & port, "ball_redS.gif")
     
                oTree.FolderIcons = "/Content/Obout/TreeView/tree2/icons"
                oTree.FolderScript = "/Content/Obout/TreeView/tree2/script"
                oTree.FolderStyle = "/Content/Obout/TreeView/tree2/style/Classic"
         
                oTree.SelectedEnable = False

     '' Try to get values from database

     Try
       Dim m_connString As String
       Dim strCmd As String = "SELECT * FROM tbUrl"

       m_connString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE={0};"
             m_connString = String.Format(m_connString, Server.MapPath("../App_Data/db.mdb"))

       Dim da As New OleDbDataAdapter(strCmd, m_connString)
       Dim ds As New DataSet()
       da.Fill(ds, "tbUrl")
       oTree.Add ("root", "dbRoot", "Links from database", False, Nothing, Nothing)
       Dim i As Integer =0


       For Each row As DataRow In ds.Tables("tbUrl").Rows
           Dim html As String = "<span style='text-decoration: underline' title='"+row("fldUrl")+"' myAttrSrc='"+row("fldUrl")+"' onmouseover=""this.style.cursor='pointer'"" onclick=""myOnClickHref(event,'"+row("fldUrl")+"',false)"" ondblclick=""myOnClickHref(event,'"+row("fldUrl")+"',true)"">"+row("fldTitle")+"</span>"
           oTree.Add ("dbRoot", "dbRoot_"+i.ToString(), html, False, "square_yellowS.gif", Nothing)
           i = i + 1
       Next row

     Catch ex As Exception
     End Try

     '' try to find folder with files

     If Directory.Exists(folder) Then
        oTree.Add ("root", "filesRoot", "Server files", False, Nothing, Nothing)
        directoryDive(oTree, "filesRoot", folder, curPath+fileFolder+"/",fileFolder+"/")
     End If

     Place.Text = oTree.HTML()
  Else
    '' not supported protocol
  End If
  End If
End Sub

Public Sub directoryDive(oTree As obout_ASPTreeView_2_NET.Tree, parentID As String, folder As String, folderPath As String, relPath As String)
  Dim currentID As String
  If parentID="myRoot" Then
     currentID = "a_"
  Else
     currentID = parentID & "_"
  End If

  Dim entires As String() = Directory.GetFileSystemEntries(folder)
  Dim i As Integer = 0

  For Each entire As String In entires
     Dim attr  As FileAttributes = File.GetAttributes(entire)
     Dim name  As String         = Path.GetFileName(entire)

     If (attr AND FileAttributes.Directory) = FileAttributes.Directory Then
        oTree.Add (parentID, currentID & i.ToString(), name, False, Nothing, Nothing)
        directoryDive(oTree, currentID & i.ToString() ,folder & "/" & name, folderPath & name & "/", relPath & name & "/")
     Else
        Dim html As String = "<span myAttrSrc='" & folderPath & name & "' onmouseover=""this.style.cursor='pointer'"" onclick=""myOnClickHref(event,'" & folderPath & name & "',false)"" ondblclick=""myOnClickHref(event,'" & folderPath & name & "',true)"">" & name & "</span>"
        oTree.Add (parentID, currentID+i.ToString(), html, False, "square_yellowS.gif", Nothing)
     End If
     i = i+1
  Next entire
End Sub
</script>

<script language="JavaScript" type="text/JavaScript">

var currentProtocol = "<%=Page.Request("protocol") %>";
var anchors = "<%=Page.Request("anchors").Replace(VbLf,"\n") %>";

function OnLoad()
{

 var height=parent.document.getElementById("innerIframe").offsetHeight;

 if(!document.all) height-=4;
 document.getElementById("placeDiv").style.height = height-4+"px";

 if(currentProtocol == "http://" || currentProtocol == "https://") // http
 {
   var seekResult = false;

   if(document.getElementById("dbRoot") != null)
     seekResult = myDiveHref(document.getElementById("dbRoot"),parent.document.getElementById("savedSrc").value.toLowerCase().replace("%20"," "));

   if(!seekResult && document.getElementById("filesRoot") != null)
     myDiveHref(document.getElementById("filesRoot"),parent.document.getElementById("savedSrc").value.toLowerCase().replace("%20"," "));
 }
 else
 if(currentProtocol == "#" && anchors.length > 0) // anchor
 {
   myDiveAnchor(document.getElementById("placeDiv"),parent.document.getElementById("savedSrc").value.toLowerCase().replace("#",""));
 }
 else
 {
   // browsing for other protocols is not supported here
   parent.document.getElementById("cancel").onclick();
 }
}

var found=null;

function myDiveAnchor(root,seek)
{
 for(var i=0; i < root.childNodes.length; i++)
 {
   var el = root.childNodes.item(i);

   if(el.tagName && el.tagName.toLowerCase()=="div")
   {
     if(el.innerHTML == seek)
     {
       found = el;
       found.style.borderColor = "black";
       found.style.backgroundColor = "#F0F0F0";
     }
   }
 }
}

function myDiveHref(el,seek)
{
 if(el.firstChild && el.firstChild.tagName && el.firstChild.tagName.toLowerCase()=="span")
 {
  var href = el.firstChild.getAttribute("myAttrSrc").replace("%20"," ");
  var rel  = href.replace("<%= prefix %>","");

  if(href.length > 0)
  {
   if(href.toLowerCase() == seek || rel.toLowerCase() == seek)
   {
     ob_t25(el);
//     try{document.getElementById(tree_selected_id).scrollIntoView(false);} catch(e){}
     return true;
   }
  }
 }

 if(ob_hasChildren)
 {
  var n = ob_getChildCount(el);
  for(var i=0; i<n; i++)
    if(myDiveHref(ob_getChildAt(el,i,false),seek)) return true; 
 }

 return false;
}
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
</head>
<body onload="OnLoad();" style="background-color:white; overflow:auto;padding:0px;margin:0px;">
<div id="placeDiv" style="overflow:auto;padding-top:2px;padding-bottom:2px;">
  <ASP:Literal id="Place" runat="server" />
</div>
</body>
<script language="JavaScript" type="text/JavaScript">
function myOnClick(ev,src,isDouble)
{
  var el;
  if(document.all)
  {
    ev = window.event;
    el = ev.srcElement;
  }
  else
  {
    el = ev.target;
  }
  while(el && (!el.tagName || el.tagName.toUpperCase() != "DIV"))
        el=el.parentNode;

//
// Important. Change the Link URL in parent window.
//--------------------------------------------------------
  parent.document.getElementById("selectedSrc").value=src;
//--------------------------------------------------------

  if(isDouble)
  {
    // Raise parent's "ok" button click event
    //---------------------------------------
    parent.document.getElementById("ok").onclick();
  }
  else
  {
    if(found)
    {
      found.style.borderColor = "white";
      found.style.backgroundColor = "white";
    }

    found = el;
    found.style.borderColor = "black";
    found.style.backgroundColor = "#F0F0F0";
  }
  return false;
}

function myOnClickHref(ev,src,isDouble)
{
  var el;
  if(document.all)
  {
    ev = window.event;
    el = ev.srcElement;
  }
  else
  {
    el = ev.target;
  }
  while(el && (!el.tagName || el.tagName.toUpperCase() != "TD"))
        el=el.parentNode;

//
// Important. Change the Image URL in parent window.
//--------------------------------------------------------
  try
  {
    parent.document.getElementById("selectedSrc").value = src.replace("<%= prefix %>","");
  }
  catch(e){}
//--------------------------------------------------------


  if(isDouble)
  {
    // Raise parent's "ok" button click event
    //---------------------------------------
    parent.document.getElementById("ok").onclick();
  }
  else
    ob_t25(el);
  return false;
}

var collapsedId = null;

function ob_OnNodeSelect(id)
{
 if(collapsedId && (collapsedId == id))
 {
   ob_prev_selected.className = "ob_t2";
   parent.document.getElementById("selectedSrc").value="";
 }
 collapsedId = null;
}

function ob_OnNodeCollapse(id)
{
 collapsedId = id;
}

//----------------------------------------------------------------------
function ob_getChildAt (ob_od, index, expand)
{
        try
        {
                if (ob_od != null && ob_hasChildren(ob_od) && index >= 0)
                {
                        if (!ob_isExpanded(ob_od) && expand) 
                        try
                        {
                                ob_od.parentNode.firstChild.firstChild.onclick();
                        } catch (e) {}
                        return ob_od.parentNode.parentNode.parentNode.parentNode.firstChild.nextSibling.firstChild.firstChild.firstChild.nextSibling.childNodes[index].firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
                }
        }
        catch (e)
        {
        }

        return null;
}

if(typeof ob_t25 != "function")
  var ob_t25 = function(ob_od) 
  {                    
          // When tree is first loaded - Highlight and Extend selected node.
          if(ob_od==null) {
                  return
          };
          var e, lensrc, s;
          e = ob_od.parentNode.firstChild.firstChild;
          if ((typeof e.src != "undefined") && (e.tagName == "IMG")) {
                  s = e.src.substr((e.src.length - 8), 8);                
                  if ((s == "usik.gif") || (s == "ik_l.gif")) {
                          e.onclick();
                  }
          }
          ob_t22(ob_od);
  }
</script>
</html>