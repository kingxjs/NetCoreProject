


$(function () {
    layui.use(['form', 'table', 'treegrid'], function () {
        var from = layui.from;
        var table = layui.table,
            treegrid = layui.treegrid;
        var $ = layui.$;
        var alayui = layui;
        if (parent.layui) {
            alayui = parent.layui;
        }
        modulelist = alayui.modulelist;
        if (typeof (modulelist) == "undefined") {
            $.ajax({
                'type': 'post',
                'url': '/SysAdmin/SysModule/GetModuleList',
                //async: false,
                error: function (message) {
                    x_admin_msg("保存失败");
                    console.info(message)
                },
                success: function (data) {
                    if (data.status == 200) {
                        alayui.modulelist = data.data;
                        set_bus_type();
                        //for (var i = 0; i < modulelist.length; i++) {
                        //    if (!modulelist[i].LAY_CHECKED) {
                        //        $("[data-bus-type='" + modulelist[i].Business + "']").remove();
                        //        $("#layui-demo").find("[data-bus-type='" + modulelist[i].Business + "']").remove();
                        //    } else {
                        //        $("[data-bus-type='" + modulelist[i].Business + "']").removeAttr("data-bus-type");
                        //        $("#layui-demo").find("[data-bus-type='" + modulelist[i].Business + "']").removeAttr("data-bus-type");
                        //    }
                        //}
                    } else {
                        x_admin_msg(data.msg);
                    }
                }
            });
        }
    })
})
function set_bus_type() {
    var alayui = layui;
    if (parent.layui) {
        alayui = parent.layui;
    }
    modulelist = alayui.modulelist;
    if (typeof (modulelist) != "undefined") {
        //for (var i = 0; i < modulelist.length; i++) {
        //    if (!modulelist[i].LAY_CHECKED) {
        //        $("[data-bus-type='" + modulelist[i].Business + "']").remove();
        //    } else {
        //        $("[data-bus-type='" + modulelist[i].Business + "']").removeAttr("data-bus-type");
        //    }
        //}
        for (var i = 0; i < modulelist.length; i++) {
            if (!modulelist[i].LAY_CHECKED) {
                $("[data-bus-type='" + modulelist[i].Business + "']").remove();
                $("#layui-demo").find("[data-bus-type='" + modulelist[i].Business + "']").remove();
            } else {
                $("[data-bus-type='" + modulelist[i].Business + "']").removeAttr("data-bus-type");
                $("#layui-demo").find("[data-bus-type='" + modulelist[i].Business + "']").removeAttr("data-bus-type");
            }
        }
    }
}



