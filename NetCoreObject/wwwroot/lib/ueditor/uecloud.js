var _me = null;
var domainUrl = localStorage.domainUrl;

//百度编辑器调用方法，弹出云图片管理器
function uecloudfile(me) {
    _me = me;
    //var frameId = window.frameElement && window.frameElement.id || '', pframeName = "";
    //if (frameId == "articleframe") {
    //    pframeName = parent.window.frameElement && parent.window.frameElement.id || '';
    //}
    var frameId = "uecloud";
    //dig.Open("选择文件", "/fytadmin/fileup/index?cltype=ue&domid=fyt&fields=&imgSrc=&frameId=" + frameId + "&pframeId=" + pframeName, "800px", "550px");
    x_admin_parent_show('选择文件', "/SysAdmin/FileUp/index?cltype=ue&domid=fyt&fields=&imgSrc=&frameId=" + frameId, 800, 550, false, function (data) {
        ueinsertfile(data);
    })
}

//定义方法，选择好图片，插入到编辑器中
function ueinsertfile(u) {
    var index = u.lastIndexOf('/');
    var str = u.substring(index + 1, u.length);
    if (u.extMatch(imgExt)) {
        //图片
        _me.execCommand('insertHtml', '<img src="'+  u + '" alt="' + str + '"/>');
    } else {
        //附件
        _me.execCommand('insertHtml', '<p style="padding:12px 20px;background-color: #edf3f5;"><a href="'+ u + '" target="_blank" title="' + str + '">' + str + '</a></p>');
    }
}

var imgExt = new Array(".png", ".jpg", ".jpeg", ".bmp", ".gif");//图片文件的后缀名
var docExt = new Array(".doc", ".docx");//word文件的后缀名
var xlsExt = new Array(".xls", ".xlsx");//excel文件的后缀名
var cssExt = new Array(".css");//css文件的后缀名
var jsExt = new Array(".js");//js文件的后缀名

//获取文件名后缀名
String.prototype.extension = function () {
    var ext = null;
    var name = this.toLowerCase();
    var i = name.lastIndexOf(".");
    if (i > -1) {
        var ext = name.substring(i);
    }
    return ext;
}

//判断Array中是否包含某个值
Array.prototype.contain = function (obj) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] === obj)
            return true;
    }
    return false;
};

String.prototype.extMatch = function (extType) {
    if (extType.contain(this.extension()))
        return true;
    else
        return false;
}

