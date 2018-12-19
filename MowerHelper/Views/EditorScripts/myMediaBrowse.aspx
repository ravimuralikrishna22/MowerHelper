
<%@ Page Language="C#" Debug="true"%>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="obout_ASPTreeView_2_NET" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<script language="C#" runat="server">
string emptySrc = "";
void Page_Load(object sender, EventArgs e)
{
        string mediaFolder = "~/Content/Obout/Editor/Content/MEDIA";

        int maxWidth = 100;
        int maxHeight= 100;

        bool ssl = (Page.Request.ServerVariables["HTTPS"]=="on");
        string protocol    = Page.Request.ServerVariables["SERVER_PROTOCOL"].Split('/')[0].ToLower()+(ssl?"s":"");
        string port        = ":"+Page.Request.ServerVariables["SERVER_PORT"];

        if(port==":80" || port==":443") port ="";

        if(System.Text.RegularExpressions.Regex.IsMatch(Page.Request.ServerVariables["HTTP_USER_AGENT"],@"MSIE (5|6)"))
        {
          emptySrc = "javascript:;";
        }

        string curPath= protocol+"://"+Page.Request.ServerVariables["SERVER_NAME"]+port;
        string folder = System.Web.HttpContext.Current.Server.MapPath(ResolveUrl(mediaFolder));

        if(Page.Request["mediasrc"] != null)
        if(Page.Request["mediasrc"].Length > 0)
        {
          string mediaFile = System.Web.HttpContext.Current.Server.MapPath(Page.Request["mediasrc"].Replace("%20"," "));

          if(File.Exists(mediaFile))
          {
            string cont="<embed showcontrols=\"0\" src=\""+Page.Request["mediasrc"].Replace("%20"," ")+"\" type=application/x-mplayer2 width="+maxWidth.ToString()+" height="+maxHeight.ToString()+"  pluginspage=\"http://download.microsoft.com/download/winmediaplayer/nsplugin/6.4/WIN98/EN-US/wmpplugin.exe\" >";

            Page.Response.Write("<html><body style='font-size:10px;margin:0px;padding:0px;padding-left:5px;'>");
            Page.Response.Write(cont);
            Page.Response.Write("</body></html>");
            Page.Response.End();
          }
          return;
        }

        obout_ASPTreeView_2_NET.Tree oTree = new obout_ASPTreeView_2_NET.Tree();
        
        oTree.AddRootNode(Page.Request.ServerVariables["SERVER_NAME"]+port, "ball_redS.gif");

        oTree.FolderIcons = "/Content/Obout/TreeView/tree2/icons";
        oTree.FolderScript = "/Content/Obout/TreeView/tree2/script";
        oTree.FolderStyle = "/Content/Obout/TreeView/tree2/style/Classic";
            
        oTree.SelectedEnable = false;

        if(Directory.Exists(folder))
        {
          oTree.Add ("root", "myRoot", "Media Root", true, null, null);
          string currentPath = ResolveUrl(Path.GetDirectoryName(Page.Request.ServerVariables["SCRIPT_NAME"])).Replace("\\","/")+"/";
          string mediaPath = ResolveUrl(mediaFolder);
          string differ = mediaPath.Replace(currentPath,"");
          directoryDive(oTree, "myRoot", folder, curPath+ResolveUrl(mediaFolder)+"/",differ+"/");
        }

        TreeView.Text = oTree.HTML();
}

void directoryDive(obout_ASPTreeView_2_NET.Tree oTree, string parentID, string folder, string folderPath, string relPath)
{
  string currentID = (parentID=="myRoot")?"a_":(parentID+"_");
  string[] entires = Directory.GetFileSystemEntries (folder);
  int i =0;

  foreach (string entire in entires) 
  {
     FileAttributes attr = File.GetAttributes(entire);
     string         name = Path.GetFileName(entire);
     string         pName= Path.GetFileNameWithoutExtension(entire);

     if((attr & FileAttributes.Directory) == FileAttributes.Directory)
     {
       oTree.Add (parentID, currentID+i.ToString(), pName, false, null, null);
       directoryDive(oTree, currentID+i.ToString() ,folder+"/"+name, folderPath+name+"/", relPath+name+"/");
     }
     else
     {
       string ext = Path.GetExtension(entire).ToLower();
       if(ext==".mpeg" || ext==".avi")
       {
         string html = "<span myAttrSrc='"+folderPath+name+"' myAttrRel='"+(relPath+name).Replace(" ","%20")+"'onmouseover=\"this.style.cursor='pointer'\" onclick=\"myOnClick(event,'"+folderPath+name+"','"+(relPath+name).Replace(" ","%20")+"',false)\" ondblclick=\"myOnClick(event,'"+folderPath+name+"','"+(relPath+name).Replace(" ","%20")+"',true)\">"+pName+"</span>";
         oTree.Add (parentID, currentID+i.ToString(), html, false, "square_yellowS.gif", null);
       }
     }
     i++;
  }
}

</script>

<script language="JavaScript" type="text/JavaScript">
//var savedOkClick;
//var savedCancelClick;

function OnLoad()
{
 var height=parent.document.getElementById("innerIframe").offsetHeight;

 if(!document.all) height-=4;
 document.getElementById("tree").style.height = height-4+"px";

 myDive(document.getElementById("myRoot"),parent.document.getElementById("savedSrc").value.toLowerCase().replace("%20"," "));
}

function myDive(el,seek)
{
 if(el==null) return false;
 if(el.firstChild && el.firstChild.tagName && el.firstChild.tagName.toLowerCase()=="span")
 {
  var href    = el.firstChild.getAttribute("myAttrSrc").replace("%20"," ");
  var hrefrel = el.firstChild.getAttribute("myAttrRel").replace("%20"," ");
  var found   = false;

  if(href.length > 0 && href.toLowerCase() == seek) found = true;
  if(!found && hrefrel.length > 0 && hrefrel.toLowerCase() == seek) found = true;

  if(found)
  {
    document.getElementById("previewFrame").src = window.location.href+"&mediasrc="+el.firstChild.getAttribute("myAttrRel");
    ob_t25(el);
    return true;
  }
 }

 if(ob_hasChildren)
 {
  var n = ob_getChildCount(el);
  for(var i=0; i<n; i++)
    if(myDive(ob_getChildAt(el,i,false),seek)) return true; 
 }

 return false;
}
</script>

<head runat="server">
    <title></title>
</head>
<body onload="OnLoad();" style="background-color:white; overflow:hidden;padding:0px;margin:0px;">
<table border="0" cellspacing="0" cellpadding="0" style="height:100%;width:100%;">
<tr>
<td align="left" valign="top" style="border-right-width:1px;border-right-style:solid;border-right-color:gray;">
 <div id="tree" style="overflow:auto;padding-top:2px;padding-bottom:2px;">
   <ASP:Literal id="TreeView" EnableViewState="false" runat="server" />
 </div>
</td>
<td align="center" valign="top" style="padding-top:2px;width:110px;">
<iframe frameborder="0" scrolling="no" id="previewFrame" src="<%= emptySrc %>" style="background-color:white;padding:0px;margin:0px;_margin-right:8px;width:108px;height:108px;border-width:0px;overflow:hidden;">
</iframe>
</td>
</tr>
</table>
</body>
<script language="JavaScript" type="text/JavaScript">
function myOnClick(ev,src,rel,isDouble)
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
// Important. Change the media URL in parent window.
//--------------------------------------------------------
  try
  {
    parent.document.getElementById("selectedSrc").value=src;
    parent.document.getElementById("selectedRel").value=rel;
  }
  catch(e){}
//--------------------------------------------------------

  document.getElementById("previewFrame").src = window.location.href+"&mediasrc="+el.firstChild.getAttribute("myAttrRel");

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
   document.getElementById("previewFrame").src = "";
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