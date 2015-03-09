function loadTabPaneData(tabPaneSelector) {
    var actionUrl = $(tabpaneId).data('action');
    $(tabpaneId).load(actionUrl);
}

function loadTabPaneData(tabPaneSelector, tpcSelector) {
    var actionUrl = $(tabpaneId).data('action') + '/' + $(tpcSelector).val();
    $(tabpaneId).load(actionUrl);
}

function loadTabPaneData(tabPaneSelector, tpcSelector, tpSelector) {
    var actionUrl = $(tabpaneId).data('action') + '/' + $(tpcSelector).val() + '/' + $(tpSelector).val();
    $(tabpaneId).load(actionUrl);
}