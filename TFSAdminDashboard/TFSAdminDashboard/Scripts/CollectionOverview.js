function loadCollectionOverviewTabPanes() {
    loadTabPaneData('#identityservice', '#TpcList');
    loadTabPaneData('#projects', '#TpcList');
    loadTabPaneData('#buildservice', '#TpcList');
    loadTabPaneData('#machineservice', '#TpcList');
}

function clearCollectionOverviewTabPanes() {
    $('#identityservice').html("");
    $('#projects').html("");
    $('#buildservice').html("");
    $('#machineservice').html("");
}

$(function () {
    if ($('#TpcList').val() != '') {
        loadCollectionOverviewTabPanes();
    }

    $('#TpcList').change(function () {
        clearCollectionOverviewTabPanes();
        if ($('#TpcList').val() != '') {
            loadCollectionOverviewTabPanes();
        }
    });
});
