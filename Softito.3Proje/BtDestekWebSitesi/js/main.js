// BT Destek Platformu - Interactive Logic

document.addEventListener('DOMContentLoaded', () => {
    // 1. Mobile Menu Toggle & Scroll Navbar Effect
    const menuToggle = document.getElementById('menuToggle');
    const navMenu = document.getElementById('navMenu');
    const topNavbar = document.getElementById('topNavbar');

    if (menuToggle && navMenu) {
        menuToggle.addEventListener('click', () => {
            navMenu.classList.toggle('open');
        });
    }

    window.addEventListener('scroll', () => {
        if (topNavbar) {
            if (window.scrollY > 40) {
                topNavbar.classList.add('scrolled');
            } else {
                topNavbar.classList.remove('scrolled');
            }
        }
    });

    // Close menu when clicking outside on mobile
    document.addEventListener('click', (e) => {
        if (window.innerWidth <= 991 && navMenu && menuToggle) {
            if (!navMenu.contains(e.target) && !menuToggle.contains(e.target) && navMenu.classList.contains('open')) {
                navMenu.classList.remove('open');
            }
        }
    });

    // Close mobile menu when clicking any nav link
    const navLinks = document.querySelectorAll('.nav-link-item');
    navLinks.forEach(link => {
        link.addEventListener('click', () => {
            if (window.innerWidth <= 991 && navMenu) {
                navMenu.classList.remove('open');
            }
        });
    });

    // 2. Interactive Project Screens Showcase Gallery
    const showcaseData = {
        dashboard: {
            title: "Yönetici Kontrol Paneli (Dashboard)",
            desc: "Tüm destek süreçlerini, aktif talepleri, çözülen bilet oranlarını ve departman bazlı yük dağılımını gerçek zamanlı grafiklerle izleyin.",
            img: "images/Dashboard.png",
            features: [
                "Anlık ve güncel destek talebi istatistikleri",
                "Kategori ve öncelik bazlı dağılım grafikleri",
                "Son açılan taleplere hızlı erişim ve durum takibi"
            ]
        },
        destek: {
            title: "Destek Talebi Yönetimi (Tickets)",
            desc: "Kullanıcıların oluşturduğu yazılım, donanım, ağ ve erişim taleplerini saniyeler içinde filtreleyip durumlarını güncelleyin.",
            img: "images/Destek.png",
            features: [
                "Açık, İşlemde ve Çözüldü durum rozetleri",
                "Anlık arama (Client-side & Server-side filtering)",
                "Tek tıkla talep detayı görüntüleme ve atama"
            ]
        },
        kategoriler: {
            title: "Dinamik Kategori Yapılandırması",
            desc: "Kurumunuzun ihtiyaçlarına göre yeni IT destek kategorileri oluşturun, açıklamalar ekleyin ve bilet akışını düzenleyin.",
            img: "images/Kategoriler.png",
            features: [
                "Yazılım, Donanım, Ağ ve Şifre sıfırlama modülleri",
                "Kolay kategori ekleme, düzenleme ve silme",
                "Düzenli veri tabanı ilişkisi"
            ]
        },
        kullanicilar: {
            title: "Kullanıcı ve Departman Entegrasyonu",
            desc: "Bilgi İşlem, İnsan Kaynakları, Finans ve Pazarlama gibi tüm departman çalışanlarının hesaplarını ve yetkilerini yönetin.",
            img: "images/Kullanıcılar.png",
            features: [
                "Rol tabanlı yetkilendirme altyapısı",
                "Departmanlara özel talep takibi",
                "Hızlı şifre ve profil yapılandırması"
            ]
        },
        raporlar: {
            title: "Gelişmiş Analiz ve Raporlama",
            desc: "IT operasyonlarının performansını ölçün, ortalama çözülme sürelerini ve talep yoğunluklarını detaylı raporlarla analiz edin.",
            img: "images/Raporlar.png",
            features: [
                "Dönemsel performans ve SLA takibi",
                "Yönetim için anlık özet rapor çıktısı",
                "Veri odaklı karar alma mekanizması"
            ]
        }
    };

    const tabButtons = document.querySelectorAll('.tab-btn');
    const displayImg = document.getElementById('showcaseImg');
    const displayTitle = document.getElementById('showcaseTitle');
    const displayDesc = document.getElementById('showcaseDesc');
    const displayFeatures = document.getElementById('showcaseFeatures');

    tabButtons.forEach(btn => {
        btn.addEventListener('click', () => {
            tabButtons.forEach(b => b.classList.remove('active'));
            btn.classList.add('active');

            const key = btn.getAttribute('data-tab');
            const data = showcaseData[key];

            if (data) {
                displayImg.style.opacity = '0.3';
                setTimeout(() => {
                    displayImg.src = data.img;
                    displayTitle.textContent = data.title;
                    displayDesc.textContent = data.desc;

                    displayFeatures.innerHTML = data.features.map(f => `
                        <li><i class="bi bi-check-circle-fill"></i> ${f}</li>
                    `).join('');

                    displayImg.style.opacity = '1';
                }, 150);
            }
        });
    });

    // 3. Lightbox Modal for Fullscreen Screenshot Zoom
    const modalBackdrop = document.getElementById('lightboxModal');
    const modalImg = document.getElementById('lightboxImg');
    const closeModalBtn = document.getElementById('closeModal');
    const zoomTargets = document.querySelectorAll('.zoomable');

    zoomTargets.forEach(target => {
        target.addEventListener('click', () => {
            const img = target.querySelector('img') || target;
            if (img && img.src) {
                modalImg.src = img.src;
                modalBackdrop.classList.add('active');
            }
        });
    });

    if (closeModalBtn && modalBackdrop) {
        closeModalBtn.addEventListener('click', () => {
            modalBackdrop.classList.remove('active');
        });
        modalBackdrop.addEventListener('click', (e) => {
            if (e.target === modalBackdrop) {
                modalBackdrop.classList.remove('active');
            }
        });
    }

    // 4. Animated Stats Counter
    const statCounters = document.querySelectorAll('.count-up');
    let animated = false;

    const animateStats = () => {
        if (animated) return;
        statCounters.forEach(counter => {
            const target = +counter.getAttribute('data-target');
            let count = 0;
            const increment = Math.ceil(target / 40);
            const timer = setInterval(() => {
                count += increment;
                if (count >= target) {
                    counter.textContent = target;
                    clearInterval(timer);
                } else {
                    counter.textContent = count;
                }
            }, 30);
        });
        animated = true;
    };

    window.addEventListener('scroll', () => {
        const statsSection = document.getElementById('istatistikler');
        if (statsSection && window.scrollY + window.innerHeight > statsSection.offsetTop + 100) {
            animateStats();
        }
    });

    setTimeout(animateStats, 500);
});
