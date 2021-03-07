let today2 = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
$('#datepicker').datepicker(
    {
        uiLibrary: 'bootstrap4',
        minDate: today2,
        format: 'dd-mm-yyyy'
    }
);
const xuliUpfile = () => {
    
    let fileExcel = $('#importExcel').val();
    alert(fileExcel);
    if (fileExcel == '') {
        bootbox.confirm("Bạn chưa chọn file", function (rusult2) { });
        return;
    }
   
   


    let file = $('#importExcel')[0].files[0];

    let data = new FormData();
    data.append('file', file);
    alert('chay xuống ajax')

    $.ajax({
        type: "POST",
        url: "/Home/Receive",
        data: data,
        processData: false,
        contentType: false,
        success: function (result) {

            alert(result)
        },
        error: function (err) {
            console.log(err)
        }
    });
}


const CapNhatData = () => {
    let id_dm = $('#id_dm').val();
    let id_mt = $('#id_mt').val();
    let id_dv = $('#id_dv').val();
    let end_date = $('#datepicker').val();
    
    let file = $('#upFile').val();
    if (id_dm == 0 || id_mt == 0 || id_dv == 0) {
        bootbox.confirm("Bạn chưa chọn đầy đủ các trường", function (rusult2) { });
        return;
    }
    if (file == '') {
        bootbox.confirm("Bạn chưa chọn file", function (rusult2) { });
        return;
    }
    
    let data = new FormData();
    data.append('file', $('#upFile')[0].files[0]);
    data.append('id_dm', id_dm);
    data.append('id_dv', id_dv);
    data.append('id_mt', id_mt);
    data.append('expiration_date', end_date);

    $.ajax({
        type: "POST",
        url: "/Home/ImportFile",
        data: data,
        processData: false,
        contentType: false,
        success: function (result) {
            window.location = "/Admin";
        },
        error: function (err) {
            console.log(err)
        }
    });
}
//"TestImport", "Home"

