var utility = {
    
    getCountry: function () {

    },
    getState: function (countryId,url, callback) {
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
    getCity: function (stateId,url, callback) {
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
    populateCountryCityUpazillaDropDown: function (dropDownControlId, responseList, defaultvalue,isBangla) {
        utility.RemoveAllOptions(dropDownControlId, "Select");
        for (i = 0; i < responseList.length; i++) {
            var isselected = defaultvalue ? defaultvalue : null;
            utility.PopulateDropdown(dropDownControlId, (isBangla ? responseList[i].BnName : responseList[i].Name), responseList[i].Id, isselected);
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
       
        if ($('#' +link).parent().parent().hasClass('treeview')) {
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


}