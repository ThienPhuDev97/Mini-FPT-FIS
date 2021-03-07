let ischecktrue = [];
let count = 0;
let ischeckALl = [];
let today = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate() + 1);

$('#datepicker1').datepicker({
    uiLibrary: 'bootstrap4',
    format: 'dd-mm-yyyy',
    maxDate: function () {
        return $('#datepicker2').val();
    }
}).on("change", function () {
    console.log("Got change event from field");
   
});
$('#datepicker2').datepicker({
    uiLibrary: 'bootstrap4',
    format: 'dd-mm-yyyy',
    minDate: function () {
        return $('#datepicker1').val();
    }
});



const handleAdd  = () => {
    let id = $('#inputGroupSelect01').val();
    window.location.href = "/Admin/AddQuestion/?idType=" + id;
}


const DeletedQuestion = (id_question) => {
    bootbox.confirm({
        message: "Bạn có chắc chắn muốn xóa không ?",
        buttons: {
            confirm: {
                label: 'Có',
                className: 'btn-success'
            },
            cancel: {
                label: 'Không',
                className: 'btn-danger'
            }
        },
        callback: function (result)
        {
            if (result == true) {
                $.ajax({
                    method: "GET",
                    url: "/Admin/DeletedQuestion",
                    data:
                    {
                        id_question: id_question
                    },
                    success: function (rs) {
                        window.location.href = '/Admin';

                    }
                })
            }
        }
    })
}


document.getElementById('checkALl').onclick = function (e) {
    let checkboxes = document.getElementsByName('check_qs');
    

    if (this.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = true;
        }
      
    }
    else {
        for (var i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = false;
        }
    }

};

const DeleteAll = () => {
    bootbox.confirm({
        message: "Bạn có chắc chắn muốn xóa hết không ?",
        buttons: {
            confirm: {
                label: 'Có',
                className: 'btn-success'
            },
            cancel: {
                label: 'Không',
                className: 'btn-danger'
            }
        },
        callback: function (result) {

            if (result == true) {
              
                let checkboxes = document.getElementsByName('check_qs');

                //console.log(checkboxes)

                for (var i = 0; i < checkboxes.length; i++) {
                    if (checkboxes[i].checked) {
                        ischecktrue.push(checkboxes[i].id)
                    }
                }
                if (ischecktrue.length === 0) {
                    bootbox.confirm("Bạn chưa chọn câu hỏi cần xóa", function (rusult2) {
                        
                    });
                    return;
                }

                console.log(ischecktrue)
                $.ajax({
                    method: "POST",
                    url: "/Admin/DeleteAll",
                    data:
                    {
                        ischecktrue: ischecktrue
                    },
                    success: function (rs) {
                        console.log(rs)
                        window.location.href = '/Admin';

                    }
                });
            }
        }
    });
    console.log("Bạn chưa chọn câu hỏi cần xóa");

}

const updateStatus = (index) => {
   
    let checkboxes = document.getElementsByName('check_qs');

    console.log(checkboxes)

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            ischecktrue.push(checkboxes[i].id)
        }
    }
    if (ischecktrue.length === 0) {
        bootbox.confirm("Bạn chưa chọn câu hỏi", function (rusult2) {

        });
        return
    }
    $.ajax({
        method: "POST",
        url: "/Admin/UpdateStatus",
        data:
        {
            id_question: ischecktrue,
            index: index
        },
        success: function (rs) {
            console.log(rs)
            window.location.href = '/Admin';

        }
    });
}
const duyet_ch = (id_status, id_qs) => {
    
    $.ajax({
        method: "POST",
        url: "/Admin/HandleUpdateStatus/",
        data:
        {
            id_qs, id_qs,
            id_status: id_status
        },
        success: function (rs) {
            console.log(rs)
            window.location.href = '/Admin';
        }
    });
}

const handleEdit = (id_question) => {
    console.log(id_question)
    $.ajax({
        method: "POST",
        url: "/Admin/HandleEdit",
        data:
        {
            id_question: id_question
        },
        success: function (rs) {
            console.log(rs)
            window.location.href = '/Admin';

        }
    });
}

const ExportExcel = () => {
    let Dm01 = $('#Dm01').val()
    let Dv01 = $('#Dv01').val()
    let Mt01 = $('#Mt01').val()
    let TypeQT01 = $('#TypeQT01').val()
    let datepicker1 = $('#datepicker1').val()
    let datepicker2 = $('#datepicker2').val()
    let Status01 = $('#Status01').val()
    window.location = "/Home/Test?Dm01=" + Dm01 + "&Dv01=" + Dv01 + "&Mt01=" + Mt01 + "&TypeQT01=" + TypeQT01 +
        "&datepicker1=" + datepicker1 + "&datepicker2=" + datepicker2 + "&Status01=" + Status01;
}


