

//厂家信息
function AjaxGetVendorInfo(id) {
    Ext.Ajax.request({
        url: '/AjaxService/GetVendorInfo',
        method: 'post',
        cache: false,
        params: {
            id: id,
            rnd: Math.random()
        },
        success: function (response) {

            //var jsonstr = eval('(' + response.responseText + ')');
            var jsonstr = Ext.JSON.decode(response.responseText);      //JSON对象化

            var tb_vendorBody = Ext.get("tb_vendor_body");//**tb_vendor_body
            Ext.DomHelper.overwrite(tb_vendorBody, ""); //清空

            for (var i = 0; i < jsonstr.length; i++) {
                var trTag = {
                    tag: "tr", children: [
                        { tag: "td", 'class':"hide",html: jsonstr[i].ID },
                        { tag: "td", html: jsonstr[i].VendorDescr },
                        { tag: "td", html: jsonstr[i].Description },
                        { tag: "td", html: jsonstr[i].SystemType },
                        { tag: "td", html: jsonstr[i].DataVersion }
                    ]
                };
                Ext.DomHelper.append(tb_vendorBody, trTag); //将tr追加到table
            }
            //Ext.Msg.alert("提示", jsonstr[0].VendorDescr);
        }
    });
}


//系统模式
function AjaxGetPattern() {
    
    Ext.Ajax.request({
        url: '/AjaxService/GetPattern',
        method: 'post',
        success: function (response) {

            var jsonstr = eval('(' + response.responseText + ')');

            var obj = Ext.get("cmbSystemPattern"); //**cmbSystemPattern
            Ext.DomHelper.overwrite(obj, ""); //清空

            for (var i = 0; i < jsonstr.length; i++) {
                var optTag = "<option value='" + jsonstr[i].ID + "'>" + jsonstr[i].Description + "</option>";
                Ext.DomHelper.append(obj, optTag); //将option追加到select
            }
        }
        
    });
}

//系统类型
function AjaxGetSysType() {
    Ext.Ajax.request({
        url: '/AjaxService/GetSysType',
        method: 'post',
        success: function (response) {

            var jsonstr = eval('(' + response.responseText + ')');

            var obj = Ext.get("cmbSystemType"); //**cmbSystemType
            Ext.DomHelper.overwrite(obj, ""); //清空

            for (var i = 0; i < jsonstr.length; i++) {
                var optTag = "<option value='" + jsonstr[i].ID + "'>" + jsonstr[i].Description + "</option>";
                Ext.DomHelper.append(obj, optTag); //将option追加到select
            }
        }

    });
}

//厂家
function AjaxGetFactory() {

    Ext.Ajax.request({
        url: '/AjaxService/GetFactory',
        method: 'post',
        success: function (response) {

            var jsonstr = eval('(' + response.responseText + ')');

            var obj = Ext.get("cmbFactory"); //**cmbGlazingType
            Ext.DomHelper.overwrite(obj, ""); //清空

            for (var i = 0; i < jsonstr.length; i++) {
                var optTag = "<option value='" + jsonstr[i].ID + "'>" + jsonstr[i].Description + "</option>";
                Ext.DomHelper.append(obj, optTag); //将option追加到select
            }
        }

    });
}

//压条安装类型
function AjaxGetGlazingType() {

    Ext.Ajax.request({
        url: '/AjaxService/GetGlazingType',
        method: 'post',
        success: function (response) {

            var jsonstr = eval('(' + response.responseText + ')');

            var obj = Ext.get("cmbGlazingType"); //**cmbGlazingType
            Ext.DomHelper.overwrite(obj, ""); //清空

            for (var i = 0; i < jsonstr.length; i++) {
                var optTag = "<option value='" + jsonstr[i].ID + "'>" + jsonstr[i].Description + "</option>";
                Ext.DomHelper.append(obj, optTag); //将option追加到select
            }
        }

    });
}