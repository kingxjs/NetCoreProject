var tid = '', cguid = '',imgtotal=0,page=0, imgVue = new Vue({
                  el: '#imgwall',
                  data: {
                      items: []
                  },
                  methods: {
                      delimg: function (m) {
                          if (confirm("确认删除吗")) {
                              $.ajax({
                                  type: "post",
                                  url: "/fytadmin/cloudfile/deletefile",
                                  data: { id: m.Key },
                                  success: function (res) {
                                      if (res.status == 200) {
                                          page = 0;
                                          imgList(cguid, '');
                                      } else {
                                          dig.msg(res.msg);
                                      }
                                  }
                              });
                          }
                      }
                  }
              });
var base = new Vue({
    el: '#fytwall',
    data: {
        items: []
    },
    methods: {
        slidetype: function (e) {
            $("#ctype").toggle(200);
            e.cancelBubble = true;
        },
        sel: function (m) {
            page = 0;
            $(".pf-t-right").removeClass("hidden").eq(1).addClass('hidden');
            $(".pf-r-title strong").html(m.Title + '<span>（1）</span>');
            tid = m.Name;
            cguid = m.Guid;
            imgList(cguid,'');
        },
        add: function () {
            $('#forms')[0].reset();
            $(".modal-backdrop,.fyt-forms").removeClass('hidden');
        },
        cedit: function (m) {
            $("#Title").val(m.Title);
            $("#Name").val(m.Name);
            $("#Guid").val(m.Guid);
            $(".modal-backdrop,.fyt-forms").removeClass('hidden');
        },
        cdelete: function (m) {
            if (confirm("确认删除吗")) {
                $.post("/fytadmin/cloudfile/delete/",
                    { id: m.Guid },
                    function (res) {
                        if (res.status == 200) {
                            loadColumn();
                        } else {
                            dig.msg(res.msg);
                        }
                    });
            }
        }
    }
});

//全部分类
function fall() {
    cguid = '';
    page = 0;
    $(".pf-t-right").removeClass("hidden").eq(1).addClass('hidden');
    $(".pf-r-title strong").html('全部分类<span>（1）</span>');
    imgList('','');
}

//根据分类查询图片列表
function imgList(prefix,key) {
    $(".loading").show();
    $(".picbox>ul>li").removeClass('active');
    var data = { prefix: prefix, pageIndex: page, pageSize: 60,key:key };
    $.post("/fytadmin/cloudfile/getlist/",
        data,
        function (res) {
            if (res.status == 200) {
                //console.log(res.data);
                if (res.data.length == 0) {
                    $("#nopic").removeClass('hidden');
                } else {
                    $("#nopic").addClass('hidden');
                }
                $(".pf-r-title strong span").html('（' + res.rows + '）');
                if (page == 0) {
                    imgVue.items = [];
                }
                imgVue.items = imgVue.items.concat(res.data);
                $(".loading").hide();
                //根据总数计算总页数
                imgtotal = Math.ceil(res.rows / 60);
                page++;
                if (page >= imgtotal) {
                    $(".loadmore").hide();
                } else {
                    $(".loadmore").show();
                }

                Vue.nextTick(function () {
                    $(".picbox>ul>li").click(function (e) {
                        $(".picbox>ul>li").removeClass('active');
                        $(this).addClass("active");
                        $(".pf-t-right").removeClass("hidden").eq(0).addClass('hidden');
                        e.stopPropagation();
                    });
                    $(".picbox>ul>li").hover(function () {
                        $(this).find(".picbox-view").show();
                    }, function () {
                        $(this).find(".picbox-view").hide();
                    });
                });
            } else {
                dig.msg(res.msg);
            }
        },
        'json');
}

//截取文件斜杠后面的内容
function imgLastUrl(s) {
    var index = s.lastIndexOf("\/");
    return s.substring(index + 1, s.length);
}

function digFailure(res) {
    dig.msg("失败了~");
}

//表单提交成功重写方法
function digSuccess(res) {
    if (res.status == 200) {
        loadColumn();
        dig.msg(res.msg);
        $(".modal-backdrop,.fyt-forms").addClass('hidden');
    } else {
        dig.msg(res.msg);
    }
    $(".btn-save").attr("disabled", false).html('确定保存');
}

$(function () {
    //加载更多图片，分页方法
    $(".loadmore a").click(function() {
        if (page < imgtotal) {
            imgList(cguid, '');
        }
    });
    //上传图片
    $(".btn-upcloud").click(function() {
        $("#inpcloud").unbind();
        $("#inpcloud").click();
        $("#inpcloud").change(function () {
            cloudSignUpfile(tid);
        });
    });
    $("#forms").validate();
    loadColumn();
    imgList('', '');
    $(".pf-t-right .search").click(function () {
        var key = $("#keys").val();
        imgList(cguid, key);
    });
    //取消添加分类
    $(".fyt-foot-submit .btn-default").click(function () {
        $(".modal-backdrop,.fyt-forms").addClass('hidden');
    });
    //取消选择
    $("#btn-cancel").click(function () {
        $(".pf-t-right").removeClass("hidden").eq(1).addClass('hidden');
        $(".picbox>ul>li").removeClass('active');
    });
    //确定选择图片
    $(".picframe-footer .btn-ok").click(function () {
        var im = $(".picbox>ul>li.active .picbox-pic").attr('data-url');
        if (im) {
            im = 'http://img.feiyit.com/' + im;
            var index = parent.layer.getFrameIndex(window.name);
            var v = getUrlParam("fields");
            var frameId = getUrlParam("frameId");
            var cltype = getUrlParam("cltype");
            //内容管理的
            if (frameId == "articleframe") {
                var pframeId = getUrlParam("pframeId");
                var pw = $(parent.window.frames[pframeId]).contents().find("#articleframe");
                //判断是否选择图片，并且给单个图片赋值
                if (cltype == "sign") {
                    var domid = getUrlParam("domid");
                    $(pw)[0].contentWindow.cloudSign(im, domid);
                }
                else if (cltype == "shops") {
                    $(pw)[0].contentWindow.selShopCloud(im);
                }
                else if (cltype == "ue") {
                    $(pw)[0].contentWindow.ueinsertfile(im);
                }
                else {
                    $(pw).contents().find("#" + v).val(im);
                }
                
            }
            //弹出页面使用的方式
            else if (frameId.indexOf('layui-layer-iframe') > -1) {
                var frames = window.parent.window.document.getElementById(frameId);
                //判断是否选择图片，并且给单个图片赋值
                if (cltype == "sign") {
                    $(frames).cloudSign(im);
                } else {
                    $(frames).contents().find("#" + v).val(im);
                }
            }
            //其他方式
            else {
                //判断是否选择图片，并且给单个图片赋值
                if (cltype == "sign") {
                    if (!im.extMatch(imgExt)) {
                        dig.msg('只能选择图片~');
                        return;
                    }
                    var domid = getUrlParam("domid");
                    $(window.parent.$("#" + frameId))[0].contentWindow.cloudSign(im, domid);
                }
                else if (cltype == "ue") {
                    /*if (!im.extMatch(imgExt)) {
                        dig.msg('只能选择图片~');
                        return;
                    }*/
                    $(window.parent.$("#" + frameId))[0].contentWindow.ueinsertfile(im);
                }
                else {
                    $(parent.window.frames[frameId]).contents().find("#" + v).val(im);
                }
            }
            parent.layer.close(index);
        } else {
            dig.msg('请选择一张图片~');
        }
    });
});
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}
//根据文件类型返回相应的信息
function getcloudfile(f) {
    var str = '';
    var fext = f.substr(f.lastIndexOf(".")).toLowerCase();//获得文件后缀名
    //图片
    if (fext.extMatch(imgExt)) {
        str='<img src="http://img.feiyit.com/' + f + '?imageView2/1/w/104/h/104" />';
    } else {
        fext = fext.replace('.','');
        str='<span class="file-type"><i class="file-type-' + fext+' file-preview"></i></span>';
    }
    return str;
}

//加载分类
function loadColumn() {
    base.items = [];
    $.post("/fytadmin/cloudfile/column/",
        null,
        function (res) {
            if (res.status == 200) {
                base.items = base.items.concat(res.data);
                Vue.nextTick(function () {
                    $(".fyt-tree li").click(function () {
                        $(".fyt-tree li").removeClass('active');
                        $(this).addClass("active");
                    });
                    $(".fyt-tree ul li").hover(function () {
                            $(this).find('div').stop(true, false).show();
                        },
                        function () {
                            $(this).find('div').stop(true, false).hide();
                        });
                });
            } else {
                dig.alertErr(res.msg);
            }
        });
}

function cloudSignUpfile(cfpath) {
    $(".uploading").show();
    var subUrl = "/fytadmin/cloudfile/upsignfiles?cfpath=" + cfpath;
    $("#forms").ajaxSubmit({
        beforeSubmit: function () {

        },
        success: function (res) {
            //console.log(res);
            if (res.status === 200) {
                $(".uploading").hide();
                dig.msg("上传成功~");
                page = 0;
                imgList(cguid,'');
            } else {
                dig.msg(res.msg);
            }
            $("#inpcloud").unbind();
        },
        error: function (e) {
            dig.msg("上传失败");
        },
        url: subUrl,
        type: "post",
        dataType: "json",
        timeout: 600000
    });
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