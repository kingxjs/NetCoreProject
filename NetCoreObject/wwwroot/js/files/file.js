var fh = '<div class="soa-select-ftype">';
fh += '<p class="alert alert-info alea-if">请选择直接上传或者到管理器选择图片</p>';
fh += '<div class="soa-select-ftype-btn">';
fh += '<input type="file" id="openfileUp" name="openfileUp">';
fh += '<button type="button" class="layui-btn layui-btn-danger" id="openSelectUp">选择文件</button>';
fh += '<button type="button" class="layui-btn" id="openImgMan">图库管理</button>';
fh += '</div></div>';
var isImg = 0, soa_ft = "", cloudNum = '', isCloud = false;

/**
 * 云单个图片选择后，赋值给页面的图片并且操作删除和更改
 * @param {u} : 文件的路径
 * @param {domid} : 当前操作元素的ID，通过它页面可以存在多个图片上传
 */
function cloudSign(u, domid) {
    var chimg = '<div class="add-photo-wall">';
    chimg += '<img src="' + u + '?imageView2/1/w/178/h/175"/>';
    chimg += '<div class="phote-edit">';
    chimg += '<a class="photo-tool" href="javascript:void(0)" onclick="editsign(\'' + domid + '\')"><i class="fa fa-pencil-square-o"></i>更换</a>';
    chimg += '<a class="photo-tool last" href="javascript:void(0)" onclick="delsign(\'' + domid + '\')"><i class="fa fa-trash-o"></i>删除</a>';
    chimg += '</div>';
    chimg += '<div class="cover">封面</div>';
    chimg += '</div></div>';
    var that = $("#" + domid).parents('.add-photo');
    that.find(".add-photo-wall").remove();
    that.find(".imgv").val(u);
    that.removeClass("default");
    $("#" + domid).hide();
    that.append(chimg);
}
//更换单个图片选择
function editsign(domid) {
    $("#" + domid).click();
}
//删除单个图片选择
function delsign(domid) {
    var that = $("#" + domid).parents('.add-photo');
    that.find(".add-photo-wall").remove();
    that.find(".imgv").val('');
    $("#" + domid).show();
    that.addClass('default');
}

$(function () {
    $("#forms").append(fh);
    //选择图片，打开云管理器
    $(".select-cloud-img").click(function () {
        var frameId = window.frameElement && window.frameElement.id || '', pframeName = "";
        if (frameId == "articleframe") {
            pframeName = parent.window.frameElement && parent.window.frameElement.id || '';
        }
        //x_admin_show("选择文件", "/SysAdmin/CloudFile/DigIndex?cltype=sign&domid=" + $(this).attr("id")+"&frameId=" + frameId + "&pframeId=" + pframeName, "920px", "550px");  
        x_admin_this_show("选择文件", "/SysAdmin/FileUp/Index?cltype=sign&domid=" + $(this).attr("id") + "&fields=&imgSrc=&frameId=" + frameId + "&pframeId=" + pframeName, 800, 570);
    });
    //选择云上传
    $(".select-cloud").click(function () {
        var inpt = $("#keys").val();
        var frameId = window.frameElement && window.frameElement.id || '', pframeName = "";
        if (frameId == "articleframe") {
            pframeName = parent.window.frameElement && parent.window.frameElement.id || '';
        }
        x_admin_this_show("选择文件", "/SysAdmin/CloudFile/DigIndex?fields=" + inpt + "&imgSrc=&frameId=" + frameId + "&pframeId=" + pframeName, 920, 570);
    });
    //云上传
    $("#btn-cloudup").click(function () {
        $("#inpcloud").unbind();
        $("#inpcloud").click();
        $("#inpcloud").change(function () {
            f.cloudSignUpfile();
        });
    });
    //支持直接上传
    $(".direct-up").click(function () {
        soa_ft = $(this).attr("data-text");
        isImg = $(this).attr("data-showimg");
        if (typeof (isImg) === "undefined") {
            isImg = 0;
        }
        if (isCloud) {
            var frameId = window.frameElement && window.frameElement.id || '', pframeName = "";
            if (frameId == "articleframe") {
                pframeName = parent.window.frameElement && parent.window.frameElement.id || '';
            }
            x_admin_this_show("选择文件", "/SysAdmin/CloudFile/DigIndex?fields=" + soa_ft + "&imgSrc=" + isImg + "&frameId=" + frameId + "&pframeId=" + pframeName, 920, 570);
            return;
        }
        $("#openfileUp").unbind();
        $("#openfileUp").click();
        $("#openfileUp").change(function () {
            f.divSignUpFile();
        });
    });
    $(".select-img").click(function (e) {
        var h = $(this).height() + 2;
        var w = $(this).outerWidth(false);
        soa_ft = $(this).attr("data-text");
        isImg = $(this).attr("data-showimg");
        if (typeof (isImg) === "undefined") {
            isImg = 0;
        }
        if (isCloud) {
            var frameId = window.frameElement && window.frameElement.id || '', pframeName = "";
            if (frameId == "articleframe") {
                pframeName = parent.window.frameElement && parent.window.frameElement.id || '';
            }

            x_admin_this_show("选择文件", "/SysAdmin/CloudFile/DigIndex?fields=" + soa_ft + "&imgSrc=" + isImg + "&frameId=" + frameId + "&pframeId=" + pframeName, 920, 570);
            return;
        }
        var offset = $(this).offset();
        e.stopPropagation();

        if ($(window).width() > 1600) {
            $(".soa-select-ftype").css({ 'left': offset.left - ($(".soa-select-ftype").width() - w), "top": offset.top + h }).addClass('fhide').show();
        } else {
            $(".soa-select-ftype").css({ 'left': offset.left - 149, "top": offset.top + h }).addClass('fhide').show();
        }

        $("#pdata").val(soa_ft);
    });
    //点击其它地方关闭弹出这个层
    $(document).click(function () {
        if ($("div.soa-select-ftype").hasClass("fhide")) {
            $("div.soa-select-ftype").addClass("fhide").hide();
        }
    });
    $("div.soa-select-ftype").click(function (e) {
        e.stopPropagation();
    });
    //打开图库
    $("#openImgMan").click(function () {
        f.operFile(soa_ft, isImg);
    });
    //选择上传文件
    $("#openSelectUp").click(function () {
        $("#openfileUp").unbind();
        $("#openfileUp").click();
        $("#openfileUp").change(function () {
            f.divSignUpFile();
        });
    });

    $(".sign-up").click(function () {
        $("#fileUp").unbind();
        $("#fileUp").click();
        $("#fileUp").change(function () {
            f.signUpFile("");
        });
    });

    $(".fclose").click(function () {
        var dialog = top.dialog.get(window);
        dialog.close().remove();
    });
});

var f = {
    operFile: function (inpt, imgSrc) {
        $(".soa-select-ftype").hide();
        var frameId = window.frameElement && window.frameElement.id || '', pframeName = "";
        if (frameId == "articleframe") {
            pframeName = parent.window.frameElement && parent.window.frameElement.id || '';
        }
        x_admin_this_show("选择文件", "/SysAdmin/FileUp/Index?fields=" + inpt + "&imgSrc=" + imgSrc + "&frameId=" + frameId + "&pframeId=" + pframeName, 800, 570, false, function (data) {
            $("#" + inpt).val(data)
        });
        return false;
    },
    //云单文件上传
    cloudSignUpfile: function () {
        var f = document.getElementById("inpcloud").files;
        var s = parseInt(f[0].size);
        s = s / 1024;
        if (s > 1024) {
            s = eval(s / 1024);
            s = s.toFixed(2) + "MB";
        } else {
            s = s.toFixed(0) + "KB";
        }
        var rand = Math.random();
        cloudNum = (1024 + Math.round(rand * 625));
        var trh = '<tr id="' + cloudNum + '"><td>' + f[0].name + '</td><td width="80">' + s + '</td><td width="80" class="probar">0%</td><td width="50"><i class="fa fa-spinner fa-pulse"></i></td></tr>';
        $(".cloud-list tbody").append(trh);
        var subUrl = "/SysAdmin/cloudfile/upsignfiles?cfpath=" + $("#cfpath").val();
        $("#forms").ajaxSubmit({
            beforeSubmit: function () {

            },
            success: function (res) {
                //console.log(res);
                if (res.status === 200) {
                    x_admin_msg("上传成功~");
                    $("#" + cloudNum + " i").removeClass('fa fa-spinner fa-pulse').addClass('fa fa-check-square-o');
                } else {
                    x_admin_msg(res.msg);
                }
                $("#inpcloud").unbind();
            },
            xhr: function () {
                var xhr = $.ajaxSettings.xhr();
                if (onprogress && xhr.upload) {
                    xhr.upload.addEventListener("progress", onprogress, false);
                    return xhr;
                }
                return xhr;
            },
            error: function (e) {
                alert("上传失败");
            },
            url: subUrl,
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    },
    //普通div文件上传
    divSignUpFile: function () {
        var subUrl = "/SysAdmin/FileUp/SignUpFile?upFiles=openfileUp&isThum=0&isWater=0";
        $("#forms").ajaxSubmit({
            beforeSubmit: function () {
                $("#openSelectUp").attr("disabled", "disabled").html("正在上传...");
            },
            success: function (res) {
                //console.log(res);
                if (res.status === 200) {
                    $("#" + soa_ft).val(res.data.FileName.replace("/wwwroot", ""));
                } else {
                    x_admin_msg(res.msg);
                }
                $("#openSelectUp").attr("disabled", false).html("选择文件");
                $("#openfileUp").unbind();
                $(".soa-select-ftype").hide();
            },
            error: function (e) {
                $("#openSelectUp").attr("disabled", false).html("选择文件");
            },
            url: subUrl,
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    },
    signUpFile: function (upInput) {
        var isThum = ($('#isthum').is(':checked') ? "1" : "0");
        var isWater = ($('#isWater').is(':checked') ? "1" : "0");
        var subUrl = "/SysAdmin/FileUp/signupfile?upFiles=fileUrl&isThum=" + isThum + "&isWater=" + isWater;
        $("#forms").ajaxSubmit({
            beforeSubmit: function () {
                $(".sign-up").attr("disabled", "disabled").html(" 正在上传...");
            },
            success: function (res) {
                //console.log(res);
                if (res.status === 200) {
                    $("#fileUrl").val(res.data.FileName.replace("/wwwroot", ""));
                } else {
                    x_admin_msg(data.msg);
                }
                $(".sign-up").attr("disabled", false).html(" 选择文件上传");
                $("#fileUp").unbind();
            },
            error: function (e) {
                $(".sign-up").attr("disabled", false).html(" 选择文件上传");
            },
            url: subUrl,
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    },
    fileSizes: function (s) {
        s = s / 1024;
        if (s > 1024) {
            s = eval(s / 1024);
            s = s.toFixed(2) + "MB";
        } else {
            s = s.toFixed(0) + "KB";
        }
        return s;
    }
};

function onprogress(evt) {
    //侦查附件上传情况
    //通过事件对象侦查
    //该匿名函数表达式大概0.05-0.1秒执行一次
    // console.log(evt.loaded);  //已经上传大小情况
    //evt.total; 附件总大小
    var loaded = evt.loaded;
    var tot = evt.total;
    var per = Math.floor(100 * loaded / tot);  //已经上传的百分比
    $("#" + cloudNum + " .probar").html(per + '%');
}


function delFolder(p, fold) {
    x_admin_confirm("确定要删除选择的文件吗？", function () {
        var ajaxUrl = "/SysAdmin/FileUp/deleteby";
        var isFile = (fold === "文件夹" ? 0 : 1);
        $.post(ajaxUrl, { path: $("#path").val() + p, isfile: isFile }, function (res) {
            if (res.status === 200)
                initFiles($("#path").val());
            else {
                x_admin_msg(res.msg);
            }
        }, "json");
    });
}

function OpenParentFolder() {
    var p = $("#path").val();
    if (p === spath) return;
    p = p.substring(0, p.lastIndexOf('/'));
    p = p.substring(0, p.lastIndexOf('/')) + "/";
    $("#path").val(p);
    initFiles(p);
}

function OpenFolder(p) {
    var npath = $("#path").val() + p + "/";
    $("#path").val(npath);
    initFiles(npath);
}

function SetFile(f, t) {
    $(t).addClass("selected").siblings().removeClass('selected');
    var p = $("#path").val().replace("/wwwroot", "");
    $("#fileUrl").val(p + f);
}

function initFiles(path) {
    if (path === spath) {
        $("#btn-menu").attr("data-tyle", "no").html('<i class="fa fa-bars"></i>');
    } else {
        $("#btn-menu").attr("data-tyle", "yes").html("返回上级");
    }
    var collSum = 5, tbody = $("#tables > tbody"), tdata = $("#tlist");
    $.post("/SysAdmin/FileUp/getfiledata", { path: path }, function (res) {
        if (res.status === 200) {
            if (res.data === "" || res.data === null || res.data.length == 0) {
                //toastr.options = { positionClass: 'toast-top-center', timeOut: 2000, progressBar:true };
                //toastr.warning("该目录下没有文件了！");
                x_admin_msg("该目录下没有文件了！");
                OpenParentFolder();
            } else {
                tbody.empty();
                tdata.tmpl(res.data).appendTo('#trows');
                //增加图片列表
                var ulHtml = '';
                $.each(res.data, function (index, fi) {
                    if (fi.ext === "文件夹") {
                        ulHtml += '<li onclick="OpenFolder(\'' + fi.name + '\')"><div class="file-wrapper"><i class="file-type-dir file-preview"></i><span class="file-title">' + fi.name + '</span></div><span class="icon"></span></li> ';
                    } else {
                        var imgUrl = $("#path").val() + fi.name;
                        //获得后缀名
                        var k = fi.name.substr(fi.name.indexOf(".") + 1);
                        k = k.toLowerCase();
                        if (k === "jpg" || k === "gif" || k === "png" || k === "bmp") {
                            ulHtml += '<li onclick="SetFile(\'' + fi.name + '\',this)"><img src="' + imgUrl.replace("/wwwroot", "") + '" /><span class="icon"></span></li> ';
                        }
                        else {
                            ulHtml += '<li onclick="SetFile(\'' + fi.name + '\',this)" title="' + fi.name + '"><div class="file-wrapper"><i class="file-type-' + k + ' file-preview"></i><span class="file-title">' + fi.name + '</span></div><span class="icon"></span></li> ';
                        }

                    }
                    $(".fyt-flist").html(ulHtml);
                });
            }
        } else {
            x_admin_msg(res.msg);
        }
    }, "json");
};
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}