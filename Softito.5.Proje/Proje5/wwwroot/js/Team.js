$(document).ready(function () {
    GetTeams();
});

// Arama için Debounce mekanizması
var searchTimer;
function SearchTeamsDebounced(val) {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function () {
        GetTeams(val);
    }, 300);
}

// 1. LİSTELEME
function GetTeams(searchVal = "") {
    $.ajax({
        url: '/Team/TeamList?search=' + encodeURIComponent(searchVal),
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            var object = '';
            if (response.length === 0) {
                object = '<tr><td colspan="4" class="text-center py-4 text-muted"><i class="fas fa-info-circle me-1"></i>Aranan kriterlere uygun takım bulunamadı.</td></tr>';
            } else {
                $.each(response, function (index, item) {
                    var teamId = item.teamId !== undefined ? item.teamId : item.TeamId;
                    var teamName = item.teamName !== undefined ? item.teamName : item.TeamName;
                    var city = item.city !== undefined ? item.city : item.City;

                    object += '<tr>';
                    object += '<td class="ps-4 fw-medium text-secondary">' + teamId + '</td>';
                    object += '<td class="fw-semibold text-dark">' + teamName + '</td>';
                    object += '<td><span class="badge bg-light text-dark border px-2.5 py-1.5 rounded-pill"><i class="fas fa-map-marker-alt text-danger me-1"></i>' + city + '</span></td>';
                    object += '<td class="text-end pe-4">' +
                        '<button class="btn btn-sm btn-outline-warning me-2" onclick="EditTeam(' + teamId + ')"><i class="fas fa-edit"></i> Düzenle</button>' +
                        '<button class="btn btn-sm btn-outline-danger" onclick="DeleteTeam(' + teamId + ')"><i class="fas fa-trash"></i> Sil</button>' +
                        '</td>';
                    object += '</tr>';
                });
            }
            $('#tblTeamBody').html(object);
        },
        error: function () {
            alert("Takımlar listelenirken bir hata oluştu!");
        }
    });
}

// 2. MODAL AÇMA
function OpenAddModal() {
    $('#TeamForm')[0].reset();
    $('#TeamId').val('');
    $('#TeamModalLabel').text("Yeni Takım Ekle");
    $('#TeamModal').modal('show');
}

// 3. EKLEME VE GÜNCELLEME
function SaveTeam() {
    var formData = {
        TeamId: $('#TeamId').val(),
        TeamName: $('#TeamName').val(),
        City: $('#City').val()
    };

    if (!formData.TeamName || !formData.City) {
        alert("Lütfen tüm alanları doldurun.");
        return;
    }

    var urlTarget = formData.TeamId ? '/Team/Update' : '/Team/AddTeam';

    $.ajax({
        url: urlTarget,
        type: 'POST',
        data: formData,
        success: function (response) {
            alert(response);
            $('#TeamModal').modal('hide');
            GetTeams($('#txtSearchTeam').val());
        },
        error: function () {
            alert("Kayıt işlemi sırasında bir hata oluştu!");
        }
    });
}

// 4. DÜZENLEME
function EditTeam(id) {
    $.ajax({
        url: '/Team/Edit?id=' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $('#TeamModal').modal('show');
            $('#TeamModalLabel').text("Takım Güncelle");

            var teamId = response.teamId !== undefined ? response.teamId : response.TeamId;
            var teamName = response.teamName !== undefined ? response.teamName : response.TeamName;
            var city = response.city !== undefined ? response.city : response.City;

            $('#TeamId').val(teamId);
            $('#TeamName').val(teamName);
            $('#City').val(city);
        },
        error: function () {
            alert("Veri getirilirken bir hata oluştu!");
        }
    });
}

// 5. SİLME
function DeleteTeam(id) {
    if (confirm("Bu takımı silmek istediğinize emin misiniz?")) {
        $.ajax({
            url: '/Team/Delete?id=' + id,
            type: 'POST',
            success: function (response) {
                alert(response);
                GetTeams($('#txtSearchTeam').val());
            },
            error: function () {
                alert("Silme işlemi başarısız!");
            }
        });
    }
}