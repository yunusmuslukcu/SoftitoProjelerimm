using System.Collections.Generic;

namespace projectcodefrst.Models
{
    public class RaporViewModel
    {
        public int ToplamKitap { get; set; }
        public int ToplamYazar { get; set; }
        public int ToplamKategori { get; set; }
        public int ToplamOdunc { get; set; }
        public double OrtalamaSayfaSayisi { get; set; }
        public Kitap? EnKalinKitap { get; set; }
        public List<KategoriRaporDto> KategoriDagilimi { get; set; } = new List<KategoriRaporDto>();
        public List<YazarRaporDto> YazarDagilimi { get; set; } = new List<YazarRaporDto>();
        
        // Yeni ödev sorguları
        public List<BookAuthorInnerJoinDto> InnerJoinResult { get; set; } = new List<BookAuthorInnerJoinDto>();
        public List<AuthorBookLeftJoinDto> LeftJoinResult { get; set; } = new List<AuthorBookLeftJoinDto>();
        public List<CategoryPagesGroupByDto> GroupByResult { get; set; } = new List<CategoryPagesGroupByDto>();
        public List<BookOrderByDto> OrderByResult { get; set; } = new List<BookOrderByDto>();
    }

    public class KategoriRaporDto
    {
        public string KategoriAdi { get; set; } = string.Empty;
        public int KitapSayisi { get; set; }
    }

    public class YazarRaporDto
    {
        public string YazarAdi { get; set; } = string.Empty;
        public int KitapSayisi { get; set; }
    }

    public class BookAuthorInnerJoinDto
    {
        public string KitapAdi { get; set; } = string.Empty;
        public string YazarAdi { get; set; } = string.Empty;
        public int Sayfa { get; set; }
    }

    public class AuthorBookLeftJoinDto
    {
        public string YazarAdi { get; set; } = string.Empty;
        public string KitapAdi { get; set; } = string.Empty;
        public int Sayfa { get; set; }
    }

    public class CategoryPagesGroupByDto
    {
        public string KategoriAdi { get; set; } = string.Empty;
        public int ToplamSayfa { get; set; }
        public int KitapSayisi { get; set; }
    }

    public class BookOrderByDto
    {
        public string KitapAdi { get; set; } = string.Empty;
        public string YazarAdi { get; set; } = string.Empty;
        public int Sayfa { get; set; }
    }
}
