function loadProjectOverviewTabPanes() {
    loadTabPaneData('#identityservice', '#TpcList', '#TpList');
    loadTabPaneData('#workitemservice', '#TpcList', '#TpList');
    loadTabPaneData('#versioncontrolservice', '#TpcList', '#TpList');
    loadTabPaneData('#buildservice', '#TpcList', '#TpList');
    loadTabPaneData('#testmanagementservice', '#TpcList', '#TpList');
}

function clearProjectOverviewTabPanes() {
    $('#identityservice').html("");
    $('#workitemservice').html("");
    $('#versioncontrolservice').html("");
    $('#buildservice').html("");
    $('#testmanagementservice').html("");
}

$(function () {
    if ($('#TpcList').val() != '' && $('#TpList').val() != '') {
        loadProjectOverviewTabPanes();
    }

    $('#TpcList').change(function () {
        clearProjectOverviewTabPanes();
        if ($('#TpcList').val() != '') {
            var URL = $('#TpcList').data('tpclistaction');
            var URL = URL + '/' + $('#TpcList').val();
            $('#TpList').load(URL);
        }
        else
        {
            $('#TpList').html('<option value="">Select the project</option>');
        }
    });

    $('#TpList').change(function () {
        clearProjectOverviewTabPanes();
        if ($('#TpList').val() != '') {
            loadProjectOverviewTabPanes();
        }
    });
});
