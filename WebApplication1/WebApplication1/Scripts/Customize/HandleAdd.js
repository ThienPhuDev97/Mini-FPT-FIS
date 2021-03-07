let today2 = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate() + 1);
$('#datepicker').datepicker(
    {
    uiLibrary: 'bootstrap4',
    minDate: today2,
    format: 'dd-mm-yyyy'
    }
);

let chiso = -1;

let answer_list = [];

$(document).ready(function ()
    {
        var now = new Date();
        var today = 'QT' + '-' + now.getMilliseconds() + '-' + now.getMinutes() + '-' + now.getHours() + '-' + now.getDate() + '-' + (now.getMonth() + 1) +
            '-' + now.getFullYear();
        $('#codequestion').val(today);
    }
)
$('#upload1').hide();



const handleAdd = () =>
    {
        id = $('#inputGroupSelect01').val();
        window.location.href = '/Admin/AddQuestion?idType=' + id;
    }
const handleAddAnswer = () =>
{
   
    var explain = CKEDITOR.instances['explain'].getData();
    var cautl = CKEDITOR.instances['cautl'].getData();

    
    var type_As = "2";
    if (cautl == "") { $('#error_answer').html('Không được để trống'); return }
    $('#error_answer').html('')
    //if (explain == "") { $('#error_explain').html('Không được để trống'); return}
    //$('#error_explain').html('')
    //if (explain == "") { $('#error_explain').html('Không được để trống'); return}
        
       
    


    $('#showerror').html('')
    let ischeck = $('#txtAddAs').text();

    if (ischeck === 'Cập Nhật câu trả lời')
        {
            let updateAnswer = { cautl: cautl, explain: explain, type_As: type_As };
            answer_list[chiso].cautl = cautl;
            answer_list[chiso].explain = explain;
        
            $('#tableAnswer').empty()
            $('#txtAddAs').html('Thêm câu trả lời')
            CKEDITOR.instances.cautl.setData('')
            CKEDITOR.instances.explain.setData('')
            let idtype = $('#type_As').val()
            show(answer_list,idtype);
        }
    else
        {
            answer_list.push({ cautl, explain, type_As })
            $('#tableAnswer').empty()
            let idtype = $('#type_As').val()
            show(answer_list, idtype)
            $('#error_date').html('')
            $('#error_answer').html('')
            $('#error_explain').html('')
            CKEDITOR.instances.cautl.setData('')
            CKEDITOR.instances.explain.setData('')
        }

}
const deleteTable = (index) =>
    {
        answer_list.splice(index, 1);

        $('#tableAnswer').empty()
        let value = $('#type_As').val()
        show(answer_list,value)

    }

const editAns = (index) =>
    {
        //luu chi so 
        chiso = index;
        CKEDITOR.instances.cautl.setData(answer_list[index].cautl)
        CKEDITOR.instances.explain.setData(answer_list[index].explain)
        $("#type_As").val(answer_list[index].type_As);
        $('#txtAddAs').html('Cập Nhật câu trả lời')
    }

const show = (array, idtype) => {
    if (idtype == 2)
    {
        $('#tableAnswer').empty();


        for (var i = 0; i < array.length; i++)
        {
            let isStatus = array[i].type_As;
            var bien = `<tr class="row"> <td scope="row" class="col-lg-2">` + (i + 1) + `</td> <td class="col-lg-4 row">`;

            bien += `<span style="margin-right:5px;"><input type="checkbox" ` + (isStatus == 1 ? "checked" : "") + `  name="check"
                    onclick="onlyOne(this,`+ i + `)" /></span><span class="fixText"> ` + array[i].cautl + `</span></td>
                      <td class="col-lg-4"><span class="fixText">`+ array[i].explain + `</span></td>
                        <td class="col-lg-2"><button type="button" class="btn btn-success"
                         onclick="editAns(`+ i + `)">Sửa</button>
                        <button type="button" class="btn btn-light"  onclick="deleteTable( `+ i + `)">Xóa</button>
                          </td>
                           </tr>`;
            $('#tableAnswer').append(bien)
        }
    } else
    {
        $('#tableAnswer').empty();
        for (var i = 0; i < array.length; i++) {
            let isStatus = array[i].type_As;


            var bien = `<tr class="row"> <td scope="row" class="col-lg-2">` + (i + 1) + `</td> <td class="col-lg-4 row">`;

            bien += `<span style="margin-right:5px;"><input type="radio" ` + (isStatus == 1 ? "checked" : "") + `  name="check"
                    onclick="onlyOne(this,`+ i + `)" /></span><span class="fixText"> ` + array[i].cautl + `</span></td>
                      <td class="col-lg-4"><span class="fixText">`+ array[i].explain + `</span></td>
                        <td class="col-lg-2"><button type="button" class="btn btn-success"
                         onclick="editAns(`+ i + `)">Sửa</button>
                        <button type="button" class="btn btn-light"  onclick="deleteTable( `+ i + `)">Xóa</button>
                          </td>
                           </tr>`;
            $('#tableAnswer').append(bien)
        }
    }
   
}

const submit = () => {
//check validation question
    let codequestion = $('#codequestion').val()
    let id_dm = $('#namedm').val()
    let id_dv = $('#namedv').val()
    let id_mt = $('#namemt').val()
    let datepicker = $('#datepicker').val()

    let type_qs = $('#type_qs').val()
    
    
    let content_question = CKEDITOR.instances['description'].getData();
    if (content_question == "")
    {
        bootbox.confirm("Chưa nhập nội dung câu hỏi", function (rusult2) { });
        $('#error_question').html('Chưa nhập nội dung câu hỏi');
        return;
    }
    $('#error_question').html('')
    let urlfile = $('#uploadfile').val() 
    var linkvideo = $('#linkvideo').val();
    let levelquestion = $('#levelquestion').val()
    let count_check = 0;

    if (answer_list.length == 0) { $('#showerrorSubmit').html('Bạn chưa có câu trả lời');return}
    //check check box trong cau tra loi
    //let type_As = $('#type_As').val();
    let checkboxes = document.getElementsByName('check')
  
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            count_check++;
        }
    }
    console.log('true ='+count_check);
    if (type_qs == 1 && count_check > 1 || type_qs == 1 && count_check == 0)
    {
        $('#showerrorSubmit').html("Bạn chưa chọn câu trả lời đúng với loại câu trả lời")
        return;
    }
    if (type_qs == 2 && count_check < 2 || type_qs == 2 && count_check == 0)
    {
        $('#showerrorSubmit').html("Bạn chưa chọn câu trả lời đúng với loại câu trả lời")
        return;
    }

    $('#showerrorSubmit').html("")
    let nametypequestion = $('#tenloaich').val();
    let id_typequestion = $('#maloaich').val(); 

   
    let answers = JSON.stringify(answer_list);
    levelquestion = levelquestion == 1 ? 'Dễ' : 'Khó';

    let question = {
                        ID_Cauhoi: codequestion,
                        ID_DM: id_dm,
                        ID_DV: id_dv,
                        ID_MT: id_mt,
                        Content_QS: content_question,
                        Level_Question: levelquestion ,
                        UrlFile: linkvideo,
                        ID_Type: id_typequestion,
                        TypeQuestion: type_qs

    }
    question = JSON.stringify(question);


    if ( answer_list.length > 0) {
       
        console.log(content_question)

        $.ajax({
            method: "POST",
            url: "/Admin/AddData",
            data:
            {
                question: question,
                answers: answers,
                endDate: datepicker
            },
            success: function (rs) {
             
                sessionStorage.clear();
                
                window.location= '/Admin';

            }
        });

    }
    else {
        
        $('#showerror').html('Chưa nhập câu trả lời')
    }
}


const xuliUpfile = () => {
    let linkvideo = $('#uploadfile').val();
    if (linkvideo == '') {
        bootbox.confirm("Bạn chưa chọn file", function (rusult2) { });
        return;
    }
    if ( linkvideo.indexOf('mp4') == -1) {
       
        $('#error_file').html('file không đúng định dạng,chọn lại file mp4');
        return;
    }
    $('#error_file').html('');
   

    let file = $('#uploadfile')[0].files[0];
   
    let data = new FormData();
    data.append('file', file);
    
    
    $.ajax({
        type: "POST",
        url: "/Admin/UpFile",
        data: data,
        processData: false, 
        contentType: false,  
        success: function (result) {
            
            $('#linkvideo').val(result)
            $('#error_linkvideo').html('');
        },
        error: function (err) {
            console.log(err)
        }
    });
}


function onlyOne(checkbox, index) {
    
    
    
    let id_type = $('#type_qs').val();
    var checkboxes = document.getElementsByName('check')

    if (id_type == 1) {

        console.log('trang thai:' + checkbox.checked)
        if (checkbox.checked) {
            answer_list[index].type_As = 1;
            checkboxes.forEach((item, i) => {
                if (item !== checkbox) { item.checked = false; answer_list[i].type_As = 2; }
            });
            
            console.log(answer_list)

        } else {
            answer_list[index].type_As = 2;
           
            console.log(answer_list)
        }
    } else {
        checkbox.checked == true ? answer_list[index].type_As = 1 : answer_list[index].type_As = 2;

        console.log(answer_list);

    }    
}

const ChooseFile = () => {
    var t = $('#uploadfile').val();
    t == '' ? $('#upload1').hide() : $('#upload1').show()
}

const ChangeSelect2 = () => {
    
    
    let value = $('#type_qs').val()
    if (value == 1) {

        //item.ID_TypesAs = 2
        answer_list.forEach(ans => ans.type_As = 2);
    }
    show(answer_list, value)
    sessionStorage.setItem('answer', JSON.stringify(answer_list))
    console.log(answer_list);

}



