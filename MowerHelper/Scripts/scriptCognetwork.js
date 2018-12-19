function revString(str) {
    var retStr = ""; for (i = str.length - 1; i > -1; i--) { retStr += str.substr(i, 1); }
    return retStr;
}

function trim(s)
{ s = s.replace(/^\s*/, "").replace(/\s*$/, ""); return s; }
function formatCurrency(ctrlname, decPoint) {
    var exponentialLen = ctrlname.value.slice(ctrlname.value.indexOf('.')).length - 1; var str = ctrlname.value; if (str.indexOf('.', 0) != -1) {
        if (exponentialLen > decPoint) {
            str = Math.round(parseFloat(str) * Math.pow(10, decPoint)) / Math.pow(10, decPoint); str = str.toString()
            if (str.indexOf('.') == -1) {
                str = str + '.'
                for (i = 0; i < decPoint; i++)
                { str = str + '0'; }
            }
        }
        if (exponentialLen < decPoint) {
            var intDiff = parseInt(decPoint) - parseInt(exponentialLen); for (i = 0; i < intDiff; i++)
            { str = str + '0'; }
        }
    }
    else {
        if (str.length > 0)
        { str = str + "."; }
        else
        { str = "0."; }
        for (i = 0; i < decPoint; i++)
        { str = str + '0'; }
    }
    ctrlname.value = str;
}
function fnCheckSpecialChars(ctl) {
    if (ctl.value.length > 0) {
        var str = ctl.value; var len = ctl.value.length; for (i = 0; i < len; i++) {
            var k = str.charAt(i); var st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz0123456789.',/-:;_()"; if ((st.indexOf(k, 0)) == -1)
            { return false; }
        }
    }
    return true;
}
function fnCheckNames(ctl) {
    if (ctl.value.length > 0) {
        var str = ctl.value; var len = ctl.value.length; for (i = 0; i < len; i++) {
            var k = str.charAt(i); var st = "0123456789"; if ((st.indexOf(k, 0)) != -1)
            { return false; }
        }
    }
    return true;
}
function Mid(str, start, len) {
    if (start < 0 || len < 0) return ""; var iEnd, iLen = String(str).length; if (start + len > iLen)
        iEnd = iLen; else
        iEnd = start + len; return String(str).substring(start, iEnd);
}
function fnSpaceError(ctl) {
    if (ctl.value.length > 0) {
        var str = ctl.value; var len = ctl.value.length; for (i = 0; i < len; i++) {
            var k = str.charAt(i); if (k == " ")
            { return "false"; }
        }
    }
    return true;
}
function CheckSpace(ctl) {
    var k = ctl.charAt(0); if (k == " ")
    { return (false); }
    return (true);
}
function validateUrl(objUrl) {
    var re; re = /(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?/; objUrl = 'http://' + objUrl
    if (re.test(objUrl) == true)
        return true; else
    { return false; }
}
function MyChars(ctl) {
  
    if (ctl.value.length == 0)
    { return "BlankError"; }

    if (ctl.value.length > 0) {
        if (CheckSpace(ctl.value) == false)
        { return "SpaceError"; }
    }
    if (ctl.value.length > 0) {
        var str = ctl.value;
        var k = str.charAt(0); var st = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'-"; if ((st.indexOf(k, 0)) == -1)
        { return "InvalidCharAtFirstError"; }
    }
    if (ctl.value.length > 0) {
        var str = ctl.value; var len = ctl.value.length; for (i = 0; i < len; i++) {
            var k = str.charAt(i); var st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz'-"; if ((st.indexOf(k, 0)) == -1)
            { return "NumError"; }
        }
    }
    if (ctl.value.length > 0) {
        var str = ctl.value; var len = ctl.value.length; for (i = 0; i < len; i++) {
            var k = str.charAt(i); var st = " "; if (k == st)
            { return "SpaceInBwnError"; }
        }
    }
    return (true);
}
function fnDtDisableCtrl() {
    if ((event.keyCode != 9) && (event.keyCode != 46))
    { event.keyCode = 0; return false; }
}
function MyNumbers(ctl) {
    if (ctl.value.length == 0)
    { return false; }
    else if (ctl.value.length > 0) {
        var str = ctl.value; var len = ctl.value.length; for (i = 0; i < len; i++) {
            var k = str.charAt(i); var st = "0123456789()-.%"; if ((st.indexOf(k, 0)) == -1)
            { return false; }
        }
    }
    return (true);
}
var testresults
function ValidateEmail(ctrl) {
    if (document.layers || document.getElementById || document.all) {
        var str = ctrl.value
        if (ctrl.value == undefined) {
            str = ctrl
        }
        else {
            str = ctrl.value
        }
        var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
        if (filter.test(str))
            testresults = true
        else
            testresults = false
        return (testresults)
    }
    else
        return true;
}
function reformat(s) {
    var arg; var sPos = 0; var resultString = ""; for (var i = 1; i < reformat.arguments.length; i++) { arg = reformat.arguments[i]; if (i % 2 == 1) resultString += arg; else { resultString += s.substring(sPos, sPos + arg); sPos += arg; } }
    return resultString;
}
var SSNDelimiters = "- "; var defaultEmptyOK = false; var digitsInSocialSecurityNumber = 9; function reformatSSN(SSN)
{ return (reformat(SSN, "", 3, "-", 2, "-", 4)) }
function checkSSN(theField, emptyOK) {
    if (checkSSN.arguments.length == 1) emptyOK = defaultEmptyOK; if ((emptyOK == true) && (isEmpty(theField.value))) return true; else {
        var normalizedSSN = stripCharsInBag(theField.value, SSNDelimiters)
        if (!isSSN(normalizedSSN, false))
            return false; else
        { return true; }
    }
}
function isSSN(s) {
    if (isEmpty(s))
        if (isSSN.arguments.length == 1) return defaultEmptyOK; else return (isSSN.arguments[1] == true); return (isInteger(s) && s.length == digitsInSocialSecurityNumber)
}
function isEmpty(s)
{ return ((s == null) || (s.length == 0)) }
function isInteger(s) {
    var i; if (isEmpty(s))
        if (isInteger.arguments.length == 1) return defaultEmptyOK; else return (isInteger.arguments[1] == true); for (i = 0; i < s.length; i++)
    { var c = s.charAt(i); if (!isDigit(c)) return false; }
    return true;
}
function isDigit(c)
{ return ((c >= "0") && (c <= "9")) }
function stripCharsInBag(s, bag) {
    var i; var returnString = ""; for (i = 0; i < s.length; i++)
    { var c = s.charAt(i); if (bag.indexOf(c) == -1) returnString += c; }
    return returnString;
}
var digitsInUSPhoneNumber = 10; var phoneNumberDelimiters = "()- "; function isUSPhoneNumber(s) {
    if (isEmpty(s))
        if (isUSPhoneNumber.arguments.length == 1) return defaultEmptyOK; else return (isUSPhoneNumber.arguments[1] == true); return (isInteger(s) && s.length == digitsInUSPhoneNumber)
}
function reformatUSPhone(USPhone)
{ return (reformat(USPhone, "(", 3, ") ", 3, "-", 4)) }
function checkUSPhone(theField, emptyOK) {
    if (checkUSPhone.arguments.length == 1) emptyOK = defaultEmptyOK; if ((emptyOK == true) && (isEmpty(theField.value))) return true; else {
        var normalizedPhone = stripCharsInBag(theField.value, phoneNumberDelimiters)
        if (!isUSPhoneNumber(normalizedPhone, false))
            return false; else
        { return true; }
    }
}
var ZIPCodeDelimeter = "-"
var ZIPCodeDelimiters = "-"; var digitsInZIPCode1 = 5
var digitsInZIPCode2 = 9
function checkZIPCode(theField, emptyOK) {
    if (checkZIPCode.arguments.length == 1) emptyOK = defaultEmptyOK; if ((emptyOK == true) && (isEmpty(theField.value))) return true; else {
        var normalizedZIP = stripCharsInBag(theField.value, ZIPCodeDelimiters)
        if (!isZIPCode(normalizedZIP, false))
            return false; else {
            theField.value = reformatZIPCode(normalizedZIP)
            return true;
        }
    }
}
function isZIPCode(s) {
    if (isEmpty(s))
        if (isZIPCode.arguments.length == 1) return defaultEmptyOK; else return (isZIPCode.arguments[1] == true); return (isInteger(s) && ((s.length == digitsInZIPCode1) || (s.length == digitsInZIPCode2)))
}
function reformatZIPCode(ZIPString)
{ if (ZIPString.length == 5) return ZIPString; else return (reformat(ZIPString, "", 5, "-", 4)); }
function fnCheckChars(ctl) {
    if (ctl.value.length > 0) {
        var str = ctl.value; var len = ctl.value.length; for (i = 0; i < len; i++) {
            var k = str.charAt(i); var st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz"; if ((st.indexOf(k, 0)) != -1)
            { return false; }
        }
    }
    return true;
}
function IsValidDate(DOB) {
    sysDate = new Date(); txtDate = new Date(DOB); if (txtDate < sysDate)
        return true; else
        return false;
}
function IsZip(ZIPString) {
    if (ZIPString.length != 5)
    { return false; }
    if (ZIPString.length == 0)
    { return false; }
    else if (ZIPString.length = 5) {
        var str = ZIPString; var len = ZIPString.length; for (i = 0; i < len; i++) {
            var k = str.charAt(i); var st = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"; if ((st.indexOf(k.toUpperCase(), 0)) == -1)
            { return false; }
        }
    }
    return (true);
}
function Glb_ShowAlert(txtMsg, OperationType, txtCANCEL, txtOK, Title, Dlgwidth, DlgHieght, txtHead, BlnScroll) {
    //alert(navigator.appName);
    if (navigator.appName == 'Microsoft Internet Explorer') {
        if (!Dlgwidth) {
            Dlgwidth = 350;
        }
        if (!DlgHieght) {
            DlgHieght = 170 + 15 * (parseInt(txtMsg.length / 37));
        }
        if (!txtHead) {
            txtHead = ''
        }
        if (!OperationType) {
            OperationType = 'Alert'
        }
        if (!Title) {
            Title = document.title;
        }
        else {
            Title = Title;
        }
        if (!BlnScroll)
        { BlnScroll = 'No'; }
        if (!txtCANCEL) {
            if (OperationType == 'Delete')
            { txtCANCEL = 'No' }
            if (OperationType == 'Help')
            { txtCANCEL = '' }
            if (OperationType == 'Alert')
            { txtCANCEL = 'Ok' }
            if (OperationType == 'Info')
            { txtCANCEL = 'Ok' }
            if (OperationType == 'Save')
            { txtCANCEL = 'No' }
            if (OperationType == 'Edit')
            { txtCANCEL = 'No' }
            if (OperationType == 'Task')
            { txtCANCEL = 'Others' }
            if (OperationType == 'CreditCardAlert')
            { txtCANCEL = 'Ok' }
            if (OperationType == 'Later')
            { txtCANCEL = 'Now' }
        }
        if (!txtOK) {
            if (OperationType == 'Delete')
            { txtOK = 'Yes' }
            if (OperationType == 'Help')
            { txtOK = 'Close' }
            if (OperationType == 'Alert')
            { txtOK = '' }
            if (OperationType == 'Info')
            { txtOK = '' }
            if (OperationType == 'Save')
            { txtOK = 'Yes' }
            if (OperationType == 'Edit')
            { txtOK = 'Yes' }
            if (OperationType == 'Task')
            { txtOK = 'Self' }
            if (OperationType == 'CreditCardAlert')
            { txtOK = '' }
            if (OperationType == 'Later')
            { txtCANCEL = 'Later' }
        }
        var Wndfeatures
        Wndfeatures = 'dialogHeight:' + DlgHieght + 'px; dialogWidth:' + Dlgwidth + 'px;'; Wndfeatures = Wndfeatures + 'scroll:' + BlnScroll + ';edge: Raised;center: Yes; help: No; resizable: No; status: No;'; var AlertArray = new Array(txtHead, txtMsg, txtOK, txtCANCEL, Title)
        alert(OperationType);
        if (OperationType == 'Delete')
        { var str = window.showModalDialog("../Modal/DeleteTemplate.htm", AlertArray, Wndfeatures); return str; }
        if (OperationType == 'Help')
        { var str = window.showModalDialog("../Modal/HelpTemplate.htm", AlertArray, Wndfeatures); return str; }
        if (OperationType == 'Alert')
        { var str = window.showModalDialog("../Modal/AlertTemplate.htm", AlertArray, Wndfeatures); return str; }
        if (OperationType == 'Info')
        { var str = window.showModalDialog("../Modal/InfoTemplate.htm", AlertArray, Wndfeatures); return str; }
        if (OperationType == 'Save')
        { var str = window.showModalDialog("../Modal/SaveTemplate.htm", AlertArray, Wndfeatures); return str; }
        if (OperationType == 'Task')
        { var str = window.showModalDialog("../Modal/SaveTemplate.htm", AlertArray, Wndfeatures); return str; }
        if (OperationType == 'Edit')
        { var str = window.showModalDialog("../Modal/EditTemplate.htm", AlertArray, Wndfeatures); return str; }
        if (OperationType == 'CreditCardAlert')
        { var str = window.showModalDialog("../Modal/Creditcard Success.htm", AlertArray, Wndfeatures); return str; }
        if (OperationType == 'Later')
        { var str = window.showModalDialog("../Modal/Later.htm", AlertArray, Wndfeatures); return str; }
    }
    else {
        if (OperationType == undefined)
        { alert(txtMsg); }
        else
        {
            if (OperationType == 'Delete' || OperationType == 'Save') {
                var truthBeTold = window.confirm(txtMsg); if (truthBeTold == true)
                { return 'True'; }
                else
                { return 'False'; }
            }
            if (OperationType == '')
            { window.alert(txtMsg); }
            else
            {
                var objVar
                objVar = confirm(txtMsg); if (objVar == true)
                { return true; }
                else
                { return false; }
            }
        }
    }
}
function isPhone(stringToSplit) {
    var Noflag = 0
    var Yesflag = 1
    var separator = "-"; arrayOfStrings = stringToSplit.split(separator); if (arrayOfStrings.length != 3)
        return Noflag; if ((arrayOfStrings[0]).length != 3 || (arrayOfStrings[1]).length != 3 || (arrayOfStrings[2]).length != 4)
            return Noflag; if (arrayOfStrings.length == 3) {
                if ((isNaN(arrayOfStrings[0])) || (isNaN(arrayOfStrings[1])) || (isNaN(arrayOfStrings[2])))
                    return Noflag;
            }
            else
                return Noflag; return Yesflag;
}
function isValidSSN(strSSN) {
    if (strSSN.length == '9')
    { return 'true'; }
    else
    { return 'false'; }
}
function isValidNumeric(ctrlname, mxlen, decpoint, e) {
    if (navigator.appName == 'Microsoft Internet Explorer') {
        e = window.event.keyCode
    }
    else {
        e = e.which
    }
    if (decpoint == 0) {
        if (e == 190 || e == 110)
        { return false; }
    }
    ctrLen = ctrlname.value.length + 1; if ((ctrLen > mxlen) && (e != 8) && (e != 9) && (e != 46))
    { return false; }
    if (e == 190 || e == 110) {
        if (ctrlname.value.indexOf('.') == -1)
        { var strval = ctrlname.value; }
        else
        { return false; }
    }
    if (navigator.appName == 'Microsoft Internet Explorer') {
        if (window.event.shiftKey)
        { return false; }
    }
    if ((e != 8) && (e != 9) && (e != 35) && (e != 36) && (e != 37) && (e != 39) && (e != 46) && (e != 48) && (e != 49) && (e != 50) && (e != 51) && (e != 52) && (e != 53) && (e != 54) && (e != 55) && (e != 56) && (e != 57) && (e != 58) && (e != 96) && (e != 97) && (e != 98) && (e != 99) && (e != 100) && (e != 101) && (e != 102) && (e != 103) && (e != 104) && (e != 105) && (e != 110) && (e != 190))
    { return false; }
    if (decpoint > 0 && e != 8 && e != 35 && e != 36 && e != 37 && e != 39 && e != 39) {
        if (ctrlname.value.indexOf('.') != -1) {
            arrayOfStrings = ctrlname.value.slice(ctrlname.value.indexOf('.')); if ((ctrlname.value.slice(ctrlname.value.indexOf('.')).length) > decpoint)
            { return false; }
        }
    }
}
function IsNumaricNumber(ctrlname, mxlen) {
    ctrLen = ctrlname.value.length + 1; if ((ctrLen > mxlen) && (event.keyCode != 8))
    { return false; }
    if ((event.keyCode != 9) && (event.keyCode != 46) && (event.keyCode != 8) && (event.keyCode != 48) && (event.keyCode != 189) && (event.keyCode != 190) && (event.keyCode != 49) && (event.keyCode != 50) && (event.keyCode != 51) && (event.keyCode != 52) && (event.keyCode != 53) && (event.keyCode != 54) && (event.keyCode != 55) && (event.keyCode != 56) && (event.keyCode != 57) && (event.keyCode != 58))
    { event.keyCode = 0; return false; }
}
function IsValidPhone(strPhone) {
    if (strPhone.length == '10')
    { return 'true'; }
    else
    { return 'false'; }
}
function gotoNext(ctrlname, mxlen, decpoint) {
    frmName = document.forms[document.forms.length - 1]; ctrLen = ctrlname.value.length + 1
    if (ctrLen > mxlen) {
        for (i = 0; i + 1 < eval("document." + frmName.name + ".elements.length") ; i++) {
            if (eval("document." + frmName.name + ".elements[i].name") == ctrlname.name)
            { eval("document." + frmName.name + ".elements[i+1].focus()"); }
        }
    }
}
function Global_Alert(strName) {
    count = 0; pos = strName.indexOf("<br>"); while (pos != -1)
    { count = count + 20; pos = strName.indexOf("<br>", pos + 1); }
    if (navigator.appName == 'Microsoft Internet Explorer') {
        strName = "<br>" + strName
        var StrMsg = Glb_ShowAlert('The following fields are required ' + strName + '  Do you want to enter now?', 'Edit', 'No', 'Yes', 'Practice-Solutions -- Powered by Cognode --', 400, 120 + count); return StrMsg;
    }
    else { strName = "<br>" + strName; var str = strName; str = str.replace(/<br>/gi, "\n"); var StrMsg = confirm('The following fields are required ' + str + '  Do you want to enter now?', 'Edit', 'No', 'Yes', 'Practice-Solutions -- Powered by Cognode --', 400, 120 + count); return StrMsg; }
}
var testresults; function isValidEmail(ctrl) {
    if (document.layers || document.getElementById || document.all) {
        var str = ctrl.value
        var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
        if (filter.test(str))
            testresults = true
        else
            testresults = false
        return (testresults)
    }
    else
        return true;
}
function isValidMaxlength(obj, ctrlMaxLength) {
    if (event.keyCode != 8 && event.keyCode != 46) {
        if (obj.value.length >= ctrlMaxLength) {
            var strText = obj.value
            obj.value = strText.substr(0, ctrlMaxLength)
            return false
        }
    }
}
var PopUpWindowFeatures; PopUpWindowFeatures = 'scroll:yes;edge: Raised;dialogtop:1px;dialogleft:1px;help: No; resizable: No; status: No;'; function fnStringFieldValidate(strActualString, strValidValues) {
    var iStringLength
    var iCount
    var iIndex
    var strActString = strActualString.value; iStringLength = strActualString.value.length
    for (iCount = 0; iCount < iStringLength; iCount++) {
        strChar = strActString.charAt(iCount); iIndex = strValidValues.indexOf(strChar.toUpperCase(), 0); if (iIndex == -1)
            return false;
    }
}
function mouse_img(ObjID, flag) {
    if (flag == 0) { document.getElementById(ObjID).className = "borderimg"; }
    else { document.getElementById(ObjID).className = "borderimg_hover"; }
}
function checkValidNumeric(ctrlname, mxlen, decpoint, e) {
    if (document.all)
    { var e = window.event.keyCode; }
    else
    { e = e.which; }
    var strNumeric = '1234567890'
    var str = ctrlname.value; ctrLen = ctrlname.value.length + 1; ctrlname.maxLength = mxlen; if (decpoint == 0 || decpoint == '') {
        if (e > 0) {
            if (strNumeric.indexOf(String.fromCharCode(e)) == -1 && e != 8)
            { return false; }
        }
    }
    if (decpoint > 0 && e != 8) {
        if (ctrlname.value.indexOf('.') != -1) {
            if (e == 46)
            { return false; }
            if ((ctrlname.value.slice(ctrlname.value.indexOf('.')).length) > decpoint) {
                var currentRange; if (document.all)
                { currentRange = document.selection.createRange().text; }
                else
                { currentRange = ctrlname.value.substring(ctrlname.selectionStart, ctrlname.selectionEnd); }
                if (currentRange != '') {
                    if (currentRange.indexOf('.') == -1) {
                        if (ctrlname.value.indexOf('.') != -1) {
                            var pos1 = ctrlname.value.indexOf('.')
                            var pos2 = doGetCaretPosition(ctrlname); if (e != 0) {
                                if (ctrlname.value.length != pos2) {
                                    if (pos2 > pos1 + 1)
                                    { return false; }
                                }
                            }
                        }
                    }
                }
                else {
                    if (ctrlname.value.indexOf('.') != -1) {
                        var pos1 = ctrlname.value.indexOf('.')
                        var pos2 = doGetCaretPosition(ctrlname); if (e != 0) {
                            if (pos2 > pos1)
                            { return false; }
                        }
                    }
                }
            }
        }
        if (strNumeric.indexOf(String.fromCharCode(e)) == -1 && e != 8 && e != 46 && e != 0)
        { return false; }
    }
}
function doGetCaretPosition(oField) {
    var iCaretPos = 0; if (document.selection) { oField.focus(); var oSel = document.selection.createRange(); oSel.moveStart('character', -oField.value.length); iCaretPos = oSel.text.length; }
    else if (oField.selectionStart || oField.selectionStart == '0')
        iCaretPos = oField.selectionStart; return (iCaretPos);
}
function checkValidDOB(e) {
    if (document.all)
    { var e = window.event.keyCode; }
    else
    { var e = e.which; }
    var strNumeric = ''
    if (strNumeric.indexOf(String.fromCharCode(e)) == -1 && e != 46 && e != 9 && e != 16)
    { return false; }
}
function checkNumeric(e) {
    var keynum; var keychar; var numcheck; if (window.event) {
        keynum = (e.which) ? e.which : e.keyCode; if ((keynum >= 48 && keynum <= 57) || keynum == 13)
            e.keyCode = e.keyCode; else
            return false;
    }
    else if (e.which) {
        keynum = e.which; keychar = String.fromCharCode(keynum); if (keynum != 8)
        { numcheck = /\d/; return numcheck.test(keychar); }
        else
            return true;
    }
}
function checknumdate(e) {
    var keynum; var keychar; var numcheck; if (window.event) {
        keynum = (e.which) ? e.which : e.keyCode; if ((keynum >= 47 && keynum <= 57) || keynum == 13)
            e.keyCode = e.keyCode; else
            return false;
    }
    else if (e.which) {
        keynum = e.which; keychar = String.fromCharCode(keynum); if (keynum != 8)
        { numcheck = /[0-9]|\//; return numcheck.test(keychar); }
        else
            return true;
    }
}
function ValidZip(e) {
    var keynum; var keychar; var numcheck; if (window.event) {
        keynum = e.keyCode; if ((keynum >= 48 && keynum <= 57) || keynum == 13 || (keynum >= 97 && keynum <= 122) || (keynum >= 65 && keynum <= 90))
            e.keyCode = e.keyCode; else
            e.keyCode = 0; return true;
    }
    else if (e.which) {
        keynum = e.which; keychar = String.fromCharCode(keynum); if (keynum != 8 && keynum != 95)
        { numcheck = /\w/; return numcheck.test(keychar); }
        else
        {
            if (keynum != 95)
            { return true; }
            else
                return false;
        }
    }
}
function checkTextAreaMaxLength(textBox, e, length) {
    var mLen = textBox["MaxLength"]; if (null == mLen)
        mLen = length; var maxLength = parseInt(mLen); if (!checkSpecialKeys(e)) {
            if (textBox.value.length > maxLength - 1) {
                if (window.event)
                    e.returnValue = false; else
                    e.preventDefault();
            }
        }
}
function checkSpecialKeys(e) {
    if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
        return false; else
        return true;
}
function checkValidNames(ctrlname1, e) {
    var ctrlname = document.getElementById(ctrlname1); if (document.all)
    { var e = window.event.keyCode; }
    else
    { e = e.which; }
    var strNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz0123456789.',/-:;_.'"; var str = ctrlname.value; ctrLen = ctrlname.value.length + 1; if (strNumeric.indexOf(String.fromCharCode(e)) == -1 && e != 8 && e != 0)
    { return false; }
}
function ClearDate(CtrlName) {
    if (document.getElementById(CtrlName).value == 'mm/dd/yyyy')
    { document.getElementById(CtrlName).value = ''; }
}
function CheckValidDate(ctrlname) {
    if (ctrlname.value.length > 0) {
        var date = ctrlname.value; var len = ctrlname.value.length; for (var i = 0; i < len; i++) {
            var k = date.charAt(i); var digits = "0123456789/"; if ((digits.indexOf(k, 0)) == -1)
            { Glb_ShowAlert('Please enter valid date'); ctrlname.value = ''; ctrlname.focus(); return false; }
        }
        return ValidateForm(ctrlname);
    }
}
function stopSpaceKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null); var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null); if (evt.keyCode == 32) {
        if (window.navigator.appName == "Microsoft Internet Explorer")
        { Glb_ShowAlert('Spaces are not allowed'); return false; }
        else
        { return false; }
    }
    else { return true; }
}
function ValidateForm(dt) {
    if (isDate(dt.value) == false) {
        dt.focus()
        return false
    }
    return true
}
var dtCh = "/"; var minYear = 1900; var maxYear = 2060; function isInteger(s) {
    var i; for (i = 0; i < s.length; i++) { var c = s.charAt(i); if (((c < "0") || (c > "9"))) return false; }
    return true;
}
function stripCharsInBag(s, bag) {
    var i; var returnString = ""; for (i = 0; i < s.length; i++) { var c = s.charAt(i); if (bag.indexOf(c) == -1) returnString += c; }
    return returnString;
}
function daysInFebruary(year) { return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28); }
function DaysArray(n) {
    for (var i = 1; i <= n; i++) {
        this[i] = 31
        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
        if (i == 2) { this[i] = 29 }
    }
    return this
}
function daysInMonth1(month, year)
{ return new Date(year, month, 0).getDate(); }
function isDate(dtStr) {
    var daysInMonth = DaysArray(12)
    var pos1 = dtStr.indexOf(dtCh)
    var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
    var strMonth = dtStr.substring(0, pos1)
    var strDay = dtStr.substring(pos1 + 1, pos2)
    var strYear = dtStr.substring(pos2 + 1)
    strYr = strYear
    if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
    if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
    for (var i = 1; i <= 3; i++) { if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1) }
    month = parseInt(strMonth)
    day = parseInt(strDay)
    year = parseInt(strYr)
    if (pos1 == -1 || pos2 == -1) {
        Glb_ShowAlert("The date format should be : mm/dd/yyyy")
        return false
    }
    if (strMonth.length < 1 || month < 1 || month > 12) {
        Glb_ShowAlert("Please enter a valid month")
        return false
    }
    if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth1(month, year)) {
        Glb_ShowAlert("Please enter a valid day")
        return false
    }
    if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
        Glb_ShowAlert("Please enter a valid 4 digit year between " + minYear + " and " + maxYear)
        return false
    }
    if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
        Glb_ShowAlert("Please enter a valid date")
        return false
    }
    return true
}
function ValidedateEntry(ctrlname, e) {
    if (document.all)
    { var e = window.event.keyCode; }
    else
    { e = e.which; }
    var strNumeric = '1234567890/'
    var str = ctrlname.value; ctrLen = ctrlname.value.length + 1; if (e > 0) {
        if (strNumeric.indexOf(String.fromCharCode(e)) == -1 && e != 8)
        { return false; }
    }
}
function getScrollBottom(p_oElem) {
    if (p_oElem.scrollHeight < 400)
    { return 1; }
    else
    { return p_oElem.scrollHeight - p_oElem.scrollTop - p_oElem.clientHeight }
}
function ltrim(str, chars) { chars = chars || "\\s"; return str.replace(new RegExp("^[" + chars + "]+", "g"), ""); }
function rtrim(str, chars) { chars = chars || "\\s"; return str.replace(new RegExp("[" + chars + "]+$", "g"), ""); }
function jsIsUserFriendlyChar(val, step) {
    if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
        return true;
    }
    if ((val > 16 && val < 21) || (val > 34 && val < 41)) {
        return true;
    }
    if (step == "Decimals") {
        if (val == 190 || val == 110) {
            return true;
        }
    }
    return false;
}
function checkValidNumeric1(e) {
    var evt = (e) ? e : window.event;
    var key = (evt.keyCode) ? evt.keyCode : evt.which;
    if (key != null) {
        key = parseInt(key, 10);
        if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
            if (!jsIsUserFriendlyChar(key, "Decimals")) {
                return false;
            }
        }
        else {
            if (evt.shiftKey) {
                return false;
            }
        }
    }
    return true;
}
function fnsetfocus(e, buttonid) {
    var evt = e ? e : window.event;

    var bt = document.getElementById(buttonid);

    if (bt) {

        if (evt.keyCode == 13) {
            bt.focus();
            bt.click();
            // alert("Enter Button is Pressed");
            return false;

        }

    }

}
function ValidateAlert(tblErrid, lblErrid, tdErrid, tdmainid, txtErrMsg) {
    document.getElementById(tblErrid).style.display = '';
    document.getElementById(lblErrid).innerHTML = txtErrMsg;
    document.getElementById(tdErrid).style.display = '';
    document.getElementById(tdErrid).bgColor = 'orange';
    document.getElementById(tdmainid).bgColor = 'orange';
}
function ValidateAlertClear(tblErrid, lblErrid, tdErrid, tdmainid, tdmainBgcolor) {
    document.getElementById(tblErrid).style.display = 'none';
    document.getElementById(lblErrid).innerHTML = '';
    document.getElementById(tdErrid).style.display = 'none';
    document.getElementById(tdmainid).bgColor = tdmainBgcolor;
}
function ValidateAlert1(tblErrid, lblErrid, tdErrid, tdmainid, txtErrMsg) {
    tblErrid.style.display = '';
    lblErrid.innerHTML = txtErrMsg;
    tdErrid.style.display = '';
    tdErrid.bgColor = 'orange';
    tdmainid.bgColor = 'orange';
}
function ValidateAlertClear1(tblErrid, lblErrid, tdErrid, tdmainid, tdmainBgcolor) {
    tblErrid.style.display = 'none';
    lblErrid.innerHTML = '';
    tdErrid.style.display = 'none';
    tdmainid.bgColor = tdmainBgcolor;
}
function InlineAlert(lblErrId, tdErrid, txtErrMsg) {
    document.getElementById(lblErrId).style.display = '';
    document.getElementById(lblErrId).innerHTML = txtErrMsg;
    document.getElementById(tdErrid).bgColor = 'orange';
}
function ClearInlineAlert(lblErrId, tdErrid, tdbgcolor) {
    document.getElementById(lblErrId).style.display = 'none';
    document.getElementById(tdErrid).bgColor = tdbgcolor;
}
function load() {
    $('html').keydown(function (e) {
        var ctrl = $(this).find('input:focus');
        var ctrltext = $(this).find('textarea:focus');
        if (e.keyCode == 116 || (e.keyCode == 8 && ctrl.prop('type') != 'text' && ctrltext.prop('type') != 'textarea')) {
            e.preventDefault();
        }
    })
}
function AlphaNumbers(e) //use this function like this (ex. onkeypress="return AlphaNumbers(event);")
{
    //alert("2");
    var keynum;
    var keychar;
    var numcheck;

    if (window.event) // IE
    {
        keynum = e.keyCode;
        if ((keynum >= 48 && keynum <= 57) || keynum == 13) {
            e.keyCode = e.keyCode;
        }
        else {
            e.keyCode = 0;
            return false;
        }
        return true;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
        keychar = String.fromCharCode(keynum);
        if (keynum != 8) {
            numcheck = /\d/;  // here d/  for number only. w/ for char and number                 
            return numcheck.test(keychar);
        }
        else
            return true;
    }
}
/********************************************************************************************************/
function SessionExpireFail(data) {
    //if (JSON.stringify(data.responseText).substring(4, 9) == 'Logon' || JSON.stringify(data.responseText).substring(39, 44) =='error') {
    //alert(JSON.stringify(data.responseText).substring(39, 44));
    //alert(JSON.stringify(data.responseText));
    //alert(JSON.stringify(data.responseText).substring(14, 21));
    if (JSON.stringify(data.responseText).substring(14, 21) == '-Error-') {
     
        window.location.href = '../Error/Display';

    }
    if (JSON.stringify(data.responseText).substring(4, 9) == 'Logon' ) {
        window.location.href = '../Home/SessionExpire';
    }
}
var txtid = ""; var url = ""; var hdnval = ""; var dataObj; var dataObj1;
function JqAutocomplete(url, txtid, hdnval, dataObj) {

    hdnval = (typeof hdnval === 'undefined') ? 'default' : hdnval;
    // alert(dataObj +"          hello");

    $('#' + txtid + '').autocomplete({
        source: function (request, response) {

            if (dataObj != '' || dataObj != 'undefined') {
               
                if (hdnval == 'hdnnoteclientid') { dataObj1 = { 'term': request.term, 'PracticeID': dataObj } }
                else if (dataObj == 'hdnproviderid') {
                    dataObj1 = { 'term': request.term, 'PracticeID': document.getElementById("hdnproviderid").value }
                }
                else {
                    dataObj1 = { 'term': request.term, 'id': dataObj };
                }
            }
            else {
                dataObj1 = { 'term': request.term };
            }
            $.ajax({
                type: 'GET',
                data: dataObj1,
                url: url,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                      
                        return {
                            label: item.Name,
                            value: item.value
                        }

                    }));
                },
                error: function (jqXHR, exception) {

                }
            })//ajax close
        },
        select: function (event, ui) {
            $('#' + txtid + '').val(ui.item.label);
            //alert(dataObj + "          hello");
            if (hdnval == 'hdn_agentid' || hdnval == 'hdnQuestion' || hdnval == 'hdnnoteclientid') {
                $('#' + hdnval + '').val(ui.item.value);
                return false;
            }
            if (hdnval != 'default' || hdnval != '') {
                var obj1 = ui.item.value;
                var obj = obj1.split("$");
                $('#' + hdnval + '').val(obj[1]);
            }
            return false;
        },
        focus: function (event, ui) {
           
            $('#' + txtid + '').val(ui.item.label);
            return false;
        },
        minLength: 1
    });

}
var url1; var txtid1;
function JqAutocomplete1(url1, txtid1) {
    $("#" + txtid1 + "").autocomplete({
        source: url1,
        minLength: 1,
        select: function (event, ui) {
            $("#" + txtid1 + "").val(ui.item.value);

        },

    });

}

function Displaydialog(Divid, viewUrl, height, width,dvLoad,txtAmount,txttransType)
{
    $("#" + Divid).dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        height: height,
        width: width
    });
    var dialogDiv = $("#" + Divid);
    $("#" + dvLoad).dialog({ modal: true, dialogClass: 'dvLoading', draggable: false, resizable: false });
    $.get(viewUrl, function (data) {
        dialogDiv.html(data);
        $("#" + dvLoad).dialog('close');
        if (txtAmount != null) {
            if ($("#" + txtAmount).val() != "" || $("#" + txtAmount).val() != "undefined") {
                if (txtAmount == 'Transaction_Amount') {
                    $("#" + txttransType).text("Charge");
                }
                if (txtAmount == 'txtPAmount') {
                    $("#" + txttransType).text("Payment");
                }
                if (txtAmount == 'txt_PAmount') {
                    $("#" + txttransType).text("Charge & Payment");
                }
            }
        }
        dialogDiv.dialog('open');
    });
    //return false;
}

//});

//}