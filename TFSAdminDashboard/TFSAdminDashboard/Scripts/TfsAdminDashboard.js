function loadTabPaneData(tabPaneSelector) {
    var actionUrl = $(tabPaneSelector).data('action');
    $(tabPaneSelector).load(actionUrl);
}

function loadTabPaneData(tabPaneSelector, tpcSelector) {
    var actionUrl = $(tabPaneSelector).data('action') + '/' + $(tpcSelector).val();
    $(tabPaneSelector).load(actionUrl);
}

function loadTabPaneData(tabPaneSelector, tpcSelector, tpSelector) {
    var actionUrl = $(tabPaneSelector).data('action') + '/' + $(tpcSelector).val() + '/' + $(tpSelector).val();
    $(tabPaneSelector).load(actionUrl);
}