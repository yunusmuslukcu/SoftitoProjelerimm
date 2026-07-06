$(document).ready(function () {
    GetPositions();
});

// Arama için Debounce mekanizması
var searchTimer;
function SearchPositionsDebounced(val) {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function () {
        GetPositions(val);
    }, 300);
}

// 1. LİSTELEME
function GetPositions(searchVal = "") {
    $.ajax({
        url: '/Position/PositionList?search=' + encodeURIComponent(searchVal),
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            var object = '';
            if (response.length === 0) {
                object = '<tr><td colspan="4" class="text-center py-4 text-muted"><i class="fas fa-info-circle me-1"></i>Aranan kriterlere uygun pozisyon bulunamadı.</td></tr>';
            } else {
                $.each(response, function (index, item) {
                    var positionId = item.positionId !== undefined ? item.positionId : item.PositionId;
                    var positionName = item.positionName !== undefined ? item.positionName : item.PositionName;
                    var shortName = item.shortName !== undefined ? item.shortName : item.ShortName;

                    object += '<tr>';
                    object += '<td class="ps-4 fw-medium text-secondary">' + positionId + '</td>';
                    object += '<td class="fw-semibold text-dark">' + positionName + '</td>';
                    object += '<td><span class="badge bg-success-subtle text-success border border-success-subtle px-2.5 py-1.5 rounded-pill"><i class="fas fa-running me-1"></i>' + shortName + '</span></td>';
                    object += '<td class="text-end pe-4">' +
                        '<button class="btn btn-sm btn-outline-warning me-2" onclick="EditPosition(' + positionId + ')"><i class="fas fa-edit"></i> Düzenle</button>' +
                        '<button class="btn btn-sm btn-outline-danger" onclick="DeletePosition(' + positionId + ')"><i class="fas fa-trash"></i> Sil</button>' +
                        '</td>';
                    object += '</tr>';
                });
            }
            $('#tblPositionBody').html(object);
        },
        error: function () {
            alert("Pozisyonlar listelenirken bir hata oluştu!");
        }
    });
}

// 2. MODAL AÇMA
function OpenAddModal() {
    $('#PositionForm')[0].reset();
    $('#PositionId').val('');
    $('#PositionModalLabel').text("Yeni Pozisyon Ekle");
    $('#PositionModal').modal('show');
}

// 3. EKLEME VE GÜNCELLEME
function SavePosition() {
    var formData = {
        PositionId: $('#PositionId').val(),
        PositionName: $('#PositionName').val(),
        ShortName: $('#ShortName').val()
    };

    if (!formData.PositionName || !formData.ShortName) {
        alert("Lütfen tüm alanları doldurun.");
        return;
    }

    var urlTarget = formData.PositionId ? '/Position/Update' : '/Position/AddPosition';

    $.ajax({
        url: urlTarget,
        type: 'POST',
        data: formData,
        success: function (response) {
            alert(response);
            $('#PositionModal').modal('hide');
            GetPositions($('#txtSearchPosition').val());
        },
        error: function () {
            alert("Kayıt işlemi sırasında bir hata oluştu!");
        }
    });
}

// 4. DÜZENLEME
function EditPosition(id) {
    $.ajax({
        url: '/Position/Edit?id=' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $('#PositionModal').modal('show');
            $('#PositionModalLabel').text("Pozisyon Güncelle");

            var positionId = response.positionId !== undefined ? response.positionId : response.PositionId;
            var positionName = response.positionName !== undefined ? response.positionName : response.PositionName;
            var shortName = response.shortName !== undefined ? response.shortName : response.ShortName;

            $('#PositionId').val(positionId);
            $('#PositionName').val(positionName);
            $('#ShortName').val(shortName);
        },
        error: function () {
            alert("Veri getirilirken bir hata oluştu!");
        }
    });
}

// 5. SİLME
function DeletePosition(id) {
    if (confirm("Bu pozisyonu silmek istediğinize emin misiniz?")) {
        $.ajax({
            url: '/Position/Delete?id=' + id,
            type: 'POST',
            success: function (response) {
                alert(response);
                GetPositions($('#txtSearchPosition').val());
            },
            error: function () {
                alert("Silme işlemi başarısız!");
            }
        });
    }
}