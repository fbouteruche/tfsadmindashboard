$(function () {
    if ($('#TpcList').val() != '' && $('#TpList').val() != '') {
        var urlIdentityService = $('#identityservice').data('action');
        var urlIdentityService = urlIdentityService + '/' + $('#TpcList').val();
        var urlIdentityService = urlIdentityService + '/' + $('#TpList').val();
        $('#identityservice').load(urlIdentityService);

        var urlWorkItemService = $('#workitemservice').data('action');
        var urlWorkItemService = urlWorkItemService + '/' + $('#TpcList').val();
        var urlWorkItemService = urlWorkItemService + '/' + $('#TpList').val();
        $('#workitemservice').load(urlWorkItemService);

        var urlVersionControlService = $('#versioncontrolservice').data('action');
        var urlVersionControlService = urlVersionControlService + '/' + $('#TpcList').val();
        var urlVersionControlService = urlVersionControlService + '/' + $('#TpList').val();
        $('#versioncontrolservice').load(urlVersionControlService);

        var urlBuildService = $('#buildservice').data('action');
        var urlBuildService = urlBuildService + '/' + $('#TpcList').val();
        var urlBuildService = urlBuildService + '/' + $('#TpList').val();
        $('#buildservice').load(urlBuildService);

        var urlTestManagementService = $('#testmanagementservice').data('action');
        var urlTestManagementService = urlTestManagementService + '/' + $('#TpcList').val();
        var urlTestManagementService = urlTestManagementService + '/' + $('#TpList').val();
        $('#testmanagementservice').load(urlTestManagementService);
    }

    $('#TpcList').change(function () {
        $('#identityservice').html("");
        $('#workitemservice').html("");
        $('#versioncontrolservice').html("");
        $('#buildservice').html("");
        $('#testmanagementservice').html("");
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
        $('#identityservice').html("");
        $('#workitemservice').html("");
        $('#versioncontrolservice').html("");
        $('#buildservice').html("");
        $('#testmanagementservice').html("");
        if ($('#TpList').val() != '') {
            var urlIdentityService = $('#identityservice').data('action');
            var urlIdentityService = urlIdentityService + '/' + $('#TpcList').val();
            var urlIdentityService = urlIdentityService + '/' + $('#TpList').val();
            $('#identityservice').load(urlIdentityService);

            var urlWorkItemService = $('#workitemservice').data('action');
            var urlWorkItemService = urlWorkItemService + '/' + $('#TpcList').val();
            var urlWorkItemService = urlWorkItemService + '/' + $('#TpList').val();
            $('#workitemservice').load(urlWorkItemService);

            var urlVersionControlService = $('#versioncontrolservice').data('action');
            var urlVersionControlService = urlVersionControlService + '/' + $('#TpcList').val();
            var urlVersionControlService = urlVersionControlService + '/' + $('#TpList').val();
            $('#versioncontrolservice').load(urlVersionControlService);

            var urlBuildService = $('#buildservice').data('action');
            var urlBuildService = urlBuildService + '/' + $('#TpcList').val();
            var urlBuildService = urlBuildService + '/' + $('#TpList').val();
            $('#buildservice').load(urlBuildService);

            var urlTestManagementService = $('#testmanagementservice').data('action');
            var urlTestManagementService = urlTestManagementService + '/' + $('#TpcList').val();
            var urlTestManagementService = urlTestManagementService + '/' + $('#TpList').val();
            $('#testmanagementservice').load(urlTestManagementService);
            
        }
    });
});
