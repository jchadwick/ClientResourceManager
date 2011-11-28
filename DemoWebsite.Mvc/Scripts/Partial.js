function makeJsonCall(url) {
    console.log("Making JSON data request...");

    $.ajax({
        url: url,
        success: function (data) {
            console.log("JSON data request succeeded");
            $('#partial-view')
                    .html('<span>The time is ' + data.timestamp + '</span>');
        }
    });
    }

console.log("Loaded Partial.js");