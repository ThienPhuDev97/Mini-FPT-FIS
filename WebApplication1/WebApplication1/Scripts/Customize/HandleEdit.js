
$('#UploadFile2').hide()
let today = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate() + 1);
$('#datepickerupdate').datepicker({
    format: 'dd-mm-yyyy',
    uiLibrary: 'bootstrap4',
    minDate: today
});
CKEDITOR.replace("descriptionupdate", {
    entities_latin: false,
    entities_greek: false
});

const updateEdit = () => {
    $('#content_answer').each(function (i, e) {

        let content = $('#content', e).text();
        let description = $('#description', e).text();
        console.log(content);
        
    });
}


const xuliUpfileEdit = () => {
    let linkvideo = $('#uploadfileupdate').val();
    console.log(linkvideo)
    if (linkvideo == '') {
        alert('chưa chọn file upload')
        return;
    }
    if (linkvideo.indexOf('mp4') == -1) {

        $('#error_file2').html('file không đúng định dạng,chọn lại file mp4');
        return;
    }
    $('#error_file2').html('');

    console.log('đúng định dạng mp4')
    let file = $('#uploadfileupdate')[0].files[0];
    console.log(file);
    let data = new FormData();
    data.append('file', file);

    $.ajax({
        type: "POST",
        url: "/Admin/UpFile",
        data: data,
        processData: false,
        contentType: false,
        success: function (result) {
           
            $('#linkvideoupdate').val(result)
            $('#error_urlvideo2').html('');
        },
        error: function (err) {
            console.log(err)
        }
    });
}

$(document).ready(function () {
    let value = $('#type_AsUpdate').val()
    showedit(list_ans, value)
});

const showedit = (list_ans, idtype) => {
    
    if (idtype == 2)
    {

        $('#tableAnswerUpdate').empty();
        for (var i = 0; i < list_ans.length; i++)
        {
            let isStatus = list_ans[i].ID_TypesAs;


            var bien = `<tr class="row"> <td scope="row" class="col-lg-2">` + (i + 1) + `</td> <td class="col-lg-4 row">`;

            bien += `<span style="margin-right:5px;"><input type="checkbox" ` + (isStatus == 1 ? "checked" : "") + `  name="checkboxupdate"
                            onclick="onlyOne2(this,`+ i + `)" /></span><span class="fixText"> ` + list_ans[i].Content_AS + `</span></td>
                              <td class="col-lg-4"><span class="fixText">`+ list_ans[i].Explain + `</span></td>
                                <td class="col-lg-2"><button type="button" class="btn btn-success"
                                 onclick="handleEdit2(`+ i + `)">Sửa</button>
                                <button type="button" class="btn btn-light"onclick="handleDelete2( `+ i + `)">Xóa</button>
                                  </td>
                                   </tr>`;
            $('#tableAnswerUpdate').append(bien)
        }
    }
    else {
            $('#tableAnswerUpdate').empty();
            for (var i = 0; i < list_ans.length; i++)
            {
                        let isStatus = list_ans[i].ID_TypesAs;


                        var bien = `<tr class="row"> <td scope="row" class="col-lg-2">` + (i + 1) + `</td> <td class="col-lg-4 row">`;

                        bien += `<span style="margin-right:5px;"><input type="radio" ` + (isStatus == 1 ? "checked" : "") + `  name="checkboxupdate"
                            onclick="onlyOne2(this,`+ i + `)" /></span><span class="fixText"> ` + list_ans[i].Content_AS + `</span></td>
                              <td class="col-lg-4"><span class="fixText">`+ list_ans[i].Explain + `</span></td>
                                <td class="col-lg-2"><button type="button" class="btn btn-success"
                                 onclick="handleEdit2(`+ i + `)">Sửa</button>
                                <button type="button" class="btn btn-light"onclick="handleDelete2( `+ i + `)">Xóa</button>
                                  </td>
                                   </tr>`;
                        $('#tableAnswerUpdate').append(bien)
            }

    }
}

//phan them vao
let checkindex = -1;



//showedit(list_ans);
let ID_Cauhoi = list_ans[0].ID_Cauhoi;
//xu li edit answer
const handleEdit2 = (index) => {
    checkindex = index;
    console.log(list_ans[index]);
    CKEDITOR.instances['cautl_update'].setData(list_ans[index].Content_AS);
    CKEDITOR.instances['explainupdate'].setData(list_ans[index].Explain);
    $('#txtAddAs2').attr('value', 1);
    $('#txtAddAs2').html('Cập nhật câu trả lời')
}
const handleAddAnswer2 = () => {

    let explain = CKEDITOR.instances['explainupdate'].getData();
    let cautl = CKEDITOR.instances['cautl_update'].getData();
    console.log('tl:' + cautl + "|" + explain);
    if (cautl == "") {
        $('#error_answerUpdate').html('không được để trống');
        return;
    }
    $('#error_answerUpdate').html('');
   
    $('#error_explainUpdate').html('');
    let checkStatusAdd = $('#txtAddAs2').val();
    console.log(checkStatusAdd);
    if (checkStatusAdd == 1) {
        list_ans[checkindex].Content_AS = cautl;
        list_ans[checkindex].Explain = explain;
        checkindex = -1;
        let value = $('#type_AsUpdate').val()
        showedit(list_ans,value);
        $('#txtAddAs2').html('Thêm câu trả lời')
        $('#txtAddAs2').attr('value', 0);
        CKEDITOR.instances.explainupdate.setData('')
        CKEDITOR.instances.cautl_update.setData('')


    }
    else {
        list_ans.push({ ID_AS: 1, Content_AS: cautl, Explain: explain, ID_TypesAs: 2, ID_Cauhoi: ID_Cauhoi })
        let value = $('#type_AsUpdate').val()
        showedit(list_ans,value);
        CKEDITOR.instances.explainupdate.setData('')
        CKEDITOR.instances.cautl_update.setData('')
    }
}
//
const handleDelete2 = (index) => {
    list_ans.splice(index, 1);
    let value = $('#type_AsUpdate').val()
    showedit(list_ans,value);
}

const submit_update = () => {
    let count_check = 0;
    let type_AsUpdate = $('#type_AsUpdate').val();


    let ID_Cauhoi = $('#codeupdate').val()
    let ID_DM = $('#namedm2').val()
    let ID_DV = $('#namedv2').val()
    let ID_MT = $('#namemt2').val()
    let typeQuestion = $('#type_AsUpdate').val()



    let Content_QS = CKEDITOR.instances['descriptionupdate'].getData();
    if (Content_QS == "") { alert('Bạn chưa nhập nội dung câu hỏi'); $('#error_question2').html('Bạn chưa nhập nội dung câu hỏi'); return; }
    $('#error_question2').html('')

    let UrlFile = $('#linkvideoupdate').val()

    $('#error_urlvideo2').html('')
    let Level_Question = $('#levelquestionupdate').val()

    let EndDate = $('#datepickerupdate').val()
   




    //end check question
    if (list_ans.length == 0) { $('#showerrorUpdate2').html(' Bạn chưa chọn câu trả lời'); retunr; }
    let checkboxes = document.getElementsByName('checkboxupdate')
    
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            count_check++;
        }
    }
   
    if (type_AsUpdate == 1 && count_check > 1 || type_AsUpdate == 1 && count_check == 0) {
        $('#showerrorUpdate2').html('Bạn chưa chọn câu trả lời đúng với loại câu trả lời'); return;
    }
    if (type_AsUpdate == 2 && count_check < 2 || type_AsUpdate == 2 && count_check == 0) {
        $('#showerrorUpdate2').html('Bạn chưa chọn câu trả lời đúng với loại câu trả lời'); return;
    }


    $('#showerrorUpdate2').html('')
    console.log('check trc khi submit');
        console.log(list_ans);

    
    let answers = JSON.stringify(list_ans);
    let question = JSON.stringify({
        ID_Cauhoi: ID_Cauhoi,
        ID_DM: ID_DM,
        ID_DV: ID_DV,
        ID_MT: ID_MT,
        Content_QS: Content_QS,
        UrlFile: UrlFile,
        Level_Question: Level_Question,
        TypeQuestion: typeQuestion
    });

    $.ajax({
        method: "POST",
        
        url: "/Admin/HandleUpdate",
        data:
        {
            answers: answers,
            question: question,
            EndDate: EndDate
        },
        success: function (rs) {
            console.log('hello');
            window.location.href = '/Admin';

        }
    });




}


function onlyOne2(checkbox, index) {

    let id_type = $('#type_AsUpdate').val();
    console.log('vi tri ss :' + index);
    var checkboxes = document.getElementsByName('checkboxupdate')

    if (id_type == 1) {

        console.log('trang thai:' + checkbox.checked)
        if (checkbox.checked) {
            list_ans[index].ID_TypesAs = 1;
            checkboxes.forEach((item, i) => {
                if (item !== checkbox) { item.checked = false; list_ans[i].ID_TypesAs = 2; }
            });

            console.log(list_ans)

        } else {
            list_ans[index].ID_TypesAs = 2;
            console.log(list_ans)
        }
    } else {
        checkbox.checked == true ? list_ans[index].ID_TypesAs = 1 : list_ans[index].ID_TypesAs = 2;
        console.log(list_ans);

    }
}

const ChooseFile2 = () => {
    var t = $('#uploadfileupdate').val();
    t == '' ? $('#UploadFile2').hide() : $('#UploadFile2').show()
}

const ChangeSelect = () => {
    let value = $('#type_AsUpdate').val()
    if (value == 1) {
        
        //item.ID_TypesAs = 2
        list_ans.forEach(ans => ans.ID_TypesAs =2);
    }
    showedit(list_ans, value)
    console.log(list_ans);
    
}

    

   