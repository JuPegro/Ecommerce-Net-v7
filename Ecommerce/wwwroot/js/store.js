let datatable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Store/GetAll"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "description", "width": "40%" },
            {
                "data": "status",
                "render": function (data) {
                    if (data == true) {
                        return "Active"
                    } else {
                        return "Inactive"
                    }
                }, "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text=center">
                            <a href="/Admin/Store/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onclick=Delete("/Admin/Store/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer")>
                                <i class="bi bi-trash3-fill"></i>
                            </a>
                        </div>
                    `
                }, "width": "20%"
            }

        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure about deleting the store?",
        text: "The record cannot be recovered once it has been deleted.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((eraser) => {
        if (eraser) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}