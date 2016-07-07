//#region Helpers

function errorFn(jqXHR, textStatus, errorThrown) {
    console.log('ERROR!: ' + this.url);
    console.log(textStatus);
    console.log(errorThrown);
    updateResultsDisplay(this.url, 'error', textStatus, errorThrown);
}

function successFn(data, textStatus, jqXHR) {
    console.log('SUCCESS: ' + this.url);
    console.log(data);
    updateResultsDisplay(this.url, 'success', textStatus, null, data);
}


function updateResultsDisplay(url, type, textStatus, errorThrown, data) {
    url = decodeURIComponent(url);

    // Extract method name from url i.e. 'Get_Default' from '/mvc/Get_Default?{"a":1,"b":"b string with spaces!","c":"2016-07-01"}'
    var str = url;
    var re = /\/mvc\/([^\?]*)/;
    var m;
    var id;
    if ((m = re.exec(str)) !== null) {
        if (m.index === re.lastIndex) {
            re.lastIndex++;
        }
        id = m[1]; // method name = matches id's used in html

        switch (type) {
            case 'error': {
                $('#' + id + '_Url').html(url);
                $('#' + id).html('ERROR!<br>' + textStatus + '<br>' + errorThrown);
                break;
            }
            case 'success': {
                $('#' + id + '_Url').html(url);
                $('#' + id).html(JSON.stringify(data, null, 2));
                break;
            }
        }
    } else {
        alert('Failed to extract method name from url: ' + url);
    }
}

//#endregion

//#region GET

function Get_Default() {
    $.ajax({
        url: '/mvc/Get_Default',
        type: 'GET',
        dataType: 'json',
        data: JSON.stringify({ a: 1, b: 'b string with spaces!', c: '2016-07-01' }),
        contentType: 'application/json',
        error: errorFn,
        success: successFn
    });
}

function Get_RequestBody_ModelBinder_JsonNet() {
    $.ajax({
        url: '/mvc/Get_RequestBody_ModelBinder_JsonNet',
        type: 'GET',
        dataType: 'json',
        data: JSON.stringify({ a: 1, b: 'b string with spaces!', c: '2016-07-01' }),
        contentType: 'application/json',
        error: errorFn,
        success: successFn,
        processData: false  // not working
    });
}

function Get_QueryString_ModelBinder_JsonNet() {
    $.ajax({
        url: '/mvc/Get_QueryString_ModelBinder_JsonNet',
        type: 'GET',
        dataType: 'json',
        data: JSON.stringify({ a: 1, b: 'b string with spaces!', c: '2016-07-01' }),
        contentType: 'application/json',
        error: errorFn,
        success: successFn
    });
}

function Get_JsonNet() {
    $.ajax({
        url: '/mvc/Get_JsonNet',
        type: 'GET',
        dataType: 'json',
        data: { json: JSON.stringify({ a: 1, b: 'b string with spaces!', c: '2016-07-01' }) },
        contentType: 'application/json',
        error: errorFn,
        success: successFn
    });
}

//#endregion

//#region POST

function Post_FormData_Default() {
    $.ajax({
        url: '/mvc/Post_FormData_Default',
        type: 'POST',
        data: JSON.stringify({ a: 1, b: 'b string with spaces!', c: '2016-07-01' }),
        error: errorFn,
        success: successFn
    });
}

function Post_RequestBody_Default() {
    $.ajax({
        url: '/mvc/Post_RequestBody_Default',
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify({ a: 1, b: 'b string with spaces!', c: '2016-07-01' }),
        contentType: 'application/json',
        error: errorFn,
        success: successFn
    });
}

function Post_FormData_JsonNet() {
    $.ajax({
        url: '/mvc/Post_FormData_JsonNet',
        type: 'POST',
        data: { json: JSON.stringify({ a: 1, b: 'b string with spaces!', c: '2016-07-01' }) },
        error: errorFn,
        success: successFn
    });
}

function Post_RequestBody_ModelBinder_JsonNet() {
    $.ajax({
        url: '/mvc/Post_RequestBody_ModelBinder_JsonNet',
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify({ a: 1, b: 'b string with spaces!', c: '2016-07-01' }),
        contentType: 'application/json',
        error: errorFn,
        success: successFn
    });
}

//#endregion

$('.panel-heading').click(function (x) {
    var spinnerHtml = 'Requesting...';
    var preResults = $(this).parent().find('pre[id]');
    var fn = $(preResults[1]).prop('id') + '()';
    preResults.html(spinnerHtml);
    setTimeout(function () {
        eval(fn);
    }, 2000);
});

function init() {
    $('.panel pre[id]').html('Requesting...');
    setTimeout(function () {
        //Get_Default();
        //Get_RequestBody_ModelBinder_JsonNet();
        //Get_QueryString_ModelBinder_JsonNet();
        //Get_JsonNet();
        //Post_FormData_Default();
        //Post_RequestBody_Default();
        //Post_FormData_JsonNet();
        Post_RequestBody_ModelBinder_JsonNet();
    }, 1000);
}

init();