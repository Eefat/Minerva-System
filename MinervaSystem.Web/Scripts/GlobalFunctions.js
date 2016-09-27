//----- global variables -----

var G_oImageFileExtensions = [".jpg", ".jpeg", ".gif", ".bmp", ".png", ".tif", ".emf"];

//----- end of global variables -----

function ParseJsonDate(jsonDate) {
    try {
        var reg = /-?\d+/
        var matches = reg.exec(jsonDate);
        var oDate = new Date(parseInt(matches[0]));
        return oDate;
    }
    catch (ex) { return null; }
}

function GetJsonColumnValue(jsonValue) {
    if (jsonValue != null)
        return jsonValue.replace(/\n/g, "<br>");
    return "";
}

function FormatDateValue(oDate, options) {
    //Following Asp.Net Convention
    //This function supprots the following formats, if provided format doesn't match then it returns "MM/dd/yyyy"

    var defaults = { format: "MM/dd/yyyy" };
    defaults = $.extend({}, defaults, options);
    //override: {"format": "MM-dd-yyyy"}
    var formattedDate = "";

    if (oDate && oDate instanceof Date) {
        var date = oDate.getDate().toString();
        if (date.length == 1) date = "0" + date;

        var month = (oDate.getMonth() + 1).toString();
        if (month.length == 1) month = "0" + month;

        var year = oDate.getFullYear().toString();

        if (defaults.format == "dd/MM/yyyy") formattedDate = date + "/" + month + "/" + year;
        else if (defaults.format == "yyyy/MM/dd") formattedDate = year + "/" + month + "/" + date;
        else if (defaults.format == "yyyy/dd/MM") formattedDate = year + "/" + date + "/" + month;
        else if (defaults.format == "yyyy-dd-MM") formattedDate = year + "-" + date + "-" + month;
        else if (defaults.format == "yyyy-MM-dd") formattedDate = year + "-" + month + "-" + date;
        else if (defaults.format == "dd-MM-yyyy") formattedDate = date + "-" + month + "-" + year;
        else if (defaults.format == "MM-dd-yyyy") formattedDate = month + "-" + date + "-" + year;
        else if (defaults.format == "yyyy-MMM-dd") formattedDate = year + "-" + GetMonthAsString(oDate) + "-" + date;
        else if (defaults.format == "dd-MMM-yyyy") formattedDate = date + "-" + GetMonthAsString(oDate) + "-" + year;
        else if (defaults.format == "MMM dd, yyyy") formattedDate = GetMonthAsString(oDate) + " " + date + ", " + year;
        else if (defaults.format == "MMMM dd, yyyy") formattedDate = GetMonthAsString(oDate, { "format": "MMMM" }) + " " + date + ", " + year;
        else if (defaults.format == "MM/dd/yyyy hh:mm tt") formattedDate = month + "/" + date + "/" + year + " " + GetTimeAsString(oDate);
        else if (defaults.format == "MM/dd/yyyy HH:mm") formattedDate = month + "/" + date + "/" + year + " " + GetTimeAsString(oDate, { "format": "HH:mm" });
        else if (defaults.format == "dd/MM/yyyy hh:mm tt") formattedDate = date + "/" + month + "/" + year + " " + GetTimeAsString(oDate);
        else if (defaults.format == "dd/MM/yyyy HH:mm") formattedDate = date + "/" + month + "/" + year + " " + GetTimeAsString(oDate, { "format": "HH:mm" });
        else formattedDate = month + "/" + date + "/" + year;
    }
    return formattedDate;
}

function GetMonthAsString(oDate, options) {
    var defaults = { format: "MMM" };
    defaults = $.extend({}, defaults, options);
    //override the default: {"format": "MMMM"}
    var month = "";

    if (oDate && oDate instanceof Date) {
        if (defaults.format == "MMMM") {
            switch (oDate.getMonth()) {
                case 0: month = "January"; break;
                case 1: month = "February"; break;
                case 2: month = "March"; break;
                case 3: month = "April"; break;
                case 4: month = "May"; break;
                case 5: month = "June"; break;
                case 6: month = "July"; break;
                case 7: month = "August"; break;
                case 8: month = "September"; break;
                case 9: month = "October"; break;
                case 10: month = "November"; break;
                case 11: month = "December"; break;
                default: month = "";
            }
        }
        else {
            switch (oDate.getMonth()) {
                case 0: month = "Jan"; break;
                case 1: month = "Feb"; break;
                case 2: month = "Mar"; break;
                case 3: month = "Apr"; break;
                case 4: month = "May"; break;
                case 5: month = "Jun"; break;
                case 6: month = "Jul"; break;
                case 7: month = "Aug"; break;
                case 8: month = "Sep"; break;
                case 9: month = "Oct"; break;
                case 10: month = "Nov"; break;
                case 11: month = "Dec"; break;
                default: month = "";
            }
        }
    }
    return month;
}

function GetTimeAsString(oDate, options) {
    var defaults = { format: "hh:mm tt" };
    defaults = $.extend({}, defaults, options);
    //override the default: {"format": "HH:mm"}
    var strTime = "";

    if (defaults.format == "HH:mm") {
        strTime = oDate.getHours() + ":" + oDate.getMinutes();
    }
    else {
        var hours = oDate.getHours();
        var minutes = oDate.getMinutes();
        var ampm = (hours >= 12) ? "PM" : "AM";

        hours = hours % 12;
        hours = hours ? hours : 12;	// the hour '0' should be '12'
        minutes = (minutes < 10) ? "0" + minutes : minutes;
        strTime = hours + ":" + minutes + " " + ampm;
    }
    return strTime;
}

function SingleSelectCheckboxInGroup(chk) {
    var name = $(chk).attr("name");
    if (typeof name != "undefined")
    {
        if (!$(chk).prop("checked")) {
            $(chk).prop("checked", false);
        }
        else {
            $("input[type='checkbox'][name='" + name + "']").prop("checked", false);   //deselect all with same name
            $(chk).prop("checked", true);
        }
    }
}

function KendoAlert(msg) {
    var content = "<p>" + msg + "</p><div class='modal-btn-toolbar'><button onclick=\"$(this).closest('[data-role=window]').kendoWindow('destroy')\" class='btn btn-default'><i class='fa fa-check-square-o btn-save'></i> OK</button></div>";
    $("<div id='AlertBox'></div>").html(content).kendoWindow({ width: 500, modal: true, visible: false, resizable: false, draggable: false, title: "Message" }).data("kendoWindow").center().open();
    $("#AlertBox").closest(".k-window").css({ top: "20%" });
}

function HashTable() {
    this.length = 0;
    this.items = [];
	
    for (var i = 0; i < arguments.length; i += 2) {
        if (typeof (arguments[i + 1]) != "undefined") {
            this.items[arguments[i]] = arguments[i + 1];
            this.length++;
        }
    }

    this.removeItem = function (in_key) {
        var tmp_previous;
        if (typeof (this.items[in_key]) != "undefined") {
            this.length--;
            tmp_previous = this.items[in_key];
            delete this.items[in_key];
        }
        return tmp_previous;
    }

    this.getItem = function (in_key) {
        return this.items[in_key];
    }

    this.setItem = function (in_key, in_value) {
        var tmp_previous;
        if (typeof (in_value) != "undefined") {
            if (typeof (this.items[in_key]) == "undefined") this.length++;
            else tmp_previous = this.items[in_key];
            this.items[in_key] = in_value;
        }
        return tmp_previous;
    }

    this.hasItem = function (in_key) {
        return typeof (this.items[in_key]) != "undefined";
    }

    this.keys = function () {
        var keys = [];
        for (var k in this.items) {
            if (this.hasItem(k)) keys.push(k);
        }
        return keys;
    }

    this.values = function () {
        var values = [];
        for (var k in this.items) {
            if (this.hasItem(k))
                values.push(this.items[k]);
        }
        return values;
    }

    this.each = function (fn) {
        for (var k in this.items) {
            if (this.hasItem(k))
                fn(k, this.items[k]);
        }
    }

    this.clear = function () {
        for (var i in this.items)
            delete this.items[i];
        this.length = 0;
    }
}

function GetQueryString(options) {
    var opt = $.extend({}, { lowercase: false }, options);
    var queryStringVals = {};

    var qs = location.search.slice(1).split('&');
    for (var i = 0; i < qs.length; i++) {
        var param = qs[i].split('=');
        var paramName = opt.lowercase ? param[0].toLowerCase() : param[0];
        queryStringVals[paramName] = decodeURIComponent(param[1] || "").replace(/\+/g, ' ');
    }

    return queryStringVals;
}