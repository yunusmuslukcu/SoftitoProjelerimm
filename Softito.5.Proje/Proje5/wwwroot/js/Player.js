// Sayfa ilk yüklendiğinde futbolcuları listele ve dropdown seçeneklerini hazırla
$(document).ready(function () {
    GetPlayers();
    LoadDropdowns();
});

// Arama için Debounce mekanizması
var searchTimer;
function SearchPlayersDebounced(val) {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function () {
        GetPlayers(val);
    }, 300);
}

// 1. LİSTELEME: Futbolcuları arama filtresine göre çeker
function GetPlayers(searchVal = "") {
    $.ajax({
        url: '/Player/PlayerList?search=' + encodeURIComponent(searchVal),
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            var object = '';
            if (response.length === 0) {
                object = '<tr><td colspan="8" class="text-center py-4 text-muted"><i class="fas fa-info-circle me-1"></i>Aranan kriterlere uygun futbolcu bulunamadı.</td></tr>';
            } else {
                $.each(response, function (index, item) {
                    var playerId = item.playerId !== undefined ? item.playerId : item.PlayerId;
                    var firstName = item.firstName !== undefined ? item.firstName : item.FirstName;
                    var lastName = item.lastName !== undefined ? item.lastName : item.LastName;
                    var age = item.age !== undefined ? item.age : item.Age;
                    var marketValue = item.marketValue !== undefined ? item.marketValue : item.MarketValue;
                    var teamName = item.teamName !== undefined ? item.teamName : item.TeamName;
                    var posName = item.positionName !== undefined ? item.positionName : item.PositionName;

                    teamName = teamName ? teamName : 'Takımsız';
                    posName = posName ? posName : 'Pozisyonsuz';
                    
                    var teamBadge = (teamName !== 'Takımsız' && teamName !== '')
                        ? '<span class="badge bg-primary-subtle text-primary border border-primary-subtle px-2.5 py-1.5 rounded-pill"><i class="fas fa-shield-halved me-1"></i>' + teamName + '</span>'
                        : '<span class="badge bg-secondary-subtle text-secondary border border-secondary-subtle px-2.5 py-1.5 rounded-pill"><i class="fas fa-times me-1"></i>Takımsız</span>';
                    
                    var posBadge = (posName !== 'Pozisyonsuz' && posName !== '')
                        ? '<span class="badge bg-success-subtle text-success border border-success-subtle px-2.5 py-1.5 rounded-pill"><i class="fas fa-running me-1"></i>' + posName + '</span>'
                        : '<span class="badge bg-secondary-subtle text-secondary border border-secondary-subtle px-2.5 py-1.5 rounded-pill"><i class="fas fa-times me-1"></i>Pozisyonsuz</span>';

                    object += '<tr>';
                    object += '<td class="ps-4 fw-medium text-secondary">' + playerId + '</td>';
                    object += '<td class="fw-semibold text-dark">' + firstName + '</td>';
                    object += '<td class="fw-semibold text-dark">' + lastName + '</td>';
                    object += '<td>' + age + '</td>';
                    object += '<td><span class="badge bg-light text-dark border px-2 py-1">' + marketValue + '</span></td>';
                    object += '<td>' + teamBadge + '</td>';
                    object += '<td>' + posBadge + '</td>';
                    object += '<td class="text-end pe-4">' +
                        '<button class="btn btn-sm btn-outline-warning me-2" onclick="EditPlayer(' + playerId + ')"><i class="fas fa-edit"></i> Düzenle</button>' +
                        '<button class="btn btn-sm btn-outline-danger" onclick="DeletePlayer(' + playerId + ')"><i class="fas fa-trash"></i> Sil</button>' +
                        '</td>';
                    object += '</tr>';
                });
            }
            $('#tblPlayerBody').html(object);
        },
        error: function () {
            alert("Futbolcular listelenirken bir hata oluştu!");
        }
    });
}

// Dropdown menüleri yükleme
function LoadDropdowns() {
    $.ajax({
        url: '/Team/TeamList',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            var options = '<option value="">Takım Seçiniz (Opsiyonel)</option>';
            $.each(response, function (index, item) {
                var teamId = item.teamId !== undefined ? item.teamId : item.TeamId;
                var teamName = item.teamName !== undefined ? item.teamName : item.TeamName;
                options += '<option value="' + teamId + '">' + teamName + '</option>';
            });
            $('#TeamId').html(options);
        }
    });

    $.ajax({
        url: '/Position/PositionList',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            var options = '<option value="">Pozisyon Seçiniz (Opsiyonel)</option>';
            $.each(response, function (index, item) {
                var positionId = item.positionId !== undefined ? item.positionId : item.PositionId;
                var positionName = item.positionName !== undefined ? item.positionName : item.PositionName;
                var shortName = item.shortName !== undefined ? item.shortName : item.ShortName;
                options += '<option value="' + positionId + '">' + positionName + ' (' + shortName + ')</option>';
            });
            $('#PositionId').html(options);
        }
    });
}

// 2. MODAL AÇMA
function OpenAddModal() {
    $('#PlayerForm')[0].reset();
    $('#PlayerId').val('');
    $('#PlayerModalLabel').text("Yeni Futbolcu Ekle");
    $('#PlayerModal').modal('show');
}

// 3. EKLEME VE GÜNCELLEME
function SavePlayer() {
    var formData = {
        PlayerId: $('#PlayerId').val(),
        FirstName: $('#FirstName').val(),
        LastName: $('#LastName').val(),
        Age: $('#Age').val(),
        MarketValue: $('#MarketValue').val(),
        TeamId: $('#TeamId').val() || null,
        PositionId: $('#PositionId').val() || null
    };

    if (!formData.FirstName || !formData.LastName || !formData.Age || !formData.MarketValue) {
        alert("Lütfen gerekli tüm alanları doldurun.");
        return;
    }

    var urlTarget = formData.PlayerId ? '/Player/Update' : '/Player/AddPlayer';

    $.ajax({
        url: urlTarget,
        type: 'POST',
        data: formData,
        success: function (response) {
            alert(response);
            $('#PlayerModal').modal('hide');
            GetPlayers($('#txtSearchPlayer').val());
        },
        error: function () {
            alert("Kayıt işlemi sırasında bir hata oluştu!");
        }
    });
}

// 4. DÜZENLEME: Veriyi modala doldurur
function EditPlayer(id) {
    $.ajax({
        url: '/Player/Edit?id=' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $('#PlayerModal').modal('show');
            $('#PlayerModalLabel').text("Futbolcu Güncelle");

            var playerId = response.playerId !== undefined ? response.playerId : response.PlayerId;
            var firstName = response.firstName !== undefined ? response.firstName : response.FirstName;
            var lastName = response.lastName !== undefined ? response.lastName : response.LastName;
            var age = response.age !== undefined ? response.age : response.Age;
            var marketValue = response.marketValue !== undefined ? response.marketValue : response.MarketValue;
            var teamId = response.teamId !== undefined ? response.teamId : response.TeamId;
            var positionId = response.positionId !== undefined ? response.positionId : response.PositionId;

            $('#PlayerId').val(playerId);
            $('#FirstName').val(firstName);
            $('#LastName').val(lastName);
            $('#Age').val(age);
            $('#MarketValue').val(marketValue);
            $('#TeamId').val(teamId || "");
            $('#PositionId').val(positionId || "");
        },
        error: function () {
            alert("Veri getirilirken bir hata oluştu!");
        }
    });
}

// 5. SİLME
function DeletePlayer(id) {
    if (confirm("Bu futbolcuyu silmek istediğinize emin misiniz?")) {
        $.ajax({
            url: '/Player/Delete?id=' + id,
            type: 'POST',
            success: function (response) {
                alert(response);
                GetPlayers($('#txtSearchPlayer').val());
            },
            error: function () {
                alert("Silme işlemi başarısız!");
            }
        });
    }
}

// PDF İndirme (jsPDF)
function ExportToPDF() {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();
    
    doc.text("Futbolcu Listesi", 14, 15);
    
    var rows = [];
    $('#tblPlayerBody tr').each(function() {
        if ($(this).find('td').length < 5) return;

        var row = [
            $(this).find('td:nth-child(1)').text(),
            $(this).find('td:nth-child(2)').text() + ' ' + $(this).find('td:nth-child(3)').text(),
            $(this).find('td:nth-child(4)').text(),
            $(this).find('td:nth-child(5)').text(),
            $(this).find('td:nth-child(6)').text(),
            $(this).find('td:nth-child(7)').text()
        ];
        rows.push(row);
    });
    
    doc.autoTable({
        head: [['ID', 'Ad Soyad', 'Yas', 'Deger', 'Takim', 'Pozisyon']],
        body: rows,
        startY: 20
    });
    
    doc.save('futbolcu-listesi.pdf');
}

// Word İndirme
function ExportToWord() {
    var html = "<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40'>";
    html += "<head><title>Futbolcu Listesi</title><style>table {width:100%; border-collapse:collapse;} th, td {border:1px solid #ccc; padding:8px; text-align:left;} th {background-color:#f2f2f2;}</style></head><body>";
    html += "<h2>Futbolcu Listesi</h2>";
    html += "<table>";
    html += "<thead><tr><th>ID</th><th>Adı</th><th>Soyadı</th><th>Yaş</th><th>Piyasa Değeri</th><th>Takım</th><th>Pozisyon</th></tr></thead>";
    html += "<tbody>";
    
    $('#tblPlayerBody tr').each(function() {
        if ($(this).find('td').length < 5) return;
        
        html += "<tr>";
        html += "<td>" + $(this).find('td:nth-child(1)').text() + "</td>";
        html += "<td>" + $(this).find('td:nth-child(2)').text() + "</td>";
        html += "<td>" + $(this).find('td:nth-child(3)').text() + "</td>";
        html += "<td>" + $(this).find('td:nth-child(4)').text() + "</td>";
        html += "<td>" + $(this).find('td:nth-child(5)').text() + "</td>";
        html += "<td>" + $(this).find('td:nth-child(6)').text() + "</td>";
        html += "<td>" + $(this).find('td:nth-child(7)').text() + "</td>";
        html += "</tr>";
    });
    
    html += "</tbody></table></body></html>";
    
    var blob = new Blob(['\ufeff' + html], {
        type: 'application/msword'
    });
    var url = URL.createObjectURL(blob);
    var a = document.createElement('a');
    a.href = url;
    a.download = 'futbolcu-listesi.doc';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}