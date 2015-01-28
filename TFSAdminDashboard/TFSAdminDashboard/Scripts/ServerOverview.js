$(function () {
    var urlIdentityService = $('#identityservice').data('action');
    $('#identityservice').load(urlIdentityService);
    var urlProjectCollections = $('#projectcollections').data('action');
    $('#projectcollections').load(urlProjectCollections);
    var urlOrganizationalCatalogService = $('#organizationcatalogservice').data('action');
    $('#organizationcatalogservice').load(urlOrganizationalCatalogService);
    var urlInfrastructureCatalogService = $('#infrastructurecatalogservice').data('action');
    $('#infrastructurecatalogservice').load(urlInfrastructureCatalogService);
    var urlMachineService = $('#machineservice').data('action');
    $('#machineservice').load(urlMachineService);
});
