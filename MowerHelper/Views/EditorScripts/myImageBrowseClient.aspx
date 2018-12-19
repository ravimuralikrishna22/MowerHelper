<%@ Page Language="C#" Debug="true"%>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="obout_ASPTreeView_2_NET" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Xml.Schema" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<script language="C#" runat="server">
string errors_string = "";
string emptySrc = "";

//
// Images browser on another site
//
string externalImagesGalleryBrowser = "http://www.obout.com/editor_new/myImageBrowseServer.aspx";

void Page_Load(object sender, EventArgs e)
{
  XmlTextReader txtReader = null;
  XmlTextReader xmsReader = null;
  XmlReader reader = null;
  XmlDocument doc = null;
  XmlElement root = null;
  ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationCallback);

  if(System.Text.RegularExpressions.Regex.IsMatch(Page.Request.ServerVariables["HTTP_USER_AGENT"],@"MSIE (5|6)"))
  {
    emptySrc = "javascript:;";
  }

  try
  {
    txtReader = new XmlTextReader(externalImagesGalleryBrowser);
    xmsReader = new XmlTextReader(Page.MapPath("xml_schemas/ExtImagesGallerySchema.xsd"));
      
    // Create the XmlSchemaSet class.
    XmlSchemaSet sc = new XmlSchemaSet();

    // Add the schema to the collection.
    sc.Add("obout:editor:external-images-gallery-schema",xmsReader);

    // Set the validation settings.
    XmlReaderSettings settings = new XmlReaderSettings();
    settings.ValidationType = ValidationType.Schema;
    settings.Schemas = sc;
    settings.ValidationEventHandler += new ValidationEventHandler(eventHandler);
 
      
    reader = XmlReader.Create(txtReader,settings);
    doc = new XmlDocument();
    doc.Load(reader);
  }
  catch(Exception ex){errors_string = ex.Message;}
  finally
  {
    if(txtReader != null) txtReader.Close();
    if(xmsReader != null) xmsReader.Close();
    if(reader != null) reader.Close();
  }

  if(errors_string.Length > 0)
  {
    TreeView.Text = "<span style='color:red;'>"+errors_string+"</span>";
  }
  else
  if(doc != null)
  {
    root = doc.DocumentElement;

    if(root != null)
    {
      string curPath = ((XmlNode)root).Attributes["prefix"].Value;

      obout_ASPTreeView_2_NET.Tree oTree = new obout_ASPTreeView_2_NET.Tree();
      
      oTree.AddRootNode(((XmlNode)root).Attributes["name"].Value, "ball_redS.gif");

      oTree.FolderIcons = "/Content/Obout/TreeView/tree2/icons";
      oTree.FolderScript = "/Content/Obout/TreeView/tree2/script";
      oTree.FolderStyle = "/Content/Obout/TreeView/tree2/style/Classic";
          
      oTree.SelectedEnable = false;

      directoryDive(oTree, "root", (XmlNode)root, curPath, "");

      TreeView.Text = oTree.HTML();
    }
    else
    {
      TreeView.Text = "<span style='color:red;'>Not available external Images browser</span>";
    }
  }
}

private void ValidationCallback(object sender, ValidationEventArgs args )
{
  errors_string += "  "+args.Message+"<br/>";
}

private void directoryDive(obout_ASPTreeView_2_NET.Tree oTree, string parentID, XmlNode parentNode, string folderPath, string relPath)
{
  string currentID = (parentID=="root")?"myRoot":((parentID=="myRoot")?"a_":(parentID+"_"));
  int i =0;

  for(int j=0; j < parentNode.ChildNodes.Count; j++)
  {
     XmlNode node = parentNode.ChildNodes[j];

     if(node.NodeType == XmlNodeType.Element)
     {
       string name = node.Attributes["path"].Value.Replace("%22","\"");
       string pName= node.Attributes["name"].Value.Replace("%22","\"");

       if(node.Name.ToLower() == "folder")
       {
         oTree.Add (parentID, (currentID=="myRoot")?"myRoot":(currentID+i.ToString()), pName, (currentID=="myRoot"), null, null);
         directoryDive(oTree, (currentID=="myRoot")?"myRoot":(currentID+i.ToString()) ,node, folderPath+name+"/", relPath+name+"/");
       }
       else
       {
         string title  = node.Attributes["title" ].Value.Replace("%22","\\\"");
         string mWidth = node.Attributes["width" ].Value;
         string mHeight= node.Attributes["height"].Value;
         string html = "<span myAttrSrc='"+folderPath+name+"' myAttrRel='"+(relPath+name).Replace(" ","%20")+"'onmouseover=\"this.style.cursor='pointer'\" onclick=\"myOnClick(event,'"+folderPath+name+"',"+mWidth+","+mHeight+",'"+title+"',false)\" ondblclick=\"myOnClick(event,'"+(folderPath+name).Replace("%22","\\\"")+"',"+mWidth+","+mHeight+",'"+title+"',true)\">"+pName+"</span>";
         oTree.Add (parentID, currentID+i.ToString(), html, false, "square_yellowS.gif", null);
       }
       i++;
     }
  }
}

</script>

<script language="JavaScript" type="text/JavaScript">
function OnLoad()
{
 var height=parent.document.getElementById("innerIframe").offsetHeight;

 if(!document.all) height-=4;
 document.getElementById("tree").style.height = height-4+"px";

 var root = document.getElementById("myRoot");
 if(root != null)
   myDive(root,parent.document.getElementById("savedSrc").value.toLowerCase().replace("%20"," "));
}

function myDive(el,seek)
{
 if(el.firstChild && el.firstChild.tagName && el.firstChild.tagName.toLowerCase()=="span")
 {
  var href = el.firstChild.getAttribute("myAttrSrc").replace("%20"," ");
  if(href.length > 0)
  {
   if(href.toLowerCase() == seek)
   {
     document.getElementById("previewFrame").src = "<%= externalImagesGalleryBrowser %>?imgsrc="+el.firstChild.getAttribute("myAttrRel");
     document.getElementById("previewFrameText").src = "<%= externalImagesGalleryBrowser %>?imgprop="+el.firstChild.getAttribute("myAttrRel");
     document.getElementById("previewFrameTitle").src = "<%= externalImagesGalleryBrowser %>?imgtitle="+el.firstChild.getAttribute("myAttrRel");
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
<iframe frameborder="0" scrolling="no" id="previewFrameText" src="<%= emptySrc %>" style="background-color:white;padding:0px;margin:0px;_margin-right:8px;width:108px;height:12px;border-width:0px;overflow:hidden;">
</iframe>
<iframe frameborder="0" scrolling="no" id="previewFrame" src="<%= emptySrc %>" style="background-color:white;padding:0px;margin:0px;_margin-right:8px;width:108px;height:108px;border-width:0px;overflow:hidden;">
</iframe>
<iframe frameborder="0" scrolling="no" id="previewFrameTitle" src="<%= emptySrc %>" style="background-color:white;padding:0px;margin:0px;_margin-right:8px;width:108px;height:50px;border-width:0px;overflow:hidden;">
</iframe>
</td>
</tr>
</table>
</body>
<script language="JavaScript" type="text/JavaScript">
function myOnClick(ev,src,mWidth,mHeight,title,isDouble)
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
    parent.document.getElementById("selectedSrc").value=src;
    parent.document.getElementById("selectedWidth").value=mWidth;
    parent.document.getElementById("selectedHeight").value=mHeight;
    parent.document.getElementById("selectedAltText").value=title;
  }
  catch(e){}
//--------------------------------------------------------

  document.getElementById("previewFrameText").src = "<%= externalImagesGalleryBrowser %>?imgprop="+el.firstChild.getAttribute("myAttrRel");
  document.getElementById("previewFrame").src = "<%= externalImagesGalleryBrowser %>?imgsrc="+el.firstChild.getAttribute("myAttrRel");
  document.getElementById("previewFrameTitle").src = "<%= externalImagesGalleryBrowser %>?imgtitle="+el.firstChild.getAttribute("myAttrRel");

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
   document.getElementById("previewFrameText").src = "";
   document.getElementById("previewFrameTitle").src = "";
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