
var loadindex = localStorage.loadindex ? localStorage.loadindex : "";
var tab = {};
$(function () {
    var clipboardDemos = new ClipboardJS('[data-clipboard-demo]');
    clipboardDemos.on('success', function (e) {
        e.clearSelection();
        x_admin_msg("复制成功")
    });
    clipboardDemos.on('error', function (e) {
        x_admin_msg("复制失败")
    });



    //加载弹出层
    layui.use(['form', 'element', 'jtagsinput'],
        function () {
            layer = layui.layer;
            element = layui.element
                , jtagsinput = layui.jtagsinput;

            tabList = sessionStorage.getItem("tabList") ? JSON.parse(sessionStorage.getItem("tabList")) : [];
            var pathname = window.location.pathname;
            //if (pathname == "/") {
            var bid = window.name;
            var breadcrumb = [];
            if (tabList.length > 0) {
                var a_id;
                var ids = [];
                var tabList2 = [];
                for (var i = 0; i < tabList.length; i++) {
                    if (tabList[i].id == bid) {
                        breadcrumb = tabList[i].breadcrumb;
                    }
                    if (tabList[i].id) {
                        tab.tabAdd(tabList[i].title, tabList[i].url, tabList[i].id)
                        if (tabList[i].show) {
                            a_id = tabList[i].id;
                        }
                        tabList2.push(tabList[i]);
                    }
                }
                sessionStorage.tabList = JSON.stringify(tabList2);
                tabList = tabList2;
                if (a_id) {
                    tab.tabChange(a_id);
                } else {
                    $(".layui-tab-title .home").addClass("layui-this")
                }
            } else {
                $(".layui-tab-title .home").addClass("layui-this")
            }
            $(".layui-tags-input").tagsInput({
                width: 'auto',
                height: 'auto',
                defaultText: '添加关键词',
                removeText: '删除关键词',
                placeholderColor: '#cccccc',
                maxCount: 5,
                class: "layui-input"
            });
            //}
            if (breadcrumb) {
                $(".layui-breadcrumb").append('<a href="javascript:;">首页</a><span lay-separator="">/</span>').children("a").click(function () {
                    parent.tab.tabChange(1);
                });
                for (var i = 0; i < breadcrumb.length; i++) {
                    if (i < breadcrumb.length - 1) {
                        $(".layui-breadcrumb").append('<a href="' + breadcrumb[i].href + '">' + breadcrumb[i].text + '</a><span lay-separator="">/</span>');
                    } else {
                        $(".layui-breadcrumb").append('<a><cite>' + breadcrumb[i].text + '</cite></a>');
                    }
                }
            }
            element.on('nav(layadmin-tabs-select)', function (data) {
                var event = $(data).parent().data("event");

                $(data).parent().parent().removeClass("layui-show");


                tab[event] ? tab[event].call(this) : '';
            });

            element.on('tab(xbs_tab)', function (data) {
                var lay_id = $(this).attr("lay-id");
                var lay_result = false;
                var tabModel;
                for (var i = 0; i < tabList.length; i++) {
                    if (tabList[i].id == lay_id) {
                        tabModel = tabList[i];
                        lay_result = true;
                        tabList[i].show = true;
                    } else {
                        tabList[i].show = false;
                    }
                }
                var lay_iframe = $("[tab-id=" + lay_id + "]");
                if (lay_iframe) {
                    var isShow = lay_iframe.attr("data-show");
                    if (isShow == "false") {
                        lay_iframe.attr("data-show", true);
                        lay_iframe.attr("src", tabModel.url);
                        lay_iframe.load(function () {
                        });
                        //document.getElementById(lay_id).contentWindow.location.reload(true);
                    }
                }
                if (lay_result == false) {

                }
                var offsetLeft = this.offsetLeft;
                var title_width = 0;
                $(".layui-tab-title").children("li").each(function (index, el) {
                    title_width += this.clientWidth;
                })
                var prev2 = $(this).prev();
                var prev2_width = 0;
                if (prev2.length > 0) {
                    prev2_width = $(this).prev()[0].clientWidth
                }
                $(".layui-tab-title").animate({ "scrollLeft": offsetLeft - prev2_width }, 500);

                sessionStorage.tabList = JSON.stringify(tabList);
            });
            element.on('tabDelete(xbs_tab)', function (data) {
                var lay_id = $(this).parent().attr("lay-id");
                for (var i = 0; i < tabList.length; i++) {
                    if (tabList[i].id == lay_id) {
                        tabList[i].id = "";
                        //tabList.splice(i, 1);
                        break;
                    }
                }
                //console.info(tabList)
                sessionStorage.tabList = JSON.stringify(tabList);
            });
        });

    //触发事件
    tab = {
        tabAdd: function (title, url, id, breadcrumb) {
            //新增一个Tab项

            var isAdd = true;
            var tabModel;
            for (var i = 0; i < tabList.length; i++) {
                if (tabList[i].id == id) {
                    tabModel = tabList[i];
                    isAdd = false;
                    break;
                }
            }
            var url2 = url;
            var isShow = false;
            if (tabModel) {
                isShow = tabModel.show;

            }
            if (!isShow) {
                url2 = "";
            }

            element.tabAdd('xbs_tab', {
                title: title
                , content: '<iframe name=' + id + ' id=' + id + ' tab-id="' + id + '" frameborder="0" data-show=' + isShow + ' src="' + url2 + '" scrolling="yes" class="x-iframe"></iframe>'
                , id: id
            })
            if (isAdd) {
                tabList.push({
                    title: title,
                    id: id,
                    url: url,
                    show: false,
                    breadcrumb: breadcrumb
                });
                sessionStorage.tabList = JSON.stringify(tabList);
            }
        }
        , tabDelete: function (lay_id) {
            if (lay_id != 1) {
                //删除指定Tab项
                element.tabDelete('xbs_tab', lay_id);
            }

            //othis.addClass('layui-btn-disabled');
        }
        , tabChange: function (id) {
            //切换到指定Tab项
            element.tabChange('xbs_tab', id);



        }
        , closeThisTabs: function () {//关闭当前
            var lay_id = $(".layui-tab-title").find("li.layui-this").attr("lay-id");

            tabList = sessionStorage.getItem("tabList") ? JSON.parse(sessionStorage.getItem("tabList")) : [];
            if (tabList.length > 0) {
                for (var i = 0; i < tabList.length; i++) {
                    if (tabList[i].id == lay_id) {
                        tab.tabDelete(tabList[i].id);
                        tabList[i].id = "";
                        break;
                    }
                }
            }
            sessionStorage.tabList = JSON.stringify(tabList);

        }, closeOtherTabs: function () {//关闭其他
            var lay_id = $(".layui-tab-title").find("li.layui-this").attr("lay-id");
            tabList = sessionStorage.getItem("tabList") ? JSON.parse(sessionStorage.getItem("tabList")) : [];
            if (tabList.length > 0) {
                for (var i = 0; i < tabList.length; i++) {
                    if (tabList[i].id != lay_id) {
                        tab.tabDelete(tabList[i].id);
                        tabList[i].id = "";
                    }
                }
            }
            sessionStorage.tabList = JSON.stringify(tabList);
        }, closeAllTabs: function () {//关闭全部
            tabList = sessionStorage.getItem("tabList") ? JSON.parse(sessionStorage.getItem("tabList")) : [];
            if (tabList.length > 0) {
                for (var i = 0; i < tabList.length; i++) {
                    tab.tabDelete(tabList[i].id);
                    tabList[i].id = "";
                }
            }
            sessionStorage.tabList = JSON.stringify(tabList);
        }, refreshThisTabs: function () {//刷新当前
            var lay_id = $(".layui-tab-title").find("li.layui-this").attr("lay-id");
            document.getElementById(lay_id).contentWindow.location.reload(true);
        }, refreshOtherTabs: function () {//刷新其他
            var lay_id = $(".layui-tab-title").find("li.layui-this").attr("lay-id");
            tabList = sessionStorage.getItem("tabList") ? JSON.parse(sessionStorage.getItem("tabList")) : [];
            if (tabList.length > 0) {
                for (var i = 0; i < tabList.length; i++) {
                    if (tabList[i].id != lay_id) {
                        document.getElementById(tabList[i].id).contentWindow.location.reload(true);
                    }
                }
            }
        }

    };

    tableCheck = {
        init: function () {
            $(".layui-form-checkbox").click(function (event) {
                if ($(this).hasClass('layui-form-checked')) {
                    $(this).removeClass('layui-form-checked');
                    if ($(this).hasClass('header')) {
                        $(".layui-form-checkbox").removeClass('layui-form-checked');
                    }
                } else {
                    $(this).addClass('layui-form-checked');
                    if ($(this).hasClass('header')) {
                        $(".layui-form-checkbox").addClass('layui-form-checked');
                    }
                }

            });
        },
        getData: function () {
            var obj = $(".layui-form-checked").not('.header');
            var arr = [];
            obj.each(function (index, el) {
                arr.push(obj.eq(index).attr('data-id'));
            });
            return arr;
        }
    }

    //开启表格多选
    tableCheck.init();
    function set_left_open() {
        if ($('.left-nav').css('left') == '0px') {
            $('.left-nav').animate({ left: '-221px' }, 100);
            $('.page-content').animate({ left: '0px' }, 100);
            $('.page-content-bg').hide();
        } else {
            $('.left-nav').animate({ left: '0px' }, 100);
            $('.page-content').animate({ left: '221px' }, 100);
            if ($(window).width() < 768) {
                $('.page-content-bg').show();
            }
        }
    }

    $(".layui-tab-menu-left").click(function (event) {
        var width = $(".layui-tab-title").width();
        var title_width = 0;
        $(".layui-tab-title").children("li").each(function (index, el) {
            title_width += this.clientWidth;
        })
        var number = Math.ceil(title_width / width);
        var index = $(".layui-tab-menu-left").data("index") ? $(".layui-tab-menu-left").data("index") : 1;
        if (index > 0) {
            $(".layui-tab-menu-right").attr("data-index", index);
            $(".layui-tab-menu-left").attr("data-index", index - 1);
            $(".layui-tab-title").animate({ "scrollLeft": (index - 1) * width }, 500);
        }
    })
    $(".layui-tab-menu-right").click(function (event) {
        var index = $(".layui-tab-menu-right").data("index") ? $(".layui-tab-menu-right").data("index") : 1;
        var width = $(".layui-tab-title").width();
        var title_width = 0;
        $(".layui-tab-title").children("li").each(function (index, el) {
            title_width += this.clientWidth;
        })
        var number = Math.ceil(title_width / width);
        if (number > 1) {
            $(".layui-tab-menu-right").attr("data-index", index + 1);
            $(".layui-tab-menu-left").attr("data-index", index);
            $(".layui-tab-title").animate({ "scrollLeft": index * width }, 500);
        }
    })
    $('.container .left_open i').click(function (event) {
        set_left_open();
    });

    $('.page-content-bg').click(function (event) {
        $('.left-nav').animate({ left: '-221px' }, 100);
        $('.page-content').animate({ left: '0px' }, 100);
        $(this).hide();
    });

    $('.layui-tab-close').click(function (event) {
        //$('.layui-tab-title li').eq(0).find('i').remove();
    });

    $("tbody.x-cate tr[fid!='0']").hide();
    // 栏目多级显示效果
    $(document).on("click", '.x-show', function () {
        if ($(this).attr('status') == 'true') {
            $(this).html('&#xe625;');
            $(this).attr('status', 'false');
            cateId = $(this).parents('tr').attr('cate-id');
            $("tbody.x-cate tr[fid=" + cateId + "]").show();
        } else {
            cateIds = [];
            $(this).html('&#xe623;');
            $(this).attr('status', 'true');
            cateId = $(this).parents('tr').attr('cate-id');
            getCateId(cateId);
            for (var i in cateIds) {
                $("tbody.x-cate tr[cate-id=" + cateIds[i] + "]").hide().find('.x-show').html('&#xe623;').attr('status', 'true');
            }
        }
    })

    //左侧菜单效果
    // $('#content').bind("click",function(event){
    $(document).on("click", '.left-nav #nav .nav_right', function (event) {
        var THIS = $(this).parent().parent("li");
        if ($(THIS).children('.sub-menu').length) {
            if ($(THIS).hasClass('open')) {
                $(THIS).removeClass('open');
                $(THIS).find('.nav_right').removeClass("fa-angle-down").addClass("fa-angle-left")
                $(THIS).children('.sub-menu').stop().slideUp();
                $(THIS).siblings().children('.sub-menu').slideUp();
            } else {
                //$('#nav').find(".nav_right").removeClass("fa-angle-down").addClass("fa-angle-left");
                $(THIS).siblings().find(".nav_right").removeClass("fa-angle-down").addClass("fa-angle-left");

                $(THIS).siblings().removeClass('open');
                $(THIS).siblings().find(".open").removeClass('open');

                $(THIS).addClass('open');
                $(THIS).children().children('.nav_right').removeClass("fa-angle-left").addClass("fa-angle-down")
                //$(this).find('.nav_right').removeClass("fa-angle-left").addClass("fa-angle-down")
                $(THIS).children('.sub-menu').stop().slideDown();
                $(THIS).siblings().find('.sub-menu').stop().slideUp();
            }
        }

        event.stopPropagation();

    })
    $(document).on("click", '.left-nav #nav li', function (event) {
        if ($(this).children('.sub-menu').length) {
            var result = setTab(this);
            if (result) {
                if ($(this).hasClass('open')) {
                    $(this).removeClass('open');
                    $(this).find('.nav_right').removeClass("fa-angle-down").addClass("fa-angle-left")
                    $(this).children('.sub-menu').stop().slideUp();
                    $(this).siblings().children('.sub-menu').slideUp();
                } else {
                    //$('#nav').find(".nav_right").removeClass("fa-angle-down").addClass("fa-angle-left");
                    $(this).siblings().find(".nav_right").removeClass("fa-angle-down").addClass("fa-angle-left");

                    $(this).siblings().removeClass('open');
                    $(this).siblings().find(".open").removeClass('open');

                    $(this).addClass('open');
                    $(this).children().children('.nav_right').removeClass("fa-angle-left").addClass("fa-angle-down")
                    //$(this).find('.nav_right').removeClass("fa-angle-left").addClass("fa-angle-down")
                    $(this).children('.sub-menu').stop().slideDown();
                    $(this).siblings().find('.sub-menu').stop().slideUp();
                }
            }
        } else {
            setTab(this);
        }

        event.stopPropagation();

    })
    //打开新的标签页
    $(document).on("click", '.layui-nav li.layui-nav-new', function (event) {
        var THIS = this;
        //var _href = $(this).find("a").attr("_href");
        var url = $(THIS).children('a').attr('_href');
        var title = $(THIS).find('cite').html();
        var icon = $(THIS).find('cite').prev("i").prop("outerHTML");
        var index = $('.left-nav #nav li').index($(THIS)) + 100;
        if (!url) {
            return true;
        }
        for (var i = 0; i < $('.x-iframe').length; i++) {
            if ($('.x-iframe').eq(i).attr('tab-id') == index + 1) {
                tab.tabChange(index + 1);
                event.stopPropagation();
                return;
            }
        };

        tab.tabAdd(icon + title, url, index + 1);
        tab.tabChange(index + 1);
    });
    //根据连接找左菜单，如果没有，则不显示标签页
    $(document).on("click", '.layui-nav li.layui-nav-link', function (event) {
        var _href = $(this).find("a").attr("_href");
        var THIS = $('.left-nav #nav li').find("[_href='" + _href + "']").first().parent("li");
        if (THIS) {
            setTab(THIS);
        }
    });




    function getParentText(THIS) {
        var breadcrumb = [];
        var text = $(THIS).children("a").children("cite").text();
        var href = $(THIS).children("a").attr("href");
        breadcrumb.unshift({
            text: text,
            href: href
        })
        $(THIS).parents("li").each(function () {
            breadcrumb.unshift({
                text: $(this).children("a").children("cite").text(),
                href: $(this).children("a").attr("href")
            })
        })
        return breadcrumb;
    }

    function setTab(THIS) {
        $(".layui-nav-ul").find(".layui-this").removeClass("layui-this");
        $(".sub-menu").find(".layui-this").removeClass("layui-this");
        $(THIS).addClass("layui-this");
        var url = $(THIS).children('a').attr('_href');
        var title = $(THIS).find('cite').html();
        var icon = $(THIS).find('cite').prev("i").prop("outerHTML");
        var index = $('.left-nav #nav li').index($(THIS));
        if (!url && $(THIS).children('.sub-menu').length) {
            return true;
        }
        var breadcrumb = getParentText(THIS);
        for (var i = 0; i < $('.x-iframe').length; i++) {
            if ($('.x-iframe').eq(i).attr('tab-id') == index + 1) {
                tab.tabChange(index + 1);
                event.stopPropagation();
                return;
            }
        };

        tab.tabAdd(icon + title, url, index + 1, breadcrumb);
        tab.tabChange(index + 1);
    }
    var tipsi;
    $(document).on("mouseover mouseout", ".layer-tips", function (event) {
        var data_tips = $(this).attr("data-tips");
        if (!data_tips) {
            data_tips = 2;
        }
        if (event.type == "mouseover") {
            var msg = $(this).attr("placeholder");
            if (msg) {
                tipsi = layer.tips(msg, this, { time: 0, tips: [data_tips, '#000'] });
            }
        }
        else if (event.type == "mouseout") {
            if (tipsi) {
                layer.close(tipsi);
            }
        }
    })
    //$(".layer-tips").hover(function () {
    //    var msg = $(this).attr("placeholder");
    //    if (msg) {
    //        tipsi = layer.tips(msg, this, { time: 0 });
    //    }
    //}, function () {
    //    if (tipsi) {
    //        layer.close(tipsi);
    //    }
    //});
})




var cateIds = [];
function getCateId(cateId) {

    $("tbody tr[fid=" + cateId + "]").each(function (index, el) {
        id = $(el).attr('cate-id');
        cateIds.push(id);
        getCateId(id);
    });
}
function RefreshCache() {
    $.ajax({
        'type': 'post',
        'url': '/SysAdmin/Home/RefreshCache',
        error: function (message) {
            console.info(message)
        },
        success: function (data) {
            if (data.status == 200) {
                x_admin_msg("已清理");
            } else {
                x_admin_msg(data.msg);
            }
        }
    });
}

/*弹出层*/
/*
    参数解释：
    title   标题
    url     请求的url
    id      需要操作的数据id
    w       弹出层宽度（缺省调默认值）
    h       弹出层高度（缺省调默认值）
    s       弹出层 点击其他关闭
    e       关闭回调函数
    b       是否添加确认按钮
*/
//在父页面打开
function x_admin_parent_show(title, url, w, h, s, e) {
    var alayer = layer;
    if (parent.layer) {
        alayer = parent.layer;
    }
    x_admin_show(alayer, title, url, w, h, s, e);
}
function x_admin_parent_show_btn(title, url, w, h, s, e) {
    var alayer = layer;
    if (parent.layer) {
        alayer = parent.layer;
    }
    x_admin_show_btn(alayer, title, url, w, h, s, e);
}
//在本页面打开
function x_admin_this_show(title, url, w, h, s, e) {
    var alayer = layer;
    x_admin_show(alayer, title, url, w, h, s, e);
}
function x_admin_this_show_btn(title, url, w, h, s, e) {
    var alayer = layer;
    x_admin_show_btn(alayer, title, url, w, h, s, e);
}

function x_admin_show(alayer, title, url, w, h, s, e) {
    if (!s) {
        s = false;
    }
    if (title == null || title == '') {
        title = false;
    };
    if (url == null || url == '') {
        url = "404.html";
    };
    if (w == null || w == '') {
        w = ($(window).width() * 0.9);
    };
    if (h == null || h == '') {
        h = ($(window).height() - 50);
    };

    //var alayer = layer;
    //if (parent.layer) {
    //    alayer = parent.layer;
    //}
    if (e) {
        var custom = alayer.custom ? alayer.custom : {};
        custom.reload = e;
        alayer.custom = custom;
    }

    return alayer.open({
        type: 2,
        area: [w + 'px', h + 'px'],
        fix: false, //不固定
        maxmin: true,
        shadeClose: s,
        shade: 0.4,
        title: title,
        content: url,
        end: function () {
            console.info("end")
        }
    });
}
//带有确认取消按钮
function x_admin_show_btn(alayer,title, url, w, h, s, e) {
    if (!s) {
        s = false;
    }
    if (title == null || title == '') {
        title = false;
    };
    if (url == null || url == '') {
        url = "404.html";
    };
    if (w == null || w == '') {
        w = ($(window).width() * 0.9);
    };
    if (h == null || h == '') {
        h = ($(window).height() - 50);
    };

    //var alayer = layer;
    //if (parent.layer) {
    //    alayer = parent.layer;
    //}
    if (e) {
        var custom = alayer.custom ? alayer.custom : {};
        custom.reload = e;
        alayer.custom = custom;
    }
    alayer.open({
        type: 2
        , title: title
        , content: url
        , maxmin: true
        , shadeClose: s
        , shade: 0.4
        , area: [w + 'px', h + 'px']
        , btn: ['确定', '取消']
        , yes: function (index, layero) {
            //点击确认触发 iframe 内容中的按钮提交
            var submit = layero.find('iframe').contents().find("#form-submit");
            submit.click();
        },
        success: function (layero, index) {
            layero.find(".layui-layer-btn0").attr("data-bus-type", "admin:btn:save");
            set_bus_type();
        }
    });
}
function x_admin_table_reload(table_name) {
    var alayui = layui;
    if (parent.layui) {
        alayui = parent.layui;
    }
    alayui.table.reload(table_name);
}
function x_admin_confirm(msg, options, fun) {
    var alayer = layer;
    if (parent.layer) {
        alayer = parent.layer;
    }
    if (options) {
        return alayer.confirm(msg, options, fun);
    } else {
        return alayer.confirm(msg, fun);
    }
}
function x_admin_alert(content, options, yes) {
    var alayer = layer;
    if (parent.layer) {
        alayer = parent.layer;
    }
    return alayer.alert(content, options, yes);
}
function x_admin_msg(msg, options) {
    var alayer = layer;
    if (parent.layer) {
        alayer = parent.layer;
    }
    return alayer.msg(msg, options);
}
//遮罩层
function x_admin_load() {
    var alayer = layer;
    if (parent.layer) {
        alayer = parent.layer;
    }
    var index = alayer.load(1, {
        shade: [0.1, '#000'] //0.1透明度的白色背景
    });
    //var index = alayer.msg('<i class="fa fa-spinner fa-pulse" style="font-size: 28px;"></i>', {
    //    time: 0 //不自动关闭
    //    , shade: [0.1, '#000']
    //});
    return index;
}

function x_admin_close_index(index) {
    var alayer = layer;
    if (parent.layer) {
        alayer = parent.layer;
    }
    alayer.close(index);
}

/*关闭弹出框口*/
function x_admin_close(data) {
    var alayer = layer;
    if (parent.layer) {
        alayer = parent.layer;
    }
    if (alayer.custom) {
        if (alayer.custom.reload) {
            alayer.custom.reload(data);
        }
    }
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}

//对Date的扩展，将 Date 转化为指定格式的String   
//月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
//年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
//例子：   
//(new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
//(new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}



//jQuery(function ($) {
//    var _ajax = $.ajax;
//    $.ajax = function (opt) {
//        var _beforeSend = opt && opt.beforeSend || function (request) { };
//        var _success = opt && opt.success || function (data) { };

//        var _opt = $.extend(opt, {
//            beforeSend: function (request) {
//                loadindex = x_admin_load();
//                localStorage.loadindex = loadindex;
//                _beforeSend(request);
//            },
//            success: function (data) {
//                localStorage.loadindex = "";
//                x_admin_close_index(loadindex)

//                _success(data);
//            }
//        });

//        _ajax(_opt);
//    };
//});