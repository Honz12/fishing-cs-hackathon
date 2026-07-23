# Kde Jsou Ryby?!

Konzolová textová rybářská hra v C# s ASCII art grafikou. Chytej ryby, vylepšuj své vybavení a staň se nejlepším rybářem!

## O hře

Hráč se ocitá v roli rybáře, který chytá ryby pomocí minihry s pohybujícím se kurzorem. Chycené ryby se zobrazují jako 16×16 pixelové ASCII obrázky přímo v terminálu.

Hra obsahuje **20+ druhů ryb** — 10 sladkovodních a 10 mořských + tajné ryby, každá s vlastní vahou, raritou a požadovanou úrovní prutu.

### Herní smyčka

1. **Hlavní menu** — možnost chytat ryby, navštívit obchod nebo odejít
2. **Chytání ryb** — minihra s pohybující se značkou; načasování určí, zda rybu chytíš
3. **Obchod** — nakup vylepšení prutu (více druhů ryb) a lodě (větší inventář)
4. **Inventář** — přehled všech chycených ryb

## Jak spustit

```bash
dotnet run
```

## Autoři

| Jméno | Co dělal |
|---|---|
| **Honz12** | Hlavní herní smyčka, minihra chytání, datový model ryb, systém ASCII obrázků, boot screen |
| **Matěj Albert** | Obchod (vylepšení prutu/lodě), úpravy menu, README & tutoriál, Testování |
| **sebastianjecny-green** | ASCII art obrázky ryb, prutů a lodí |
