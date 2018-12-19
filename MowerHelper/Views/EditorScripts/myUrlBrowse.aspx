<%@ Page Language="C#" Debug="true"%>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="obout_ASPTreeView_2_NET" %>

<script language="C#" runat="server">
string prefix ="";
void Page_Load(object sender, EventArgs e)
{
  string currentProtocol = Page.Request["protocol"];
  string anchors = Page.Request["anchors" ];

  if(currentProtocol == "#")
  {
    // Let view all available anchors for Hrefs

    if(anchors.Length > 0)
    {
      string [] anchorsArray = anchors.Split('\n');

      for(int i=0; i<anchorsArray.Length; i++)
      {
        string html = "<div style='margin:1px; padding:1px; border-width:1px; border-color:white; border-style:solid; color:blue;background-color:white;' onmouseover=\"this.style.cursor='pointer'\" onclick=\"myOnClick(event,'#"+anchorsArray[i]+"',false)\" ondblclick=\"myOnClick(event,'#"+anchorsArray[i]+"',true)\">"+anchorsArray[i]+"</div>";
        Place.Text += html;
      }
    }
  }
  else
  if(currentProtocol == "http://" || currentProtocol == "https://")
  {
    // Let view all available values for Href

    string fileFolder = "~/Content/Obout/Editor/Content/files";
    bool ssl = (Page.Request.ServerVariables["HTTPS"]=="on");
    string protocol    = Page.Request.ServerVariables["SERVER_PROTOCOL"].Split('/')[0].ToLower()+(ssl?"s":"");
    string port        = ":"+Page.Request.ServerVariables["SERVER_PORT"];

    if(port==":80" || port==":443") port ="";

    prefix = protocol+"://"+Page.Request.ServerVariables["SERVER_NAME"]+port;
    string curPath= prefix;
    string folder = System.Web.HttpContext.Current.Server.MapPath(ResolveUrl(fileFolder));

    obout_ASPTreeView_2_NET.Tree oTree = new obout_ASPTreeView_2_NET.Tree();
    
    oTree.AddRootNode(Page.Request.ServerVariables["SERVER_NAME"]+port, "ball_redS.gif");

    oTree.FolderIcons = "/Content/Obout/TreeView/tree2/icons";
    oTree.FolderScript = "/Content/Obout/TreeView/tree2/script";
    oTree.FolderStyle = "/Content/Obout/TreeView/tree2/style/Classic";

        
    oTree.SelectedEnable = false;

    // Try to get values from database

    try
    {
      string m_connString;
      string strCmd = "SELECT * FROM tbUrl";

      m_connString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE={0};";
      m_connString = String.Format(m_connString, Server.MapPath("../App_Data/db.mdb"));

      OleDbDataAdapter da = new OleDbDataAdapter(strCmd, m_connString);
      DataSet ds = new DataSet();
      da.Fill(ds, "tbUrl");

      oTree.Add ("root", "dbRoot", "Links from database", false, null, null);
      int i =0;

      foreach(DataRow row in ds.Tables["tbUrl"].Rows)
      {
        string html = "<span style='text-decoration: underline' title='"+row["fldUrl"]+"' myAttrSrc='"+row["fldUrl"]+"' onmouseover=\"this.style.cursor='pointer'\" onclick=\"myOnClickHref(event,'"+row["fldUrl"]+"',false)\" ondblclick=\"myOnClickHref(event,'"+row["fldUrl"]+"',true)\">"+row["fldTitle"]+"</span>";
        oTree.Add ("dbRoot", "dbRoot_"+i.ToString(), html, false, "square_yellowS.gif", null);
        i++;
      }
    }
    catch{}

    // try to find folder with files

    if(Directory.Exists(folder))
    {
      oTree.Add ("root", "filesRoot", "Server files", false, null, null);
      directoryDive(oTree, "filesRoot", folder, curPath+ResolveUrl(fileFolder)+"/",ResolveUrl(fileFolder)+"/");
    }

    Place.Text = oTree.HTML();
  }
  else
  {
    // not supported protocol
  }
}

void directoryDive(obout_ASPTreeView_2_NET.Tree oTree, string parentID, string folder, string folderPath, string relPath)
{
  string currentID = (parentID=="filesRoot")?"a_":(parentID+"_");
  string[] entires = Directory.GetFileSystemEntries (folder);
  int i =0;

  foreach (string entire in entires) 
  {
     FileAttributes attr = File.GetAttributes(entire);
     string         name = Path.GetFileName(entire);

     if((attr & FileAttributes.Directory) == FileAttributes.Directory)
     {
       oTree.Add (parentID, currentID+i.ToString(), name, false, null, null);
       directoryDive(oTree, currentID+i.ToString() ,folder+"/"+name, folderPath+name+"/", relPath+name+"/");
     }
     else
     {
         string html = "<span myAttrSrc='"+folderPath+name+"' onmouseover=\"this.style.cursor='pointer'\" onclick=\"myOnClickHref(event,'"+folderPath+name+"',false)\" ondblclick=\"myOnClickHref(event,'"+folderPath+name+"',true)\">"+name+"</span>";
         oTree.Add (parentID, currentID+i.ToString(), html, false, "square_yellowS.gif", null);
     }
     i++;
  }
}
</script>

<script language="JavaScript" type="text/JavaScript">

var currentProtocol = "<%=Page.Request["protocol"] %>";
var anchors = "<%=Page.Request["anchors"].Replace("\n","\\n") %>";

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