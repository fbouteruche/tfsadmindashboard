$(function () {
    if ($('#TpcList').val() != '') {
        var urlIdentityService = $('#identityservice').data('action');
        var urlIdentityService = urlIdentityService + '/' + $('#TpcList').val();
        $('#identityservice').load(urlIdentityService);
        var urlProjects = $('#projects').data('action');
        var urlProjects = urlProjects + '/' + $('#TpcList').val();
        $('#projects').load(urlProjects);
        var urlBuildService = $('#buildservice').data('action');
        var urlBuildService = urlBuildService + '/' + $('#TpcList').val();
        $('#buildservice').load(urlBuildService);
        var urlMachineService = $('#machineservice').data('action');
        var urlMachineService = urlMachineService + '/' + $('#TpcList').val();
        $('#machineservice').load(urlMachineService);
    }

    $('#TpcList').change(function () {
        $('#identityservice').html("");
        $('#projects').html("");
        $('#buildservice').html("");
        $('#machineservice').html("");
        if ($('#TpcList').val() != '') {
            var urlIdentityService = $('#identityservice').data('action');
            var urlIdentityService = urlIdentityService + '/' + $('#TpcList').val();
            $('#identityservice').load(urlIdentityService);
            var urlProjects = $('#projects').data('action');
            var urlProjects = urlProjects + '/' + $('#TpcList').val();
            $('#projects').load(urlProjects);
            var urlBuildService = $('#buildservice').data('action');
            var urlBuildService = urlBuildService + '/' + $('#TpcList').val();
            $('#buildservice').load(urlBuildService);
            var urlMachineService = $('#machineservice').data('action');
            var urlMachineService = urlMachineService + '/' + $('#TpcList').val();
            $('#machineservice').load(urlMachineService);
        }
    });
});
