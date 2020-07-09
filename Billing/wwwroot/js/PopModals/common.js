var addStockUrl = "/Stock/StockPost/";
var editStockUrl = "/Stock/StockEditPost/";
var addCatagoryUrl = '/Category/CategoryPost/';
var editCategoryUrl = '/Category/CategoryEditPost/';
var AddProductTypeUrl = '/ProductType/ProductTypePost/';
var EditProductTypeUrl = '/ProductType/ProductTypeEditPost/';
var AddCompanyUrl = '/Company/CompanyPost/';
var EditCompanyUrl = '/Company/CompanyEditPost/';
var AddEmpUrl = '/Employee/EmployeePost/';
var EditEmpUrl = '/Employee/EmployeeEditPost/';
var AddClientUrl = '/Client/ClientPost/';
var EditClientUrl = '/Client/ClientEditPost/';
var AddSuplierUrl = '/Suplier/SuplierPost/';
var EditSuplierUrl = '/Suplier/SuplierEditPost/';
var payinvoice = '/Stock/PayInvoice/';
var companyprofileurl = '/Companyprofile/updateCompanyprofile';
function JsonPOST(url, data) {
    $.ajax({
        url: url,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json',
        data: JSON.stringify(data),
        processData: false,
        async: true,
        success: function (response) {
            sweetAlert('Congratulations!', response.status, 'success');
            $(".modal").modal("hide");
        },
        error: function (e) {
            swal("Oops", e.responseText, "error");
            $(".modal").modal("hide");
        }
    });
}

function DoAction(action, data) {
    if (action === "Add Stock") {
        JsonPOST(addStockUrl, data);
        LoadData();
    }
    else if (action === "Edit Stock") {
        JsonPOST(editStockUrl, data);
        LoadData();
    }
    else if (action === "Add Caytogory") {
        JsonPOST(addCatagoryUrl, data);
        LoadData();
    }
    else if (action === "Edit Caytogory") {
        JsonPOST(editCategoryUrl, data);
        LoadData();
    }
    else if (action === "Add ProductType") {
        JsonPOST(AddProductTypeUrl, data);
        LoadData();
    }
    else if (action === "Edit ProductType") {
        JsonPOST(EditProductTypeUrl, data);
        LoadData();
    }
    else if (action === "Add Product Company") {
        JsonPOST(AddCompanyUrl, data);
        LoadData();
    }
    else if (action === "Edit Product Company") {
        JsonPOST(EditCompanyUrl, data);
        LoadData();
    }
    else if (action === "Add Product Company") {
        JsonPOST(AddCompanyUrl, data);
        LoadData();
    }
    else if (action === "Edit Product Company") {
        JsonPOST(EditCompanyUrl, data);
        LoadData();
    }
    else if (action === "Add Employee") {
        JsonPOST(AddEmpUrl, data);
        LoadData();
    }
    else if (action === "Edit Employee") {
        JsonPOST(EditEmpUrl, data);
        LoadData();
    }
    else if (action === "Add Client") {
        JsonPOST(AddClientUrl, data);
        LoadData();
    }
    else if (action === "Edit Client") {
        JsonPOST(EditClientUrl, data);
        LoadData();
    }
    else if (action === "Add Suplier") {
        JsonPOST(AddSuplierUrl, data);
        LoadData();
    }
    else if (action === "Edit Suplier") {
        JsonPOST(EditSuplierUrl, data);
        LoadData();
    }
    else if (action === "Pay Invoice") {
        JsonPOST(payinvoice, data);
    }
    else if (action === "Company Profile") {
        JsonPOST(companyprofileurl, data);
    }
}