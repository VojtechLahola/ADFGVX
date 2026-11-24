# ADFG(V)X Šifra – Demonstrační aplikace (1. světová válka)

Aplikace pro šifrování a dešifrování historickou německou polní šifrou **ADFGX** a **ADFGVX** z 1. světové války, vytvořená v moderním **C# .NET 10** s přehledným grafickým uživatelským rozhraním (**Windows Forms**) a sadou automatických jednotkových testů (**MSTest**).

---

## 🎯 Splnění požadavků zadání (Hodnocení – max. 10 bodů)

| Kritérium | Body | Implementace |
| :--- | :---: | :--- |
| **Správná filtrace vstupních dat** (odstranění diakritiky, velká písmena, zachování čísel a mezer) | **0,5 b** | Metoda `AdfgvxCipher.FilterInput(text, variant)`. Podporuje diakritiku (Unicode FormD unaccenting), velká písmena, převod $W \to V$ (CZ) nebo $J \to I$ (EN), v ADFGVX přímo číslice 0–9. Mezery jsou při šifrování reprezentovány zástupným symbolem/tokenem `SpaceToken` (`XSPACEX`), takže se při dešifrování beze ztráty plně obnoví původní rozestupy mezi slovy. |
| **Generování náhodné abecedy pro ADFG(V)X** | **0,5 b** | Metoda `AdfgvxCipher.GenerateAlphabet(variant)`. Využívá kryptograficky bezpečný Fisher-Yates shuffle (`RandomNumberGenerator`), generuje 25 unikátních písmen (5×5) nebo 36 znaků (6×6). |
| **Funkce pro ADFGX šifrování** | **0,5 b** | Metoda `AdfgvxCipher.EncryptAdfgx(plaintext, key, matrix, isCzechVersion)`. |
| **Funkce pro ADFGVX šifrování** | **0,5 b** | Metoda `AdfgvxCipher.EncryptAdfgvx(plaintext, key, matrix)`. |
| **Funkce pro ADFGX dešifrování** | **0,5 b** | Metoda `AdfgvxCipher.DecryptAdfgx(ciphertext, key, matrix, isCzechVersion)`. Obnovuje text včetně původních mezer. |
| **Funkce pro ADFGVX dešifrování** | **0,5 b** | Metoda `AdfgvxCipher.DecryptAdfgvx(ciphertext, key, matrix)`. Obnovuje text včetně číslic 0–9 a původních mezer. |
| **Uživatelské rozhraní (GUI)** | **6 b** | • Pole pro zadání textu pro šifrování a dešifrování (`txtInput`)<br>• Pole pro zadání klíče (`txtKey`)<br>• Pole pro zadání matice ručně (`txtMatrix`) s **vyřazováním a znázorněním zbývajících znaků** (`lblRemaining`)<br>• Výpis zašifrovaného textu **s mezerami dle sloupců** (`txtOutput`)<br>• Textový výpis aktuální šifrovací tabulky se záhlavími A, D, F, G, (V), X (`txtTablePreview`)<br>• Tlačítka pro volbu šifrování i dešifrování v jednom okně (`btnEncrypt`, `btnDecrypt`)<br>• Přepínač pro volbu verze ADFGX (CZ 5×5 $W\to V$ / EN 5×5 $J\to I$) a ADFGVX (6×6)<br>• Přepínač pro volbu zadání matice ručně nebo náhodného generování. |
| **Umělecký dojem** | **1 b** | Minimalistický čistý design, tlačítko pro rychlé prohození vstupu a výstupu, přehledné ovládání bez zbytečných těžkopádných mřížek. |

---

## 🏗 Struktura projektu

Projekt je navržen podle zásad čistého a udržitelného kódu:

```
├── ADFGVX/
│   ├── AdfgvxCipher.cs      # Samostatná třída s veškerou kryptografickou logikou
│   ├── Form1.cs             # Logika a obsluha uživatelského rozhraní
│   ├── Form1.Designer.cs    # Definice a rozložení ovládacích prvků formuláře
│   ├── Program.cs           # Vstupní bod aplikace
│   └── ADFGVX.csproj        # Projektový soubor Windows Forms (.NET 10)
├── ADFGVX.Tests/
│   ├── AdfgvxCipherTests.cs # 21 automatických jednotkových testů
│   └── ADFGVX.Tests.csproj  # Testovací projekt MSTest (.NET 10)
├── ADFGVX.slnx              # Řešení Visual Studio (Solution)
└── README.md                # Tato dokumentace
```

---

## 🚀 Jak spustit aplikaci

### Požadavky
- [.NET 10 SDK](https://dotnet.microsoft.com/) nebo novější
- Windows OS (aplikace využívá Windows Forms)

### Sestavení a spuštění
V kořenovém adresáři projektu spusťte:

```powershell
# Sestavení celého řešení
dotnet build

# Spuštění aplikace
dotnet run --project ADFGVX
```

### Spuštění jednotkových testů
Pro ověření funkčnosti všech algoritmů a okrajových případů:

```powershell
dotnet test
```

Všechny testy ověřují:
- Filtraci vstupních dat a odstranění diakritiky
- Náhradu $W \to V$ u české verze a $J \to I$ u anglické verze
- Zachování číslic 0–9 v ADFGVX
- Generování náhodné matice s unikátními znaky
- Sledování a vyřazování zbývajících znaků pro manuální zadání
- Polybiovu substituci (frakcionaci) a její inverzi
- Sloupcovou nepravidelnou transpozici s klíčem (včetně duplicitních písmen v klíči)
- Kompletní šifrování a dešifrování (Round-trip) pro ADFGX CZ, ADFGX EN a ADFGVX
- Validaci chybných stavů (lichá délka šifrového textu, neplatné znaky, prázdný klíč)
