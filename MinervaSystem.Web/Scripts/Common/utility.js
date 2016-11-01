
var utility = {

    loadDateSorting: function () {
        jQuery.extend(jQuery.fn.dataTableExt.oSort, {
            "date-uk-pre": function (a) {
                var ukDatea = a.split('/');
                return (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
            },

            "date-uk-asc": function (a, b) {
                return ((a < b) ? -1 : ((a > b) ? 1 : 0));
            },

            "date-uk-desc": function (a, b) {
                return ((a < b) ? 1 : ((a > b) ? -1 : 0));
            }
        });
    },
    getCountry: function () {

    },
    getState: function (countryId, url, callback) {
        $.ajax({
            url: url,
            type: "POST",
            data: { countryId: countryId },
            dataType: "json",
            success: function (response) {
                if (callback && typeof callback == 'function') {
                    callback(response);
                }
            }, error: function (response) {
                console.log(response);
            }
        });
    },
    getCity: function (stateId, url, callback) {
        $.ajax({
            url: url,
            type: "POST",
            data: { stateId: stateId },
            dataType: "json",
            success: function (response) {
                if (callback && typeof callback == 'function') {
                    callback(response);
                }
            }, error: function (response) {
                console.log(response);
            }
        });
    },
    getUpazilla: function (cityId, url, callback) {
        $.ajax({
            url: url,
            type: "POST",
            data: { cityId: cityId },
            dataType: "json",
            success: function (response) {
                if (callback && typeof callback == 'function') {
                    callback(response);
                }
            }, error: function (response) {
                console.log(response);
            }
        });
    },
    populateCountryCityUpazillaDropDown: function (dropDownControlId, responseList, defaultvalue, isBangla, callback) {
        utility.RemoveAllOptions(dropDownControlId, "Select");
        for (i = 0; i < responseList.length; i++) {
            var isselected = null;
            utility.PopulateDropdown(dropDownControlId, (isBangla ? responseList[i].BnName : responseList[i].Name), responseList[i].Id, isselected);
        }
        if (defaultvalue) $(dropDownControlId).val(defaultvalue);
        if (callback && typeof callback == 'function') {
            callback();
        }
    },
    PopulateDropdown: function (dropDownControlId, text, value, isselected) {
        try {
            var option = $("<option/>");
            option.html(text);
            option.attr('value', value);
            if (isselected) {
                option.prop('selected', true);
            }

            $(dropDownControlId).append(option);
        }
        catch (e) { console.log(e); }
    },
    RemoveAllOptions: function (dropDownControlId, defaulttext) {
        $(dropDownControlId).empty();
        var text = defaulttext ? defaulttext : 'Select';
        $(dropDownControlId).append('<option Value="">' + text + '</option>');
    },
    addActiveLink: function (link) {
        utility.removeActiveLink();

        if ($('#' + link).parent().parent().hasClass('treeview')) {
            $('#lnkInbox').parent().parent().addClass('active');
            $('#lnkInbox').parent().parent().addClass('menu-open');
        }
        $('#' + link).addClass('active');
    },
    removeActiveLink: function () {
        $('#homeSideBarMenu').removeClass('active');
        $('#farmerSideBarMenu').removeClass('active');
        $('#sOrderSideBarMenu').removeClass('active');
        $('#homeSideBarMenu').removeClass('active');
        $('#sugerMillSideBarMenu').removeClass('active');
        $('#homeSideBarMenu').removeClass('active');

        $('#lnkReportFarmer').removeClass('active');
        $('#lnkReportCollector').removeClass('active');
        $('#lnkInbox').removeClass('active');
        $('#lnkCompose').removeClass('active');
    },
    dtConvFromJSON: function (data) {
        if (data == null) return '1/1/1950';
        var r = /\/Date\(([0-9]+)\)\//gi
        var matches = data.match(r);
        if (matches == null) return '1/1/1950';
        var result = matches.toString().substring(6, 19);
        var epochMilliseconds = result.replace(
        /^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
        '$1');
        var b = new Date(parseInt(epochMilliseconds));
        var c = new Date(b.toString());
        var curr_date = c.getDate();
        var curr_month = c.getMonth() + 1;
        var curr_year = c.getFullYear();
        var curr_h = c.getHours();
        var curr_m = c.getMinutes();
        var curr_s = c.getSeconds();
        var curr_offset = c.getTimezoneOffset() / 60
        var d = curr_date + '/' + curr_month.toString() + '/' + curr_year + " " + curr_h + ':' + curr_m + ':' + curr_s;
        return d;
    },
    monthDiff: function (d1, d2) {
        d2.setDate(d2.getDate() - d1.getDate());
        var months;
        months = (d2.getFullYear() - d1.getFullYear()) * 12;
        months -= d1.getMonth() + 1;
        months += d2.getMonth();
        return months <= 0 ? 0 : months + 2;
    },
    startDate: '',
    endDate:'',
    loadScheduledSummayTable: function (url, startDate, endDate) {
        $('#tblScheduleSummay_previous').on('click', function () {
            var stDate = utility.startDate;
            var enDate = utility.startDate;
            stDate = Date.parse(stDate);
            enDate = Date.parse(enDate);
            utility.loadScheduledSummayTable(url, stDate.addDays(-6).toString('dd/MM/yyyy'), enDate.addDays(-1).toString('dd/MM/yyyy'));
        });
        $('#tblScheduleSummay_next').on('click', function () {
            var stDate = utility.endDate;
            var enDate = utility.endDate;
            stDate = Date.parse(stDate, "dd/MM/yyyy");
            enDate = Date.parse(enDate, "dd/MM/yyyy");
            utility.loadScheduledSummayTable(url, enDate.addDays(1).toString('dd/MM/yyyy'), enDate.addDays(7).toString('dd/MM/yyyy'));
        });
        var SchedulerWeekSummary = new Object();
        SchedulerWeekSummary.startDate = startDate ? startDate : null;
        SchedulerWeekSummary.endDate = endDate ? endDate : null;
        things = JSON.stringify({ 'schedulerWeekSummary': SchedulerWeekSummary });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            url: url,
            type: "POST",
            //dataType: 'json',
            data: things,
            success: function (response) {
                if (response.startDate) utility.startDate = FormatDateValue(ParseJsonDate(response.startDate), { "format": "MM/dd/yyyy" });
                if (response.endDate) utility.endDate = FormatDateValue(ParseJsonDate(response.endDate), { "format": "MM/dd/yyyy" });
                var tbody = $('tbody', '#tblScheduleSummay');
                tbody.empty();
                for (var i = 0; i < response.aaData.length; i++) {
                    var element = response.aaData[i];
                    var tr = $('<tr>').appendTo(tbody);
                    $('<td align="center">').html(FormatDateValue(ParseJsonDate(element.date), { "format": "dd/MM/yyyy" })).appendTo(tr);
                    $('<td align="right">').html(element.totalScheduledAmount).appendTo(tr);
                    $('<td align="right">').html(1200 - parseFloat(element.totalScheduledAmount)).appendTo(tr);
                }

            }, error: function (response) {
                alert("Error occured!!! Contact to admonistrator!!!");
            }
        });
    }
}

var supplyOrder1 = {
    smsData: "",
    initFarmerCheckBox: function (datatable) {
        $('#cbFarmerSelectAll').on('click', function () {
            var rows = datatable.rows({ 'search': 'applied' }).nodes();
            $('input[type="checkbox"]', rows).prop('checked', this.checked);
        });
        $('#cbFarmerSelectAll tbody').on('change', 'input[type="checkbox"]', function () {
            if (!this.checked) {
                var el = $('#cbFarmerSelectAll').get(0);
                if (el && el.checked && ('indeterminate' in el)) {
                    el.indeterminate = true;
                }
            }
        });
    },
    deleteBulkFarmers: function (datatable, url, callback) {
        var listIds = [];
        datatable.$('input[type="checkbox"].childCBFarmer').each(function () {
            if (this.checked) {
                var tr = $(this).closest("tr");
                var row = datatable.row(tr);
                listIds.push(row.data().Id);
            }
        });
        things = JSON.stringify({ 'farmerIds': listIds });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            url: url,
            type: "POST",
            //dataType: 'json',
            data: things,
            success: function (response) {
                if (response.status == 0) {
                    if (callback && typeof callback === 'function') {
                        callback(response);
                    }
                } else {
                    alert(response.responseMsg);
                }

            }, error: function (response) {
                alert("Error occured!!! Contact to admonistrator!!!");
            }
        });
    },
    initSupplyInformationCheckBox: function (datatable) {
        $('#cbSiSelectAll').on('click', function () {
            var rows = datatable.rows({ 'search': 'applied' }).nodes();
            $('input[type="checkbox"]', rows).prop('checked', this.checked);
        });
        $('#cbSiSelectAll tbody').on('change', 'input[type="checkbox"]', function () {
            if (!this.checked) {
                var el = $('#cbSiSelectAll').get(0);
                if (el && el.checked && ('indeterminate' in el)) {
                    el.indeterminate = true;
                }
            }
        });
    },
    deleteBulkSI: function (datatable, url) {
        var listIds = [];
        datatable.$('input[type="checkbox"].siCB').each(function () {
            if (this.checked) {
                var tr = $(this).closest("tr");
                var row = datatable.row(tr);
                listIds.push(row.data().Id);
            }
        });
        things = JSON.stringify({ 'supplyIds': listIds });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            url: url,
            type: "POST",
            //dataType: 'json',
            data: things,
            success: function (response) {
                if (response.status == 0) {
                    searchSupplyInformations();
                } else {
                    alert(response.responseMsg);
                }

            }, error: function (response) {
                alert("Error occured!!! Contact to admonistrator!!!");
            }
        });
    },
    initSupplyOrderCheckBox: function (datatable) {
        $('#cbSoSelectAll').on('click', function () {
            var rows = datatable.rows({ 'search': 'applied' }).nodes();
            $('input[type="checkbox"].childCB', rows).prop('checked', this.checked);
        });
        $('#cbSoSelectAll tbody').on('change', 'input[type="checkbox"].childCB', function () {
            if (!this.checked) {
                var el = $('#cbSoSelectAll').get(0);
                if (el && el.checked && ('indeterminate' in el)) {
                    el.indeterminate = true;
                }
            }
        });
    },
    initTodaysSupplyOrderCheckBox: function (datatable) {
        $('#cbTSOSelectAll').on('click', function () {
            var rows = datatable.rows({ 'search': 'applied' }).nodes();
            $('input[type="checkbox"]', rows).prop('checked', this.checked);
        });
        $('#cbTSOSelectAll tbody').on('change', 'input[type="checkbox"]', function () {
            if (!this.checked) {
                var el = $('#cbTSOSelectAll').get(0);
                if (el && el.checked && ('indeterminate' in el)) {
                    el.indeterminate = true;
                }
            }
        });
    },
    deleteBulkSO: function (datatable, url, parentTr, callback) {
        var listIds = [];
        datatable.$('input[type="checkbox"].childCB').each(function () {
            if (this.checked) {
                var tr = $(this).closest("tr");
                var row = datatable.row(tr);
                listIds.push(row.data().Id);
            }
        });
        things = JSON.stringify({ 'schedulerIds': listIds });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            url: url,
            type: "POST",
            //dataType: 'json',
            data: things,
            success: function (response) {
                if (response.status == 0) {
                    winViewItem.close();
                    $(parentTr).find("i.details-control").click();
                    $(parentTr).find("i.details-control").click();
                    if (callback && typeof callback === 'function') {
                        callback(response);
                    }
                } else {
                    alert(response.responseMsg);
                }

            }, error: function (response) {
                alert("Error occured!!! Contact to admonistrator!!!");
            }
        });
    },
    sendBulkSMSsi: function (datatable) {
        datatable.$('input[type="checkbox"].siCB').each(function () {
            if (this.checked) {
                var tr = $(this).closest("tr");
                var row = datatable.row(tr);
                //alert(row.data());

                var smsRequest = new Object();
                smsRequest.responseMsg = "Prio " + row.data().FarmerName + ", Apner member id " + row.data().MemberKey + "."
                                         + "Apner chashkrito jomir poriman " + row.data().LandArea + " bigha "
                                         + "ebong shomvabbo fosholer poriman " + row.data().EstimatedAmount + " ton. Apner tothho shothik vabe halnagad kora hoese."
                                         + "Akh shongroher poroborti tarikh apnake sms er maddhome janie dea hobe."
                                         + " Eita supply order create korar shathe shathei jabe";
                smsRequest.mobileNo = supplyOrder1.smsData.countryCode + row.data().MobileNo;
                supplyOrder1.sendSMS(smsRequest);
            }
        });
    },
    sendBulkSMSsO: function (datatable) {
        datatable.$('input[type="checkbox"].childCB').each(function () {
            if (this.checked) {
                var tr = $(this).closest("tr");
                var row = datatable.row(tr);
                // alert(row.data());

                var smsRequest = new Object();
                smsRequest.responseMsg = "Prio " + row.data().FarmerName + ", " + row.data().SugerMillName + " kotipokkho agami "
                                         + "" + FormatDateValue(ParseJsonDate(row.data().CollectionDate), { "format": "dd-MM-yyyy" }) + " tarikhe apner kas theke " + row.data().AmounttoCollect + " ton akh shongroho korbe."
                                         + "Apnake jotha shomoe akh nie uposthit hober jonno onurodh kora hosse."
                smsRequest.mobileNo = supplyOrder1.smsData.countryCode + row.data().MobileNo;
                supplyOrder1.sendSMS(smsRequest);
            }
        });
    },
    sendSMS: function (response) {
        //var smsRequest = supplyorder.getSMSrequest(response);
        window.location.href = "https://rest.nexmo.com/sms/xml?api_key=a04d07fb&api_secret=83bfa18e&from=Admin&to=" + response.mobileNo + "&text=" + response.responseMsg;
    },
    getSMSrequest: function (response) {
        var smsRequest = new Object();
        smsRequest.from = response.from;
        smsRequest.to = response.mobileNo;
        smsRequest.text = response.responseMsg;
        return smsRequest;
    },
}


